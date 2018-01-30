using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace USe.Common.InteropServices
{
    /// <summary>
    /// Win32 DLL动态装载基类。
    /// </summary>
    public class DllLoader
    {
        private string m_dllName;
        private IntPtr m_dllHandle;

        /// <summary>
        /// 初始化DllLoader类的新实例。
        /// </summary>
        public DllLoader()
        {
            m_dllHandle = IntPtr.Zero;
        }

        /// <summary>
        /// DllLoader类的析构方法。
        /// </summary>
        ~DllLoader()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放由DllLoader占用的所有资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放由DllLoader占用的资源。
        /// </summary>
        /// <param name="disposing">资源释放标志，为True释放所有资源；为False则仅释放非受控资源。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (m_dllHandle != IntPtr.Zero)
            {
                WIN32_API.FreeLibrary(m_dllHandle);
                m_dllHandle = IntPtr.Zero;
            }
        }


        /// <summary>
        /// 已装载的动态链接库的文件名称。
        /// </summary>
        /// <remarks>
        /// 注：动态链接库还未装载时返回空。
        /// </remarks>
        public string DllName
        {
            get
            {
                return m_dllName;
            }
        }

        /// <summary>
        /// 动态链接库已装载标志。
        /// </summary>
        public bool IsLoaded
        {
            get
            {
                return (m_dllHandle != IntPtr.Zero);
            }
        }


        /// <summary>
        /// 装载动态链接库。
        /// </summary>
        /// <param name="dllName">装载动态链接库的文件名称。</param>
        public virtual void Load(string dllName)
        {
            if (IsLoaded)
            {
                throw new InvalidOperationException("Libaray already loaded.");
            }

            IntPtr dllHandle = WIN32_API.LoadLibrary(dllName);
            if (dllHandle == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            try
            {
                GetAllProcAddress(dllHandle);
            }
            catch (Exception)
            {
                WIN32_API.FreeLibrary(dllHandle);
                throw;
            }

            m_dllName = dllName;
            m_dllHandle = dllHandle;
        }

        /// <summary>
        /// 从指定的动态链接库获取所有需导入的函数指针。
        /// </summary>
        /// <param name="dllHandle">动态链接库的句柄。</param>
        protected virtual void GetAllProcAddress(IntPtr dllHandle)
        {
            FieldInfo[] fields = this.GetType().GetFields();

            List<FieldInfo> list = new List<FieldInfo>(fields.Length);

            foreach (FieldInfo field in fields)
            {
                object[] attrs = field.GetCustomAttributes(typeof(DllLoadAttribute), false);

                foreach (DllLoadAttribute attr in attrs)
                {
                    attr.Initialize(field);
                    //Console.WriteLine("==>DllLoadAttribute: " + attr);

                    Debug.Assert(!String.IsNullOrEmpty(attr.EntryPoint));
                    IntPtr procPtr = GetProcAddress(dllHandle, attr.EntryPoint, attr.IsRequired);

                    Delegate d = null;
                    if (procPtr != IntPtr.Zero)
                    {
                        Debug.Assert(attr.DelegateType != null);
                        d = Marshal.GetDelegateForFunctionPointer(procPtr, attr.DelegateType);
                        Debug.Assert(d != null);
                    }
                    else
                    {
                        Debug.Assert(attr.IsRequired);
                    }

                    field.SetValue(this, d);

                    break;
                }
            }
        }

        /// <summary>
        /// 按照指定Win32动态链接库句柄和函数名称获取函数指针。
        /// </summary>
        /// <param name="dllHandle">Win32动态链接库句柄。</param>
        /// <param name="procName">函数名称。</param>
        /// <param name="isRequired">指定的函数是否必须存在标志。</param>
        /// <returns>
        /// Win32函数指针。
        /// </returns>
        protected IntPtr GetProcAddress(IntPtr dllHandle, string procName, bool isRequired)
        {
            Debug.Assert(dllHandle != IntPtr.Zero);

            IntPtr procPtr = WIN32_API.GetProcAddress(dllHandle, procName);
            if (procPtr == IntPtr.Zero && isRequired)
            {
                Win32Exception ex = new Win32Exception(Marshal.GetLastWin32Error());
                string message = String.Format("Get {0} proc-address failed, {1}", procName, ex.Message);
                throw new Win32Exception(ex.NativeErrorCode, message);
            }

            return procPtr;
        }
    }
}
