using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32.SafeHandles;

namespace USe.Common.InteropServices
{
    /// <summary>
    /// Win32共享内存类。
    /// </summary>
    public class SharedMemory// : IDisposable
    {
        private string m_mappingName;
        private SafeFileMappingHandle m_mappingHandle;
        private SafeViewOfFileHandle m_mappingView;
        private ulong m_mappingOffset;
        private uint m_viewSize;


        /// <summary>
        /// 初始化SharedMemory类的新实例。
        /// </summary>
        public SharedMemory()
        {
            m_mappingName = null;
            m_mappingHandle = null;
            m_mappingView = null;
            m_mappingOffset = 0;
            m_viewSize = 0;
        }

        /// <summary>
        /// SharedMemory类的析构方法。
        /// </summary>
        ~SharedMemory()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放SharedMemory所使用的资源。
        /// </summary>
      	/// <param name="disposing">资源释放标志，True: 释放所有资源；False: 仅释放非受控资源。</param>
	    protected virtual void Dispose(bool disposing)
        {
            if (m_mappingView != null && !m_mappingView.IsInvalid)
            {
                m_mappingView.Dispose();
                m_mappingView = null;
            }

            if (m_mappingHandle != null && !m_mappingHandle.IsInvalid)
            {
                m_mappingHandle.Dispose();
                m_mappingHandle = null;
            }

            m_mappingOffset = 0;
            m_viewSize = 0;
            m_mappingName = null;
        }


        /// <summary>
        /// 获取共享内存的文件映射名称，可能为null/empty。
        /// </summary>
        public string MappingName
        {
            get
            {
                return m_mappingName;
            }
        }

        /// <summary>
        /// 获取共享内存的文件映射的偏移量，单位：字节。
        /// </summary>
        public ulong MappingOffset
        {
            get
            {
                return m_mappingOffset;
            }
        }

        /// <summary>
        /// 获取共享内存的视图大小，单位：字节。
        /// </summary>
        public uint ViewSize
        {
            get
            {
                return m_viewSize;
            }
        }

        /// <summary>
        /// 获取共享内存的视图基地址。
        /// </summary>
        public IntPtr ViewBase
        {
            get
            {
                return m_mappingView.DangerousGetHandle();
            }
        }


        /// <summary>
        /// 返回表示当前SharedMemory类对象的字符串。
        /// </summary>
        /// <returns>表示当前SharedMemory类对象的字符串。</returns>
        public override string ToString()
        {
            return String.Format("SharedMemory[{0}]", (String.IsNullOrEmpty(this.MappingName) ? "<Anonymous>" : this.MappingName));
        }


        /// <summary>
        /// 创建文件映射对象，并映射视图。
        /// </summary>
        /// <param name="mappingName">文件映射对象的名称，可为null/empty。</param>
        /// <param name="pageAccessRights">内存页面访问权限。</param>
        /// <param name="mappingAccessRights">文件映射的访问权限。</param>
        /// <param name="viewSize">文件映射的视图大小，单位：字节。</param>
        public void Create(string mappingName, WIN32_API.PageAccess pageAccessRights,
                                WIN32_API.FileMappingAccess mappingAccessRights, uint viewSize)
        {
            if (m_mappingView != null && !m_mappingView.IsInvalid)
            {
                throw new InvalidOperationException("SharedMemory already created.");
            }

            InternalCreate(mappingName, pageAccessRights, mappingAccessRights, 0, viewSize);
        }


        /// <summary>
        /// 创建文件映射对象，并映射(内存)视图。
        /// </summary>
        /// <param name="mappingName">文件映射对象的名称，可为null/empty。</param>
        /// <param name="pageAccessRights">内存页面访问权限。</param>
        /// <param name="mappingAccessRights">文件映射的访问权限。</param>
        /// <param name="mappingOffset">文件映射的偏移量(起始位置)。</param>
        /// <param name="viewSize">文件映射的视图大小，单位：字节。</param>
        protected void InternalCreate(string mappingName, WIN32_API.PageAccess pageAccessRights,
                                    WIN32_API.FileMappingAccess mappingAccessRights, ulong mappingOffset, uint viewSize)
        {
            SafeFileMappingHandle handle = null;
            SafeViewOfFileHandle view = null;

            handle = CreateMapping(mappingName, pageAccessRights, mappingAccessRights, mappingOffset, ref viewSize);

            view = WIN32_API.MapViewOfFile(handle,
                                           mappingAccessRights,
                                           (uint)(mappingOffset >> 32),
                                           (uint)mappingOffset,
                                           viewSize);
            if (view.IsInvalid)
            {
                handle.Dispose();
                handle = null;
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }

            m_mappingName = mappingName;
            m_mappingHandle = handle;
            m_mappingView = view;
            m_mappingOffset = mappingOffset;
            m_viewSize = viewSize;
        }


        /// <summary>
        /// 创建文件映射。
        /// </summary>
        /// <param name="mappingName">文件映射对象的名称。</param>
        /// <param name="pageAccessRights">页面访问权限。</param>
        /// <param name="mappingAccessRights">文件映射的访问权限。</param>
        /// <param name="mappingOffset">文件映射的偏移量(起始位置)。</param>
        /// <param name="viewSize">文件映射的视图大小。</param>
        /// <exception cref="System.ComponentModel.Win32Exception">创建文件映射失败时。</exception>
        protected virtual SafeFileMappingHandle CreateMapping(string mappingName, WIN32_API.PageAccess pageAccessRights,
                                        WIN32_API.FileMappingAccess mappingAccessRights, ulong mappingOffset, ref uint viewSize)
        {
            SafeFileMappingHandle result = null;

            result = WIN32_API.CreateFileMapping(new SafeFileHandle(WIN32_API.InvalidHandleValue, true),
                                                 IntPtr.Zero,
                                                 pageAccessRights,
                                                 0,
                                                 viewSize,
                                                 mappingName);
            if (result.IsInvalid)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }

            return result;
        }


        /// <summary>
        /// 按指定的偏移量获取其在文件映射视图内的地址。
        /// </summary>
        /// <param name="offset">相对于文件映射视图基地址的偏移量，单位：字节。</param>
        /// <returns>
        /// 文件映射视图内的地址。
        /// </returns>
        public IntPtr GetViewAddressAt(uint offset)
        {
            if (offset > this.ViewSize)
            {
                throw new ArgumentOutOfRangeException("Offset exceeds the size of the view.");
            }

            return new IntPtr(this.ViewBase.ToInt64() + offset);
        }


        /// <summary>
        /// 按指定地址读取指定数量的字节序列。
        /// </summary>
        /// <param name="address">文件映射视图的地址。</param>
        /// <param name="count">字节数量。</param>
        /// <returns>
        /// 读取的字节序列。
        /// </returns>
        /// <remarks>
        /// 未做地址的合法性检查。
        /// </remarks>
        public byte[] Read(IntPtr address, int count)
        {
            byte[] result = new byte[count];

            Debug.Assert(address.ToInt64() >= this.ViewBase.ToInt64() && address.ToInt64() <= this.ViewBase.ToInt64() + this.ViewSize - count);
            Marshal.Copy(address, result, 0, count);

            return result;
        }

        /// <summary>
        /// 按指定地址读取指定数量的字节序列。
        /// </summary>
        /// <param name="address">文件映射视图的地址。</param>
        /// <param name="bytes">字节序列的数组。</param>
        /// <param name="startIndex">字节序列的起始位置。</param>
        /// <param name="count">字节数量。</param>
        /// <remarks>
        /// 未做地址的合法性检查。
        /// </remarks>
        public void Read(IntPtr address, byte[] bytes, int startIndex, int count)
        {
            Debug.Assert(address.ToInt64() >= this.ViewBase.ToInt64() && address.ToInt64() <= this.ViewBase.ToInt64() + this.ViewSize - count);
            Marshal.Copy(address, bytes, startIndex, count);
        }

        /// <summary>
        /// 按指定地址读取指定数量的字节序列。
        /// </summary>
        /// <param name="offset">相对ViewBase的偏移量，单位：字节。</param>
        /// <param name="count">字节数量。</param>
        /// <returns>
        /// 读取的字节序列。
        /// </returns>
        /// <remarks>
        /// 未做地址的合法性检查。
        /// </remarks>
        public byte[] Read(uint offset, int count)
        {
            byte[] result = new byte[count];

            Debug.Assert(offset < this.ViewSize);
            IntPtr address = new IntPtr(this.ViewBase.ToInt64() + offset);

            Debug.Assert(address.ToInt64() >= this.ViewBase.ToInt64() && address.ToInt64() <= this.ViewBase.ToInt64() + this.ViewSize - count);
            Marshal.Copy(address, result, 0, count);

            return result;
        }

        /// <summary>
        /// 按指定地址读取指定数量的字节序列。
        /// </summary>
        /// <param name="offset">相对ViewBase的偏移量，单位：字节。</param>
        /// <param name="bytes">字节序列的数组。</param>
        /// <param name="startIndex">字节序列的起始位置。</param>
        /// <param name="count">字节数量。</param>
        /// <remarks>
        /// 未做地址的合法性检查。
        /// </remarks>
        public void Read(uint offset, byte[] bytes, int startIndex, int count)
        {
            Debug.Assert(offset < this.ViewSize);
            IntPtr address = new IntPtr(this.ViewBase.ToInt64() + offset);

            Debug.Assert(address.ToInt64() >= this.ViewBase.ToInt64() && address.ToInt64() <= this.ViewBase.ToInt64() + this.ViewSize - count);
            Marshal.Copy(address, bytes, startIndex, count);
        }

        /// <summary>
        /// 按指定地址写入指定数量的字节序列。
        /// </summary>
        /// <param name="address">文件映射视图的地址。</param>
        /// <param name="bytes">字节序列。</param>
        /// <remarks>
        /// 未做地址的合法性检查。
        /// </remarks>
        public void Write(IntPtr address, byte[] bytes)
        {
            Debug.Assert(address.ToInt64() >= this.ViewBase.ToInt64() && address.ToInt64() <= this.ViewBase.ToInt64() + this.ViewSize - bytes.Length);
            Marshal.Copy(bytes, 0, address, bytes.Length);
        }

        /// <summary>
        /// 按指定地址写入指定数量的字节序列。
        /// </summary>
        /// <param name="address">文件映射视图的地址。</param>
        /// <param name="bytes">字节序列的数组。</param>
        /// <param name="startIndex">字节序列的起始位置。</param>
        /// <param name="count">字节数量。</param>
        /// <remarks>
        /// 未做地址的合法性检查。
        /// </remarks>
        public void Write(IntPtr address, byte[] bytes, int startIndex, int count)
        {
            Debug.Assert(address.ToInt64() >= this.ViewBase.ToInt64() && address.ToInt64() <= this.ViewBase.ToInt64() + this.ViewSize - count);
            Marshal.Copy(bytes, startIndex, address, count);
        }

        /// <summary>
        /// 按指定地址写入指定数量的字节序列。
        /// </summary>
        /// <param name="offset">相对ViewBase的偏移量，单位：字节。</param>
        /// <param name="bytes">字节序列。</param>
        /// <remarks>
        /// 未做地址的合法性检查。
        /// </remarks>
        public void Write(uint offset, byte[] bytes)
        {
            Debug.Assert(offset < this.ViewSize);
            IntPtr address = new IntPtr(this.ViewBase.ToInt64() + offset);

            Debug.Assert(address.ToInt64() >= this.ViewBase.ToInt64() && address.ToInt64() <= this.ViewBase.ToInt64() + this.ViewSize - bytes.Length);
            Marshal.Copy(bytes, 0, address, bytes.Length);
        }

        /// <summary>
        /// 按指定地址写入指定数量的字节序列。
        /// </summary>
        /// <param name="offset">相对ViewBase的偏移量，单位：字节。</param>
        /// <param name="bytes">字节序列的数组。</param>
        /// <param name="startIndex">字节序列的起始位置。</param>
        /// <param name="count">字节数量。</param>
        /// <remarks>
        /// 未做地址的合法性检查。
        /// </remarks>
        public void Write(uint offset, byte[] bytes, int startIndex, int count)
        {
            Debug.Assert(offset < this.ViewSize);
            IntPtr address = new IntPtr(this.ViewBase.ToInt64() + offset);

            Debug.Assert(address.ToInt64() >= this.ViewBase.ToInt64() && address.ToInt64() <= this.ViewBase.ToInt64() + this.ViewSize - count);
            Marshal.Copy(bytes, startIndex, address, count);
        }


        /// <summary>
        /// 按指定地址读取指定结构类型的值。
        /// </summary>
        /// <typeparam name="T">结构类型。</typeparam>
        /// <param name="address">文件映射视图的地址。</param>
        /// <returns>
        /// 读取的结构类型的值。
        /// </returns>
        /// <remarks>
        /// 未做地址的合法性检查。
        /// </remarks>
        public T Read<T>(IntPtr address) where T : struct
        {
            Debug.Assert(address.ToInt64() >= this.ViewBase.ToInt64() && address.ToInt64() <= this.ViewBase.ToInt64() + this.ViewSize - Marshal.SizeOf(typeof(T)));
            return (T)Marshal.PtrToStructure(address, typeof(T));
        }

        /// <summary>
        /// 按指定的相对ViewBase的偏移量读取指定结构类型的值。
        /// </summary>
        /// <typeparam name="T">结构类型。</typeparam>
        /// <param name="offset">相对ViewBase的偏移量，单位：字节。</param>
        /// <returns>
        /// 读取的结构类型的值。
        /// </returns>
        /// <remarks>
        /// 未做地址的合法性检查。
        /// </remarks>
        public T Read<T>(uint offset) where T : struct
        {
            Debug.Assert(offset < this.ViewSize);
            IntPtr address = new IntPtr(this.ViewBase.ToInt64() + offset);

            Debug.Assert(address.ToInt64() >= this.ViewBase.ToInt64() && address.ToInt64() <= this.ViewBase.ToInt64() + this.ViewSize - Marshal.SizeOf(typeof(T)));
            return (T)Marshal.PtrToStructure(address, typeof(T));
        }

        /// <summary>
        /// 按指定地址读取指定结构类型的值。
        /// </summary>
        /// <typeparam name="T">结构类型。</typeparam>
        /// <param name="address">文件映射视图的地址。</param>
        /// <param name="result">[out] 读取的结构类型的值。</param>
        /// <remarks>
        /// 未做地址的合法性检查。
        /// </remarks>
        public void Read<T>(IntPtr address, out T result) where T : struct
        {
            Debug.Assert(address.ToInt64() >= this.ViewBase.ToInt64() && address.ToInt64() <= this.ViewBase.ToInt64() + this.ViewSize - Marshal.SizeOf(typeof(T)));
            result = (T)Marshal.PtrToStructure(address, typeof(T));
        }

        /// <summary>
        /// 按指定的相对ViewBase的偏移量读取指定结构类型的值。
        /// </summary>
        /// <typeparam name="T">结构类型。</typeparam>
        /// <param name="offset">相对ViewBase的偏移量，单位：字节。</param>
        /// <param name="result">[out] 读取的结构类型的值。</param>
        /// <remarks>
        /// 未做地址的合法性检查。
        /// </remarks>
        public void Read<T>(uint offset, out T result) where T : struct
        {
            Debug.Assert(offset < this.ViewSize);
            IntPtr address = new IntPtr(this.ViewBase.ToInt64() + offset);

            Debug.Assert(address.ToInt64() >= this.ViewBase.ToInt64() && address.ToInt64() <= this.ViewBase.ToInt64() + this.ViewSize - Marshal.SizeOf(typeof(T)));
            result = (T)Marshal.PtrToStructure(address, typeof(T));
        }

        /// <summary>
        /// 按指定地址写入指定结构类型的值。
        /// </summary>
        /// <typeparam name="T">结构类型。</typeparam>
        /// <param name="address">文件映射视图的地址。</param>
        /// <param name="value">结构类型的值</param>
        /// <remarks>
        /// 未做地址的合法性检查。
        /// </remarks>
        public void Write<T>(IntPtr address, ref T value) where T : struct
        {
            Debug.Assert(address.ToInt64() >= this.ViewBase.ToInt64() && address.ToInt64() <= this.ViewBase.ToInt64() + this.ViewSize - Marshal.SizeOf(typeof(T)));
            Marshal.StructureToPtr(value, address, false);
        }

        /// <summary>
        /// 按的相对ViewBase的偏移量写入指定结构类型的值。
        /// </summary>
        /// <typeparam name="T">结构类型。</typeparam>
        /// <param name="offset">相对ViewBase的偏移量，单位：字节。</param>
        /// <param name="value">结构类型的值</param>
        /// <remarks>
        /// 未做地址的合法性检查。
        /// </remarks>
        public void Write<T>(uint offset, ref T value) where T : struct
        {
            Debug.Assert(offset < this.ViewSize);
            IntPtr address = new IntPtr(this.ViewBase.ToInt64() + offset);

            Debug.Assert(address.ToInt64() >= this.ViewBase.ToInt64() && address.ToInt64() <= this.ViewBase.ToInt64() + this.ViewSize - Marshal.SizeOf(typeof(T)));
            Marshal.StructureToPtr(value, address, false);
        }
    }
}

