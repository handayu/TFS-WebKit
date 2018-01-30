using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UseOnlineTradingSystem
{
    public class Helper
    {
        public static string GetAppConfig(string key)
        {
            string[] arr = ConfigurationManager.AppSettings.GetValues(key);
            if (null != arr && arr.Length >= 1)
            {
                return arr[0];
            }
            return null;
        }

        public static bool SetAppConfig(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (!config.HasFile)
            {
                return false;
            }
            KeyValueConfigurationElement _key = config.AppSettings.Settings[key];
            if (_key == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            return true;
        }

        public static string GetConfig(string name, string key)
        {
            var v = ConfigurationManager.GetSection(name);
            if (v != null)
            {
                IDictionary dic = v as IDictionary;
                if (dic != null && dic.Contains(key))
                {
                    return dic[key].ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// 序列化成json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 反序列化成对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }

        /// <summary>
        /// 获取枚举的自定义属性
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(Enum value)
        {
            if (value == null)
            {
                throw new ArgumentException("value");
            }
            string description = value.ToString();
            var fieldInfo = value.GetType().GetField(description);
            var attributes =
                (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            return description;
        }

        /// <summary>
        /// Long类型转化为DataTime
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static string LongConvertToDataTimeStr(string strResult)
        {
            long l;
            long.TryParse(strResult, out l);
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            strResult = start.AddMilliseconds(l).ToLocalTime().ToString("HH:mm:ss");

            return strResult;
        }

        /// <summary>
        /// 获取对应的URL
        /// </summary>
        /// <param name="hc"></param>
        /// <returns></returns>
        public static string GetURL(HTTPServiceUrlCollection hc,string Parameter1="")
        {
            bool r = USeManager.Instance.Release;//是否正式版
            string url = USeManager.Instance.MqttAddress;
            string sso = USeManager.Instance.SSOAddress;
            if (url != null && sso != null)
            {
                try
                {
                    switch (hc)
                    {
                        /// <summary>
                        /// 获取资金金额和资金使用率
                        /// </summary>
                        case HTTPServiceUrlCollection.GetAvaliableQualityCash:
                            if (r)
                            {
                                return string.Format("http://{0}/account/fund/balance", Helper.GetAppConfig("orderHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:8089/account/fund/balance", url);
                            }
                        /// <summary>
                        /// 获取授信额度
                        /// </summary>
                        case HTTPServiceUrlCollection.GetDelistReditQuality:
                            if (r)
                            {
                                return string.Format("http://{0}/v1/companyCredit/queryCreditInfo?", Helper.GetAppConfig("serviceHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:9242/v1/companyCredit/queryCreditInfo?", url);
                            }

                        /// <summary>
                        /// 获取保证金比例[hanyu URL需要确认一下]
                        /// </summary>
                        case HTTPServiceUrlCollection.GetCommIdMarginRadio:
                            if (r)
                            {
                                return string.Format("http://{0}/depositscale/", Helper.GetAppConfig("userHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:8081/v1/depositscale/", url);
                            }

                        /// <summary>
                        /// 获取合约基础信息
                        /// </summary>
                        case HTTPServiceUrlCollection.GetBaseInstrumentsInfoUrl:
                            if (r)
                            {
                                return string.Format("http://{0}/futuredata/data/base", Helper.GetAppConfig("futuredataHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:8088/futuredata/data/base", url);
                            }

                        /// <summary>
                        /// 获取所有挂牌列表信息
                        /// </summary>
                        case HTTPServiceUrlCollection.GetList:
                            if (r)
                            {
                                return string.Format("http://{0}/v1/commodityInfo/list", Helper.GetAppConfig("serviceHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:9242/v1/commodityInfo/list", url);
                            }

                        /// <summary>
                        /// 获取自己挂牌列表信息
                        /// </summary>
                        case HTTPServiceUrlCollection.GetMyPutBrandList:
                            if (r)
                            {
                                return string.Format("http://{0}/v1/orderInfo/commlist?", Helper.GetAppConfig("serviceHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:9242/v1/orderInfo/commlist?", url);
                            }

                        /// <summary>
                        /// 获取个人信息
                        /// </summary>
                        case HTTPServiceUrlCollection.GetPersonalInformation:
                            if (r)
                            {
                                return string.Format("http://{0}/user/me", Helper.GetAppConfig("userHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:8081/user/me", url);
                            }

                        /// <summary>
                        /// 获取仓库基本信息
                        /// </summary>
                        case HTTPServiceUrlCollection.GetBaseWareHouseInfoUrl:
                            if (r)
                            {
                                return string.Format("http://{0}/v1/warehouseInfo/list", Helper.GetAppConfig("serviceHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:9242/v1/warehouseInfo/list", url);
                            }

                        /// <summary>
                        /// 获取指定仓库URL
                        /// </summary>
                        case HTTPServiceUrlCollection.GetWareHouseInfoUrl:
                            //http://172.16.88.152/tradesystem/pay/transform.html?id=3&path=/tradesystem/pay/warehouse/detail.html
                            //http://172.16.88.152/tradesystem/pay/transform.html?id=3&path=/tradesystem/pay/warehouse/detail.html
                            //return string.Format("http://{0}/tradesystem/pay/transform.html", url);
                            if (r)
                            {
                                return string.Format("http://{0}/tradesystem/pay/transform.html?id={1}&path=/tradesystem/pay/warehouse/detail.html", Helper.GetAppConfig("address"), Parameter1);
                            }
                            else
                            {
                                return string.Format("http://{0}/tradesystem/pay/transform.html?id={1}&path=/tradesystem/pay/warehouse/detail.html", url, Parameter1);
                            }
                        /// <summary>
                        /// 品牌等级信息
                        /// </summary>
                        case HTTPServiceUrlCollection.GetBaseLevelBrandInfoUrl:
                            if (r)
                            {
                                return string.Format("http://{0}/v1/levelBrandInfo/list?", Helper.GetAppConfig("serviceHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:9242/v1/levelBrandInfo/list?", url);
                            }

                        /// <summary>
                        /// 摘牌成交列表
                        /// </summary>
                        case HTTPServiceUrlCollection.GetDelistTradedListInfoUrl:
                            if (r)
                            {
                                return string.Format("http://{0}/v1/orderInfo/orderlist?", Helper.GetAppConfig("serviceHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:9242/v1/orderInfo/orderlist?", url);
                            }

                        /// <summary>
                        /// 挂牌操作
                        /// </summary>
                        case HTTPServiceUrlCollection.PostBrandOrderRequireInfoUrl:
                            if (r)
                            {
                                return string.Format("http://{0}/v1/commodityInfo/listing", Helper.GetAppConfig("serviceHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:9242/v1/commodityInfo/listing", url);
                            }

                        /// <summary>
                        /// 摘牌操作
                        /// </summary>
                        case HTTPServiceUrlCollection.PostDelistBrandOrderRequireInfoUrl:
                            if (r)
                            {
                                return string.Format("http://{0}/v1/delistComm/delistCommodityCS", Helper.GetAppConfig("serviceHost"));
                                //return string.Format("http://{0}/v1/delistComm/delistCommodity", Helper.GetAppConfig("serviceHost"));                          
                            }
                            else
                            {
                                //return string.Format("http://{0}/v1/delistComm/delistCommodityCS", "172.16.84.32:9240");
                                return string.Format("http://{0}:9242/v1/delistComm/delistCommodity", url);
                            }

                        /// <summary>
                        /// 核销操作
                        /// </summary>
                        case HTTPServiceUrlCollection.PostDelistBrandOrderVerifyRequireInfoUrl:
                            if (r)
                            {
                                return string.Format("http://{0}/v1/delistComm/delistConfirm", Helper.GetAppConfig("serviceHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:9242/v1/delistComm/delistConfirm", url);
                            }

                        /// <summary>
                        /// 撤单操作
                        /// </summary>
                        case HTTPServiceUrlCollection.PostActionBrandOrderRequireInfoUrl:
                            if (r)
                            {
                                return string.Format("http://{0}/v1/commodityInfo/revoke", Helper.GetAppConfig("serviceHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:9242/v1/commodityInfo/revoke", url);
                            }

                        /// <summary>
                        /// 登录请求
                        /// </summary>
                        case HTTPServiceUrlCollection.LoginHttp:
                            return string.Format("http://{0}/api/user/login", sso);

                        /// <summary>
                        /// 登录操作
                        /// </summary>
                        case HTTPServiceUrlCollection.Login:
                            if (r)
                            {
                                return string.Format("http://{1}/sso/sign/signin.html?next=http://{0}/tradesystem/pay/home/index.html&label=useonline-gold", Helper.GetAppConfig("address"), sso); ;
                            }
                            else
                            {
                                return string.Format("http://{1}/sso/sign/signin.html?next=http://{0}/tradesystem/pay/home/index.html&label=useonline-gold", url, sso); 
                            }

                        /// <summary>
                        /// 登出操作
                        /// </summary>
                        case HTTPServiceUrlCollection.LoginOff:
                            if (r)
                            {
                                return string.Format("http://{0}/uc/logout", Helper.GetAppConfig("userHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:8081/uc/logout", url);
                            }

                        /// <summary>
                        /// 历史交易查询
                        /// </summary>
                        case HTTPServiceUrlCollection.History:
                            if (r)
                            {
                                return string.Format("http://{0}/tradesystem/pay/transform.html?path=/tradesystem/pay/capital/index.html", Helper.GetAppConfig("address"));
                            }
                            else
                            {
                                return string.Format("http://{0}/tradesystem/pay/transform.html?path=/tradesystem/pay/capital/index.html", url);
                            }
                        //return string.Format("http://{0}/tradesystem/pay/capital/index.html", url);

                        /// <summary>
                        /// 基础管理
                        /// </summary>
                        case HTTPServiceUrlCollection.BasicManagement:
                            if (r)
                            {
                                return string.Format("http://{0}/tradesystem/pay/transform.html?path=/tradesystem/pay/accountManage/personal/index.html", Helper.GetAppConfig("address"));
                            }
                            else
                            {
                                return string.Format("http://{0}/tradesystem/pay/transform.html?path=/tradesystem/pay/accountManage/personal/index.html", url);
                            }
                        //return string.Format("http://{0}/tradesystem/pay/accountManage/personal/index.html", url);

                        /// <summary>
                        /// 注册
                        /// </summary>
                        case HTTPServiceUrlCollection.Registered:
                            if (r)
                            {
                                return string.Format("http://{1}/sso/sign/signup.html?next=http://{0}/tradesystem/pay/home/index.html&label=useonline-gold", Helper.GetAppConfig("address"), sso);
                            }
                            else
                            {
                                return string.Format("http://{1}/sso/sign/signup.html?next=http://{0}/tradesystem/pay/home/index.html&label=useonline-gold", url, sso);
                            }

                        /// <summary>
                        /// 黑白名单列表
                        /// </summary>
                        //http://172.16.88.152:8081/page/blackwhitelist
                        case HTTPServiceUrlCollection.GetBlackAndWhiteList:
                            if (r)
                            {
                                return string.Format("http://{0}/page/blackwhitelist", Helper.GetAppConfig("userHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:8081/page/blackwhitelist", url);
                            }

                        /// <summary>
                        /// 是否启用白名单
                        /// </summary>
                        case HTTPServiceUrlCollection.GetWhiteState:
                            if (r)
                            {
                                return string.Format("http://{0}/blackwhitelist/status/1", Helper.GetAppConfig("userHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:8081/blackwhitelist/status/1", url);
                            }

                        /// <summary>
                        /// 是否启用黑名单
                        /// </summary>
                        case HTTPServiceUrlCollection.GetBlackState:
                            if (r)
                            {
                                return string.Format("http://{0}/blackwhitelist/status/0", Helper.GetAppConfig("userHost"));
                            }
                            else
                            {
                                return string.Format("http://{0}:8081/blackwhitelist/status/0", url);
                            }

                        /// <summary>
                        /// 判断当前客户是否存在于挂牌方的黑名单中
                        /// </summary>
                        case HTTPServiceUrlCollection.GetP2PBlackState:
                            if (r)
                            {
                                return string.Format("http://{0}/v1/commodityInfo/judgeBlackList?publisherId={1}", Helper.GetAppConfig("serviceHost"), Parameter1);
                            }
                            else
                            {
                                return string.Format("http://{0}:9242/v1/commodityInfo/judgeBlackList?publisherId={1}", url, Parameter1);
                            }

                        /// <summary>
                        /// 其他
                        /// </summary>
                        default: return "";
                    }
                }
                catch(Exception err)
                {
                    Logger.LogError(err);
                }
            }
            return "";
        }
    }
}
