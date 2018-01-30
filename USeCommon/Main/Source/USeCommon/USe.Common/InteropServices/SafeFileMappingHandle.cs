using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace USe.Common.InteropServices
{
    /// <summary>
    /// Win32 API文件映射句柄类。
    /// </summary>
    public sealed class SafeFileMappingHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// 初始化SafeFileMappingHandle类的新实例。
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        internal SafeFileMappingHandle()
            : base(true)
        {
        }

        /// <summary>
        /// 初始化SafeFileMappingHandle类的新实例。
        /// </summary>
        /// <param name="handle">文件映射句柄的指针类型值。</param>
        /// <param name="ownsHandle">拥有句柄标志。</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        internal SafeFileMappingHandle(IntPtr handle, bool ownsHandle)
            : base(ownsHandle)
        {
            base.SetHandle(handle);
        }

        /// <summary>
        /// 释放文件映射句柄。
        /// </summary>
        /// <returns>
        /// 释放文件映射句柄成功与否标志。
        /// </returns>
        protected override bool ReleaseHandle()
        {
            return (WIN32_API.CloseHandle(base.handle) != 0);
        }
    }
}
