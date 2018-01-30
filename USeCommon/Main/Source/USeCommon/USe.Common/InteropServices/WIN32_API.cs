using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace USe.Common.InteropServices
{
    /// <summary>
    /// Win32 API定义。
    /// </summary>
    public static class WIN32_API
    {
        #region Win32常量定义
        /// <summary>
        /// These are the generic rights.
        /// </summary>
        [Flags]
        public enum GenericRight : uint
        {
            /// <summary>
            /// #define GENERIC_READ (0x80000000L)
            /// </summary>
            GENERIC_READ = 0x80000000,

            /// <summary>
            /// #define GENERIC_WRITE (0x40000000L)
            /// </summary>
            GENERIC_WRITE = 0x40000000,

            /// <summary>
            /// #define GENERIC_EXECUTE (0x20000000L)
            /// </summary>
            GENERIC_EXECUTE = 0x20000000,

            /// <summary>
            /// #define GENERIC_ALL (0x10000000L)
            /// </summary>
            GENERIC_ALL = 0x10000000
        }

        /// <summary>
        /// Page access rights.
        /// </summary>
        [Flags]
        public enum PageAccess : uint
        {
            /// <summary>
            /// #define PAGE_NOACCESS 0x01
            /// </summary>
            PAGE_NOACCESS = 0x01,

            /// <summary>
            /// #define PAGE_READONLY 0x02
            /// </summary>
            PAGE_READONLY = 0x02,

            /// <summary>
            /// #define PAGE_READWRITE 0x04
            /// </summary>
            PAGE_READWRITE = 0x04,

            /// <summary>
            /// #define PAGE_WRITECOPY 0x08
            /// </summary>
            PAGE_WRITECOPY = 0x08,

            /// <summary>
            /// #define PAGE_EXECUTE 0x10
            /// </summary>
            PAGE_EXECUTE = 0x10,

            /// <summary>
            /// #define PAGE_EXECUTE_READ 0x20
            /// </summary>
            PAGE_EXECUTE_READ = 0x20,

            /// <summary>
            /// #define PAGE_EXECUTE_READWRITE 0x40
            /// </summary>
            PAGE_EXECUTE_READWRITE = 0x40,

            /// <summary>
            /// #define PAGE_EXECUTE_WRITECOPY 0x80
            /// </summary>
            PAGE_EXECUTE_WRITECOPY = 0x80,

            /// <summary>
            /// #define PAGE_GUARD 0x100
            /// </summary>
            PAGE_GUARD = 0x100,

            /// <summary>
            /// #define PAGE_NOCACHE 0x200
            /// </summary>
            PAGE_NOCACHE = 0x200,

            /// <summary>
            /// #define PAGE_WRITECOMBINE 0x400
            /// </summary>
            PAGE_WRITECOMBINE = 0x400,

            /// <summary>
            /// #define MEM_COMMIT 0x1000
            /// </summary>
            MEM_COMMIT = 0x1000,

            /// <summary>
            /// #define MEM_RESERVE 0x2000
            /// </summary>
            MEM_RESERVE = 0x2000,

            /// <summary>
            /// #define MEM_DECOMMIT 0x4000
            /// </summary>
            MEM_DECOMMIT = 0x4000,

            /// <summary>
            /// #define MEM_RELEASE 0x8000
            /// </summary>
            MEM_RELEASE = 0x8000,

            /// <summary>
            /// #define MEM_FREE 0x10000
            /// </summary>
            MEM_FREE = 0x10000,

            /// <summary>
            /// #define MEM_PRIVATE 0x20000
            /// </summary>
            MEM_PRIVATE = 0x20000,

            /// <summary>
            /// #define MEM_MAPPED 0x40000
            /// </summary>
            MEM_MAPPED = 0x40000,

            /// <summary>
            /// #define MEM_RESET 0x80000
            /// </summary>
            MEM_RESET = 0x80000,

            /// <summary>
            /// #define MEM_TOP_DOWN 0x100000
            /// </summary>
            MEM_TOP_DOWN = 0x100000,

            /// <summary>
            /// #define MEM_WRITE_WATCH 0x200000
            /// </summary>
            MEM_WRITE_WATCH = 0x200000,

            /// <summary>
            /// #define MEM_PHYSICAL 0x400000
            /// </summary>
            MEM_PHYSICAL = 0x400000,

            /// <summary>
            /// #define MEM_LARGE_PAGES 0x20000000
            /// </summary>
            MEM_LARGE_PAGES = 0x20000000,

            /// <summary>
            /// #define MEM_4MB_PAGES 0x80000000
            /// </summary>
            MEM_4MB_PAGES = 0x80000000,

            /// <summary>
            /// #define SEC_FILE 0x800000
            /// </summary>
            SEC_FILE = 0x800000,

            /// <summary>
            /// #define SEC_IMAGE 0x1000000
            /// </summary>
            SEC_IMAGE = 0x1000000,

            /// <summary>
            /// #define SEC_RESERVE 0x4000000
            /// </summary>
            SEC_RESERVE = 0x4000000,

            /// <summary>
            /// #define SEC_COMMIT 0x8000000
            /// </summary>
            SEC_COMMIT = 0x8000000,

            /// <summary>
            /// #define SEC_NOCACHE 0x10000000
            /// </summary>
            SEC_NOCACHE = 0x10000000,

            /// <summary>
            /// #define MEM_IMAGE SEC_IMAGE
            /// </summary>
            MEM_IMAGE = SEC_IMAGE,

            /// <summary>
            /// #define WRITE_WATCH_FLAG_RESET 0x01
            /// </summary>
            WRITE_WATCH_FLAG_RESET = 0x01,
        }

        /// <summary>
        /// File Mapping Security and Access Rights.
        /// </summary>
        [Flags]
        public enum FileMappingAccess : uint
        {
            /// <summary>
            /// #define FILE_MAP_COPY SECTION_QUERY<br/>
            /// #define SECTION_QUERY 0x0001
            /// </summary>
            FILE_MAP_COPY = 0x0001,

            /// <summary>
            /// #define FILE_MAP_WRITE SECTION_MAP_WRITE<br/>
            /// #define SECTION_MAP_WRITE 0x0002
            /// </summary>
            FILE_MAP_WRITE = 0x0002,

            /// <summary>
            /// #define FILE_MAP_READ SECTION_MAP_READ<br/>
            /// #define SECTION_MAP_READ 0x0004
            /// </summary>
            FILE_MAP_READ = 0x0004,

            /// <summary>
            /// #define FILE_MAP_ALL_ACCESS SECTION_ALL_ACCESS<br/>
            /// #define STANDARD_RIGHTS_REQUIRED (0x000F0000L)<br/>
            /// #define SECTION_MAP_EXECUTE 0x0008<br/>
            /// #define SECTION_EXTEND_SIZE 0x0010<br/>
            /// #define SECTION_ALL_ACCESS (STANDARD_RIGHTS_REQUIRED | SECTION_QUERY | SECTION_MAP_WRITE | SECTION_MAP_READ | SECTION_MAP_EXECUTE | SECTION_EXTEND_SIZE)
            /// </summary>
            FILE_MAP_ALL_ACCESS = 0x000f0000 | 0x0001 | 0x0002 | 0x0004 | 0x0008 | 0x0010,
        }

        /// <summary>
        /// 无效的文件句柄值，#define INVALID_HANDLE_VALUE  ((HANDLE)((LONG_PTR)-1))。
        /// </summary>
        public static readonly IntPtr InvalidHandleValue = new IntPtr(-1);
        #endregion

        #region Win32函数导入
        /// <summary>
        /// Maps the specified executable module into the address space of the calling process.
        /// </summary>
        /// <param name="fileName">The name of the executable module (either a .dll or .exe file). The name specified is the file name of the module and is not related to the name stored in the library module itself, as specified by the LIBRARY keyword in the module-definition (.def) file. </param>
        /// <returns>
        /// If the function succeeds, the return value is a handle to the module.<br/>
        /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// HMODULE WINAPI LoadLibraryA(LPCSTR lpFileName);<br/>
        /// HMODULE WINAPI LoadLibraryW(LPCWSTR lpFileName);
        /// </remarks>
        [DllImport("Kernel32.dll", EntryPoint = "LoadLibraryW", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary(string fileName);

        /// <summary>
        /// Decrements the reference count of the loaded dynamic-link library (DLL). When the reference count reaches zero, the module is unmapped from the address space of the calling process and the handle is no longer valid.
        /// </summary>
        /// <param name="moduleHandle">A handle to the loaded DLL module. The LoadLibrary or GetModuleHandle function returns this handle.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.<br/>
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// BOOL WINAPI FreeLibrary(HMODULE hModule);
        /// </remarks>
        [DllImport("Kernel32.dll", EntryPoint = "FreeLibrary", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int FreeLibrary(IntPtr moduleHandle);

        /// <summary>
        /// Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="moduleHandle">A handle to the DLL module that contains the function or variable. The LoadLibrary or GetModuleHandle function returns this handle.</param>
        /// <param name="procName">The function or variable name, or the function's ordinal value. If this parameter is an ordinal value, it must be in the low-order word; the high-order word must be zero.</param>
        /// <returns>
        /// If the function succeeds, the return value is the address of the exported function or variable.<br/>
        /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// FARPROC WINAPI GetProcAddress(HMODULE hModule, LPCSTR lpProcName);
        /// </remarks>
        [DllImport("Kernel32.dll", EntryPoint = "GetProcAddress", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr GetProcAddress(IntPtr moduleHandle, string procName);

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="handle">A valid handle to an open object.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.<br/>
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// BOOL WINAPI CloseHandle(HANDLE hObject);
        /// </remarks>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern int CloseHandle(IntPtr handle);

        /// <summary>
        /// Creates or opens a file, file stream, directory, physical disk, volume, console buffer, tape drive, communications resource, mailslot, or named pipe. The function returns a handle that can be used to access the object.
        /// </summary>
        /// <param name="lpFileName">The name of the object to be created or opened.</param>
        /// <param name="dwDesiredAccess">The access to the object, which can be read, write, or both.</param>
        /// <param name="dwShareMode">The sharing mode of an object, which can be read, write, both, or none.</param>
        /// <param name="lpSecurityAttributes">A pointer to a SECURITY_ATTRIBUTES structure that determines whether or not the returned handle can be inherited by child processes.</param>
        /// <param name="dwCreationDisposition">An action to take on files that exist and do not exist.</param>
        /// <param name="dwFlagsAndAttributes">The file attributes and flags.</param>
        /// <param name="hTemplateFile">A handle to a template file with the GENERIC_READ access right. The template file supplies file attributes and extended attributes for the file that is being created. This parameter can be NULL.</param>
        /// <returns>
        /// If the function succeeds, the return value is an open handle to a specified file. If a specified file exists before the function call and dwCreationDisposition is CREATE_ALWAYS or OPEN_ALWAYS, a call to GetLastError returns ERROR_ALREADY_EXISTS, even when the function succeeds. If a file does not exist before the call, GetLastError returns zero (0).<br/>
        /// If the function fails, the return value is INVALID_HANDLE_VALUE. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("Kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern SafeFileHandle CreateFile(string lpFileName,
                                                         [MarshalAs(UnmanagedType.U4)] GenericRight dwDesiredAccess,
                                                         [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode,
                                                         IntPtr lpSecurityAttributes,
                                                         [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition,
                                                         [MarshalAs(UnmanagedType.U4)] FileAttributes dwFlagsAndAttributes,
                                                         IntPtr hTemplateFile);

        /// <summary>
        /// Retrieves the size of the specified file. The file size that can be reported by this function is limited to a DWORD value.
        /// </summary>
        /// <param name="hFile">A handle to the file.</param>
        /// <param name="lpdwFileSizeHigh">A pointer to the variable where the high-order doubleword of the file size is returned. This parameter can be NULL if the application does not require the high-order doubleword.</param>
        /// <returns>
        /// If the function succeeds, the return value is the low-order doubleword of the file size, and, if lpFileSizeHigh is non-NULL, the function puts the high-order doubleword of the file size into the variable pointed to by that parameter.<br/>
        /// If the function fails and lpFileSizeHigh is NULL, the return value is INVALID_FILE_SIZE. To get extended error information, call GetLastError. When lpFileSizeHigh is NULL, the results returned for large files are ambiguous, and you will not be able to determine the actual size of the file. It is recommended that you use GetFileSizeEx instead.<br/>
        /// If the function fails and lpFileSizeHigh is non-NULL, the return value is INVALID_FILE_SIZE and GetLastError will return a value other than NO_ERROR.
        /// </returns>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern uint GetFileSize(SafeFileHandle hFile, out uint lpdwFileSizeHigh);

        /// <summary>
        /// Creates or opens a named or unnamed file mapping object for a specified file.
        /// </summary>
        /// <param name="hFile">A handle to the file from which to create a file mapping object. </param>
        /// <param name="lpAttributes">A pointer to a SECURITY_ATTRIBUTES structure that determines whether a returned handle can be inherited by child processes. </param>
        /// <param name="flProtect">The protection for the file view, when the file is mapped. </param>
        /// <param name="dwMaximumSizeHigh">The high-order DWORD of the maximum size of the file mapping object.</param>
        /// <param name="dwMaximumSizeLow">The low-order DWORD of the maximum size of the file mapping object. </param>
        /// <param name="lpName">The name of the file mapping object. </param>
        /// <returns></returns>
        [DllImport("Kernel32", EntryPoint = "CreateFileMappingW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern SafeFileMappingHandle CreateFileMapping(SafeFileHandle hFile,
                                                                       IntPtr lpAttributes,
                                                                       [MarshalAs(UnmanagedType.U4)] PageAccess flProtect,
                                                                       uint dwMaximumSizeHigh,
                                                                       uint dwMaximumSizeLow,
                                                                       string lpName);

        /// <summary>
        /// Opens a named file mapping object.
        /// </summary>
        /// <param name="dwDesiredAccess">The access to the file mapping object. This access is checked against any security descriptor on the target file mapping object.</param>
        /// <param name="bInheritHandle">If this parameter is TRUE, a process created by the CreateProcess function can inherit the handle; otherwise, the handle cannot be inherited.</param>
        /// <param name="lpName">The name of the file mapping object to be opened. If there is an open handle to a file mapping object by this name and the security descriptor on the mapping object does not conflict with the dwDesiredAccess parameter, the open operation succeeds. </param>
        /// <returns>
        /// If the function succeeds, the return value is an open handle to the specified file mapping object.<br/>
        /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("Kernel32", EntryPoint = "OpenFileMappingW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern SafeFileMappingHandle OpenFileMapping([MarshalAs(UnmanagedType.U4)] FileMappingAccess dwDesiredAccess,
                                                                     bool bInheritHandle,
                                                                     string lpName);

        /// <summary>
        /// Maps a view of a file mapping into the address space of a calling process.
        /// </summary>
        /// <param name="hFileMappingObject">A handle to a file mapping object. The CreateFileMapping and OpenFileMapping functions return this handle.</param>
        /// <param name="dwDesiredAccess">The type of access to a file mapping object, which ensures the protection of the pages.</param>
        /// <param name="dwFileOffsetHigh">A high-order DWORD of the file offset where the view begins.</param>
        /// <param name="dwFileOffsetLow">A low-order DWORD of the file offset where the view is to begin. </param>
        /// <param name="dwNumberOfBytesToMap">The number of bytes of a file mapping to map to the view. All bytes must be within the maximum size specified by CreateFileMapping. If this parameter is 0 (zero), the mapping extends from the specified offset to the end of the file mapping.</param>
        /// <returns>
        /// If the function succeeds, the return value is the starting address of the mapped view.<br/>
        /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("Kernel32", SetLastError = true)]
        public static extern SafeViewOfFileHandle MapViewOfFile(SafeFileMappingHandle hFileMappingObject,
                                                                  [MarshalAs(UnmanagedType.U4)] FileMappingAccess dwDesiredAccess,
                                                                  uint dwFileOffsetHigh,
                                                                  uint dwFileOffsetLow,
                                                                  uint dwNumberOfBytesToMap);

        /// <summary>
        /// Unmaps a mapped view of a file from the calling process's address space.
        /// </summary>
        /// <param name="lpBaseAddress">A pointer to the base address of the mapped view of a file that is to be unmapped. This value must be identical to the value returned by a previous call to the MapViewOfFile or MapViewOfFileEx function.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero, and all dirty pages within the specified range are written "lazily" to disk.<br/>
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("Kernel32", SetLastError = true)]
        public static extern int UnmapViewOfFile(IntPtr lpBaseAddress);
        #endregion
    }
}

