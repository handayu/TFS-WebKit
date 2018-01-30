#region Copyright & Version
//==============================================================================
// 文件名称: USeResetEvent.cs
// 
//   Version 1.0.0.0
// 
//   Copyright(c) 2012 Shanghai MyWealth Investment Management LTD.
// 
// 创 建 人: Yang Ming
// 创建日期: 2012/04/12
// 描    述: 自定义ResetEvent(主要用于同步CTP查询，CTP查询为异步并且分条返回)。
//==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Threading;

namespace USe.TradeDriver.Common
{
    /// <summary>
    /// 自定义ResetEvent。
    /// </summary>
    public class USeResetEvent
    {
        #region member
        private AutoResetEvent m_event = null;

        private int m_eventID = 0;
        private object m_tag = null;
        private bool m_isError = false;
        private bool m_isFinish = false;

        #endregion // member

        #region construction
        /// <summary>
        /// 构造USeResetEvent实例。
        /// </summary>
        public USeResetEvent()
            : this(0)
        {
        }

        /// <summary>
        /// 构造USeResetEvent实例。
        /// </summary>
        /// <param name="eventID">Event ID。</param>
        public USeResetEvent(int eventID)
        {
            m_eventID = eventID;
            m_event = new AutoResetEvent(false);
        }
        #endregion // construction

        #region property
        /// <summary>
        /// Event ID。
        /// </summary>
        public int EventID
        {
            get { return m_eventID; }
        }

        /// <summary>
        /// Tag。
        /// </summary>
        public object Tag
        {
            get { return m_tag; }
            set { m_tag = value; }
        }

        /// <summary>
        /// 是否结束。
        /// </summary>
        public bool IsFinish
        {
            get { return m_isFinish; }
            set { m_isFinish = value; }
        }

        /// <summary>
        /// 是否有错。
        /// </summary>
        public bool IsError
        {
            get { return m_isError; }
        }
        #endregion // property

        #region methods
        public void Close()
        {
            m_event.Close();
        }

        public bool Reset()
        {
            return m_event.Reset();
        }

        public bool Set()
        {
            return Set(false);
        }

        public bool Set(bool isError)
        {
            m_isError = isError;
            return m_event.Set();
        }

        public bool WaitOne()
        {
            return m_event.WaitOne();
        }

        public bool WaitOne(int millisecondsTimeout)
        {
            return m_event.WaitOne(millisecondsTimeout);
        }

        public bool WaitOne(System.TimeSpan timeout)
        {
            return m_event.WaitOne(timeout, false);
        }

        public bool WaitOne(int millisecondsTimeout, bool exitContext)
        {
            return m_event.WaitOne(millisecondsTimeout, exitContext);
        }

        public bool WaitOne(System.TimeSpan timeout, bool exitContext)
        {
            return m_event.WaitOne(timeout, exitContext);
        }

        public void Clear()
        {
            m_tag = null;
            m_isError = false;
        }
        #endregion // methods
    }
}