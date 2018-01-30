using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using UseHttpHelper;
using Xilium.CefGlue;

namespace UseOnlineTradingSystem
{
    class DataManager
    {
        private static readonly DataManager instance = new DataManager();

        static DataManager()
        {

        }

        private DataManager()
        {
            //数据提取
            USeManager.Instance.MQTTService.UpdataEvent += MQTTService_UpdataEvent;
        }

        public static DataManager Instance
        {
            get
            {
                return instance;
            }
        }

        private bool isLogin = false;

        /// <summary>
        /// 是否已登录
        /// </summary>
        public bool IsLogin
        {
            get { return isLogin; }
        }

        private DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 账号
        /// </summary>
        public string Account;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password;
        /// <summary>
        /// 缓存信息
        /// </summary>
        private string cookies;
        private string currentCode =null;//cu
        private ContractCategoryDic currentCategory = null;//当前选中的品类
        public string CurrentContractCode = null;//cu1801
        /// <summary>
        /// 当前选中的品类名称
        /// </summary>
        public string CurrentCode
        {
            get { return currentCode; }
            set
            {
                if (value != null)
                {
                    var cc = GetContractCategoryDic(value);
                    if (cc != null)
                    {
                        currentCode = value;
                        currentCategory = cc;
                    }
                    else
                    {
                        currentCode = null;
                        currentCategory = null;
                    }
                }
            }
        }
        public ContractCategoryDic CurrentCategory
        {
            get { return currentCategory; }
        }
        public Mine LoginData;
        public LoginInfo LoginInfo;
        private Dictionary<string, ContractCategoryDic> categoryDY=new Dictionary<string, ContractCategoryDic>();
        private Dictionary<string, ContractLastPrice> lastPriceDY= new Dictionary<string, ContractLastPrice>();
        public Dictionary<string, WareHouseInfo> wareHouses = new Dictionary<string, WareHouseInfo>();
        public Dictionary<int, LevelBrandList> LevelBrandDy = new Dictionary<int, LevelBrandList>();
        private Dictionary<string, OneListed> CommodityDataDy = new Dictionary<string, OneListed>();
        public Dictionary<string, BlackAndWhite> BlackDY = new Dictionary<string, BlackAndWhite>();
        public Dictionary<string, BlackAndWhite> WhiteDY = new Dictionary<string, BlackAndWhite>();
        public bool BlackEnable = false;
        public bool WhiteEnable = false;
        public List<Transaction> Delist;
        public List<Transaction> TradedList;
        public event Action<bool> IsLoginEvent;
        public event Action<bool> IsLoginingEvent;
        public event Action<OneListed, int> UpdataCommodityInfoEvent;
        public event Action<string> UpdataEvent;
        public event Action UpdataAllContractEvent;
        public event Action UpdataAllCommodityEvent;
        public event Action<Transaction> UpdataDelistingEvent;//摘牌事件
        /// <summary>
        /// 会话状态
        /// </summary>
        public string Cookies
        {
            get { return cookies; }
        }

        public bool RemoveCommodityData(string key)
        {
            if (key != null && CommodityDataDy.ContainsKey(key))
            {
                return CommodityDataDy.Remove(key);
            }
            return false;
        }

        public void InitializationData()
        {
            GetContract();
           
            GetCommodity();
        }

        /// <summary>
        /// 获取最新价格信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ContractLastPrice GetContractLastPrice(string key)
        {
            if (key == null) return null; 

            if (lastPriceDY.ContainsKey(key))
            {
                return lastPriceDY[key];
            }
            foreach (var v in categoryDY)
            {
                foreach (var v1 in v.Value.contractMonthMap)
                {
                    if (v1.Value.categoryName == key)
                    {
                        ContractLastPrice va;
                        lastPriceDY.TryGetValue(v1.Value.category, out va);
                        return va;
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///  获取基础价格信息
        /// </summary>
        /// <param name="key1">品类</param>
        /// <param name="key2">合约</param>
        /// <returns></returns>
        public ContractBasePrice GetContractBasePrice(string key1, string key2)
        {
            if(categoryDY.ContainsKey(key1)) 
            {
                if (categoryDY[key1].contractMonthMap.ContainsKey(key2))
                {
                    return categoryDY[key1].contractMonthMap[key2];
                }
            }
            return null;
        }

        /// <summary>
        ///  获取品类信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ContractCategoryDic GetContractCategoryDic(string key)
        {
            if (categoryDY.ContainsKey(key))
            {              
                return categoryDY[key];
            }
            return null;
        }

        /// <summary>
        /// 获取仓库信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public WareHouseInfo GetWareHouseInfo(string key)
        {
            if (wareHouses != null)
            {
                if (wareHouses.ContainsKey(key))
                {
                    return wareHouses[key];
                }
            }
            return null;
        }

        /// <summary>
        /// 获取品类相关合约结算价信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, ContractCategoryDic> GetContractcCategoryVo()
        {
            return categoryDY;
        }

        ///// <summary>
        ///// 登录
        ///// </summary>
        //public void Login()
        //{
        //    if (!isLogin)
        //    {
        //        ThreadPool.QueueUserWorkItem(new WaitCallback(IsLogining));
        //    }
        //}

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="zh"></param>
        /// <param name="mm"></param>
        public bool Login(string zh, string mm,out string msg)
        {
             msg="";
            if (!isLogin)
            {
                var logindata = HttpService.Login(zh,mm,out msg);
                if (logindata != null&& logindata.ticket!=null)
                {
                    LoginInfo = logindata;
                    DataManager.Instance.Account = zh;
                    DataManager.Instance.Password = mm;
                    DataManager.Instance.SetCookies( HttpUtility.UrlDecode(LoginInfo.ticket));
                    IsLogining(null);
                    return isLogin;
                    //ThreadPool.QueueUserWorkItem(new WaitCallback(IsLogining));
                }
            }
            return false;
        }

        /// <summary>
        /// 登录中
        /// </summary>
        /// <param name="obj"></param>
        private void IsLogining(object obj)
        {
            IsLoginingEvent?.Invoke(true);//触发登录成功前事件

            #region 拉取个人信息
            var mine = HttpService.GetMine();//个人信息
            if (mine != null && mine.Success && mine.data != null)
            {
                LoginData = mine.data;
                if (cookies != null && LoginInfo != null&& LoginData.currentCompany!=null)
                {
                    //USeManager.Instance.Stop();
                    string clientid = LoginData.currentCompany.id + "_pc";
                    USeManager.Instance.Start(clientid, LoginInfo.username, HttpUtility.UrlEncode(cookies));
                }
            }
            #endregion

            #region 拉取合约行情列表 重新初始化
            InitializationData();
            #endregion

            #region 拉取黑白名单
            GetWhiteAndBlack();
            #endregion

            #region 拉取全部的等级和品牌信息
            var LevelBrand = HttpService.GetBaseLevelBrandInfo();
            if (LevelBrand != null && LevelBrand.Result != null)
            {
                LevelBrandDy.Clear();
                foreach (var v in LevelBrand.Result)
                {
                    if (!LevelBrandDy.ContainsKey(v.id))
                    {
                        LevelBrandDy.Add(v.id, v);
                    }
                    else
                    {
                        LevelBrandDy[v.id] = v;
                    }
                }
            }
            #endregion

            #region 拉取成交列表
            var Tradedlisting = HttpService.GetTradedlisting();
            if (Tradedlisting != null && Tradedlisting.data != null)
            {
                TradedList = Tradedlisting.data;
            }
            #endregion

            isLogin = true;
            IsLoginEvent?.Invoke(true);//触发登录成功事件
        }

        /// <summary>
        /// 注销
        /// </summary>
        public void LoginOff()
        {
            if (isLogin)
            {
                if (DataManager.Instance.Cookies != null)
                {
                    HttpService.LoginOff();

                    LoginData = null;
                    Account = null;
                    Password = null;
                    cookies = null;
                    LevelBrandDy.Clear();
                    Delist = null;
                    wareHouses.Clear();
                    WhiteEnable = false;
                    BlackEnable = false;
                    USeManager.Instance.Stop();
                    USeManager.Instance.Start();
                    isLogin = false;
                    IsLoginEvent?.Invoke(false);//触发注销成功事件
                }
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cookies"></param>
        public void SetCookies(string cookies)
        {
            this.cookies = cookies;
        }

        private void MQTTService_UpdataEvent(string obj)
        {
            if (obj != null)
            {
                if (obj == "getBaseInfo")
                {
                    //刷新
                    GetContract();
                    return;
                }

                //挂牌信息
                var b1 = Helper.Deserialize<OneListedResponse>(obj);

                if (b1.Success && b1.Result != null && CommodityDataDy != null)
                {
                    int type = 2;
                    OneListed cd = b1.Result;
                    if (cd!=null&& !HttpService.JudgmentBlack(cd.PublisherId))
                    {
                        if (cd.id != null)
                        {
                            if (CommodityDataDy.ContainsKey(cd.id))
                            {
                                //更新
                                CommodityDataDy[cd.id].Update(cd);
                                type = 2;//更新
                            }
                            else
                            {
                                type = cd.transType;
                                CommodityDataDy.Add(cd.id, cd);
                            }
                            UpdataCommodityInfoEvent?.Invoke(cd, type);
                        }
                    }
                    return;
                }

                //if (b1.Result.TransStatusName == "摘牌")
                //{
                //    //摘牌
                //    var b2 = Helper.Deserialize<DelistBrandOrderResponseArguments>(obj);
                //    if (b2.data != null)
                //    {
                //        UpdataDelistingEvent?.Invoke(b2.data);
                //        return;
                //    }
                //}
                ////核销
                //var b3 = Helper.Deserialize<DelistBrandVerifyResponseArguments>(obj);
                //if (b3.Success && b3.data != null)
                //{
                //    NoticeWriteOffEvent?.Invoke(b3.data);
                //}
                ////撤牌
                //var b4 = Helper.Deserialize<ActionOrderResponseArguments>(obj);
                //if (b4.Success && b4.result != null)
                //{
                //    NoticeWithdrawTheCard?.Invoke(b4.result);
                //}
                UpdataEvent?.Invoke(obj);
            }
        }

        /// <summary>
        /// 拉取合约和仓库信息
        /// </summary>
        public void GetContract()
        {
            #region 拉取合约
            var contract = HttpService.GetContract();
            if (contract != null && contract.Success && contract.data != null)
            {
                currentCategory = null;
                currentCode = null;
                CurrentContractCode = null;
                categoryDY.Clear();
                lastPriceDY.Clear();
                var ContractData = contract.data;
                if (ContractData.categoryVoMap != null)
                {
                    categoryDY = ContractData.categoryVoMap;

                    #region 拉取仓库信息
                    wareHouses.Clear();
                    foreach (var v in ContractData.categoryVoMap.Values)
                    {
                        var WareHouseInfo = HttpService.GetWareHouseInfo(v.id);
                        if (WareHouseInfo != null && WareHouseInfo.Success&& WareHouseInfo.Result!=null)
                        {
                            foreach (var v2 in WareHouseInfo.Result)
                            {
                                if (!wareHouses.ContainsKey(v2.id))
                                {
                                    wareHouses.Add(v2.id, v2);
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (ContractData.lastPriceMap != null)
                {
                    lastPriceDY = ContractData.lastPriceMap;
                }
                CurrentCode = "cu";
                UpdataAllContractEvent?.Invoke();
            }
            #endregion
        }

        /// <summary>
        /// 获取挂牌列表
        /// </summary>
        public void GetCommodity()
        {
            #region 拉取挂牌列表
            var commodity = HttpService.GetList();
            if (commodity != null && commodity.Success && commodity.Result != null)
            {
                AllListedList CommodityData = commodity.Result;
                CommodityDataDy.Clear();
                foreach (var v in CommodityData.sellList)
                {
                    if (v!=null&& v.id!=null && !HttpService.JudgmentBlack(v.PublisherId))
                    {
                        if (!CommodityDataDy.ContainsKey(v.id))
                        {
                            CommodityDataDy.Add(v.id, v);
                        }
                    }
                }
                foreach (var v in CommodityData.buyList)
                {
                    if (v != null && v.id != null&& !HttpService.JudgmentBlack(v.PublisherId))
                    {
                        if (!CommodityDataDy.ContainsKey(v.id))
                        {
                            CommodityDataDy.Add(v.id, v);
                        }
                    }
                }
                UpdataAllCommodityEvent?.Invoke();
            }
            #endregion
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        public List<OneListed> SortCommodityData()
        {
            List<OneListed> list = new List<OneListed>();

            if (CommodityDataDy != null)
            {
                list = CommodityDataDy.Values.ToList();

                list.Sort((obj1, obj2) =>
                {
                    int res = 0;
                    if ((null == obj1) && (null == obj2)) return 0;
                    else if ((null != obj1) && (null == obj2)) return 1;
                    else if ((null == obj1) && (null != obj2)) return -1;
                    int num1, num2;
                    if (!int.TryParse(obj1.premium, out num1))
                    {
                        num1 = int.MinValue;
                    }
                    if (!int.TryParse(obj2.premium, out num2))
                    {
                        num2= int.MinValue;
                    }
                    long l1, l2;
                    long.TryParse(obj1.publisherDate, out l1);
                    long.TryParse(obj2.publisherDate, out l2);
                    DateTime dt1 = start.AddMilliseconds(l1).ToLocalTime();
                    DateTime dt2 = start.AddMilliseconds(l2).ToLocalTime();
                    if (num1 > num2)
                    {
                        res = -1;
                    }
                    else if (num1 == num2)
                    {
                        if (dt1 > dt2)
                        {
                            res = -1;
                        }
                        else
                        {
                            res = 1;
                        }
                    }
                    else
                    {
                        res = 1;
                    }
                    return res;
                });
            }
            return list;
        }

        /// <summary>
        /// 拉取黑白名单
        /// </summary>
        public void GetWhiteAndBlack()
        {
            #region 拉取黑白名单
            WhiteEnable = HttpService.GetWhiteState();
            BlackEnable = HttpService.GetBlackState();
            var BlackWhiteList = HttpService.GetBlackWhiteList();
            if (BlackWhiteList != null && BlackWhiteList.dataList != null)
            {
                BlackDY.Clear();
                WhiteDY.Clear();
                foreach (var v in BlackWhiteList.dataList)
                {
                    if (v.type == "0")//黑名单
                    {
                        if (!BlackDY.ContainsKey(v.companyName))
                        {
                            BlackDY.Add(v.companyName, v);
                        }
                        else
                        {
                            BlackDY[v.companyName] = v;
                        }
                    }
                    else if (v.type == "1")//白名单
                    {
                        if (!WhiteDY.ContainsKey(v.companyName))
                        {
                            WhiteDY.Add(v.companyName, v);
                        }
                        else
                        {
                            WhiteDY[v.companyName] = v;
                        }
                    }
                }
            }
            #endregion
        }
    }
}

