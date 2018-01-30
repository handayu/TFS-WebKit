using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.Common.HttpJson
{
    /// <summary>
    /// Http请求头。
    /// </summary>
    public class HttpHeader
    {
        public string ContentType { get; set; }

        public long ContentLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Accept { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserAgent { get; set; }

        public string Method { get; set; }

        public int MaxTry { get; set; }

        public Encoding Encoding { get; set; }

        public string Referer { get; set; }

        public string Param { get; set; }

        public bool UseAutomaticDecompression { get; set; }

        public static HttpHeader DefaultGetHeader
        {
            get
            {
                HttpHeader header = new HttpHeader();
                //header.ContentType = "application/x-www-form-urlencoded";
                header.ContentType = "application/json";
                header.ContentLength = 0;
                header.Accept = string.Empty;
                header.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Trident/7.0; rv:11.0) like Gecko";
                header.Method = "GET";
                header.MaxTry = 5;
                header.Encoding = Encoding.UTF8;
                header.Referer = string.Empty;
                header.Param = string.Empty;
                header.UseAutomaticDecompression = false;

                return header;
            }
        }

        public static HttpHeader DefaultPostHeader
        {
            get
            {
                HttpHeader header = new HttpHeader();
                header.ContentType = "application/json";
                header.ContentLength = 0;
                header.Accept = string.Empty;
                header.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Trident/7.0; rv:11.0) like Gecko";
                header.Method = "POST";
                header.MaxTry = 5;
                header.Encoding = Encoding.UTF8;
                header.Referer = string.Empty;
                header.Param = string.Empty;
                header.UseAutomaticDecompression = false;

                return header;
            }
        }
    }
}
