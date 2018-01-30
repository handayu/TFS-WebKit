using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
//using System.Web;
using System.IO;
using System.Windows.Forms;
using UseHttpHelper;
using System.Diagnostics;
using System.Web;

namespace UseOnlineTradingSystem
{
    public class HttpService
    {
        /// <summary>
        /// post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookies"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static string PostData(string url, string cookies, string data, string contentType = "application/json;charset=UTF-8")
        {
            Logger.LogInfo("POST数据,链接："+ url);
            HttpHelper hh = new HttpHelper();
            HttpItem hi = new HttpItem();
            hi.Encoding = Encoding.UTF8;
            hi.Method = "POST";
            hi.URL = url;
            if (cookies != null)
            {
                hi.Header.Add("Security-Token", cookies);
            }
            hi.ContentType = contentType;
            if (data != null)
            {
                hi.Postdata = data;
                hi.PostDataType = UseHttpHelper.Enum.PostDataType.String;
                hi.PostEncoding = Encoding.UTF8;
            }
            HttpResult hr = hh.GetHtml(hi);
            if (hr != null)
            {
                if (DataManager.Instance.IsLogin && hr.Html.Contains("未登录"))
                {
                    Logger.LogInfo("登录已过期，请重新登录！");
                    MessageBox.Show("登录已过期，请重新登录！");
                    DataManager.Instance.LoginOff();
                    return null;
                }
                else
                {
                    return hr.Html;
                }
            }
            return null;
        }

        /// <summary>
        /// get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        private static string GetData(string url, string cookies)
        {
            Logger.LogInfo("GET数据,链接：" + url);
            HttpHelper hh = new HttpHelper();
            HttpItem hi = new HttpItem();
            hi.Encoding = Encoding.UTF8;
            hi.URL = url;
            if (cookies != null)
            {
                hi.Header.Add("Security-Token", cookies);
            }
            HttpResult hr = hh.GetHtml(hi);
            if (hr != null)
            {
                if (DataManager.Instance.IsLogin&&hr.Html.Contains("未登录"))
                {
                    Logger.LogInfo("登录已过期，请重新登录！");
                    MessageBox.Show("登录已过期，请重新登录！");
                    DataManager.Instance.LoginOff();
                    return null;
                }
                else
                {
                    return hr.Html;
                }
            }
            return null;
        }

        /// <summary>
        /// 挂牌
        /// </summary>
        public static ListedResponse PostBrandOrder(ListedRequest require)
        {
            Logger.LogInfo("HTTP挂牌请求");
            if (DataManager.Instance.Cookies != null)
            {
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.PostBrandOrderRequireInfoUrl);
                string data = Helper.Serialize(require);
                string hr = PostData(url, cookies, data);
                if (hr != null)
                {
                    try
                    {
                        var bi = Helper.Deserialize<ListedResponse>(hr);
                        return bi;
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 摘牌
        /// </summary>
        public static DelistingResponse PostDelistBrandOrder(DelistingRequest require)
        {
            if (DataManager.Instance.Cookies != null)
            {
                string cookies = DataManager.Instance.Cookies;
                //组装成URL后缀请求
                ////string str = "mqID=" + require.mqId + "&";
                //string str = "clientId=" + require.clientId + "&";
                //str = str + "commId=" + require.commId.ToString() + "&";
                ////str = str + "cid=" + require.cid + "&";
                //str = str + "operationType=" + require.operationType + "&";
                //str = str + "basePrice=" + require.basePrice + "&";
                //str = str + "commQuantity=" + require.commQuantity;// + "&";
                ////str = str + "remarks=" + require.remarks;
                ////string str = "mqID=" + require.mqId + "&";
                //string url = Helper.GetURL(HTTPServiceUrlCollection.PostDelistBrandOrderRequireInfoUrl) + "?" + HttpUtility.UrlEncode(str);
                //string hr = PostData(url, cookies, null, "application/json");

                //string str = "clientId=" + require.clientId + "&";
                //str = str + "commId=" + require.commId.ToString() + "&";
                //str = str + "operationType=" + require.operationType + "&";
                //str = str + "basePrice=" + require.basePrice + "&";
                //str = str + "commQuantity=" + require.commQuantity;

                string str = "{\"clientId\" :\"" + require.clientId + "\",";
                str = str + "\"commId\" :\"" + require.commId.ToString() + "\",";
                str = str + "\"operationType\" :\"" + require.operationType + "\",";
                str = str + "\"basePrice\" :\"" + require.basePrice + "\",";
                str = str + "\"commQuantity\" :\"" + require.commQuantity + "\"}";

                string url = Helper.GetURL(HTTPServiceUrlCollection.PostDelistBrandOrderRequireInfoUrl);
                string hr = PostData(url, cookies, str, "application/json");

                if (hr != null)
                {
                    try
                    {
                        var bi = Helper.Deserialize<DelistingResponse>(hr);
                        return bi;
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 撤单
        /// </summary>
        public static CancelOrderResponse PostActionBrandOrder(CancelOrderRequest require)
        {
            if (DataManager.Instance.Cookies != null)
            {
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.PostActionBrandOrderRequireInfoUrl);
                string data = Helper.Serialize(require);
                string hr = PostData(url, cookies, data);
                if (hr != null)
                {
                    try
                    {
                        var bi = Helper.Deserialize<CancelOrderResponse>(hr);
                        return bi;
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 核销
        /// </summary>
        public static WriteOffResponse PostDelistBrandOrderVerify(WriteOffRequest require)
        {
            if (DataManager.Instance.Cookies != null)
            {
                string cookies = DataManager.Instance.Cookies;
                string str = "mqID=" + require.mqId + "&";
                str = str + "clientId=" + require.clientId + "&";
                str = str + "commId=" + require.commId + "&";
                str = str + "operationType=" + require.operationType + "&";
                str = str + "confirmPrice=" + require.confirmPrice + "&";
                str = str + "orderNo=" + require.orderNo + "&";
                str = str + "remarks=" + require.remarks;
                string url = Helper.GetURL(HTTPServiceUrlCollection.PostDelistBrandOrderVerifyRequireInfoUrl);
                string hr = PostData(url, cookies, str, "application/x-www-form-urlencoded");
                if (hr != null)
                {
                    try
                    {
                        var bi = Helper.Deserialize<WriteOffResponse>(hr);
                        return bi;
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 授信额度查询
        /// </summary>
        public static CreditQuotaResponse GetDelistAditQuatity(CreditQuotaRequest require)
        {
            if (DataManager.Instance.Cookies != null)
            {
                string str = "compId=" + require.compId;
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.GetDelistReditQuality) + str;
                string hr = GetData(url, cookies);
                if (hr != null)
                {
                    try
                    {
                        var bi = Helper.Deserialize<CreditQuotaResponse>(hr);
                        return bi;
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 查询等级和品牌信息
        /// </summary>
        /// <param name="nowProcessId"></param>
        /// <returns></returns>
        public static LevelBrandResponse GetBaseLevelBrandInfo(string nowProcessId = null)
        {
            if (DataManager.Instance.Cookies != null)
            {
                string url = Helper.GetURL(HTTPServiceUrlCollection.GetBaseLevelBrandInfoUrl);
                if (nowProcessId != null)
                {
                    url = url + "cid=" + nowProcessId;
                }
                string cookies = DataManager.Instance.Cookies;
                string hr = GetData(url, cookies);
                if (hr != null)
                {
                    try
                    {
                        var bi = Helper.Deserialize<LevelBrandResponse>(hr);
                        return bi;
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 查询仓库信息
        /// </summary>
        /// <param name="nowProcessId"></param>
        /// <returns></returns>
        public static WareHouseResponse GetWareHouseInfo(string nowProcessId)
        {
            string url = Helper.GetURL(HTTPServiceUrlCollection.GetBaseWareHouseInfoUrl) + "?cid=" + nowProcessId;
            string cookies = DataManager.Instance.Cookies;
            string hr = GetData(url, cookies);
            if (hr != null)
            {
                try
                {
                    var bi = Helper.Deserialize<WareHouseResponse>(hr);
                    return bi;
                }
                catch (Exception err)
                {
                    Logger.LogError(err.ToString());
                }
            }
            return null;
        }

        /// <summary>
        /// 获取个人信息
        /// </sum mary>
        public static MineResponse GetMine()
        {
            if (DataManager.Instance.Cookies != null)
            {
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.GetPersonalInformation) /*+ "?Security-Token=" + HttpUtility.UrlEncode(cookies)*/;
                string hr = GetData(url, cookies);
                if (hr != null)
                {
                    try
                    {
                        var bi = Helper.Deserialize<MineResponse>(hr);
                        return bi;
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 查询摘牌列表
        /// </summary>
        public static void GetDelisting()
        {
            //if (DataManager.Instance.Cookies != null)
            //{
            //    string code = DataManager.Instance.Cookies;
            //    //获取行情列表
            //    HttpHelper hh = new HttpHelper();
            //    HttpItem hi = new HttpItem();
            //    hi.Encoding = Encoding.UTF8;
            //    hi.URL = Helper.GetDescription(HTTPServiceUrlCollection.GetDelistTradedListInfoUrl);
            //    hi.Header.Add("Security-Token", code);
            //    HttpResult hr = hh.GetHtml(hi);
            //    if (hr != null)
            //    {
            //        try
            //        {
            //            var bi = Helper.Deserialize<DelistList>(hr.Html);
            //            if (/*bi.Success*/bi != null && bi.data != null)
            //            {
            //                Delist = bi.data.dataList;
            //            }
            //        }
            //        catch (Exception err)
            //        {
            //            Logger.LogError(err.ToString());
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 查询成交列表
        /// </summary>
        public static TransactionList GetTradedlisting()
        {
            if (DataManager.Instance.Cookies != null)
            {
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.GetDelistTradedListInfoUrl);
                string hr = GetData(url, cookies);
                if (hr != null)
                {
                    try
                    {
                        var bi = Helper.Deserialize<TransactionList>(hr);
                        return bi;
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 查询挂牌列表
        /// </summary>
        public static AllListedResponse GetList()
        {
            string cookies = DataManager.Instance.Cookies;
            string url = Helper.GetURL(HTTPServiceUrlCollection.GetList);
            string hr = GetData(url, cookies);
            if (hr != null)
            {
                try
                {
                    var bi = Helper.Deserialize<AllListedResponse>(hr);
                    return bi;
                }
                catch (Exception err)
                {
                    Logger.LogError(err.ToString());
                }

            }
            return null;
        }

        /// <summary>
        /// 查询合约信息
        /// </summary>
        public static ContractResponse GetContract()
        {
            string cookies = DataManager.Instance.Cookies;
            string url = Helper.GetURL(HTTPServiceUrlCollection.GetBaseInstrumentsInfoUrl);
            string hr = GetData(url, cookies);
            if (hr != null)
            {
                try
                {
                    var bi = Helper.Deserialize<ContractResponse>(hr);
                    return bi;
                }
                catch (Exception err)
                {
                    Logger.LogError(err.ToString());
                }
            }
            return null;
        }

        /// <summary>
        /// 查询摘牌方授信额度
        /// </summary>
        private void GetQualityMargin()
        {

        }

        /// <summary>
        /// 查询历史的成交列表(包含撤销)
        /// </summary>
        private void GetHistoryTradingList()
        {

        }

        /// <summary>
        /// 查询指定CmmBrandID的CombrandName
        /// </summary>
        public static string QueryHttpBrandName(OneListed info)
        {
            if (info == null || info.commBrandId == "" || info.cid == "") return "";
            LevelBrandResponse levelBrandInfo = new LevelBrandResponse();
            try
            {
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.GetBaseLevelBrandInfoUrl) + "cid=" + info.cid;
                string hr = GetData(url, cookies);
                if (hr != null)
                {
                    try
                    {
                        levelBrandInfo = Helper.Deserialize<LevelBrandResponse>(hr);
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }

                if (levelBrandInfo.Result == null) return "";
                foreach (LevelBrandList infos in levelBrandInfo.Result)
                {
                    if (infos.infoName != info.commLevelName) continue;
                    if (infos.infoList == null || infos.infoList.Count < 0) continue;
                    List<LevelBrand> infoList = infos.infoList;
                    foreach (LevelBrand it in infoList)
                    {
                        if (it.id.ToString() != info.commBrandId) continue;
                        return it.infoName;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("摘牌品牌名称获取异常:" + ex.Message);
            }

            return "";
        }

        /// <summary>
        /// 查某commId保证金比例
        /// </summary>
        public static MaginRadio GetCommpMarginRaioQuatity(MaginRadioRequest require)
        {
            if (DataManager.Instance.Cookies != null)
            {
                //组装成URL后缀请求
                string str = require.categoryId;
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.GetCommIdMarginRadio) + str + "?Security-Token=" + HttpUtility.UrlEncode(cookies);
                string hr = GetData(url, cookies);
                if (hr != null)
                {
                    try
                    {
                        MaginRadioResponse bi = Helper.Deserialize<MaginRadioResponse>(hr);
                        if (bi == null || bi.data == null) return null;
                        return bi.data;
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 查询可用资金
        /// </summary>
        /// <returns></returns>
        public static Funds GetAccountDataInfos()
        {
            if (DataManager.Instance.Cookies != null)
            {
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.GetAvaliableQualityCash);
                string hr = GetData(url, cookies); if (hr != null)
                {
                    try
                    {
                        FundsResponse bi = Helper.Deserialize<FundsResponse>(hr);
                        if (bi == null || bi.data == null) return null;
                        return bi.data;
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取黑白名单
        /// </summary>
        /// <returns></returns>
        public static BlackAndWhiteListPage GetBlackWhiteList()
        {
            if (DataManager.Instance.Cookies != null)
            {
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.GetBlackAndWhiteList);
                string hr = GetData(url, cookies);
                if (hr != null)
                {
                    try
                    {
                        BlackAndWhiteList bi = Helper.Deserialize<BlackAndWhiteList>(hr);
                        if (bi == null || bi.data == null) return null;
                        return bi.data;
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取白名单状态
        /// </summary>
        /// <returns></returns>
        public static bool GetWhiteState()
        {
            if (DataManager.Instance.Cookies != null)
            {
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.GetWhiteState);
                string hr = GetData(url, cookies);
                if (hr != null)
                {
                    try
                    {
                        BlackWhiteState bi = Helper.Deserialize<BlackWhiteState>(hr);
                        if (bi != null && bi.data == "1")
                        {
                            return true;
                        }
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取黑名单状态
        /// </summary>
        /// <returns></returns>
        public static bool GetBlackState()
        {
            if (DataManager.Instance.Cookies != null)
            {
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.GetBlackState);
                string hr = GetData(url, cookies);
                if (hr != null)
                {
                    try
                    {
                        BlackWhiteState bi = Helper.Deserialize<BlackWhiteState>(hr);
                        if (bi != null && bi.data == "1")
                        {
                            return true;
                        }
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否在黑名单中
        /// </summary>
        /// <returns></returns>
        public static bool JudgmentBlack(String id)
        {
            if (id == null) return false;
            if (DataManager.Instance.Cookies != null)
            {
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.GetP2PBlackState, id);
                string hr = GetData(url, cookies);
                if (hr != null)
                {
                    try
                    {
                        BlackState bi = Helper.Deserialize<BlackState>(hr);
                        if (bi != null && !string.IsNullOrWhiteSpace(bi.result))
                        {
                            bool b;
                            bool.TryParse(bi.result, out b);
                            return b;
                        }
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return false;
        }

        public class LoginDataInfo
        {
            public string loginName;
            public string password;
            public string verifyCode;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="zh">账号</param>
        /// <param name="mm">密码</param>
        public static LoginInfo Login(string zh, string mm, out string msg)
        {
            msg = "";
            //string cookies = DataManager.Instance.Cookies;
            //string str = string.Format("password={0}&login_name={1}&label={2}", mm, zh, "useonline-gold");//"utrade");
            //string url = Helper.GetURL(HTTPServiceUrlCollection.LoginHttp);
            //string hr = PostData(url, cookies, str, "application/x-www-form-urlencoded");

            LoginDataInfo data = new LoginDataInfo()
            {
                loginName = zh,
                password = mm,
                verifyCode = ""
            };

            string strData = Helper.Serialize(data);
            string cookies = null;
            string url = Helper.GetURL(HTTPServiceUrlCollection.Login);
            string hr = PostData(url, cookies, strData, "application/json");

            if (hr != null)
            {
                try
                {
                    var bi = Helper.Deserialize<LoginResponse>(hr);
                    if (bi != null)
                    {
                        msg = bi.Msg;
                        return bi.data;
                    }
                }
                catch (Exception err)
                {
                    Logger.LogError(err.ToString());
                }
            }
            return null;
        }

        /// <summary>
        /// 注销
        /// </summary>
        public static void LoginOff()
        {
            if (DataManager.Instance.Cookies != null)
            {
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.LoginOff);
                string hr = GetData(url, cookies);
            }
        }

        /// <summary>
        /// 获取个人挂牌列表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<SelfListed> GetSelfListedList(string id)
        {
            if (DataManager.Instance.Cookies != null)
            {
                string str = "categoryId=" + id;
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.GetMyPutBrandList) + str;
                string hr = GetData(url, cookies);
                if (hr != null)
                {
                    try
                    {
                        var bi = Helper.Deserialize<SelfListedResponse>(hr);
                        if (bi.data != null)
                        {
                            return bi.data;
                        }
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取摘牌列表或者成交列表
        /// </summary>
        /// <param name="status">1:摘牌列表 2:成交列表</param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static List<Transaction> GetTransactionList(string status, string categoryId)
        {
            if (DataManager.Instance.Cookies != null)
            {
                string str = "status=" + status + "&" + "categoryId=" + categoryId;
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.GetDelistTradedListInfoUrl) + str;
                string hr = GetData(url, cookies);
                if (hr != null)
                {
                    try
                    {
                        var bi = Helper.Deserialize<TransactionList>(hr);
                        if (bi != null && bi.data != null)
                        {
                            return bi.data;
                        }
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return null;
        }

        public static List<UseKLine> GetKLineData(string category)
        {
            if (DataManager.Instance.Cookies != null&& category!=null)
            {
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.GetKLineData, category);
                string hr = GetData(url, cookies);
                if (hr != null)
                {
                    try
                    {
                        var bi = Helper.Deserialize<UseKLineResponse>(hr);
                        if (bi != null && bi.data != null)
                        {
                            return bi.data;
                        }
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 返回创建合同信息
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public static string GetCreateContractInfo(string orderNo)
        {
            if (DataManager.Instance.Cookies != null)
            {
                string str = "orderNo=" + orderNo;
                string cookies = DataManager.Instance.Cookies;
                string url = Helper.GetURL(HTTPServiceUrlCollection.GetCreatContractInfo) + str;
                string hr = GetData(url, cookies);
                if (hr != null)
                {
                    try
                    {
                        UseCreateContract bi = Helper.Deserialize<UseCreateContract>(hr);
                        if (bi != null && bi.Success && bi.result != null)
                        {
                            return bi.result;
                        }
                    }
                    catch (Exception err)
                    {
                        Logger.LogError(err.ToString());
                    }
                }
            }
            return null;
        }
    }
}