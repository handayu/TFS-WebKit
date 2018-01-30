using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace USe.Common.HttpJson
{
    /// <summary>
    /// Http Json数据访问器。
    /// </summary>
    public class HttpJsonDataVistor
    {
        #region member
        private CookieContainer m_cookieContainer = null;
        #endregion // member

        #region construction
        public HttpJsonDataVistor()
        {
            m_cookieContainer = new CookieContainer();
        }
        #endregion // construction

        #region public methods
        /// <summary>
        /// 获取Html数据。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public JsonData GetJsonData(string url)
        {
            return GetJsonData(url, HttpHeader.DefaultGetHeader);
        }

        /// <summary>
        /// 获取Html数据。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public JsonData POSTJsonData(string url)
        {
            return GetJsonData(url, HttpHeader.DefaultPostHeader);
        }

        /// <summary>
        /// 获取网页数据
        /// </summary>
        /// <param name="url">网站地址</param>
        /// <param name="encoding">编码</param>
        /// <param name="method">发送方法</param>        
        public JsonData GetJsonData(string url, HttpHeader header)
        {
            return GetJsonData(url, header, null);
        }

        /// <summary>
        /// 获取网页数据
        /// </summary>
        /// <param name="url">网站地址</param>
        /// <param name="encoding">编码</param>
        /// <param name="method">发送方法</param>        
        public JsonData GetJsonData(string url, HttpHeader header, List<RequestHeader> requestHeaderList)
        {
            HttpWebRequest request;
            HttpWebResponse response;

            request = WebRequest.Create(url) as HttpWebRequest;

            request.Method = header.Method;
            if (header.UseAutomaticDecompression)
            {
                request.AutomaticDecompression = DecompressionMethods.GZip;
            }
            if (!string.IsNullOrEmpty(header.ContentType))
            {
                request.ContentType = header.ContentType;
            }
            if (header.ContentLength >= 0)
            {
                request.ContentLength = header.ContentLength;
            }
            request.KeepAlive = true;
            if (m_cookieContainer != null)
            {
                request.CookieContainer = m_cookieContainer;
            }
            if (header.MaxTry > 0)
            {
                request.ServicePoint.ConnectionLimit = header.MaxTry;
            }
            if (string.IsNullOrEmpty(header.Accept) == false)
            {
                request.Accept = header.Accept;
            }
            if (string.IsNullOrEmpty(header.UserAgent) == false)
            {
                request.UserAgent = header.UserAgent;
            }
            if (string.IsNullOrEmpty(header.Referer) == false)
            {
                request.Referer = header.Referer;
            }
            if (requestHeaderList != null && requestHeaderList.Count > 0)
            {
                foreach (RequestHeader requestHeader in requestHeaderList)
                {
                    if (requestHeader.KeyIsEnum)
                    {
                        request.Headers.Add(requestHeader.Header, requestHeader.Value);


                    }
                    else
                    {
                        request.Headers.Add(requestHeader.HeaderKey, requestHeader.Value);
                    }
                }
            }

            if (string.IsNullOrEmpty(header.Param) == false)
            {
                byte[] paramData = header.Encoding.GetBytes(header.Param);
                request.ContentLength = paramData.Length;
                using (Stream sm = request.GetRequestStream())
                {
                    sm.Write(paramData, 0, paramData.Length);
                    sm.Close();
                }
            }

            try
            {
                //获取服务器返回的资源   
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    JsonData jsonData = new JsonData();
                    jsonData.StatusCode = response.StatusCode;
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), header.Encoding))
                    {
                        jsonData.JsonString = reader.ReadToEnd();
                    }

                    return jsonData;
                }
            }
            catch (WebException e)
            {
                using (WebResponse response1 = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response1;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response1.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        JsonData jsonData = new JsonData();
                        jsonData.StatusCode = httpResponse.StatusCode;
                        jsonData.JsonString = reader.ReadToEnd();

                        return jsonData;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
    }
}
