using System.Net;

namespace USe.Common.HttpJson
{
    public class RequestHeader
    {
        public RequestHeader(string headerKey, string value)
        {
            this.HeaderKey = headerKey;
            this.Value = value;
            this.KeyIsEnum = false;
        }

        public RequestHeader(HttpRequestHeader header, string value)
        {
            this.Header = header;
            this.Value = value;
            this.KeyIsEnum = true;
        }

        public bool KeyIsEnum
        {
            private set;
            get;
        }

        public string HeaderKey
        {
            get;
            set;
        }

        public HttpRequestHeader Header
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }
    }
}
