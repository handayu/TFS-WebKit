using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Xilium.CefGlue;

namespace UseOnlineTradingSystem
{
    public class BsCtl: Control
    {
        public CefBrowser bs;
        //BSCompletionCallback bsc = new BSCompletionCallback();
        //BSSetCookieCallback bscc = new BSSetCookieCallback();
        public BsCtl(Control ctl, string url, string type)
        {
            Parent = ctl;
            var cwi = CefWindowInfo.Create();
            cwi.SetAsChild(Parent.Handle, new CefRectangle(0, 0, Parent.Width, Parent.Height));
            var bc = new BsClient(type, ctl);
            bc.OnCreated += bc_OnCreated;
            var bss = new CefBrowserSettings() { };            
            CefBrowserHost.CreateBrowser(cwi, bc, bss, url);//,rc);
            Parent.SizeChanged += Parent_SizeChanged;

        }

        /// <summary>
        /// 设置cookies
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="cookies"></param>
        /// <param name="domain"></param>
        public static void SetCookie(string domain, Dictionary<string, string> cookies)
        {
            CefCookieManager manager = CefCookieManager.GetGlobal(null);
            string url = "http://" + domain;
            foreach (var c in cookies)
            {
                CefCookie cookie = new CefCookie();
                cookie.Name = c.Key;
                cookie.Value = c.Value;
                cookie.Domain = domain;
                cookie.Path = "/";
                cookie.HttpOnly = false;
                //cookie.Expires = DateTime.Now.AddDays(100);
                //cookie.expires.year = 2200;
                //cookie.expires.month = 4;
                //cookie.expires.day_of_week = 5;
                //cookie.expires.day_of_month = 11;
                // BSCookieTask bt = new BSCookieTask(url, cookie);
                //CefRuntime.PostTask(CefThreadId.IO, bt);
                manager.SetCookie(url, cookie, null);
            }
        }

        /// <summary>
        /// 清除cookies
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="names"></param>
        public static void DelectCookie(string domain, List<string> names)
        {
            CefCookieManager manager = CefCookieManager.GetGlobal(null);
            string url = "http://" + domain;
            foreach (var name in names)
            {
                manager.DeleteCookies(url, name, null);
            }
        }
        void bc_OnCreated(object sender, EventArgs e)
        {
            bs = (CefBrowser)sender;
            var handle = bs.GetHost().GetWindowHandle();
            ResizeWindow(handle, Parent.Width, Parent.Height);
        }

        void Parent_SizeChanged(object sender, EventArgs e)
        {
            if (bs != null)
            {
                var handle = bs.GetHost().GetWindowHandle();
                ResizeWindow(handle, Parent.Width, Parent.Height);
            }
        }
        public void LoadUrl(string url)
        {
            if (bs != null)
            {
              
                bs.GetMainFrame().LoadUrl(url);
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private void ResizeWindow(IntPtr handle, int width, int height)
        {
            if (handle != IntPtr.Zero)
            {
                //其中0x0002相当于SWP_NOMOVE；0x0004相当于SWP_NOZORDER
                SetWindowPos(handle, IntPtr.Zero,0, 0, width, height,0x0002 | 0x0004);
            }
        }
    }
}
