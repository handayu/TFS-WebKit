using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Timers;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Concurrent;

namespace UseOnlineTradingSystem
{
    #region 消息处理
    public class InterProcessService : CallBackBase, IDisposable
    {
        private CallBackFrm callBackFrm;

        public string HostName
        {
            set
            {
                callBackFrm.HostName = value;
            }
            get
            {
                return callBackFrm.HostName;
            }
        }

        public string ClientName
        {
            set
            {
                callBackFrm.ClientName = value;
            }
            get
            {
                return callBackFrm.ClientName;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public InterProcessService()
            : base()
        {
            callBackFrm = new CallBackFrm();
            callBackFrm.MessageReceived += new MessageReceivedHandler(Receive);
            connecting = true;
        }

        protected override void SendMessage(ProcessMessage msg)
        {
            callBackFrm.SendMessage(msg);
        }

        #region 垃圾清理
        private bool IsDisposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool Disposing)
        {
            if (!IsDisposed)
            {
                if (Disposing)
                {
                    //清理托管资源
                }
                //清理非托管资源
                isRun = false;
            }
            IsDisposed = true;
        }

        ~InterProcessService()
        {
            Dispose(false);
        }
        #endregion
    }
    #endregion

    #region 通信窗体
    /// <summary>
    /// 客户端回调窗体
    /// </summary>
    public class CallBackFrm : Form
    {
        private string clientName;
        private string hostName = "";

        /// <summary>
        /// 获取客户端名称
        /// </summary>
        public string ClientName
        {
            set { this.Text= clientName = value; }
            get { return clientName; }
            //get { return this.Handle.ToInt32(); }
        }

        /// <summary>
        /// 获取服务端名称
        /// </summary>
        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        public event MessageReceivedHandler MessageReceived;

        /// <summary>
        /// 消息名
        /// </summary>
        int WM_COPYDATA = 0x004A;


        /// <summary>
        /// 定义一个新的窗口消息
        /// </summary>
        /// <param name="lpString">（被注册）消息的名字</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "RegisterWindowMessageA")]
        private static extern int RegisterWindowMessage(string lpString);

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
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 消息发送
        /// </summary>
        /// <param name="hWnd">目标窗口的句柄</param>
        /// <param name="Msg">消息</param>
        /// <param name="wParam">第一个消息参数</param>
        /// <param name="lParam">第二个消息参数</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);

        /// <summary>
        /// 消息发送
        /// </summary>
        /// <param name="hWnd">目标窗口的句柄</param>
        /// <param name="Msg">消息</param>
        /// <param name="wParam">第一个消息参数</param>
        /// <param name="lParam">第二个消息参数</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 创建窗体
        /// </summary>
        public CallBackFrm()
        {
            InitializeComponent();
            this.clientName = Guid.NewGuid().ToString();
            this.hostName = "";

            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(1, 1);
            this.Location = new System.Drawing.Point(-1000000, -1000000);
            this.Text = clientName;
            this.Load += new EventHandler(ClientCallBackFrm_Load);
            this.Show();
            this.Hide();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientCallBackFrm_Load(object sender, EventArgs e)
        {
            try
            {
                ChangeWindowMessageFilter((uint)WM_COPYDATA, ChangeWindowMessageFilterFlags.Add);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息</param>
        public void SendMessage(ProcessMessage message)
        {
            try
            {
                message.SendPID = ClientName.ToString();
                string str = message.ToString();
                string ReceivePID = hostName;
                int WINDOW_HANDLER = 0;
                if (message.ReceivePID != null && message.ReceivePID.Length > 0)
                {
                    ReceivePID = message.ReceivePID;
                }
                WINDOW_HANDLER = FindWindow(null, ReceivePID);
                if (WINDOW_HANDLER != 0)
                {
                    IntPtr ptr = Marshal.StringToHGlobalUni(str);
                    COPYDATASTRUCT mystr = new COPYDATASTRUCT();
                    mystr.dwData = (IntPtr)59;
                    mystr.cbData = str.Length * 2;
                    mystr.lpData = ptr;
                    SendMessage(WINDOW_HANDLER, WM_COPYDATA, 0, ref mystr);
                    Marshal.FreeHGlobal(ptr);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="request">消息</param>
        private void ReceiveMessage(object request)
        {
            try
            {
                if (MessageReceived != null)
                {
                    ProcessMessage requestMessage = request as ProcessMessage;
                    if (requestMessage != null)
                    {
                        MessageReceived(requestMessage);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="m">消息</param>
        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == WM_COPYDATA)
            {
                try
                {
                    COPYDATASTRUCT mystr = new COPYDATASTRUCT();
                    mystr = (COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(COPYDATASTRUCT));
                    if (mystr.cbData > 0)
                    {
                        if (mystr.lpData != IntPtr.Zero)
                        {
                            int nLength = mystr.cbData/2;
                            string str = Marshal.PtrToStringUni(mystr.lpData, nLength);
                            if (str.Length > nLength)
                            {
                                str = str.Substring(0, nLength);
                            }
                            ProcessMessage requestMessage = new ProcessMessage();
                            requestMessage.ToClass(str);
                            ReceiveMessage(requestMessage);
                        }
                        else
                        {
                            ProcessMessage requestMessage = new ProcessMessage();
                            requestMessage.RequestString = mystr.cbData.ToString();
                            ReceiveMessage(requestMessage);
                        }
                    }
                }
                catch
                {

                }
            }
            else
            {
                base.DefWndProc(ref m);
            }
        }

        #region other
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ClientCallBackFrm
            // 
            this.ClientSize = new System.Drawing.Size(1, 1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.DoubleBuffered = false;
            this.ResumeLayout(false);
        }

        #endregion
    }
    #endregion

    public class CallBackBase
    {
        protected bool connecting = false;
        protected ConcurrentQueue<ProcessMessage> sendQueue;
        protected ConcurrentQueue<ProcessMessage> receiveQueue;
        //protected List<IServices> serviceList;
        protected bool isRun;

        public CallBackBase()
        {
            sendQueue = new ConcurrentQueue<ProcessMessage>();
            receiveQueue = new ConcurrentQueue<ProcessMessage>();
            //serviceList = new List<IServices>();

            isRun = true;

            Thread tdSend = new Thread(new ThreadStart(QueueSend));
            tdSend.IsBackground = true;
            tdSend.Start();
            tdSend = null;

            Thread tdReceive = new Thread(new ThreadStart(QueueReceive));
            tdReceive.IsBackground = true;
            tdReceive.Start();
            tdReceive = null;

        }

        /// <summary>
        /// 事件触发：收到一条消息来自服务器的管道消息
        /// </summary>
        public event MessageReceivedHandler MessageReceived;

        ///// <summary>
        ///// 添加服务
        ///// </summary>
        ///// <param name="service"></param>
        ///// <returns></returns>
        //public bool AddService(IServices service)
        //{
        //    lock (service)
        //    {
        //        if (!serviceList.Contains(service))
        //        {
        //            serviceList.Add(service);
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 去除服务
        ///// </summary>
        ///// <param name="service"></param>
        ///// <returns></returns>
        //public bool RemoveService(IServices service)
        //{
        //    lock (service)
        //    {
        //        if (serviceList.Contains(service))
        //        {
        //            serviceList.Remove(service);
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        /// <summary>
        /// 发送队列中的消息
        /// </summary>
        protected void QueueSend()
        {
            while (isRun)
            {
                if (connecting && sendQueue.Count > 0)
                {
                    ProcessMessage msg;
                    if (sendQueue.TryDequeue(out msg))
                    {
                        SendMessage(msg);
                    }
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 接收队列中的消息
        /// </summary>
        protected void QueueReceive()
        {
            while (isRun)
            {
                if (connecting && receiveQueue.Count > 0)
                {
                    // 事件不为为空时
                    if (MessageReceived != null)
                    {
                        ProcessMessage msg;
                        if (receiveQueue.TryDequeue(out msg))
                        {
                            MessageReceived(msg);
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 状态回调
        /// </summary>
        /// <param name="rs">验证结果</param>
        protected void Receive(ProcessMessage rs)
        {
            receiveQueue.Enqueue(rs);
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="db">请求内容</param>
        public void Send(ProcessMessage db)
        {
            sendQueue.Enqueue(db);
        }

        protected virtual void SendMessage(ProcessMessage msg)
        {

        }

        ///// <summary>
        ///// 开始服务
        ///// </summary>
        //protected virtual void Start()
        //{
        //    foreach (IServices service in serviceList)
        //    {
        //        service.Start();
        //    }
        //}
    }

    public delegate void DelegateSendMessage(ProcessMessage message);
    public delegate void MessageReceivedHandler(ProcessMessage message);

    /// <summary>
    /// 进程间通信数据结构
    /// </summary>
    public class ProcessMessage
    {
        private string sendPID; //标识发送端
        private string receivePID;//标识接收端
        private string serviceName = "empty";//服务名称
        private string requestType = "empty"; //标识请求类型
        private string requestString = "empty"; //请求内容
        private int requestID = 0; //请求编号
        private string filter = "";
        private byte[] buff;
        private object tag;

        public byte[] Buff
        {
            get { return buff; }
            set { buff = value; }
        }
        /// <summary>
        /// 过滤器
        /// </summary>
        public string Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessMessage()
        {
        }

        /// <summary>
        /// 获取或设置标识发送端
        /// </summary>
        public string SendPID
        {
            get { return sendPID; }
            set { sendPID = value; }
        }

        /// <summary>
        /// 获取或设置接收端
        /// </summary>
        public string ReceivePID
        {
            get { return receivePID; }
            set { receivePID = value; }
        }

        /// <summary>
        /// 获取或设置服务名称
        /// </summary>
        public string ServiceName
        {
            get { return serviceName; }
            set { serviceName = value; }
        }

        /// <summary>
        /// 获取或设置标识请求类型
        /// </summary>
        public string RequestType
        {
            get { return requestType; }
            set { requestType = value; }
        }

        /// <summary>
        /// 获取或设置请求内容
        /// </summary>
        public string RequestString
        {
            get { return requestString; }
            set { requestString = value; }
        }

        /// <summary>
        /// 获取或设置请求编号
        /// </summary>
        public int RequestID
        {
            get { return requestID; }
            set { requestID = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(filter);
            sb.Append("\r\n");
            sb.Append(sendPID);
            sb.Append("\r\n");
            sb.Append(receivePID);
            sb.Append("\r\n");
            sb.Append(requestID);
            sb.Append("\r\n");
            sb.Append(serviceName);
            sb.Append("\r\n");
            sb.Append(requestType);
            sb.Append("\r\n");
            sb.Append(requestString);
            return sb.ToString();
        }

        public void ToClass(string str)
        {
            int begin = 0;
            int index0 = str.IndexOf("\r\n", begin);
            this.filter = str.Substring(begin, index0);
            begin = index0 + "\r\n".Length;

            int index1 = str.IndexOf("\r\n", begin);
            this.sendPID = str.Substring(begin, index1 - index0 - "\r\n".Length);
            begin = index1 + "\r\n".Length;

            int index2 = str.IndexOf("\r\n", begin);
            this.receivePID = str.Substring(begin, index2 - index1 - "\r\n".Length);
            begin = index2 + "\r\n".Length;

            int index3 = str.IndexOf("\r\n", begin);
            string temp = str.Substring(begin, index3 - index2 - "\r\n".Length);
            begin = index3 + "\r\n".Length;
            int rsID;
            Int32.TryParse(temp, out rsID);
            this.requestID = rsID; //请求编号

            int index4 = str.IndexOf("\r\n", begin);
            this.serviceName = str.Substring(begin, index4 - index3 - "\r\n".Length);
            begin = index4 + "\r\n".Length;

            int index5 = str.IndexOf("\r\n", begin);
            this.requestType = str.Substring(begin, index5 - index4 - "\r\n".Length);

            begin = index5 + "\r\n".Length;
            this.requestString = str.Substring(begin);
        }

        /// <summary>
        /// 获取响应对象
        /// </summary>
        /// <returns>响应对象</returns>
        public ProcessMessage Response()
        {
            ProcessMessage message = new ProcessMessage();
            message.sendPID = this.receivePID;
            message.receivePID = this.sendPID;
            message.serviceName = this.serviceName;
            message.requestType = this.requestType;
            message.requestID = this.requestID;
            message.requestString = this.requestString; //请求内容
            message.filter = this.filter;
            message.buff = this.buff;
            message.tag = this.tag;
            return message;
        }
    }
}
