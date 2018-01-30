using System;
using System.Threading;

namespace USe.Common
{
    /// <summary>
    /// 普通号码创建者类
    /// </summary>
    public class CommonIdCreator
    {
        protected int m_current = 0;

        /// <summary>
        /// 获取下一个ID号码
        /// </summary>
        /// <returns>新的ID号码</returns>
        public int Next()
        {
            return Interlocked.Increment(ref m_current);
        }

        /// <summary>
        /// 判断某个ID号码是否由本类对象生成
        /// </summary>
        /// <param name="value">待判断的ID号码</param>
        /// <returns>似乎匹配</returns>
        public bool Match(int value)
        {
            return true;
        }

        /// <summary>
        /// 将当前Creator当前ID设置为指定值。
        /// </summary>
        /// <param name="currentID"></param>
        public void Set(int currentID)
        {
            Interlocked.Exchange(ref m_current, currentID);
        }
    }
}
