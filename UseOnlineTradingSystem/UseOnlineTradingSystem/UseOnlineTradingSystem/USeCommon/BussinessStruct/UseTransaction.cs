using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 成交列表响应
    /// </summary>
    public class TransactionListResponse : BaseResponse
    {
        public List<TransactionList> dataList;
    }
    /// <summary>
    /// 成交列表
    /// </summary>
    public class TransactionList
    {
        public string code;
        public string msg;
        public List<Transaction> data;
    }

    /// <summary>
    /// 成交信息
    /// </summary>
    public class Transaction : USeBaseViewModel
    {
        //[{"commId":"690","orderNo":"20171018163507915","transTime":1508315708000,"transType":0,"transTypeName":"采购","commBrandName":"中条山高纯","commLevelName":"平水铜三类","premium":0,"transQuantity":234,"commTotalQuantity":null,"commAvailableQuantity":null,"warehouseName":"上海裕强供应链管理有限公司","oppoCompId":"232","oppoCompName":"郭先堂","oppoPhone":"18016321103","confirmPrice":null,"confirmStatus":"0","confirmStatusInfo":"待双方确认价格","pricingMethod":1,"fixedPrice":12222.0000,"warehouseReceiptNum":null,"contract":null,"ensureMethod":0,"warehouseId":5,"contractName":null,"cid":1},
        public string commId;
        public string orderNo;
        public string transTime;
        public string transType;
        public string transTypeName;
        public string commBrandName;
        public string commLevelName;
        public string premium;
        public string transQuantity;
        public string commTotalQuantity;
        public string commAvailableQuantity;
        public string warehouseName;
        public string oppoCompId;
        public string oppoCompName;
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

        public string FixedPrice
        {
            get { return fixedPrice; }
            set
            {
                fixedPrice = value;
                SetProperty(() => this.FixedPrice);
            }
        }

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


        public string BrandName
        {
            get { return commBrandName; }
            set
            {
                commBrandName = value;
                SetProperty(() => this.BrandName);
            }
        }

        public string Level
        {
            get { return commLevelName; }
            set
            {
                commLevelName = value;
                SetProperty(() => this.Level);
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

        public string Warehouse
        {
            get { return warehouseName; }
            set
            {
                warehouseName = value;
                SetProperty(() => this.Warehouse);
            }
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

        public string TransTypeInfo
        {
            get
            {
                if(transType == "0")
                {
                    return "采购";
                }
                else
                {
                    return "销售";
                }
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

        public string ConfirmPrice
        {
            get { return confirmPrice; }
            set
            {
                confirmPrice = value;
                SetProperty(() => this.ConfirmPrice);
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
