
using System.Collections.Generic;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 摘牌操作的请求
    /// </summary>
    public class DelistingRequest : BaseRequest
    {
        public string cid;
        public string basePrice;//基价
        public string commQuantity;//购买数量(吨)
        public string remarks;//备注
    }

    /// <summary>
    /// 摘牌响应
    /// </summary>
    public class DelistingResponse
    {
        public string code;
        public string msg;

        public Transaction data
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 摘牌信息
    /// </summary>
    public class Delisting : USeBaseViewModel
    {
        public string commId;
        public string orderNo;
        public string transTime;
        public string transType;
        public string transTypeName;
        public string commBrandName;
        public string commLevelName;
        public string premium;
        public string transQuantity;
        public string commAvailableQuantity;
        public string commTotalQuantity;
        public string warehouseName;
        public string oppoCompName;
        public string oppoCompId;
        public string oppoPhone;
        public string confirmPrice;
        public string confirmStatus;
        public string confirmStatusInfo;
        public string pricingMethod;
        public string fixedPrice;
        public string warehouseReceiptNum;
        public string contract;
        public string ensureMethod;
        public string warehouseId;
        public string contractName;
        public string cid;
        public string clientId;
        public string operationType;
        public string confirmStateInfo;


        public string TransTime
        {
            get { return transTime; }
            set
            {
                transTime = value;
                SetProperty(() => this.TransTime);
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
                SetProperty(() => this.CommLevelName);
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

        public string WarehouseName
        {
            get { return warehouseName; }
            set
            {
                warehouseName = value;
                SetProperty(() => this.WarehouseName);
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

        public string ConfirmPrice
        {
            get { return confirmPrice; }
            set
            {
                confirmPrice = value;
                SetProperty(() => this.ConfirmPrice);
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

        public string FixPrice
        {
            get { return fixedPrice; }
            set
            {
                fixedPrice = value;
                SetProperty(() => this.FixPrice);
            }
        }

        public string TransQuantity
        {
            get { return transQuantity; }
            set
            {
                transQuantity = value;
                SetProperty(() => this.TransQuantity);
            }
        }

        public string OppoCompName
        {
            get { return oppoCompName; }
            set
            {
                oppoCompName = value;
                SetProperty(() => this.OppoCompName);
            }
        }

        public string ConfirmStatusInfo
        {
            get { return confirmStatusInfo; }
            set
            {
                confirmStatusInfo = value;
                SetProperty(() => this.ConfirmStatusInfo);
            }
        }
    }


}
