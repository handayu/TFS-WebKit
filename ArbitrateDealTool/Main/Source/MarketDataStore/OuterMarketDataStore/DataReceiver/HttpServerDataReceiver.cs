using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Diagnostics;
using USe.Common;
using USe.TradeDriver.Common;
using System.Collections.Concurrent;
using HttpServer;
using USe.Common.AppLogger;
using System;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace OuterMarketDataStore
{
    //public class HttpServerDataReceiver
    //{
    //    public void Start()
    //    {
    //        HttpListener httpListenner;
    //        httpListenner = new HttpListener();
    //        httpListenner.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
    //        httpListenner.Prefixes.Add("http://localhost:8080/");
    //        httpListenner.Start();
    //        new Thread(new ThreadStart(delegate
    //        {
    //            try
    //            {
    //                loop(httpListenner);
    //            }
    //            catch (Exception)
    //            {
    //                httpListenner.Stop();
    //            }
    //        })).Start();
    //    }

    //    private void loop(HttpListener httpListenner)
    //    {
    //        while (true)
    //        {
    //            try
    //            {
    //                HttpListenerContext context = httpListenner.GetContext();
    //                if (context == null) continue;
    //                HttpListenerRequest request = context.Request;
    //                HttpListenerResponse response = context.Response;
    //                Servlet servlet = new MyServlet();
    //                servlet.onCreate();
    //                if (request.HttpMethod == "POST")
    //                {
    //                    servlet.onPost(request, response);
    //                }
    //                else if (request.HttpMethod == "GET")
    //                {
    //                    servlet.onGet(request, response);
    //                }
    //                response.Close();
    //            }
    //            catch(Exception ex)
    //            {
    //                throw new Exception("Lisnter异常:" + ex.Message);
    //            }

    //        }
    //    }

    //    public void Stop()
    //    {

    //    }
    //}

    //public class Servlet
    //{
    //    public virtual void onGet(System.Net.HttpListenerRequest request, System.Net.HttpListenerResponse response) { }
    //    public virtual void onPost(System.Net.HttpListenerRequest request, System.Net.HttpListenerResponse response) { }
    //    public virtual void onCreate() { }
    //}

    //public class MyServlet : Servlet
    //{
    //    public override void onCreate()
    //    {
    //        base.onCreate();
    //    }
    //    public override void onGet(HttpListenerRequest request, HttpListenerResponse response)
    //    {
    //        Console.WriteLine("GET:" + request.Url);
    //        byte[] buffer = Encoding.UTF8.GetBytes("OK");
    //        System.IO.Stream output = response.OutputStream;
    //        output.Write(buffer, 0, buffer.Length);
    //        // You must close the output stream.             
    //        output.Close();
    //        //listener.Stop();         
    //    }

    //    public override void onPost(HttpListenerRequest request, HttpListenerResponse response)
    //    {
    //        Console.WriteLine("POST:" + request.Url);
    //        var sr = new StreamReader(response.OutputStream, Encoding.UTF8);
    //        var str=sr.ReadToEnd();
    //        sr.Close();


    //    }
    //}





    public class HttpServerDataReceiver : IUSeNotifier
    {
        #region event
        /// <summary>
        /// 通知事件。
        /// </summary>
        public event EventHandler<USeNotifyEventArgs> Notify; //接收的信息

        public delegate void OutLMEMarketDataReceiveHandel(USeMarketData marketData);
        public event OutLMEMarketDataReceiveHandel OutLMEMarketDataReceiveEvent;

        #endregion

        private Thread m_threadListener = null; //监听线程

        #region
        private int m_receiverCount = 0;
        private int m_instrumentCount = 0;
        private DateTime? m_lastMarketDataTime = null;
        private HttpReceiverSection m_config = null;
        private List<IMarketDataListener> m_listenerList = null;
        private Dictionary<string, USeMarketData> m_instrumentDic = new Dictionary<string, USeMarketData>();
        private Thread m_receiveThread = null;
        private IAppLogger m_eventLogger = null;
        #endregion

        HttpServer.HttpListener m_listener = HttpServer.HttpListener.Create(IPAddress.Any, 8088);

        /// <summary>
        /// 启动行情接收
        /// </summary>
        public void Start()
        {
            m_listener.RequestReceived += OnHttpMarketDataChanged;

            try
            {
                m_threadListener = new Thread(new ThreadStart(delegate
                {
                    try
                    {
                        m_eventLogger.WriteError("尝试启动监听...");

                        StartListner(m_listener);
                    }
                    catch (Exception ex)
                    {
                        m_eventLogger.WriteError("Http监听停止thread..." + ex.Message);
                        m_listener.Stop();
                    }
                }));

                m_threadListener.Start();
            }
            catch (Exception ex)
            {
                m_eventLogger.WriteError("Http监听停止..." + ex.Message);
            }


            m_eventLogger.WriteError("建立Http连接成功...");
        }

        private void StartListner(HttpServer.HttpListener httpListenner)
        {
            m_listener.Start(5);
            m_eventLogger.WriteError("启动Http监听成功...");

            string text = string.Format("时间:" + DateTime.Now.ToString() + "\n" + "  建立HttpServer,启动Http监听成功..");
            USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Warning, text);
            SafeRaiseNotifyEvent(this, notify);
        }

        private List<USeMarketData> ProcessRegionDataToMaretData(string readBody)
        {
            Debug.Assert(readBody != null && readBody != "");
            List<USeMarketData> marketDataList = new List<USeMarketData>();
            string[] marketDataArray = readBody.Split('|');
            foreach (string str in marketDataArray)
            {
                string[] strMarketData = str.Split(',');
                USeMarketData marketData = new USeMarketData();
                marketData.Instrument = new USeInstrument(strMarketData[0], "", USeMarket.LME);
                marketData.QuoteDay = DateTime.Now.Date;
                TimeSpan quoteTime;
                TimeSpan.TryParse(strMarketData[1], out quoteTime);
                marketData.UpdateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, quoteTime.Hours, quoteTime.Minutes, quoteTime.Milliseconds);
                marketData.QuoteTime = quoteTime;
                marketData.OpenPrice = Convert.ToDecimal(strMarketData[2]);
                marketData.HighPrice = Convert.ToDecimal(strMarketData[3]);
                marketData.LowPrice = Convert.ToDecimal(strMarketData[4]);
                marketData.ClosePrice = Convert.ToDecimal(strMarketData[5]);
                marketData.Volume = Convert.ToInt32(strMarketData[6]);
                marketData.BidPrice = Convert.ToDecimal(strMarketData[7]);
                marketData.AskPrice = Convert.ToDecimal(strMarketData[8]);

                marketDataList.Add(marketData);

                //m_instrumentDic[strMarketData[0]] = marketData;
            }

            return marketDataList;
        }

        /// <summary>
        /// 行情变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHttpMarketDataChanged(object sender, RequestEventArgs e)
        {
            StreamReader reader = new StreamReader(e.Request.Body);
            string readBody = reader.ReadToEnd();

            if (readBody == null || readBody == "") return;

            //原始接收的行情数据切割转换成UseMarketData形式存储;
            List<USeMarketData> marketDataList = ProcessRegionDataToMaretData(readBody);

            if (m_listenerList != null && m_listenerList.Count > 0)
            {
                foreach (USeMarketData marketData in marketDataList)
                {
                    try
                    {
                        SafeMarketDataEvent(marketData);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                    foreach (IMarketDataListener listener in m_listenerList)
                    {
                        try
                        {
                            listener.ReceiveMarketData(marketData);
                        }
                        catch (Exception ex)
                        {
                            Debug.Assert(false, ex.Message);
                        }
                    }

                    Interlocked.Increment(ref m_receiverCount);
                    m_lastMarketDataTime = marketData.UpdateTime;

                    m_eventLogger.WriteError(string.Format("时间:{0} 合约:{1}", marketData.UpdateTime, marketData.Instrument.InstrumentCode));

                }
            }
        }

        public void Stop()
        {
            try
            {
                m_listener.Stop();
            }
            catch(Exception ex)
            {
                string text = string.Format("时间:" + DateTime.Now.ToString() + "\n" + "监听停止异常" + ex.Message);
                USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Warning, text);
                SafeRaiseNotifyEvent(this, notify);
                m_eventLogger.WriteError(text);
            }

            while (true) 
            {
                try
                {
                    //停止行情接收线程
                    if (m_threadListener.ThreadState == System.Threading.ThreadState.Running || m_threadListener.IsAlive)
                    {
                        m_threadListener.Abort();
                    }
                    else
                    {
                        break;
                    }
                }
                catch(ThreadAbortException ex)
                {
                    string text = string.Format("时间:" + DateTime.Now.ToString() + "\n" + "监听线程停止异常" + ex.Message);
                    USeNotifyEventArgs notify = new USeNotifyEventArgs(USeNotifyLevel.Warning, text);
                    SafeRaiseNotifyEvent(this, notify);
                    m_eventLogger.WriteError(text);
                }

            }

        }

        #region construction
        public HttpServerDataReceiver(HttpReceiverSection config)
        {
            m_config = config;
            m_eventLogger = AppLogger.InitInstance();
            m_listenerList = new List<IMarketDataListener>();
        }
        #endregion

        #region property
        /// <summary>
        /// 合约数量。
        /// </summary>
        public int InstrumentCount
        {
            get { return m_instrumentCount = m_instrumentDic.Count; }
        }

        /// <summary>
        /// 接收数量。
        /// </summary>
        public int ReceiveCount
        {
            get { return m_receiverCount; }
        }

        /// <summary>
        /// 最后一笔行情时间。
        /// </summary>
        public DateTime? LastMarketDataTime
        {
            get { return m_lastMarketDataTime; }
        }
        #endregion

        /// <summary>
        /// 安全地发布指定的通知事件。
        /// </summary>
        /// <param name="sender">通知事件发送者对象。</param>
        /// <param name="e">通知事件参数对象。</param>
        protected void SafeRaiseNotifyEvent(object sender, USeNotifyEventArgs e)
        {
            EventHandler<USeNotifyEventArgs> handler = this.Notify;
            if (handler != null)
            {
                try
                {
                    handler(sender, e);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        /// <summary>
        /// 安全地发布指定的通知事件。
        /// </summary>
        /// <param name="sender">通知事件发送者对象。</param>
        /// <param name="e">通知事件参数对象。</param>
        protected void SafeMarketDataEvent(USeMarketData marketData)
        {
            OutLMEMarketDataReceiveHandel handler = this.OutLMEMarketDataReceiveEvent;
            if (handler != null)
            {
                try
                {
                    handler(marketData);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        public override string ToString()
        {
            return "HttpMarketDataReceiver";
        }

        /// <summary>
        /// 注册行情监听者。
        /// </summary>
        /// <param name="listener">行情监听者。</param>
        public void RegisterHttpMarketDataListener(IMarketDataListener listener)
        {
            m_listenerList.Add(listener);
        }

        /// <summary>
        /// 注销行情监听者。
        /// </summary>
        /// <param name="listener"></param>
        public void UnRegisterHttpMarketDataListener(IMarketDataListener listener)
        {
            for (int i = 0; i < m_listenerList.Count; i++)
            {
                if (m_listenerList[i] == listener)
                {
                    m_listenerList.RemoveAt(i);
                    break;
                }
            }
        }
    }
}