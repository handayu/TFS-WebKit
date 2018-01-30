using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32.SafeHandles;

namespace USe.Common.InteropServices
{
    /// <summary>
    /// Win32文件映射类。
    /// </summary>
    public class FileMapping : SharedMemory
    {
        private string m_filePath;
        private FileMode m_creationMode;
        private FileAccess m_accessRights;
        private FileShare m_shareMode;

        private SafeFileHandle m_fileHandle;

        /// <summary>
        /// 初始化FileMapping类的新实例。
        /// </summary>
        public FileMapping()
        {
            m_filePath = null;
            m_fileHandle = null;
        }

        /// <summary>
        /// FileMapping类的析构方法。
        /// </summary>
        ~FileMapping()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放FileMapping所使用的资源。
        /// </summary>
      	/// <param name="disposing">资源释放标志，True: 释放所有资源；False: 仅释放非受控资源。</param>
	    protected override void Dispose(bool disposing)
        {
            if (m_fileHandle != null && !m_fileHandle.IsInvalid)
            {
                m_fileHandle.Dispose();
                m_fileHandle = null;
            }

            base.Dispose(disposing);
        }


        /// <summary>
        /// 返回表示当前FileMapping类对象的字符串。
        /// </summary>
        /// <returns>表示当前FileMapping类对象的字符串。</returns>
        public override string ToString()
        {
            return String.Format("FileMapping[{0}, {1}]",
                                 (String.IsNullOrEmpty(this.MappingName) ? "<Anonymous>" : this.MappingName),
                                 m_filePath);
        }


        /// <summary>
        /// 创建文件映射。
        /// </summary>
        /// <param name="mappingName">文件映射的名称。</param>
        /// <param name="filePath">被映射文件的全路径名称。</param>
        /// <param name="creationMode">文件创建/打开模式。</param>
        /// <param name="accessRights">文件访问权限。</param>
        /// <param name="shareMode">文件共享模式。</param>
        /// <param name="mappingOffset">文件映射的偏移量(起始位置)。</param>
        /// <param name="viewSize">文件映射的视图大小，等于0时表示按文件实际大小取值。</param>
        /// <exception cref="System.ComponentModel.Win32Exception">创建/打开文件失败时。</exception>
        public void Create(string mappingName, string filePath, FileMode creationMode,
                            FileAccess accessRights, FileShare shareMode, ulong mappingOffset, uint viewSize)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("FilePath can not be null or empty.");
            }
            if (viewSize != uint.MaxValue && ulong.MaxValue - mappingOffset < viewSize)
            {
                throw new ArgumentOutOfRangeException("MappingOffset plus MappingSize is greater than the max value.");
            }

            if (m_fileHandle != null && !m_fileHandle.IsInvalid)
            {
                throw new InvalidOperationException("FileMapping already created.");
            }

            m_filePath = new FileInfo(filePath).FullName;
            m_creationMode = creationMode;
            m_accessRights = accessRights;
            m_shareMode = shareMode;

            WIN32_API.PageAccess pageAccessRights;
            WIN32_API.FileMappingAccess mappingAccessRights;
            switch (m_accessRights)
            {
                case FileAccess.Read:
                    pageAccessRights = WIN32_API.PageAccess.PAGE_READONLY;
                    mappingAccessRights = WIN32_API.FileMappingAccess.FILE_MAP_READ;
                    break;

                case FileAccess.Write:
                    pageAccessRights = WIN32_API.PageAccess.PAGE_READWRITE;
                    mappingAccessRights = WIN32_API.FileMappingAccess.FILE_MAP_WRITE;
                    break;

                case FileAccess.ReadWrite:
                    pageAccessRights = WIN32_API.PageAccess.PAGE_READWRITE;
                    mappingAccessRights = WIN32_API.FileMappingAccess.FILE_MAP_READ | WIN32_API.FileMappingAccess.FILE_MAP_WRITE;
                    break;

                default:
                    Debug.Assert(false);
                    pageAccessRights = WIN32_API.PageAccess.PAGE_READONLY;
                    mappingAccessRights = WIN32_API.FileMappingAccess.FILE_MAP_READ;
                    break;
            }

            try
            {
                base.InternalCreate(mappingName, pageAccessRights, mappingAccessRights, mappingOffset, viewSize);
            }
            catch (Exception)
            {
                if (m_fileHandle != null && !m_fileHandle.IsInvalid)
                {
                    m_fileHandle.Dispose();
                    m_fileHandle = null;
                }
                throw;
            }

            Debug.Assert(m_fileHandle != null && !m_fileHandle.IsInvalid);
        }


        /// <summary>
        /// 创建文件映射。
        /// </summary>
        /// <param name="mappingName">文件映射对象的名称。</param>
        /// <param name="pageAccessRights">页面访问权限。</param>
        /// <param name="mappingAccessRights">文件映射的访问权限。</param>
        /// <param name="mappingOffset">文件映射的偏移量(起始位置)。</param>
        /// <param name="viewSize">[in, out]文件映射的视图大小，等于0时表示按文件实际大小取值。</param>
        /// <exception cref="System.ComponentModel.Win32Exception">创建文件映射失败时。</exception>
        protected override SafeFileMappingHandle CreateMapping(string mappingName, WIN32_API.PageAccess pageAccessRights,
                                        WIN32_API.FileMappingAccess mappingAccessRights, ulong mappingOffset, ref uint viewSize)
        {
            SafeFileMappingHandle result = null;

            WIN32_API.GenericRight genericRights = 0;
            if ((m_accessRights & FileAccess.Read) == FileAccess.Read)
            {
                genericRights |= WIN32_API.GenericRight.GENERIC_READ;
            }
            if ((m_accessRights & FileAccess.Write) == FileAccess.Write)
            {
                genericRights |= WIN32_API.GenericRight.GENERIC_WRITE;
            }

            SafeFileHandle handle = null;
            try
            {
                Debug.Assert(!String.IsNullOrEmpty(m_filePath));
                handle = WIN32_API.CreateFile(m_filePath, genericRights, m_shareMode, IntPtr.Zero, m_creationMode, FileAttributes.Normal, IntPtr.Zero);
                if (handle.IsInvalid)
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }

                ulong actualFileSize = 0;
                {
                    uint fileSizeHigh = 0;
                    uint fileSizeLow = WIN32_API.GetFileSize(handle, out fileSizeHigh);
                    if (fileSizeLow == uint.MaxValue)
                    {
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                    }

                    actualFileSize = (((ulong)fileSizeHigh) << 32) + fileSizeLow;
                }

                ulong expectedFileSize = 0;
                if (viewSize == 0)
                {
                    if (actualFileSize == 0)
                    {
                        throw new ArgumentException("The actual file size is zero, Can't set the value of MappingSize.");
                    }
                    if (actualFileSize <= mappingOffset)
                    {
                        throw new ArgumentOutOfRangeException("MappingOffset is greater than the actual file size, Can't set the value of MappingSize.");
                    }
                    if ((actualFileSize - mappingOffset) > uint.MaxValue)
                    {
                        throw new ArgumentException("The actual file size is too large to set the value of MappingSize.");
                    }

                    viewSize = (uint)(actualFileSize - mappingOffset);
                    expectedFileSize = actualFileSize;
                }
                else
                {
                    Debug.Assert(ulong.MaxValue - mappingOffset >= viewSize);
                    expectedFileSize = mappingOffset + viewSize;
                }

                // 不需处理，创建FileMapping时，会自动扩大
                //if (expectedFileSize < actualFileSize)
                //{
                //	// Resize File
                //}

                result = WIN32_API.CreateFileMapping(handle,
                                                     IntPtr.Zero,
                                                     pageAccessRights,
                                                     (uint)(expectedFileSize >> 32),
                                                     (uint)expectedFileSize,
                                                     mappingName);
                if (result.IsInvalid)
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            catch (Exception)
            {
                if (handle != null && !handle.IsInvalid)
                {
                    handle.Dispose();
                    handle = null;
                }
                throw;
            }

            Debug.Assert(m_fileHandle == null);
            m_fileHandle = handle;

            return result;
        }
    }
}
