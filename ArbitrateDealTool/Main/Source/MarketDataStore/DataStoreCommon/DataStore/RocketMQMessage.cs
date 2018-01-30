using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStoreCommon
{
    /// <summary>
    /// RocketMQ消息。
    /// </summary>
    internal class RocketMQMessage
    {
        public string body { get; set; }

        public string keys { get; set; }

        public string producerGroup { get; set; }

        public string tags { get; set; }

        public string topic { get; set; }
    }
}
