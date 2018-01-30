using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace USe.Common.InteropServices
{
    /// <summary>
    /// Win32 API文件映射视图句柄类。
    /// </summary>
    public sealed class SafeViewOfFileHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// 初始化SafeViewOfFileHandle类的新实例。
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        internal SafeViewOfFileHandle()
            : base(true)
        {
        }

        /// <summary>
        /// 初始化SafeViewOfFileHandle类的新实例。
        /// </summary>
        /// <param name="handle">文件映射视图句柄的指针类型值。</param>
        /// <param name="ownsHandle">拥有句柄标志。</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        internal SafeViewOfFileHandle(IntPtr handle, bool ownsHandle)
            : base(ownsHandle)
        {
            base.SetHandle(handle);
        }

        /// <summary>
        /// 释放文件映射视图句柄。
        /// </summary>
        /// <returns>
        /// 释放文件映射视图句柄成功与否标志。
        /// </returns>
        protected override bool ReleaseHandle()
        {
            if (WIN32_API.UnmapViewOfFile(base.handle) != 0)
            {
                base.handle = IntPtr.Zero;
                return true;
            }
            return false;
        }
    }


}

