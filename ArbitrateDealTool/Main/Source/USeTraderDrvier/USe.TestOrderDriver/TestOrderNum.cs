using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;

namespace USe.TradeDriver.Test
{
    public class TestOrderNum :USeOrderNum
    {
        private int m_orderNum = 0;
        public TestOrderNum()
        {

        }
        public TestOrderNum(int num)
        {
            this.OrderNum = num;
        }
        public int OrderNum
        {
            get { return m_orderNum; }
            set { m_orderNum = value; }
        }
        /// <summary>
        /// 委托单号字符串描述。
        /// </summary>
        public override string OrderString
        {
            get { return m_orderNum.ToString(); }
        }

        /// <summary>
        /// 克隆USeOrderNum。
        /// </summary>
        /// <returns></returns>
        public override USeOrderNum Clone()
        {
            TestOrderNum orderNum = new TestOrderNum();
            orderNum.OrderNum = this.OrderNum;

            return orderNum;
        }

        public override string ToString()
        {
            return m_orderNum.ToString();
        }
    }
}
