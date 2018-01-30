using mPaint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UseOnlineTradingSystem
{
    public class WinAPI
    {
        public static int WM_SYSCOMMAND = 0x0112;

        public static int SC_MOVE = 0xF010;

        public static int HTCAPTION = 0x0002;

        public static int CS_DropSHADOW = 0x20000;

        public static int GCL_STYLE = -26;

        public static int WM_COPYDATA = 0x004A;


        /// <summary>
        /// 定义一个新的窗口消息
        /// </summary>
        /// <param name="lpString">（被注册）消息的名字</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "RegisterWindowMessageA")]
        public static extern int RegisterWindowMessage(string lpString);

        public enum ChangeWindowMessageFilterFlags : uint
        {
            Add = 1, Remove = 2
        };

        /// <summary>
        /// 从用户界面特权隔离 (UIPI) 消息过滤器，添加或删除一条消息。
        /// </summary>
        /// <param name="msg">要从过滤器添加或删除的消息</param>
        /// <param name="flags">要执行的操作</param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern bool ChangeWindowMessageFilter(uint msg, ChangeWindowMessageFilterFlags flags);

        /// <summary>
        /// 通信结构
        /// </summary>
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

        /// <summary>
        /// 寻找窗体
        /// </summary>
        /// <param name="lpClassName">指针类名</param>
        /// <param name="lpWindowName">指向窗口的名字</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 消息发送
        /// </summary>
        /// <param name="hWnd">目标窗口的句柄</param>
        /// <param name="Msg">消息</param>
        /// <param name="wParam">第一个消息参数</param>
        /// <param name="lParam">第二个消息参数</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);

        /// <summary>
        /// 消息发送
        /// </summary>
        /// <param name="hWnd">目标窗口的句柄</param>
        /// <param name="Msg">消息</param>
        /// <param name="wParam">第一个消息参数</param>
        /// <param name="lParam">第二个消息参数</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        #region 窗体边框阴影效果变量申明
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);
        #endregion

        #region 控制窗体移动的WindowsAPI
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        #endregion
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);
    }
}
