using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Xilium.CefGlue;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 绑定测试处理器
    /// </summary>
    public class BsAv8Handler : CefV8Handler
    {
        #region 声明常量变量

        /// <summary>
        /// 内容
        /// </summary>
        public  string MyParam { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// Cookies
        /// </summary>
        public string Cookies { get; set; }

        #endregion 声明常量变量

        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BsAv8Handler()
        {
            MyParam = "";
        }

        #endregion 构造函数

        #region 事件

        /// <summary>
        /// 网页脚本与后台程序交互方法
        /// 提示一：如果 returnValue = null; 则会导致网页前端出现错误：Cannot read property 'constructor' of undefined
        /// 提示二：还存在其他的可能，导致导致网页前端出现错误：Cannot read property 'constructor' of undefined
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="obj">对象</param>
        /// <param name="arguments">参数</param>
        /// <param name="returnValue">返回值</param>
        /// <param name="exception">返回异常信息</param>
        /// <returns></returns>
        protected override bool Execute(string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
        {
            string result = string.Empty;

            switch (name)
            {
                case "MyFunction":
                    MyFunction();
                    break;
                case "GetMyParam":
                    result = GetMyParam();
                    break;
                case "SetMyParam":
                    result = SetMyParam(arguments[0].GetStringValue());
                    break;
                case "SetAccount":
                    SetAccount(arguments[0].GetStringValue(), arguments[1].GetStringValue());
                    break;
                case "SetCookies":
                    SetCookies(arguments[0].GetStringValue());
                    break;
                default:
                    MessageBox.Show(string.Format("JS调用C# >> {0} >> {1} 返回值", name, obj.GetType()), "系统提示", MessageBoxButtons.OK);
                    break;
            }

            returnValue = CefV8Value.CreateString(result);
            exception = null;

            return true;
        }

        #endregion 事件

        #region 方法

        /// <summary>
        /// 我的函数
        /// </summary>
        public void MyFunction()
        {
            MessageBox.Show("ExampleAv8Handler : JS调用C# >> MyFunction >> 无 返回值", "系统提示", MessageBoxButtons.OK);
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <returns></returns>
        public string GetMyParam()
        {
            return MyParam;
        }

        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public string SetMyParam(string value)
        {
            MyParam = value;
            return MyParam;
        }

        /// <summary>
        /// 设置帐号
        /// </summary>
        /// <param name="account"></param>
        public void SetAccount(string account, string password)
        {
            //ProcessMessage pm = new ProcessMessage();
            //pm.ReceivePID = "mainformCallBack";
            //pm.RequestString = string.Format("{0},{1}", account, password);
            //pm.RequestType = "AccountAndPassword";
            //USeManager.Instance.MCallBack.Send(pm);
            //Account = account;
            //PassWord = password;
            //MessageBox.Show(Account + ',' + PassWord);
        }

        /// <summary>
        /// 设置cookies
        /// </summary>
        /// <param name="cookies"></param>
        public void SetCookies(string cookies)
        {
            //if (!string.IsNullOrWhiteSpace(cookies))
            //{
            //    //ProcessMessage pm = new ProcessMessage();
            //    //pm.ReceivePID = "mainformCallBack";
            //    //pm.RequestString = cookies;
            //    //pm.RequestType = "Cookies";
            //    //USeManager.Instance.MCallBack.Send(pm);
            //    //Cookies = cookies;
            //    //MessageBox.Show(cookies);
            //}
        }

        #endregion 方法
    }
}