using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Xilium.CefGlue;

namespace UseOnlineTradingSystem
{
    class BsLoadHandler : CefLoadHandler
    {
        private Control control;
        private string type;
        //private int state = 0;
        public BsLoadHandler(string t, Control c)
        {
           type=t;
           control = c;
        }
        protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
        {
            if (type == "login")
            {
                //// 网页登录
                //if (DataManager.Instance.loginType == 0)
                //{
                //    if (state == 0)
                //    {
                //        var script = "var bObj = document.getElementsByTagName('span')[2]; bObj.onclick= function(){exampleA.setAccount(document.getElementsByTagName('input')[0].value,document.getElementsByTagName('input')[1].value);};";
                //        script += "document.getElementsByTagName('header')[0].setAttribute('style', 'display: none');";
                //        script += "document.getElementsByTagName('footer')[0].setAttribute('style', 'display: none');";
                //        script += "document.getElementsByTagName('body')[0].setAttribute('style', 'overflow:-Scroll;overflow-y:hidden;overflow:-Scroll;overflow-x:hidden;');";
                //        script += "var tags=document.getElementsByTagName('div'); for(var i in tags){if (tags[i].getAttribute('class') == 'sign-left') tags[i].setAttribute('style', 'display: none'); else if (tags[i].getAttribute('class') == 'sign-right') tags[i].setAttribute('style', 'float:left; margin: 0px -100px 0px 0px;');}";
                //        frame.ExecuteJavaScript(script, frame.Url, 0);
                //        state = 1;
                //    }
                //    else if (state == 1)
                //    {

                //        Form fm = control as Form;
                //        if (fm != null)
                //        {
                //            fm.BeginInvoke(new Action(() => {
                //                //fm.Location = new System.Drawing.Point(-1000000, -1000000);
                //                //MessageBox.Show("登录成功！");
                //                fm.Hide();
                //                //发送登录成功消息
                //                ProcessMessage pm = new ProcessMessage();
                //                pm.ReceivePID = "mainformCallBack";
                //                pm.RequestString = "IsLogin";
                //                pm.RequestType = "IsLogin";
                //                USeManager.Instance.MCallBack.Send(pm);
                //            }));
                //        }

                //        state = 0;
                //    }
                //}
            }
            else if (type == "capital")
            {
                //Thread.Sleep(1000);
                //string script = "var tags=document.getElementsByTagName('div'); for(var i in tags){if (tags[i].getAttribute('class') == 'page-home') tags[i].setAttribute('style', 'display: none'); else if (tags[i].getAttribute('class') == 'header-nav') tags[i].setAttribute('style', 'display: none');}";
                //frame.ExecuteJavaScript(script, frame.Url, 0);
                //script = @" var arr = document.cookie.split(';');
                //            if(arr.length>1)
                //                {
                //                    var temp = arr[1].split('=');
                //                     if(temp.length>1)
                //                        {
                //                            exampleA.setCookies(temp[1]);
                //                        }
                //                   }";
                //// 网页登录
                //if (DataManager.Instance.loginType == 0)
                //{
                //    string script = " exampleA.setCookies(unescape(document.cookie));";
                //    frame.ExecuteJavaScript(script, frame.Url, 0);
                //}
                //script = "var d= document.getElementsByTagName('div'); if(d[d.length-1].getAttribute('class') == 'pop-success'){ var vv = document.getElementsByTagName('span'); vv[vv.length-1].click();}";
                //frame.ExecuteJavaScript(script, frame.Url, 0);
                //Thread.Sleep(1000);
                //if (DataManager.Instance.Cookies != null)
                //{
                //    string cookies = string.Format("SecurityToken={0};SecurityTicket={1}", DataManager.Instance.Cookies, DataManager.Instance.Cookies);
                //    script = " document.cookie="+ System.Web.HttpUtility.HtmlEncode(cookies) +";";
                //    frame.ExecuteJavaScript(script, frame.Url, 0);
                //}
                //Thread td = new Thread(new ParameterizedThreadStart(xxx));
                //td.Start(frame);
            }
            //state++;
        }
        protected override void OnLoadingStateChange(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward)
        {
            //System.Windows.Forms.MessageBox.Show("2");
        }
        protected override void OnLoadStart(CefBrowser browser, CefFrame frame, CefTransitionType transitionType)
        {
            //System.Windows.Forms.MessageBox.Show("1");
            //state++;
        }
    }
}
