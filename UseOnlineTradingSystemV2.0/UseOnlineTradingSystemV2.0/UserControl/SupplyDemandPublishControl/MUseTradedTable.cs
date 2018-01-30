using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using UseOnlineTradingSystem;
using UseOnlineTradingSystem.Properties;

namespace mPaint
{
    public class MUseTradedTable : MControlBase
    {
        private TableStyleEnum m_tableStyle = TableStyleEnum.Unknow;

        /// <summary>
        /// 鼠标是否按下
        /// </summary>
        private bool isMouseDown = false;

        /// <summary>
        /// 鼠标是否进入
        /// </summary>
        private bool isMouseEnter = false;

        /// <summary>
        /// 记录前一个被操作的控件
        /// </summary>
        internal MControlBase previousControl = null;

        public MControlBase SelectRow = null;
        public event Action<object, Point> MouseRightUpEvent;
        public event Action<object, Point> MouseLeftUpEvent;
        private MLabels tableHeader;//列头
        private MImage image2;//标记图片
        private MImage image3;//提示标记图片内容
        private MLine line1;//表头横线
        private MRectangle mrt;//上表背景
        protected List<UData> m_Data = new List<UData>();//数据行
        private int dataInterval = 24;//数据行之间的间隔
        private MVScrollBar scrollbar;//上滚动条
        protected int offset = 0;//上偏移量

        /// <summary>
        /// 根据表格类型初始化
        /// </summary>
        /// <param name="tableStyle"></param>
        public MUseTradedTable(TableStyleEnum tableStyle)
        {
            m_tableStyle = tableStyle;

            #region 表格
            image3 = new MImage();
            image3.BackgroundImage = Resources.tips;
            image3.Visible = false;

            #region 标题栏
            //列头：标记，序号，采购/销售，等级，升贴水，绝对价格，数量，仓库，公司，备注，发布时间
            tableHeader = new MLabels();
            tableHeader.BackColor = COLOR.RGB(MCommonData.fontColor5);
            tableHeader.MouseBackColor = COLOR.RGB(MCommonData.fontColor5);
            for (int i = 0; i < 11; i++)
            {
                MLabel ml = new MLabel();
                ml.Font = MCommonData.d2Font;
                ml.ForeColor = COLOR.RGB(MCommonData.fontColor4);
                ml.BackColor = -1;
                ml.LeftAligned = true;
                tableHeader.lbs.Add(ml);
            }

            switch (tableStyle)
            {
                case TableStyleEnum.PutBrandTable:
                    tableHeader.lbs[0].Text = "时间";
                    tableHeader.lbs[1].Text = "品牌";
                    tableHeader.lbs[2].Text = "等级";
                    tableHeader.lbs[3].Text = "参考合约";
                    tableHeader.lbs[4].Text = "仓库";
                    tableHeader.lbs[5].Text = "买卖";
                    tableHeader.lbs[6].Text = "升贴水";
                    tableHeader.lbs[7].Text = "绝对价格";
                    tableHeader.lbs[8].Text = "委托量";
                    tableHeader.lbs[9].Text = "可撤";
                    tableHeader.lbs[10].Text = "备注";
                    break;
                case TableStyleEnum.DelistTable:
                    tableHeader.lbs[0].Text = "时间";
                    tableHeader.lbs[1].Text = "品牌";
                    tableHeader.lbs[2].Text = "等级";
                    tableHeader.lbs[3].Text = "参考合约";
                    tableHeader.lbs[4].Text = "仓库";
                    tableHeader.lbs[5].Text = "买卖";
                    tableHeader.lbs[6].Text = "交收盘面价";
                    tableHeader.lbs[7].Text = "升贴水";
                    tableHeader.lbs[8].Text = "绝对价格";
                    tableHeader.lbs[9].Text = "成交量(吨)";
                    tableHeader.lbs[10].Text = "交易对手";
                    tableHeader.lbs[10].Text = "交易描述";
                    break;
                case TableStyleEnum.TradedTable:
                    tableHeader.lbs[0].Text = "时间";
                    tableHeader.lbs[1].Text = "品牌";
                    tableHeader.lbs[2].Text = "等级";
                    tableHeader.lbs[3].Text = "参考合约";
                    tableHeader.lbs[4].Text = "仓库";
                    tableHeader.lbs[5].Text = "买卖";
                    tableHeader.lbs[6].Text = "交收盘面价";
                    tableHeader.lbs[7].Text = "升贴水";
                    tableHeader.lbs[8].Text = "绝对价格";
                    tableHeader.lbs[9].Text = "交易对手";
                    break;
                default:
                    break;
            }

            #region 表格数据

            //横线
            line1 = new MLine();
            line1.Width = 1;
            line1.LineColor = COLOR.RGB(MCommonData.fontColor2);

            #endregion

            //标记图片
            image2 = new MImage();
            image2.BackgroundImage = Resources.mark;

            #endregion

            //上表背景
            mrt = new MRectangle();
            mrt.BackColor = COLOR.RGB(MCommonData.fontColor6);


            #region 滚动条
            scrollbar = new MVScrollBar();
            scrollbar.BackColor = COLOR.RGB(MCommonData.fontColor6);
            scrollbar.MouseBackColor = COLOR.RGB(MCommonData.fontColor6);
            scrollbar.MouseClickBackColor = COLOR.RGB(MCommonData.fontColor6);
            scrollbar.ForeColor = COLOR.RGB(MCommonData.fontColor14);
            scrollbar.MouseForeColor = COLOR.RGB(MCommonData.fontColor15);
            scrollbar.ValueChanged += Scrollbar1_ValueChanged;
            scrollbar.MinInitialized += Scrollbar1_MinInitialized;

            #endregion
            #endregion
        }

        /// <summary>
        /// 根据表的类型切换标题栏
        /// </summary>
        /// <param name="tableStyle"></param>
        public void CheckTableHeader(TableStyleEnum tableStyle)
        {
            //清除表头
            #region 标题栏
            tableHeader.lbs.Clear();

            tableHeader = new MLabels();
            tableHeader.BackColor = COLOR.RGB(MCommonData.fontColor5);
            tableHeader.MouseBackColor = COLOR.RGB(MCommonData.fontColor5);
            for (int i = 0; i < 11; i++)
            {
                MLabel ml = new MLabel();
                ml.Font = MCommonData.d2Font;
                ml.ForeColor = COLOR.RGB(MCommonData.fontColor4);
                ml.BackColor = -1;
                ml.LeftAligned = true;
                tableHeader.lbs.Add(ml);
            }

            switch (tableStyle)
            {
                case TableStyleEnum.PutBrandTable:
                    tableHeader.lbs[0].Text = "时间";
                    tableHeader.lbs[1].Text = "品牌";
                    tableHeader.lbs[2].Text = "等级";
                    tableHeader.lbs[3].Text = "参考合约";
                    tableHeader.lbs[4].Text = "仓库";
                    tableHeader.lbs[5].Text = "买卖";
                    tableHeader.lbs[6].Text = "升贴水";
                    tableHeader.lbs[7].Text = "绝对价格";
                    tableHeader.lbs[8].Text = "委托量";
                    tableHeader.lbs[9].Text = "可撤";
                    tableHeader.lbs[10].Text = "备注";
                    break;
                case TableStyleEnum.DelistTable:
                    tableHeader.lbs[0].Text = "时间";
                    tableHeader.lbs[1].Text = "品牌";
                    tableHeader.lbs[2].Text = "等级";
                    tableHeader.lbs[3].Text = "参考合约";
                    tableHeader.lbs[4].Text = "仓库";
                    tableHeader.lbs[5].Text = "买卖";
                    tableHeader.lbs[6].Text = "交收盘面价";
                    tableHeader.lbs[7].Text = "升贴水";
                    tableHeader.lbs[8].Text = "绝对价格";
                    tableHeader.lbs[9].Text = "成交量(吨)";
                    tableHeader.lbs[10].Text = "交易对手";
                    tableHeader.lbs[10].Text = "交易描述";
                    break;
                case TableStyleEnum.TradedTable:
                    tableHeader.lbs[0].Text = "时间";
                    tableHeader.lbs[1].Text = "品牌";
                    tableHeader.lbs[2].Text = "等级";
                    tableHeader.lbs[3].Text = "参考合约";
                    tableHeader.lbs[4].Text = "仓库";
                    tableHeader.lbs[5].Text = "买卖";
                    tableHeader.lbs[6].Text = "交收盘面价";
                    tableHeader.lbs[7].Text = "升贴水";
                    tableHeader.lbs[8].Text = "绝对价格";
                    tableHeader.lbs[9].Text = "交易对手";
                    break;
                default:
                    break;
            }

            #region 表格数据

            //横线
            line1 = new MLine();
            line1.Width = 1;
            line1.LineColor = COLOR.RGB(MCommonData.fontColor2);

            #endregion

            //标记图片
            image2 = new MImage();
            image2.BackgroundImage = Resources.mark;

            #endregion

            //表头的属性
        }

        #region 添加表数据
        ///// <summary>
        ///// 挂牌数据对象-添加-更新
        ///// </summary>
        ///// <param name="info">数据</param>
        ///// <param name="type">0为买 1为卖</param>
        //public void InsertDataOneListed(SelfListed info)
        //{
        //    // if (info.blackWhiteType != "0")
        //    {
        //        MLabels data = new MLabels();
        //        data.BackColor = COLOR.RGB(MCommonData.fontColor6);
        //        data.MouseBackColor = COLOR.RGB(MCommonData.fontColor10);
        //        UData ud = new UData();
        //        for (int i = 0; i < 10; i++)
        //        {
        //            MLabel ml = new MLabel();
        //            ml.ForeColor = COLOR.RGB(MCommonData.fontColor4);
        //            ml.MouseForeColor = COLOR.RGB(MCommonData.fontColor4);
        //            ml.BackColor = -1;
        //            ml.Font = MCommonData.d4Font;
        //            ml.LeftAligned = true;
        //            data.lbs.Add(ml);
        //        }
        //        data.lbs[6].MouseForeColor = COLOR.RGB(MCommonData.fontColor2);
        //        data.lbs[6].Underline = true;
        //        data.lbs[9].ForeColor = COLOR.RGB(MCommonData.fontColor8);

        //        Data.Add(ud);

        //        SetDataOneListed(info, data, ud);
        //    }
        //}

        //private void SetDataOneListed(SelfListed info, MLabels data, UData ud)
        //{
        //    if (info == null || data == null || ud == null || data.lbs == null || data.lbs.Count == 0) return;
        //    data.lbs[0].Text = info.transTypeName;
        //    data.lbs[1].Text = info.commLevelName;
        //    if (string.IsNullOrWhiteSpace(info.contract))
        //    {
        //        data.lbs[2].Text = "--";
        //    }
        //    else
        //    {
        //        data.lbs[2].Text = info.contract;
        //    }
        //    if (string.IsNullOrWhiteSpace(info.premium))
        //    {
        //        data.lbs[3].Text = "--";
        //    }
        //    else
        //    {
        //        data.lbs[3].Text = info.premium;
        //    }
        //    if (info.pricingMethod == "0" && info.premium != null)
        //    {
        //        var vvv = DataManager.Instance.GetContractLastPrice(info.contract);
        //        if (vvv != null)
        //        {
        //            int premium;
        //            int.TryParse(info.premium, out premium);
        //            if (premium != 0)
        //            {
        //                info.fixedPrice = (vvv.bidPrice + premium).ToString();
        //            }
        //        }
        //    }

        //    decimal money;
        //    decimal.TryParse(info.fixedPrice, out money);
        //    if (money > 0)
        //    {
        //        data.lbs[4].Text = string.Format("{0:C}", money);
        //    }
        //    else
        //    {
        //        data.lbs[4].Text = "";
        //    }
        //    data.lbs[5].Text = info.commAvailableQuantity;
        //    string text = info.warehouseName;
        //    if (text != null && text.Length >= 6)
        //    {
        //        text = text.Substring(0, 6) + "...";
        //    }
        //    data.lbs[6].Text = text;
        //    data.lbs[7].Text = info.publisher;
        //    data.lbs[8].Text = info.remarks;
        //    long l;
        //    long.TryParse(info.publisherDate, out l);
        //    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //    data.lbs[9].Text = start.AddMilliseconds(l).ToLocalTime().ToString("HH:mm:ss");

        //    if (DataManager.Instance.LoginData != null)
        //    {
        //        ud.data1image = new MImage();

        //        if (info.createdBy == DataManager.Instance.LoginData.id)
        //        {
        //            ud.data1image.BackgroundImage = Resources.icon_publish;
        //        }
        //        else if (DataManager.Instance.WhiteEnable && DataManager.Instance.WhiteDY != null && info.publisher != null && DataManager.Instance.WhiteDY.ContainsKey(info.publisher))
        //        {
        //            ud.data1image.BackgroundImage = Resources.icon_white;
        //        }
        //        else if (DataManager.Instance.BlackEnable && DataManager.Instance.BlackDY != null && info.publisher != null && DataManager.Instance.BlackDY.ContainsKey(info.publisher))
        //        {
        //            ud.data1image.BackgroundImage = Resources.icon_black;
        //        }
        //        else
        //        {
        //            ud.data1image = null;
        //        }
        //    }
        //    data.Tag = ud;
        //    ud.obj = info;
        //    ud.data = data;
        //    ud.house = data.lbs[6];//仓库信息

        //    scrollbar.CalculationSliderWith(Data.Count * dataInterval);

        //}

        //public void UpdataOneListed(SelfListed info)
        //{
        //    if (info == null) return;
        //    for (int i = 0; i < Data.Count; i++)
        //    {
        //        var v = Data[i];
        //        OneListed ci = v.obj as OneListed;
        //        if (ci != null && info.id == ci.id)
        //        {
        //            ci.Update(info);
        //            if (ci.blackWhiteType == "0" || (ci.transStatus == "2" && ci.commAvailableQuantity == "0") || ci.transStatus == "3" || ci.transStatus == "4")
        //            {
        //                Data.Remove(v);
        //            }
        //            else
        //            {
        //                SetData(ci, v.data, v);
        //            }
        //            return;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 摘牌数据对象-添加-更新
        ///// </summary>
        ///// <param name="info"></param>
        //public void InsertDataDelist(Transaction info)
        //{
        //    // if (info.blackWhiteType != "0")
        //    {
        //        MLabels data = new MLabels();
        //        data.BackColor = COLOR.RGB(MCommonData.fontColor6);
        //        data.MouseBackColor = COLOR.RGB(MCommonData.fontColor10);
        //        UData ud = new UData();
        //        for (int i = 0; i < 10; i++)
        //        {
        //            MLabel ml = new MLabel();
        //            ml.ForeColor = COLOR.RGB(MCommonData.fontColor4);
        //            ml.MouseForeColor = COLOR.RGB(MCommonData.fontColor4);
        //            ml.BackColor = -1;
        //            ml.Font = MCommonData.d4Font;
        //            ml.LeftAligned = true;
        //            data.lbs.Add(ml);
        //        }
        //        data.lbs[6].MouseForeColor = COLOR.RGB(MCommonData.fontColor2);
        //        data.lbs[6].Underline = true;
        //        data.lbs[9].ForeColor = COLOR.RGB(MCommonData.fontColor8);

        //        Data.Add(ud);

        //        SetData(info, data, ud);
        //    }
        //}

        //private void SetDataDelisted(Transaction info, MLabels data, UData ud)
        //{
        //    if (info == null || data == null || ud == null || data.lbs == null || data.lbs.Count == 0) return;
        //    data.lbs[0].Text = info.transTypeName;
        //    data.lbs[1].Text = info.commLevelName;
        //    if (string.IsNullOrWhiteSpace(info.contract))
        //    {
        //        data.lbs[2].Text = "--";
        //    }
        //    else
        //    {
        //        data.lbs[2].Text = info.contract;
        //    }
        //    if (string.IsNullOrWhiteSpace(info.premium))
        //    {
        //        data.lbs[3].Text = "--";
        //    }
        //    else
        //    {
        //        data.lbs[3].Text = info.premium;
        //    }
        //    if (info.pricingMethod == "0" && info.premium != null)
        //    {
        //        var vvv = DataManager.Instance.GetContractLastPrice(info.contract);
        //        if (vvv != null)
        //        {
        //            int premium;
        //            int.TryParse(info.premium, out premium);
        //            if (premium != 0)
        //            {
        //                info.fixedPrice = (vvv.bidPrice + premium).ToString();
        //            }
        //        }
        //    }

        //    decimal money;
        //    decimal.TryParse(info.fixedPrice, out money);
        //    if (money > 0)
        //    {
        //        data.lbs[4].Text = string.Format("{0:C}", money);
        //    }
        //    else
        //    {
        //        data.lbs[4].Text = "";
        //    }
        //    data.lbs[5].Text = info.commAvailableQuantity;
        //    string text = info.warehouseName;
        //    if (text != null && text.Length >= 6)
        //    {
        //        text = text.Substring(0, 6) + "...";
        //    }
        //    data.lbs[6].Text = text;
        //    data.lbs[7].Text = info.publisher;
        //    data.lbs[8].Text = info.remarks;
        //    long l;
        //    long.TryParse(info.publisherDate, out l);
        //    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //    data.lbs[9].Text = start.AddMilliseconds(l).ToLocalTime().ToString("HH:mm:ss");

        //    if (DataManager.Instance.LoginData != null)
        //    {
        //        ud.data1image = new MImage();

        //        if (info.createdBy == DataManager.Instance.LoginData.id)
        //        {
        //            ud.data1image.BackgroundImage = Resources.icon_publish;
        //        }
        //        else if (DataManager.Instance.WhiteEnable && DataManager.Instance.WhiteDY != null && info.publisher != null && DataManager.Instance.WhiteDY.ContainsKey(info.publisher))
        //        {
        //            ud.data1image.BackgroundImage = Resources.icon_white;
        //        }
        //        else if (DataManager.Instance.BlackEnable && DataManager.Instance.BlackDY != null && info.publisher != null && DataManager.Instance.BlackDY.ContainsKey(info.publisher))
        //        {
        //            ud.data1image.BackgroundImage = Resources.icon_black;
        //        }
        //        else
        //        {
        //            ud.data1image = null;
        //        }
        //    }
        //    data.Tag = ud;
        //    ud.obj = info;
        //    ud.data = data;
        //    ud.house = data.lbs[6];//仓库信息

        //    scrollbar.CalculationSliderWith(Data.Count * dataInterval);

        //}

        //public void UpdataDelisted(Transaction info)
        //{
        //    if (info == null) return;
        //    for (int i = 0; i < Data.Count; i++)
        //    {
        //        var v = Data[i];
        //        OneListed ci = v.obj as OneListed;
        //        if (ci != null && info.id == ci.id)
        //        {
        //            ci.Update(info);
        //            if (ci.blackWhiteType == "0" || (ci.transStatus == "2" && ci.commAvailableQuantity == "0") || ci.transStatus == "3" || ci.transStatus == "4")
        //            {
        //                Data.Remove(v);
        //            }
        //            else
        //            {
        //                SetData(ci, v.data, v);
        //            }
        //            return;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 成交数据对象-添加-更新
        ///// </summary>
        ///// <param name="info"></param>
        //public void InsertDataTraded(Transaction info)
        //{
        //    // if (info.blackWhiteType != "0")
        //    {
        //        MLabels data = new MLabels();
        //        data.BackColor = COLOR.RGB(MCommonData.fontColor6);
        //        data.MouseBackColor = COLOR.RGB(MCommonData.fontColor10);
        //        UData ud = new UData();
        //        for (int i = 0; i < 10; i++)
        //        {
        //            MLabel ml = new MLabel();
        //            ml.ForeColor = COLOR.RGB(MCommonData.fontColor4);
        //            ml.MouseForeColor = COLOR.RGB(MCommonData.fontColor4);
        //            ml.BackColor = -1;
        //            ml.Font = MCommonData.d4Font;
        //            ml.LeftAligned = true;
        //            data.lbs.Add(ml);
        //        }
        //        data.lbs[6].MouseForeColor = COLOR.RGB(MCommonData.fontColor2);
        //        data.lbs[6].Underline = true;
        //        data.lbs[9].ForeColor = COLOR.RGB(MCommonData.fontColor8);

        //        Data.Add(ud);

        //        SetData(info, data, ud);
        //    }
        //}

        //private void SetDataTraded(Transaction info, MLabels data, UData ud)
        //{
        //    if (info == null || data == null || ud == null || data.lbs == null || data.lbs.Count == 0) return;
        //    data.lbs[0].Text = info.transTypeName;
        //    data.lbs[1].Text = info.commLevelName;
        //    if (string.IsNullOrWhiteSpace(info.contract))
        //    {
        //        data.lbs[2].Text = "--";
        //    }
        //    else
        //    {
        //        data.lbs[2].Text = info.contract;
        //    }
        //    if (string.IsNullOrWhiteSpace(info.premium))
        //    {
        //        data.lbs[3].Text = "--";
        //    }
        //    else
        //    {
        //        data.lbs[3].Text = info.premium;
        //    }
        //    if (info.pricingMethod == "0" && info.premium != null)
        //    {
        //        var vvv = DataManager.Instance.GetContractLastPrice(info.contract);
        //        if (vvv != null)
        //        {
        //            int premium;
        //            int.TryParse(info.premium, out premium);
        //            if (premium != 0)
        //            {
        //                info.fixedPrice = (vvv.bidPrice + premium).ToString();
        //            }
        //        }
        //    }

        //    decimal money;
        //    decimal.TryParse(info.fixedPrice, out money);
        //    if (money > 0)
        //    {
        //        data.lbs[4].Text = string.Format("{0:C}", money);
        //    }
        //    else
        //    {
        //        data.lbs[4].Text = "";
        //    }
        //    data.lbs[5].Text = info.commAvailableQuantity;
        //    string text = info.warehouseName;
        //    if (text != null && text.Length >= 6)
        //    {
        //        text = text.Substring(0, 6) + "...";
        //    }
        //    data.lbs[6].Text = text;
        //    data.lbs[7].Text = info.publisher;
        //    data.lbs[8].Text = info.remarks;
        //    long l;
        //    long.TryParse(info.publisherDate, out l);
        //    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //    data.lbs[9].Text = start.AddMilliseconds(l).ToLocalTime().ToString("HH:mm:ss");

        //    if (DataManager.Instance.LoginData != null)
        //    {
        //        ud.data1image = new MImage();

        //        if (info.createdBy == DataManager.Instance.LoginData.id)
        //        {
        //            ud.data1image.BackgroundImage = Resources.icon_publish;
        //        }
        //        else if (DataManager.Instance.WhiteEnable && DataManager.Instance.WhiteDY != null && info.publisher != null && DataManager.Instance.WhiteDY.ContainsKey(info.publisher))
        //        {
        //            ud.data1image.BackgroundImage = Resources.icon_white;
        //        }
        //        else if (DataManager.Instance.BlackEnable && DataManager.Instance.BlackDY != null && info.publisher != null && DataManager.Instance.BlackDY.ContainsKey(info.publisher))
        //        {
        //            ud.data1image.BackgroundImage = Resources.icon_black;
        //        }
        //        else
        //        {
        //            ud.data1image = null;
        //        }
        //    }
        //    data.Tag = ud;
        //    ud.obj = info;
        //    ud.data = data;
        //    ud.house = data.lbs[6];//仓库信息

        //    scrollbar.CalculationSliderWith(Data.Count * dataInterval);

        //}

        //public void UpdataTraded(Transaction info)
        //{
        //    if (info == null) return;
        //    for (int i = 0; i < Data.Count; i++)
        //    {
        //        var v = Data[i];
        //        OneListed ci = v.obj as OneListed;
        //        if (ci != null && info.id == ci.id)
        //        {
        //            ci.Update(info);
        //            if (ci.blackWhiteType == "0" || (ci.transStatus == "2" && ci.commAvailableQuantity == "0") || ci.transStatus == "3" || ci.transStatus == "4")
        //            {
        //                Data.Remove(v);
        //            }
        //            else
        //            {
        //                SetData(ci, v.data, v);
        //            }
        //            return;
        //        }
        //    }
        //}

        #endregion

        /// <summary>
        /// 重置为最小初始状态
        /// </summary>
        private void Scrollbar1_MinInitialized()
        {
            offset = 0;
        }

        /// <summary>
        /// 滚动条值变化
        /// </summary>
        /// <param name="obj"></param>
        private void Scrollbar1_ValueChanged(double obj)
        {
            if (obj != 0)
            {
                offset -= (int)(((m_Data.Count) * dataInterval - (scrollbar.Rectangle.bottom - scrollbar.Rectangle.top)) * obj) / 100;
            }
        }

        /// <summary>
        /// 计算各个元素的坐标
        /// </summary>
        /// <param name="offset">偏移量</param>
        private void CalculationTable()
        {
            int avg = 70;//表头分隔权值
            int interval = (Rectangle.right - Rectangle.left - avg * 11) / 10;//表头分割值
            int lbY = Rectangle.top + 10;//列头文字Y坐标

            int dataWith = 100;//数据文字宽度
            int of = 0;//数据左右偏移量

            #region 表头

            //表头背景绘图区域设置
            tableHeader.Rectangle = new RECT(Rectangle.left, Rectangle.top, Rectangle.right, Rectangle.top + 45);

            //列头位置
            //标记
            int lb1x = Rectangle.left + 10;//列头开始位置Y坐标
            //标记坐标
            tableHeader.lbs[0].Rectangle = new RECT(lb1x + 10, lbY, lb1x + 40, lbY + 20);
            //标记贴图位置
            int image2x = tableHeader.lbs[0].Rectangle.right + 1;
            int image2y = tableHeader.lbs[0].Rectangle.top + 2;
            image2.Rectangle = new RECT(image2x - 2, image2y, image2x + image2.BackgroundImage.Width - 5, image2y + image2.BackgroundImage.Height - 4);
            //其余表头文字区域设置
            for (int i = 1; i < tableHeader.lbs.Count; i++)
            {
                int tableHeaderX = tableHeader.lbs[i - 1].Rectangle.right + interval;
                tableHeader.lbs[i].Rectangle = new RECT(tableHeaderX + 10, lbY, tableHeaderX + 70, lbY + 20);
            }
            #endregion

            #region 数据

            int dataChoose1Y = 0;

            //卖
            int scrollbarY1 = tableHeader.Rectangle.bottom;
            //分割线
            int scrollbarfg = scrollbarY1 + (int)(0.6 * (Rectangle.bottom - scrollbarY1 - 60));
            //买
            int scrollbarY2 = Rectangle.bottom - 60;
            //上表的背景绘图区域设置 
            mrt.Rectangle = new RECT(Rectangle.left, scrollbarY1, Rectangle.right, scrollbarfg);

            int dataX1 = mrt.Rectangle.left;//左边
            int dataX2 = mrt.Rectangle.right;//右边

            bool needScrollbar = false;

            #region 分割线
            //买卖分割横线位置
            int lineY = dataChoose1Y + offset;//买卖分割横线Y坐标
            //不能超过分割线
            if (lineY > scrollbarfg)
            {
                lineY = scrollbarfg;
            }
            line1.FirstPoint = new POINT(dataX1, lineY);
            line1.SecondPoint = new POINT(dataX2, lineY);
            #endregion

            #region 买
            int data2y = 0;//数据行中文字的Y坐标
            int dataChoose2Y = 0;//是否选中背景颜色的Y坐标
            int offset3 = 0;
            //买
            foreach (var v in m_Data)
            {
                int TopY = dataChoose2Y + offset;//数据（买）数据绘图区域上部Y坐标
                int bottomY = dataChoose2Y + dataInterval + offset; //数据（买）数据绘图区域下部Y坐标
                if (bottomY > scrollbarY2)
                {
                    needScrollbar = true;
                }
                if (TopY < 0 || TopY > scrollbarY2)
                {
                    //无需绘制，隐藏
                    v.Hide();
                }
                else
                {
                    //需要绘制显示
                    if (offset3 == 0)
                    {
                        //第一个要显示的位置
                        offset3 = v.data.Rectangle.top - 0;
                        if (offset3 < 0)
                        {
                            offset3 = 0;
                        }
                    }
                    v.Show();
                }
                //数据（买）背景绘制位置
                v.data.Rectangle = new RECT(dataX1, TopY, dataX2, bottomY);
                //数据（买）图片绘制位置
                if (v.data1image != null)
                {
                    int data1imagex = tableHeader.lbs[0].Rectangle.left + of;
                    if (v.data1image != null)
                    {
                        v.data1image.Rectangle = new RECT(data1imagex + 5, data2y + 5 + offset, data1imagex + 5 + 15, data2y + 5 + 15 + offset);
                    }
                    else
                    {
                        v.data1image.Rectangle = new RECT(data1imagex - 10 + 10, data2y + 5 + offset, data1imagex + 10 + 15, data2y + 5 + 15 + offset);
                    }
                }
                //数据（买）文字绘制位置
                for (int i = 0; i < v.data.lbs.Count; i++)
                {
                    //if (i + 1 > v.data.lbs.Count) break;

                    int data2x = tableHeader.lbs[i].Rectangle.left + of;
                    v.data.lbs[i].Rectangle = new RECT(data2x, data2y + offset + 50, data2x + dataWith, data2y + 20 + offset + 50);
                }
                data2y += dataInterval;
                dataChoose2Y += dataInterval;
            }
            #endregion

            #endregion

            #region 滚动条
            if (needScrollbar)
            {
                scrollbar.Visible = true;
                scrollbar.Rectangle = new RECT(dataX2 - 10, scrollbarY1, dataX2, 0);
                scrollbar.ChangeRectangle();
            }
            else
            {
                scrollbar.Visible = false;
            }

            #endregion
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void ClearData()
        {
            m_Data.Clear();
            scrollbar.CalculationSliderWith(m_Data.Count * dataInterval);
        }

        #region 添加-挂牌-摘牌-成交数据
        //挂牌
        public void InsertPutBrandData(SelfListed info)
        {

            MLabels data = new MLabels();
            data.BackColor = COLOR.RGB(MCommonData.fontColor6);
            data.MouseBackColor = COLOR.RGB(MCommonData.fontColor10);
            UData ud = new UData();
            
            for (int i = 0; i < 11; i++)
            {
                MLabel ml = new MLabel();
                ml.ForeColor = COLOR.RGB(MCommonData.fontColor4);
                ml.MouseForeColor = COLOR.RGB(MCommonData.fontColor4);
                ml.BackColor = -1;
                ml.Font = MCommonData.d4Font;
                ml.LeftAligned = true;
                ml.Visible = Visible;
                data.lbs.Add(ml);
            }
            data.lbs[6].MouseForeColor = COLOR.RGB(MCommonData.fontColor2);
            data.lbs[6].Underline = true;
            data.lbs[9].ForeColor = COLOR.RGB(MCommonData.fontColor8);

            m_Data.Add(ud);

            SetPutBrandData(info, data, ud);
        }
        private void SetPutBrandData(SelfListed info, MLabels data, UData ud)
        {
            if (info == null || data == null || ud == null || data.lbs == null || data.lbs.Count == 0) return;
            data.lbs[0].Text = info.transTime;//时间
            data.lbs[1].Text = info.commBrandName;//品牌
            data.lbs[2].Text = info.commLevelName;//等级

            //参考合约
            if (string.IsNullOrWhiteSpace(info.contract))
            {
                data.lbs[3].Text = "--";
            }
            else
            {
                data.lbs[3].Text = info.contract;
            }

            //仓库
            if (string.IsNullOrWhiteSpace(info.warehouseName))
            {
                data.lbs[4].Text = "--";
            }
            else
            {
                data.lbs[4].Text = info.warehouseName;
            }

            //买卖
            data.lbs[5].Text = info.transType;

            //升贴水
            data.lbs[5].Text = info.premium;

            //绝对价格
            data.lbs[5].Text = info.fixedPrice;

            //委托量
            data.lbs[5].Text = info.commAvailableQuantity;

            //可撤
            data.lbs[5].Text = info.commAvailableQuantity;

            //备注
            data.lbs[5].Text = info.remarks;

            //if (info.pricingMethod == "0" && info.premium != null)
            //{
            //    var vvv = DataManager.Instance.GetContractLastPrice(info.contract);
            //    if (vvv != null)
            //    {
            //        int premium;
            //        int.TryParse(info.premium, out premium);
            //        if (premium != 0)
            //        {

            //            info.fixedPrice = (vvv.bidPrice + premium).ToString();

            //        }
            //    }
            //}

            //decimal money;
            //decimal.TryParse(info.fixedPrice, out money);
            //if (money > 0)
            //{
            //    data.lbs[4].Text = string.Format("{0:C}", money);
            //}
            //else
            //{
            //    data.lbs[4].Text = "";
            //}
            //data.lbs[5].Text = info.commAvailableQuantity;
            //string text = info.warehouseName;
            //if (text != null && text.Length >= 6)
            //{
            //    text = text.Substring(0, 6) + "...";
            //}
            //data.lbs[6].Text = text;
            //data.lbs[7].Text = info.publisher;
            //data.lbs[8].Text = info.remarks;
            //long l;
            //long.TryParse(info.publisherDate, out l);
            //DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //data.lbs[9].Text = start.AddMilliseconds(l).ToLocalTime().ToString("HH:mm:ss");

            //if (DataManager.Instance.LoginData != null)
            //{
            //    ud.data1image = new MImage();

            //    if (info.createdBy == DataManager.Instance.LoginData.id)
            //    {
            //        ud.data1image.BackgroundImage = Resources.icon_publish;
            //    }
            //    else if (DataManager.Instance.WhiteEnable && DataManager.Instance.WhiteDY != null && info.publisher != null && DataManager.Instance.WhiteDY.ContainsKey(info.publisher))
            //    {
            //        ud.data1image.BackgroundImage = Resources.icon_white;
            //    }
            //    else if (DataManager.Instance.BlackEnable && DataManager.Instance.BlackDY != null && info.publisher != null && DataManager.Instance.BlackDY.ContainsKey(info.publisher))
            //    {
            //        ud.data1image.BackgroundImage = Resources.icon_black;
            //    }
            //    else
            //    {
            //        ud.data1image = null;
            //    }
            //}
            data.Tag = ud;
            ud.obj = info;
            ud.data = data;
           
            //ud.house = data.lbs[6];//仓库信息

            scrollbar.CalculationSliderWith(m_Data.Count * dataInterval);

        }

        //摘牌
        public void InsertDelistData(Transaction info)
        {

            MLabels data = new MLabels();
            data.BackColor = COLOR.RGB(MCommonData.fontColor6);
            data.MouseBackColor = COLOR.RGB(MCommonData.fontColor10);
            UData ud = new UData();

            for (int i = 0; i < 11; i++)
            {
                MLabel ml = new MLabel();
                ml.ForeColor = COLOR.RGB(MCommonData.fontColor4);
                ml.MouseForeColor = COLOR.RGB(MCommonData.fontColor4);
                ml.BackColor = -1;
                ml.Font = MCommonData.d4Font;
                ml.LeftAligned = true;
                ml.Visible = Visible;
                data.lbs.Add(ml);
            }
            data.lbs[6].MouseForeColor = COLOR.RGB(MCommonData.fontColor2);
            data.lbs[6].Underline = true;
            data.lbs[9].ForeColor = COLOR.RGB(MCommonData.fontColor8);

            m_Data.Add(ud);

            SetDelistData(info, data, ud);
        }
        private void SetDelistData(Transaction info, MLabels data, UData ud)
        {
            if (info == null || data == null || ud == null || data.lbs == null || data.lbs.Count == 0) return;
            data.lbs[0].Text = info.transTime;//时间
            data.lbs[1].Text = info.commBrandName;//品牌
            data.lbs[2].Text = info.commLevelName;//等级

            //参考合约
            if (string.IsNullOrWhiteSpace(info.contract))
            {
                data.lbs[3].Text = "--";
            }
            else
            {
                data.lbs[3].Text = info.contract;
            }

            //仓库
            if (string.IsNullOrWhiteSpace(info.warehouseName))
            {
                data.lbs[4].Text = "--";
            }
            else
            {
                data.lbs[4].Text = info.warehouseName;
            }

            //买卖
            data.lbs[5].Text = info.transType;

            //升贴水
            data.lbs[6].Text = info.premium;

            //绝对价格
            data.lbs[7].Text = info.fixedPrice;

            //委托量
            data.lbs[8].Text = info.commAvailableQuantity;

            //可撤
            data.lbs[9].Text = info.commAvailableQuantity;

            //备注
            //data.lbs[10].Text = info.remarks;

            //if (info.pricingMethod == "0" && info.premium != null)
            //{
            //    var vvv = DataManager.Instance.GetContractLastPrice(info.contract);
            //    if (vvv != null)
            //    {
            //        int premium;
            //        int.TryParse(info.premium, out premium);
            //        if (premium != 0)
            //        {

            //            info.fixedPrice = (vvv.bidPrice + premium).ToString();

            //        }
            //    }
            //}

            //decimal money;
            //decimal.TryParse(info.fixedPrice, out money);
            //if (money > 0)
            //{
            //    data.lbs[4].Text = string.Format("{0:C}", money);
            //}
            //else
            //{
            //    data.lbs[4].Text = "";
            //}
            //data.lbs[5].Text = info.commAvailableQuantity;
            //string text = info.warehouseName;
            //if (text != null && text.Length >= 6)
            //{
            //    text = text.Substring(0, 6) + "...";
            //}
            //data.lbs[6].Text = text;
            //data.lbs[7].Text = info.publisher;
            //data.lbs[8].Text = info.remarks;
            //long l;
            //long.TryParse(info.publisherDate, out l);
            //DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //data.lbs[9].Text = start.AddMilliseconds(l).ToLocalTime().ToString("HH:mm:ss");

            //if (DataManager.Instance.LoginData != null)
            //{
            //    ud.data1image = new MImage();

            //    if (info.createdBy == DataManager.Instance.LoginData.id)
            //    {
            //        ud.data1image.BackgroundImage = Resources.icon_publish;
            //    }
            //    else if (DataManager.Instance.WhiteEnable && DataManager.Instance.WhiteDY != null && info.publisher != null && DataManager.Instance.WhiteDY.ContainsKey(info.publisher))
            //    {
            //        ud.data1image.BackgroundImage = Resources.icon_white;
            //    }
            //    else if (DataManager.Instance.BlackEnable && DataManager.Instance.BlackDY != null && info.publisher != null && DataManager.Instance.BlackDY.ContainsKey(info.publisher))
            //    {
            //        ud.data1image.BackgroundImage = Resources.icon_black;
            //    }
            //    else
            //    {
            //        ud.data1image = null;
            //    }
            //}
            data.Tag = ud;
            ud.obj = info;
            ud.data = data;

            //ud.house = data.lbs[6];//仓库信息

            scrollbar.CalculationSliderWith(m_Data.Count * dataInterval);

        }

        //成交
        public void InsertTradedData(Transaction info)
        {

            MLabels data = new MLabels();
            data.BackColor = COLOR.RGB(MCommonData.fontColor6);
            data.MouseBackColor = COLOR.RGB(MCommonData.fontColor10);
            UData ud = new UData();

            for (int i = 0; i < 11; i++)
            {
                MLabel ml = new MLabel();
                ml.ForeColor = COLOR.RGB(MCommonData.fontColor4);
                ml.MouseForeColor = COLOR.RGB(MCommonData.fontColor4);
                ml.BackColor = -1;
                ml.Font = MCommonData.d4Font;
                ml.LeftAligned = true;
                ml.Visible = Visible;
                data.lbs.Add(ml);
            }
            data.lbs[6].MouseForeColor = COLOR.RGB(MCommonData.fontColor2);
            data.lbs[6].Underline = true;
            data.lbs[9].ForeColor = COLOR.RGB(MCommonData.fontColor8);

            m_Data.Add(ud);

            SetTradedData(info, data, ud);
        }
        private void SetTradedData(Transaction info, MLabels data, UData ud)
        {
            if (info == null || data == null || ud == null || data.lbs == null || data.lbs.Count == 0) return;
            data.lbs[0].Text = info.transTime;//时间
            data.lbs[1].Text = info.commBrandName;//品牌
            data.lbs[2].Text = info.commLevelName;//等级

            //参考合约
            if (string.IsNullOrWhiteSpace(info.contract))
            {
                data.lbs[3].Text = "--";
            }
            else
            {
                data.lbs[3].Text = info.contract;
            }

            //仓库
            if (string.IsNullOrWhiteSpace(info.warehouseName))
            {
                data.lbs[4].Text = "--";
            }
            else
            {
                data.lbs[4].Text = info.warehouseName;
            }

            //买卖
            data.lbs[5].Text = info.transType;

            //升贴水
            data.lbs[6].Text = info.premium;

            //绝对价格
            data.lbs[7].Text = info.fixedPrice;

            //委托量
            data.lbs[8].Text = info.commAvailableQuantity;

            //可撤
            data.lbs[9].Text = info.commAvailableQuantity;

            //备注
            //data.lbs[10].Text = info.remarks;

            //if (info.pricingMethod == "0" && info.premium != null)
            //{
            //    var vvv = DataManager.Instance.GetContractLastPrice(info.contract);
            //    if (vvv != null)
            //    {
            //        int premium;
            //        int.TryParse(info.premium, out premium);
            //        if (premium != 0)
            //        {

            //            info.fixedPrice = (vvv.bidPrice + premium).ToString();

            //        }
            //    }
            //}

            //decimal money;
            //decimal.TryParse(info.fixedPrice, out money);
            //if (money > 0)
            //{
            //    data.lbs[4].Text = string.Format("{0:C}", money);
            //}
            //else
            //{
            //    data.lbs[4].Text = "";
            //}
            //data.lbs[5].Text = info.commAvailableQuantity;
            //string text = info.warehouseName;
            //if (text != null && text.Length >= 6)
            //{
            //    text = text.Substring(0, 6) + "...";
            //}
            //data.lbs[6].Text = text;
            //data.lbs[7].Text = info.publisher;
            //data.lbs[8].Text = info.remarks;
            //long l;
            //long.TryParse(info.publisherDate, out l);
            //DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //data.lbs[9].Text = start.AddMilliseconds(l).ToLocalTime().ToString("HH:mm:ss");

            //if (DataManager.Instance.LoginData != null)
            //{
            //    ud.data1image = new MImage();

            //    if (info.createdBy == DataManager.Instance.LoginData.id)
            //    {
            //        ud.data1image.BackgroundImage = Resources.icon_publish;
            //    }
            //    else if (DataManager.Instance.WhiteEnable && DataManager.Instance.WhiteDY != null && info.publisher != null && DataManager.Instance.WhiteDY.ContainsKey(info.publisher))
            //    {
            //        ud.data1image.BackgroundImage = Resources.icon_white;
            //    }
            //    else if (DataManager.Instance.BlackEnable && DataManager.Instance.BlackDY != null && info.publisher != null && DataManager.Instance.BlackDY.ContainsKey(info.publisher))
            //    {
            //        ud.data1image.BackgroundImage = Resources.icon_black;
            //    }
            //    else
            //    {
            //        ud.data1image = null;
            //    }
            //}
            data.Tag = ud;
            ud.obj = info;
            ud.data = data;

            //ud.house = data.lbs[6];//仓库信息

            scrollbar.CalculationSliderWith(m_Data.Count * dataInterval);

        }
        #endregion


        #region 鼠标-重绘-滚动消息
        public void Initialization()
        {
            offset = 0;
        }

        protected override void OnPaint(MPaintEventArgs e)
        {
            CalculationTable();

            #region 绘制数据
            mrt.DoPaint(e);
            //绘买
            for (int i = 0; i < m_Data.Count; i++)
            {
                var v = m_Data[i];
                if (v.data.Visible)
                {
                    v.data.DoPaint(e);
                    if (v.data1image != null)
                    {
                        v.data1image.DoPaint(e);
                    }
                }
            }
            ////恢复绘图
            //RECT rect = new RECT(500, 100, 300, 200);
            //e.CPaint.DrawImage(bmp,rect);
            #endregion

            tableHeader.DoPaint(e);
            image2.DoPaint(e);
            line1.DoPaint(e);
            if (scrollbar.Visible)
            {
                scrollbar.DoPaint(e);
            }

            if (image3.Visible)
            {
                image3.DoPaint(e);
            }
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MMouseEventArgs e)
        {
            if (!this.Enable)
                return;
            if (e.MouseEventArgs.Button == MouseButtons.Left)
            {
                this.isMouseDown = true;
                if (scrollbar != null)
                {
                    if (scrollbar.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                    {
                        scrollbar.DoMouseDown(e);
                    }
                }
            }
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MMouseEventArgs e)
        {
            if (!this.Enable)
                return;
            if (e.MouseEventArgs.Button == MouseButtons.Left)
            {
                this.isMouseDown = false;
                if (image3 != null)
                {
                    if (!image2.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                    {
                        image3.Visible = false;
                    }
                    else
                    {
                        image3.Visible = true;
                        int image3X = e.MouseEventArgs.X;
                        int image3Y = e.MouseEventArgs.Y;
                        image3.Rectangle = new RECT(image3X + 10, e.MouseEventArgs.Y + 5, image3Y + image3.BackgroundImage.Width / 2 + 10, image3Y + image3.BackgroundImage.Height / 2 + 5);
                    }
                }
                if (scrollbar != null)
                {
                    scrollbar.DoMouseUp(e);
                }

                if (SelectRow != null)
                {
                    UData ud = SelectRow.Tag as UData;
                    if (ud != null && ud.house != null)
                    {
                        if (ud.house.Rectangle.Contains(e.MouseEventArgs.Location))
                        {
                            MouseLeftUpEvent?.Invoke(ud.obj, e.MouseEventArgs.Location);
                        }
                    }
                }
            }
            else if (e.MouseEventArgs.Button == MouseButtons.Right)
            {
                if (SelectRow != null)
                {
                    UData ud = SelectRow.Tag as UData;
                    if (ud != null)
                    {
                        MouseRightUpEvent?.Invoke(ud.obj, e.MouseEventArgs.Location);
                    }
                }
            }
        }

        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(MMouseEventArgs e)
        {
            if (!this.Enable)
                return;
            if (e.MouseEventArgs.Button == MouseButtons.None)
            {
                this.isMouseEnter = true;
                e.Board.Cursor = Cursors.Hand;
                if (scrollbar != null)
                {
                    if (scrollbar.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                    {
                        scrollbar.DoMouseEnter(e);
                    }
                }
            }
        }

        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(MMouseEventArgs e)
        {
            if (!this.Enable)
                return;
            if (e.MouseEventArgs.Button == MouseButtons.None)
            {
                this.isMouseEnter = false;
                e.Board.Cursor = Cursors.Default;
                if (scrollbar != null)
                {
                    scrollbar.DoMouseLeave(e);
                }
            }
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MMouseEventArgs e)
        {
            if (!this.Enable)
                return;
            if (previousControl != null)
            {
                if (isMouseDown && previousControl is MVScrollBar)
                {
                    previousControl.DoMouseMove(e);
                }
                else if (previousControl.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                {
                    previousControl.DoMouseMove(e);
                }
                else
                {
                    if (!isMouseDown)
                    {
                        this.previousControl.DoMouseLeave(e);
                        this.previousControl = null;
                    }
                }
            }
            else
            {
                SelectRow = null;
                MControlBase control = null;
                if (scrollbar.Visible && scrollbar.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                {
                    control = scrollbar;
                }

                else
                {
                    for (int i = m_Data.Count - 1; i >= 0; i--)
                    {
                        MControlBase controlBase = m_Data[i].data;
                        if (controlBase != null && controlBase.Visible && !(controlBase is MRectangle) && controlBase.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                        {
                            control = controlBase;
                            break;
                        }
                    }
                }
                if (control != null)
                {
                    control.DoMouseEnter(e);
                    this.previousControl = control;
                    if (control is MLabels)
                    {
                        MLabels mbs = control as MLabels;
                        if (mbs != null)
                        {
                            SelectRow = control;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 滚动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MMouseEventArgs e)
        {
            if (e.MouseEventArgs.Y > scrollbar.Rectangle.top && e.MouseEventArgs.Y < scrollbar.Rectangle.bottom)
            {
                scrollbar.DoMouseWheel(e);
            }
        }

        #endregion
    }
}
