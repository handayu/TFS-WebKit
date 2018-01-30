using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xilium.CefGlue;

namespace UseOnlineTradingSystem
{
    class BSCookieTask : CefTask
    {
        private CefCookie cookie;
        private string url;
        public BSCookieTask(string url, CefCookie newCookie)
        {
            this.cookie = newCookie;
            this.url = url;
        }

        protected override void Execute()
        {
            
            bool tmp = CefCookieManager.GetGlobal(null).SetCookie(url, cookie,null);
            bool temp = CefCookieManager.GetGlobal(null).VisitUrlCookies(url, false, new CefWebCookieVisitor(cookie));
        }
    }
    public class CefWebCookieVisitor : CefCookieVisitor
    {
        private CefCookie _cefCookie;

        public CefWebCookieVisitor(CefCookie cookie)
        {
            _cefCookie = cookie;
        }
        protected override bool Visit(CefCookie cookie, int count, int total, out bool delete)
        {

            delete = false;
            return false;

        }

    }
}
