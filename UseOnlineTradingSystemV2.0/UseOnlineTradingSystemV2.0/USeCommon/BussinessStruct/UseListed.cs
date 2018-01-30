
using System;
using System.Collections.Generic;

namespace UseOnlineTradingSystem
{
    /// <summary>
    ///  挂牌操作请求
    /// </summary>
    public class ListedRequest
    {
        public string mqId;

        public string clientId;

        public int operationType;

        public string commId;

        public string cid;

        public int commBrandId;

        public int commLevel;

        public string commTotalQuantity;

        public string contract;

        public string contractName;

        public int ensureMethod;

        public string fixedPrice;

        public string minDealQuantity;

        public string premium;

        public int pricingMethod;

        public string publisher;

        public string publisherId;

        public string remarks;

        public string securityToken;

        public string showCompany; 

        public TransType transType;

        public string warehouseId;

        public string warehouseReceiptNum;
    }

    /// <summary>
    /// 挂牌操作响应
    /// </summary>
    public class ListedResponse : BaseResponse
    {
        /// <summary>
        /// 返回Field
        /// </summary>
        public Listed Result;
    }

    /// <summary>
    /// 挂牌信息
    /// </summary>
    public class Listed
    {
        public string mqId;
        public string clientId;
        public string operationType;
        public string transStatus;
        public string transType;
        public string commId;
        public string cid;
        public string id;
        public string seqNum;
        public string transTypeName;
        public string transStatusName;
        public string commBrandId;
        public string commBrandName;
        public string commLevel;
        public string commLevelName;
        public string premium;
        public string commTotalQuantity;
        public string commAvailableQuantity;
        public string minDealQuantity;
        public string commTotalProportion;
        public string commAvailableProportion;
        public string warehouseId;
        public string warehouseName;
        public string publisher;
        public string publisherId;
        public string remarks;
        public string publisherDate;
        public string createdBy;
        public string showCompany;
        public string transTime;
        public string pricingMethod;
        public string fixedPrice;
        public string warehouseReceiptNum;
        public string contract;
        public string ensureMethod;
        public string contractName;
    }

    /// <summary>
    /// 自己的挂牌列表请求
    /// </summary>
    public class SelfListedResponse
    {
        public string code;
        public string msg;
        public List<SelfListed> data;
    }

    /// <summary>
    /// 自己的挂牌列表信息
    /// </summary>
    public class SelfListed : USeBaseViewModel
    {
        //[{"commId":"718","transTime":1508400920000,"basePrice":null,"transType":1,"transTypeName":"销售","commBrandName":"TAMANO-P","commLevelName":"平水铜二类","premium":50.0000,"commTotalQuantity":22,"commAvailableQuantity":22,"warehouseName":"中储发展股份有限公司","warehouseId":3,"minDealQuantity":5,"pricingMethod":0,"fixedPrice":null,"warehouseReceiptNum":null,"contract":"","ensureMethod":0,"remarks":"","contractName":null,"cid":1},

        public string commId;
        public string transTime;
        public string basePrice;
        public string transType;
        public string transTypeName;
        public string commBrandName;
        public string commLevelName;
        public string premium;
        public string commTotalQuantity;
        public string commAvailableQuantity;
        public string warehouseName;
        public string warehouseId;
        public string minDealQuantity;
        public string pricingMethod;
        public string fixedPrice;
        public string warehouseReceiptNum;
        public string contract;
        public string ensureMethod;
        public string remarks;
        public string contractName;
        public string cid;


        public string TransTime
        {
            get { return transTime; }
            set
            {
                transTime = value;
                SetProperty(() => this.TransTime);
                SetProperty(() => this.TransTimeDateTime);
            }
        }

        public string TransTimeDateTime
        {
            get { return Helper.LongConvertToDataTimeStr(transTime); }
        }

        public string TransType
        {
            get { return transType; }
            set
            {
                transType = value;
                SetProperty(() => this.TransType);
                SetProperty(() => this.TransTypeInfo);
            }
        }

        /// <summary>
        /// 展示属性
        /// </summary>
        public string TransTypeInfo
        {
            get
            {
                if (Convert.ToInt32(transType) == 0)
                {
                    return Helper.GetDescription(UseOnlineTradingSystem.TransType.Buy);
                }
                else
                {
                    return Helper.GetDescription(UseOnlineTradingSystem.TransType.Sell);
                }
            }
        }


        public string TransTypeName
        {
            get { return transTypeName; }
            set
            {
                transTypeName = value;
                SetProperty(() => this.TransTypeName);
            }
        }

        //public string TransStatus
        //{
        //    get { return transStatus; }
        //    set
        //    {
        //        transStatus = value;
        //        SetProperty(() => this.TransStatus);
        //        SetProperty(() => this.TransStatusInfo);
        //    }
        //}

        ///// <summary>
        ///// 展示属性
        ///// </summary>
        //public string TransStatusInfo
        //{
        //    get
        //    {
        //        if (Convert.ToInt32(transStatus) == 1)
        //        {
        //            return Helper.GetDescription(UseOnlineTradingSystem.OperationType.PutBrand);
        //        }
        //        else if (Convert.ToInt32(transStatus) == 2)
        //        {
        //            return Helper.GetDescription(UseOnlineTradingSystem.OperationType.DelistBrand);
        //        }
        //        else if (Convert.ToInt32(transStatus) == 3)
        //        {
        //            return Helper.GetDescription(UseOnlineTradingSystem.OperationType.LockBrand);
        //        }
        //        else
        //        {
        //            return Helper.GetDescription(UseOnlineTradingSystem.OperationType.DelistBrand);
        //        }
        //    }
        //}



        //public string TransStatusName
        //{
        //    get { return transStatusName; }
        //    set
        //    {
        //        TransStatusName = value;
        //        SetProperty(() => this.TransStatusName);
        //    }
        //}

        public string Cid
        {
            get { return cid; }
            set
            {
                cid = value;
                SetProperty(() => this.Cid);
            }
        }

        public string CommBrandName
        {
            get { return commBrandName; }
            set
            {
                commBrandName = value;
                SetProperty(() => this.CommBrandName);
            }
        }

        public string CommLevelName
        {
            get { return commLevelName; }
            set
            {
                commLevelName = value;
                SetProperty(() => this.commLevelName);
            }
        }

        public string Premium
        {
            get { return premium; }
            set
            {
                premium = value;
                SetProperty(() => this.Premium);
            }
        }

        public string CommTotalQuantity
        {
            get { return commTotalQuantity; }
            set
            {
                commTotalQuantity = value;
                SetProperty(() => this.CommTotalQuantity);
            }
        }

        public string CommAvailableQuantity
        {
            get { return commAvailableQuantity; }
            set
            {
                commAvailableQuantity = value;
                SetProperty(() => this.CommAvailableQuantity);
            }
        }

        public string MinDealQuantity
        {
            get { return minDealQuantity; }
            set
            {
                minDealQuantity = value;
                SetProperty(() => this.MinDealQuantity);
            }
        }

        public string WarehouseId
        {
            get { return warehouseId; }
            set
            {
                warehouseId = value;
                SetProperty(() => this.WarehouseId);
            }
        }

        public string WarehouseName
        {
            get { return warehouseName; }
            set
            {
                warehouseName = value;
                SetProperty(() => this.WarehouseName);
            }
        }

        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                SetProperty(() => this.Remarks);
            }
        }

        public string PricingMethod
        {
            get { return pricingMethod; }
            set
            {
                pricingMethod = value;
                SetProperty(() => this.PricingMethod);
                SetProperty(() => this.PricingMethodInfo);

            }
        }

        public string PricingMethodInfo
        {
            get
            {
                if (Convert.ToInt32(pricingMethod) == (int)UseOnlineTradingSystem.PricingMethod.SpotPrice)
                {
                    return Helper.GetDescription(UseOnlineTradingSystem.PricingMethod.SpotPrice);
                }
                else
                {
                    return Helper.GetDescription(UseOnlineTradingSystem.PricingMethod.DeadPrice);
                }
            }
        }


        public string FixedPrice
        {
            get { return fixedPrice; }
            set
            {
                fixedPrice = value;
                SetProperty(() => this.FixedPrice);
            }
        }

        public string EnsureMethod
        {
            get { return ensureMethod; }
            set
            {
                ensureMethod = value;
                SetProperty(() => this.EnsureMethod);
            }
        }

        public string Contract
        {
            get { return contract; }
            set
            {
                contract = value;
                SetProperty(() => this.Contract);
            }
        }

        public string WarehouseReceiptNum
        {
            get { return warehouseReceiptNum; }
            set
            {
                warehouseReceiptNum = value;
                SetProperty(() => this.WarehouseReceiptNum);
            }
        }

    }

    /// <summary>
    /// 所有的挂牌列表响应
    /// </summary>
    public class AllListedResponse : BaseResponse
    {
        public AllListedList Result;
    }

    /// <summary>
    /// 所有的挂牌列表
    /// </summary>
    public class AllListedList
    {
        public List<OneListed> sellList;
        public List<OneListed> buyList;
    }

    /// <summary>
    /// 单条挂牌列表响应
    /// </summary>
    public class OneListedResponse : BaseResponse
    {
        public OneListed Result;
    }

    /// <summary>
    /// 单条挂牌信息
    /// </summary>
    public class OneListed : USeBaseViewModel
    {
        public string id;//商品ID
        public string seqNum;//返回信息
        public int transType;//卖和买。 0：采购 1：销售
        public string transTypeName;//卖和买名称
        public string transStatus;//状态。 1：挂牌 2：摘牌 3：锁定 4：下架
        public string transStatusName; //状态名称
        public string cid;//品类ID
        public string commBrandId;//商品品牌ID
        public string commLevel;//商品等级ID
        public string commLevelName;//商品品牌
        public string premium;//升贴水
        public string commTotalQuantity;// 	数量
        public string commAvailableQuantity;//可用数量
        public string minDealQuantity;//最小成交数
        public string commTotalProportion;//总保证金
        public string commAvailableProportion;//可用保证金
        public string warehouseId;//仓库ID
        public string warehouseName;//仓库名称
        public string publisher;//发布公司(公司名称)
        public string publisherId;//发布公司ID(公司ID)
        public string remarks;//备注
        public string publisherDate; //发布时间
        public string createdBy;//创建人(用户ID)
        public string blackWhiteType;//黑白名单 0：黑名单 1：白名单
        public string oneself;//自己：true 他人：false
        public string pricingMethod;//定价方式：0 点价 1 一口价
        public string fixedPrice;//固定价格
        public string ensureMethod;//保证方式 0:保证金 1:实货凭证
        public string contract;//参考合约，例：1709
        public string warehouseReceiptNum;//仓单号
        public string outSource;//标示外部系统数据，为空标示内部系统数据
        public string outId;//外部系统数据ID

        public void Update(OneListed obj)
        {
            seqNum = obj.seqNum;//返回信息
            transType = obj.transType;//卖和买。 0：采购 1：销售
            transTypeName = obj.transTypeName;//卖和买名称
            transStatus = obj.transStatus;//状态。 1：挂牌 2：摘牌 3：锁定 4：下架
            transStatusName = obj.transStatusName; //状态名称
            cid = obj.cid;//品类ID
            commBrandId = obj.commBrandId;//商品品牌ID
            commLevel = obj.commLevel;//商品等级ID
            commLevelName = obj.commLevelName;//商品品牌
            premium = obj.premium;//升贴水
            commTotalQuantity = obj.commTotalQuantity;// 	数量
            commAvailableQuantity = obj.commAvailableQuantity;//可用数量
            minDealQuantity = obj.minDealQuantity;//最小成交数
            commTotalProportion = obj.commTotalProportion;//总保证金
            commAvailableProportion = obj.commAvailableProportion;//可用保证金
            warehouseId = obj.warehouseId;//仓库ID
            warehouseName = obj.warehouseName;//仓库名称
            publisher = obj.publisher;//发布公司(公司名称)
            publisherId = obj.publisherId;//发布公司ID(公司ID)
            remarks = obj.remarks;//备注
            publisherDate = obj.publisherDate; //发布时间
            createdBy = obj.createdBy;//创建人(用户ID)
            blackWhiteType = obj.blackWhiteType;//黑白名单 0：黑名单 1：白名单
            oneself = obj.oneself;//自己：true 他人：false
            pricingMethod = obj.pricingMethod;//定价方式：0 点价 1 一口价
            fixedPrice = obj.fixedPrice;//固定价格
            ensureMethod = obj.ensureMethod;//保证方式 0:保证金 1:实货凭证
            contract = obj.contract;//参考合约，例：1709
            warehouseReceiptNum = obj.warehouseReceiptNum;//仓单号
            outSource = obj.outSource;
            outId = obj.outId;
        }

        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                SetProperty(() => this.Id);
            }
        }

        public string SeqNum
        {
            get { return seqNum; }
            set
            {
                seqNum = value;
                SetProperty(() => this.SeqNum);
            }
        }

        public int TransType
        {
            get { return transType; }
            set
            {
                transType = value;
                SetProperty(() => this.TransType);
                SetProperty(() => this.TransTypeInfo);
            }
        }

        /// <summary>
        /// 展示属性
        /// </summary>
        public string TransTypeInfo
        {
            get
            {
                if (Convert.ToInt32(transType) == 0)
                {
                    return Helper.GetDescription(UseOnlineTradingSystem.TransType.Buy);
                }
                else
                {
                    return Helper.GetDescription(UseOnlineTradingSystem.TransType.Sell);
                }
            }
        }

        public string TransTypeName
        {
            get { return transTypeName; }
            set
            {
                transTypeName = value;
                SetProperty(() => this.TransTypeName);
            }
        }

        public string TransStatus
        {
            get { return transStatus; }
            set
            {
                transStatus = value;
                SetProperty(() => this.TransStatus);
                SetProperty(() => this.TransStatusInfo);
            }
        }

        /// <summary>
        /// 展示属性
        /// </summary>
        public string TransStatusInfo
        {
            get
            {
                if (Convert.ToInt32(transStatus) == 1)
                {
                    return Helper.GetDescription(OperationType.PutBrand);
                }
                else if (Convert.ToInt32(transStatus) == 2)
                {
                    return Helper.GetDescription(OperationType.DelistBrand);
                }
                else if (Convert.ToInt32(transStatus) == 3)
                {
                    return Helper.GetDescription(OperationType.LockBrand);
                }
                else
                {
                    return Helper.GetDescription(OperationType.DelistBrand);
                }
            }
        }


        public string TransStatusName
        {
            get { return transStatusName; }
            set
            {
                TransStatusName = value;
                SetProperty(() => this.TransStatusName);
            }
        }

        public string Cid
        {
            get { return cid; }
            set
            {
                cid = value;
                SetProperty(() => this.Cid);
            }
        }

        public string CommBrandId
        {
            get { return commBrandId; }
            set
            {
                commBrandId = value;
                SetProperty(() => this.CommBrandId);
            }
        }

        public string CommLevel
        {
            get { return commLevel; }
            set
            {
                commLevel = value;
                SetProperty(() => this.CommLevel);
            }
        }

        public string CommLevelName
        {
            get { return commLevelName; }
            set
            {
                commLevelName = value;
                SetProperty(() => this.CommLevelName);
            }
        }

        public string Premium
        {
            get { return premium; }
            set
            {
                premium = value;
                SetProperty(() => this.Premium);
            }
        }

        public string CommTotalQuantity
        {
            get { return commTotalQuantity; }
            set
            {
                commTotalQuantity = value;
                SetProperty(() => this.CommTotalQuantity);
            }
        }

        public string CommAvailableQuantity
        {
            get { return commAvailableQuantity; }
            set
            {
                commAvailableQuantity = value;
                SetProperty(() => this.CommAvailableQuantity);
            }
        }

        public string MinDealQuantity
        {
            get { return minDealQuantity; }
            set
            {
                minDealQuantity = value;
                SetProperty(() => this.MinDealQuantity);
            }
        }

        public string CommTotalProportion
        {
            get { return commTotalProportion; }
            set
            {
                commTotalProportion = value;
                SetProperty(() => this.CommTotalProportion);
            }
        }

        public string CommAvailableProportion
        {
            get { return commAvailableProportion; }
            set
            {
                commAvailableProportion = value;
                SetProperty(() => this.CommAvailableProportion);
            }
        }

        public string WarehouseId
        {
            get { return warehouseId; }
            set
            {
                warehouseId = value;
                SetProperty(() => this.WarehouseId);
            }
        }

        public string WarehouseName
        {
            get { return warehouseName; }
            set
            {
                warehouseName = value;
                SetProperty(() => this.WarehouseName);
            }
        }

        public string Publisher
        {
            get { return publisher; }
            set
            {
                publisher = value;
                SetProperty(() => this.Publisher);
            }
        }

        public string PublisherId
        {
            get { return publisherId; }
            set
            {
                publisherId = value;
                SetProperty(() => this.PublisherId);
            }
        }

        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                SetProperty(() => this.Remarks);
            }
        }

        public string PublisherDate
        {
            get { return publisherDate; }
            set
            {
                publisherDate = value;
                SetProperty(() => this.PublisherDate);
            }
        }

        public string CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                SetProperty(() => this.CreatedBy);
            }
        }

        public string BlackWhiteType
        {
            get { return blackWhiteType; }
            set
            {
                blackWhiteType = value;
                SetProperty(() => this.BlackWhiteType);
            }
        }

        public string Oneself
        {
            get { return oneself; }
            set
            {
                oneself = value;
                SetProperty(() => this.Oneself);
            }
        }

        public string PricingMethod
        {
            get { return pricingMethod; }
            set
            {
                pricingMethod = value;
                SetProperty(() => this.PricingMethod);
                SetProperty(() => this.PricingMethodInfo);

            }
        }

        public string PricingMethodInfo
        {
            get
            {
                if (Convert.ToInt32(pricingMethod) == (int)UseOnlineTradingSystem.PricingMethod.SpotPrice)
                {
                    return Helper.GetDescription(UseOnlineTradingSystem.PricingMethod.SpotPrice);
                }
                else
                {
                    return Helper.GetDescription(UseOnlineTradingSystem.PricingMethod.DeadPrice);
                }
            }
        }


        public string FixedPrice
        {
            get { return fixedPrice; }
            set
            {
                fixedPrice = value;
                SetProperty(() => this.FixedPrice);
            }
        }

        public string EnsureMethod
        {
            get { return ensureMethod; }
            set
            {
                ensureMethod = value;
                SetProperty(() => this.EnsureMethod);
            }
        }

        public string Contract
        {
            get { return contract; }
            set
            {
                contract = value;
                SetProperty(() => this.Contract);
            }
        }

        public string WarehouseReceiptNum
        {
            get { return warehouseReceiptNum; }
            set
            {
                warehouseReceiptNum = value;
                SetProperty(() => this.WarehouseReceiptNum);
            }
        }

        public static OneListed CreateModelCommmodiInfo(OneListed info)
        {
            OneListed infoModel = new OneListed();

            infoModel.Id = info.Id;
            infoModel.SeqNum = info.SeqNum;
            infoModel.transType = info.transType;
            infoModel.transTypeName = info.transTypeName;
            infoModel.transStatus = info.transStatus;
            infoModel.transStatusName = info.transStatusName;
            infoModel.Cid = info.Cid;
            infoModel.CommBrandId = info.CommBrandId;
            infoModel.CommLevel = info.CommLevel;
            infoModel.CommLevelName = info.CommLevelName;
            infoModel.Premium = info.Premium;
            infoModel.CommTotalQuantity = info.CommTotalQuantity;
            infoModel.CommAvailableQuantity = info.CommAvailableQuantity;
            infoModel.MinDealQuantity = info.MinDealQuantity;
            infoModel.CommTotalProportion = info.CommTotalProportion;
            infoModel.CommAvailableProportion = info.CommAvailableProportion;
            infoModel.WarehouseId = info.WarehouseId;
            infoModel.WarehouseName = info.WarehouseName;
            infoModel.Publisher = info.Publisher;
            infoModel.PublisherId = info.PublisherId;
            infoModel.Remarks = info.Remarks;
            infoModel.PublisherDate = info.PublisherDate;
            infoModel.CreatedBy = info.CreatedBy;
            infoModel.BlackWhiteType = info.BlackWhiteType;
            infoModel.Oneself = info.Oneself;
            infoModel.PricingMethod = info.PricingMethod;
            infoModel.FixedPrice = info.FixedPrice;
            infoModel.EnsureMethod = info.EnsureMethod;
            infoModel.Contract = info.Contract;
            infoModel.WarehouseReceiptNum = info.WarehouseReceiptNum;
            infoModel.outSource = info.outSource;
            infoModel.outId = info.outId;

            return infoModel;
        }
    }
}
