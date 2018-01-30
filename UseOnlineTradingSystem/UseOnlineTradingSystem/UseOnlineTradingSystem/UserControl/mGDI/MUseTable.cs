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
    public class MUseTable : MControlBase
    {
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
        public event Action<object,Point> MouseRightUpEvent;
        public event Action<object, Point> MouseLeftUpEvent;
        private MLabels tableHeader;//列头：标记，采购/销售，等级，参考合约，升贴水，绝对价格，数量，仓库，公司，备注，发布时间，(元/吨)，(元/吨)，(吨)
        private MImage image2;//标记图片
        private MImage image3;//提示标记图片内容
        private MLine line1;//表头横线
        private MRectangle mrt1;//上表背景
        private MRectangle mrt2;//下表背景
        protected List<UData> buyData = new List<UData>();//买数据行
        protected List<UData> sellData = new List<UData>();//卖数据行
        private int dataInterval = 24;//数据行之间的间隔
        private MVScrollBar scrollbar1;//上滚动条
        protected int offset1 = 0;//上偏移量
        private MVScrollBar scrollbar2;//下滚动条
        protected int offset2 = 0;//下偏移量
        public MUseTable()
        {
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

            tableHeader.lbs[0].Text = "标记";
            tableHeader.lbs[1].Text = "采购/销售";
            tableHeader.lbs[2].Text = "等级";
            tableHeader.lbs[3].Text = "参考合约";
            tableHeader.lbs[4].Text = "升贴水\r\n(元/吨)";
            tableHeader.lbs[5].Text = "绝对价格\r\n(元/吨)";
            tableHeader.lbs[6].Text = "数量\r\n(吨)";
            tableHeader.lbs[7].Text = "仓库";
            tableHeader.lbs[8].Text = "公司";
            tableHeader.lbs[9].Text = "备注";
            tableHeader.lbs[10].Text = "发布时间";

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
            mrt1 = new MRectangle();
            mrt1.BackColor = COLOR.RGB(MCommonData.fontColor6);

            //下表背景
            mrt2 = new MRectangle();
            mrt2.BackColor = COLOR.RGB(MCommonData.fontColor6);

            #region 滚动条
            scrollbar1 = new MVScrollBar();
            scrollbar1.BackColor = COLOR.RGB(MCommonData.fontColor6);
            scrollbar1.MouseBackColor = COLOR.RGB(MCommonData.fontColor6);
            scrollbar1.MouseClickBackColor = COLOR.RGB(MCommonData.fontColor6);
            scrollbar1.ForeColor = COLOR.RGB(MCommonData.fontColor14);
            scrollbar1.MouseForeColor = COLOR.RGB(MCommonData.fontColor15);
            scrollbar1.ValueChanged += Scrollbar1_ValueChanged;
            scrollbar1.MinInitialized += Scrollbar1_MinInitialized;

            scrollbar2 = new MVScrollBar();
            scrollbar2.BackColor = COLOR.RGB(MCommonData.fontColor6);
            scrollbar2.MouseBackColor = COLOR.RGB(MCommonData.fontColor6);
            scrollbar2.MouseClickBackColor = COLOR.RGB(MCommonData.fontColor6);
            scrollbar2.ForeColor = COLOR.RGB(MCommonData.fontColor14);
            scrollbar2.MouseForeColor = COLOR.RGB(MCommonData.fontColor15);
            scrollbar2.ValueChanged += Scrollbar2_ValueChanged;
            scrollbar2.MinInitialized += Scrollbar2_MinInitialized;
            #endregion

            #endregion
        }

        /// <summary>
        /// 重置为最小初始状态
        /// </summary>
        private void Scrollbar1_MinInitialized()
        {
            offset1 = 0;
        }

        /// <summary>
        /// 重置为最小初始状态
        /// </summary>
        private void Scrollbar2_MinInitialized()
        {
            offset2 = 0;
        }

        /// <summary>
        /// 滚动条值变化
        /// </summary>
        /// <param name="obj"></param>
        private void Scrollbar1_ValueChanged(double obj)
        {
            if (obj != 0)
            {
                offset1 -= (int)(((sellData.Count) * dataInterval - (scrollbar1.Rectangle.bottom - scrollbar1.Rectangle.top)) * obj) / 100;
            }
        }

        /// <summary>
        /// 滚动条值变化
        /// </summary>
        /// <param name="obj"></param>
        private void Scrollbar2_ValueChanged(double obj)
        {
            if (obj != 0)
            {
                offset2 -= (int)(((buyData.Count) * dataInterval - (scrollbar2.Rectangle.bottom - scrollbar2.Rectangle.top)) * obj) / 100;
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
            //卖
            int scrollbarY1 = tableHeader.Rectangle.bottom;
            //分割线
            int scrollbarfg = scrollbarY1 + (int)(0.6 * (Rectangle.bottom- scrollbarY1 - 60));
            //买
            int scrollbarY2 = Rectangle.bottom  - 60;
            //上表的背景绘图区域设置 
            mrt1.Rectangle = new RECT(Rectangle.left, scrollbarY1, Rectangle.right, scrollbarfg);
            //下表的背景绘图区域设置 
            mrt2.Rectangle = new RECT(Rectangle.left, scrollbarfg, Rectangle.right, scrollbarY2);

            int dataX1 = mrt1.Rectangle.left;//左边
            int dataX2 = mrt1.Rectangle.right;//右边

            bool needScrollbar1 = false;
            bool needScrollbar2 = false;

            #region 卖
            int data1y = Rectangle.top + 22 + dataInterval;//数据行中文字的Y坐标
            int dataChoose1Y = tableHeader.Rectangle.bottom;//是否选中背景颜色的Y坐标
            //卖
            foreach (var v in sellData)
            {
                int TopY = dataChoose1Y + offset1;//数据（卖）数据绘图区域上部Y坐标
                int bottomY = dataChoose1Y + dataInterval + offset1; //数据（卖）数据绘图区域下部Y坐标
                if (bottomY > scrollbarfg)
                {
                    needScrollbar1 = true;
                }
                if (bottomY < scrollbarY1)
                {
                    //无需绘制，隐藏
                    v.Hide();
                }
                else
                {
                    //需要绘制显示
                    v.Show();
                }
                //数据（卖）背景绘制位置
                v.data.Rectangle = new RECT(dataX1, TopY, dataX2, bottomY);
                //数据（卖）图片绘制位置
                if (v.data1image != null)
                {
                    int data1imagex = tableHeader.lbs[0].Rectangle.left + of;
                    if (v.data1image != null)
                    {
                        v.data1image.Rectangle = new RECT(data1imagex + 5, data1y + 5 + offset1, data1imagex + 5 + 15, data1y + 5 + 15 + offset1);
                    }
                    else
                    {
                        v.data1image.Rectangle = new RECT(data1imagex - 10 + 10, data1y + 5 + offset1, data1imagex + 10 + 15, data1y + 5 + 15 + offset1);
                    }

                }
                //数据（卖）文字绘制位置
                for (int i = 0; i < v.data.lbs.Count; i++)
                {
                    int data1x = tableHeader.lbs[i + 1].Rectangle.left + of;
                    v.data.lbs[i].Rectangle = new RECT(data1x, data1y + offset1, data1x + dataWith, data1y + 20 + offset1);
                }
                //下一条数据
                data1y += dataInterval;
                dataChoose1Y += dataInterval;
            }
            #endregion

            #region 分割线
            //买卖分割横线位置
            int lineY = dataChoose1Y + offset1;//买卖分割横线Y坐标
            //不能超过分割线
            if (lineY > scrollbarfg)
            {
                lineY = scrollbarfg;
            }
            line1.FirstPoint = new POINT(dataX1, lineY);
            line1.SecondPoint = new POINT(dataX2, lineY);
            #endregion

            #region 买
            int data2y = lineY;//数据行中文字的Y坐标
            int dataChoose2Y = lineY;//是否选中背景颜色的Y坐标
            int offset3 = 0;
            //买
            foreach (var v in buyData)
            {
                int TopY = dataChoose2Y + offset2;//数据（买）数据绘图区域上部Y坐标
                int bottomY = dataChoose2Y + dataInterval + offset2; //数据（买）数据绘图区域下部Y坐标
                if (bottomY > scrollbarY2)
                {
                    needScrollbar2 = true;
                }
                if (TopY < lineY||TopY > scrollbarY2)
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
                        offset3 = v.data.Rectangle.top - lineY;
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
                        v.data1image.Rectangle = new RECT(data1imagex + 5, data2y + 5 + offset2, data1imagex + 5 + 15, data2y + 5 + 15 + offset2);
                    }
                    else
                    {
                        v.data1image.Rectangle = new RECT(data1imagex - 10 + 10, data2y + 5 + offset2, data1imagex + 10 + 15, data2y + 5 + 15 + offset2);
                    }
                }
                //数据（买）文字绘制位置
                for (int i = 0; i < v.data.lbs.Count; i++)
                {
                    int data2x = tableHeader.lbs[i + 1].Rectangle.left + of;
                    v.data.lbs[i].Rectangle = new RECT(data2x, data2y + offset2, data2x + dataWith, data2y + 20 + offset2);
                }
                data2y += dataInterval;
                dataChoose2Y += dataInterval;
            }
            #endregion

            #endregion

            #region 滚动条
            if (needScrollbar1)
            {
                scrollbar1.Visible = true;
                scrollbar1.Rectangle = new RECT(dataX2 - 10, scrollbarY1, dataX2, lineY);
                scrollbar1.ChangeRectangle();
            }
            else
            {
                scrollbar1.Visible = false;
            }
            if (needScrollbar2)
            {
                scrollbar2.Visible = true;
                scrollbar2.Rectangle = new RECT(dataX2 - 10, lineY, dataX2, scrollbarY2);
                scrollbar2.ChangeRectangle();
            }
            else
            {
                scrollbar2.Visible = false;
            }
            #endregion
        }

        Random rdd = new Random();
        /// <summary>
        /// 数据对象
        /// </summary>
        /// <param name="info">数据</param>
        /// <param name="type">0为买 1为卖</param>
        public void InsertData(OneListed info, int type)
        {
            // if (info.blackWhiteType != "0")
            {
                MLabels data = new MLabels();
                data.BackColor = COLOR.RGB(MCommonData.fontColor6);
                data.MouseBackColor = COLOR.RGB(MCommonData.fontColor10);
                UData ud = new UData();
                for (int i = 0; i < 10; i++)
                {
                    MLabel ml = new MLabel();
                    ml.ForeColor = COLOR.RGB(MCommonData.fontColor4);
                    ml.MouseForeColor = COLOR.RGB(MCommonData.fontColor4);
                    ml.BackColor = -1;
                    ml.Font = MCommonData.d4Font;
                    ml.LeftAligned = true;
                    data.lbs.Add(ml);
                }
                data.lbs[6].MouseForeColor = COLOR.RGB(MCommonData.fontColor2);
                data.lbs[6].Underline = true;
                data.lbs[9].ForeColor = COLOR.RGB(MCommonData.fontColor8);
                if (type == 0)
                {
                    buyData.Add(ud);
                }
                else
                {
                    sellData.Add(ud);
                }
                SetData(info, data, ud, type);
            }
        }

        private void SetData(OneListed info, MLabels data, UData ud, int type)
        {
            if (info == null || data == null || ud == null || data.lbs == null || data.lbs.Count == 0) return;
            data.lbs[0].Text = info.transTypeName;
            data.lbs[1].Text = info.commLevelName;
            if (string.IsNullOrWhiteSpace(info.contract))
            {
                data.lbs[2].Text ="--";
            }
            else
            {
                data.lbs[2].Text = info.contract;
            }
            if (string.IsNullOrWhiteSpace(info.premium))
            {
                data.lbs[3].Text = "--";
            }
            else
            {
                data.lbs[3].Text = info.premium;
            }
            if (info.pricingMethod == "0" && info.premium != null)
            {
                var vvv = DataManager.Instance.GetContractLastPrice(info.contract);
                if (vvv != null)
                {
                    int premium;
                    int.TryParse(info.premium, out premium);
                    if (premium != 0)
                    {
                        if (type == 0)
                        {
                            info.fixedPrice = (vvv.bidPrice + premium).ToString();
                        }
                        else
                        {
                            info.fixedPrice = (vvv.askPrice + premium).ToString();
                        }
                    }
                }
            }

            decimal money;
            decimal.TryParse(info.fixedPrice, out money);
            if (money > 0)
            {
                data.lbs[4].Text = string.Format("{0:C}", money); 
            }
            else
            {
                data.lbs[4].Text = "";
            }
            data.lbs[5].Text = info.commAvailableQuantity;
            string text = info.warehouseName;
            if (text != null && text.Length >= 6)
            {
                text = text.Substring(0, 6) + "...";
            }
            data.lbs[6].Text = text;
            data.lbs[7].Text = info.publisher;
            data.lbs[8].Text = info.remarks;
            long l;
            long.TryParse(info.publisherDate, out l);
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            data.lbs[9].Text = start.AddMilliseconds(l).ToLocalTime().ToString("HH:mm:ss");

            if (DataManager.Instance.LoginData != null)
            {
                ud.data1image = new MImage();

                if (info.createdBy == DataManager.Instance.LoginData.id)
                {
                    ud.data1image.BackgroundImage = Resources.icon_publish;
                }
                else if (DataManager.Instance.WhiteEnable && DataManager.Instance.WhiteDY != null && info.publisher != null && DataManager.Instance.WhiteDY.ContainsKey(info.publisher))
                {
                    ud.data1image.BackgroundImage = Resources.icon_white;
                }
                else if (DataManager.Instance.BlackEnable && DataManager.Instance.BlackDY != null && info.publisher != null && DataManager.Instance.BlackDY.ContainsKey(info.publisher))
                {
                    ud.data1image.BackgroundImage = Resources.icon_black;
                }
                else
                {
                    ud.data1image = null;
                }
            }
            data.Tag = ud;
            ud.obj = info;
            ud.data = data;
            ud.house = data.lbs[6];//仓库信息
            if (type == 0)
            {
                scrollbar2.CalculationSliderWith(buyData.Count * dataInterval);
            }
            else
            {
                scrollbar1.CalculationSliderWith(sellData.Count * dataInterval);
            }
        }

        public void Updata(OneListed info)
        {
            if (info == null) return;
            for (int i = 0; i < buyData.Count; i++)
            {
                var v = buyData[i];
                OneListed ci = v.obj as OneListed;
                if (ci != null && info.id == ci.id)
                {
                    ci.Update(info);
                    if (ci.blackWhiteType=="0"||(ci.transStatus=="2"&&ci.commAvailableQuantity=="0") ||ci.transStatus == "3" || ci.transStatus == "4")
                    {
                        buyData.Remove(v);
                    }
                    else
                    {
                        SetData(ci, v.data, v, 1);
                    }
                    return;
                }
            }
            for (int i = 0; i < sellData.Count; i++)
            {
                var v = sellData[i];
                OneListed ci = v.obj as OneListed;
                if (ci != null && info.id == ci.id)
                {
                    ci.Update(info);
                    if (ci.blackWhiteType == "0" || (ci.transStatus == "2" && ci.commAvailableQuantity == "0") || ci.transStatus == "3" || ci.transStatus == "4")
                    {
                        sellData.Remove(v);
                    }
                    else
                    {
                        SetData(ci, v.data, v, 0);
                    }
                    return;
                }
            }
        }

        public void Updata(string category, float buyPrice, float sellPrice)
        {
            if (sellPrice != 0)
            {
                for (int i = 0; i < sellData.Count; i++)
                {
                    var v = sellData[i];
                    OneListed ci = v.obj as OneListed;
                    if (ci != null)
                    {
                        if ( ci.pricingMethod == "0" && ci.premium != null && category.EndsWith(ci.contract))
                        {
                            SetData(ci, v.data, v, 1);
                        }
                    }
                }
            }
            if (buyPrice != 0)
            {
                for (int i = 0;  i < buyData.Count; i++)
                {
                    var v = buyData[i];
                    OneListed ci = v.obj as OneListed;
                    if (ci != null)
                    {
                        if (ci.pricingMethod == "0" && ci.premium != null && category.EndsWith(ci.contract))
                        {
                            SetData(ci, v.data, v, 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void ClearData()
        {
            sellData.Clear();
            scrollbar1.CalculationSliderWith(sellData.Count * dataInterval);
            buyData.Clear();
            scrollbar2.CalculationSliderWith(buyData.Count * dataInterval);
        }

        //public void InsertSellData()
        //{
        //    MLabels data = new MLabels();
        //    data.BackColor = COLOR.RGB(MCommonData.fontColor6);
        //    data.MouseBackColor = COLOR.RGB(MCommonData.fontColor10);
        //    for (int i = 0; i < 10; i++)
        //    {
        //        MLabel ml = new MLabel();
        //        ml.ForeColor = COLOR.RGB(MCommonData.fontColor4);
        //        ml.BackColor = -1;
        //        ml.Font = MCommonData.d4Font;
        //        ml.LeftAligned = true;
        //        data.lbs.Add(ml);
        //    }
        //    data.lbs[0].Text = "卖(8)";
        //    data.lbs[0].ForeColor = COLOR.RGB(MCommonData.fontColor7);
        //    data.lbs[1].Text = "采购";
        //    data.lbs[2].Text = "升水铜";
        //    data.lbs[3].Text = "100";
        //    data.lbs[4].Text = "54040";
        //    data.lbs[5].Text = "2";
        //    data.lbs[6].Text = "上海国储天威...";
        //    data.lbs[7].Text = "左岗集团";
        //    data.lbs[8].Text = "";
        //    data.lbs[9].Text = "16:22:22";
        //    data.lbs[9].ForeColor = COLOR.RGB(MCommonData.fontColor8);

        //    UData ud = new UData();
        //    ud.data = data;
        //    ud.data1image = new MImage();
        //    int d = rdd.Next(0, 5);
        //    if (d == 0)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_new;
        //    }
        //    else if (d == 1)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_publish;
        //    }
        //    else if (d == 2)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_white;
        //    }
        //    else if (d == 3)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_black;
        //    }
        //    else
        //    {
        //        ud.data1image = null;
        //    }
        //    sellData.Add(ud);
        //    scrollbar1.CalculationSliderWith(sellData.Count * dataInterval);
        //}

        //public void InsertBuyData()
        //{
        //    MLabels data = new MLabels();
        //    data.BackColor = COLOR.RGB(MCommonData.fontColor6);
        //    data.MouseBackColor = COLOR.RGB(MCommonData.fontColor10);
        //    for (int i = 0; i < 10; i++)
        //    {
        //        MLabel ml = new MLabel();
        //        ml.ForeColor = COLOR.RGB(MCommonData.fontColor4);
        //        ml.BackColor = -1;
        //        ml.Font = MCommonData.d4Font;
        //        ml.LeftAligned = true;
        //        data.lbs.Add(ml);
        //    }
        //    data.lbs[0].Text = "买(9)";
        //    data.lbs[0].ForeColor = COLOR.RGB(MCommonData.fontColor9);
        //    data.lbs[1].Text = "采购";
        //    data.lbs[2].Text = "升水铜";
        //    data.lbs[3].Text = "100";
        //    data.lbs[4].Text = "54040";
        //    data.lbs[5].Text = "2";
        //    data.lbs[6].Text = "上海主流仓库";
        //    data.lbs[7].Text = "左岗集团";
        //    data.lbs[8].Text = "";
        //    data.lbs[9].Text = "16:22:22";
        //    data.lbs[9].ForeColor = COLOR.RGB(MCommonData.fontColor8);

        //    UData ud = new UData();
        //    ud.data = data;
        //    ud.data1image = new MImage();
        //    int d = rdd.Next(0, 5);
        //    if (d == 0)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_new;
        //    }
        //    else if (d == 1)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_publish;
        //    }
        //    else if (d == 2)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_white;
        //    }
        //    else if (d == 3)
        //    {
        //        ud.data1image.BackgroundImage = Resources.icon_black;
        //    }
        //    else
        //    {
        //        ud.data1image = null;
        //    }
        //    buyData.Add(ud);
        //    scrollbar2.CalculationSliderWith(buyData.Count * dataInterval);
        //}

        public void Initialization()
        {
            offset1 = 0;
            offset2 = 0;
        }

        protected override void OnPaint(MPaintEventArgs e)
        {
            CalculationTable();
            #region 绘制数据
            mrt1.DoPaint(e);
            //绘卖
            foreach (var v in sellData)
            {
                if (v.data.Visible)
                {
                    v.data.DoPaint(e);
                    if (v.data1image != null)
                    {
                        v.data1image.DoPaint(e);
                    }
                }

            }
            //保存
            //Graphics gc = e.CPaint.GetGraphics();
            //GraphicsState gs = gc.Save();
            //g.Restore(gs);

            mrt2.DoPaint(e);
            //绘买
            for (int i=0; i<buyData.Count;i++)
            {
                var v = buyData[i];
                if (v.data.Visible)
                {
                    v.data.DoPaint(e);
                    if(v.data1image!=null)
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
            if (scrollbar1.Visible)
            {
                scrollbar1.DoPaint(e);
            }
            if (scrollbar2.Visible)
            {
                scrollbar2.DoPaint(e);
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
                if (scrollbar1 != null)
                {
                    if (scrollbar1.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                    {
                        scrollbar1.DoMouseDown(e);
                    }
                }
                if (scrollbar2!= null)
                {
                    if (scrollbar2.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                    {
                        scrollbar2.DoMouseDown(e);
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
                        image3.Rectangle = new RECT(image3X + 10, e.MouseEventArgs.Y + 5, image3Y + image3.BackgroundImage.Width/2 +10, image3Y + image3.BackgroundImage.Height/2 +5);
                    }
                }
                if (scrollbar1 != null)
                {
                    scrollbar1.DoMouseUp(e);
                }
                if (scrollbar2 != null)
                {
                    scrollbar2.DoMouseUp(e);
                }

                if (SelectRow != null)
                {
                    UData ud = SelectRow.Tag as UData;
                    if (ud != null&&ud.house!=null)
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
                if (scrollbar1 != null)
                {
                    if (scrollbar1.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                    {
                        scrollbar1.DoMouseEnter(e);
                    }
                }
                if (scrollbar2 != null)
                {
                    if (scrollbar2.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                    {
                        scrollbar2.DoMouseEnter(e);
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
                if (scrollbar1 != null)
                {
                    scrollbar1.DoMouseLeave(e);
                }
                if (scrollbar2 != null)
                {
                    scrollbar2.DoMouseLeave(e);
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
                if (scrollbar1.Visible && scrollbar1.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                {
                    control = scrollbar1;
                }
                else if (scrollbar2.Visible && scrollbar2.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                {
                    control = scrollbar2;
                }
                else
                {
                    for (int i = buyData.Count - 1; i >= 0; i--)
                    {
                        MControlBase controlBase = buyData[i].data;
                        if (controlBase != null && controlBase.Visible && !(controlBase is MRectangle) && controlBase.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                        {
                            control = controlBase;
                            break;
                        }
                    }
                    if (control == null)
                    {
                        for (int i = sellData.Count - 1; i >= 0; i--)
                        {
                            MControlBase controlBase = sellData[i].data;
                            if (controlBase.Visible && !(controlBase is MRectangle) && controlBase.Rectangle.Contains(e.MouseEventArgs.X, e.MouseEventArgs.Y))
                            {
                                control = controlBase;
                                break;
                            }
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
            if (e.MouseEventArgs.Y > scrollbar1.Rectangle.top && e.MouseEventArgs.Y < scrollbar1.Rectangle.bottom)
            {
                scrollbar1.DoMouseWheel(e);
            }
            if (e.MouseEventArgs.Y > scrollbar2.Rectangle.top && e.MouseEventArgs.Y < scrollbar2.Rectangle.bottom)
            {
                scrollbar2.DoMouseWheel(e);
            }
        }
    }
    public class UData
    {
        public MImage data1image;//测试数据1标记图片
        public MLabel house;//仓库信息
        public MLabels data;//测试数据1
        public object obj;//原始数据
        public void Show()
        {
            if (data1image != null)
            {
                data1image.Visible = true;
            }
            if (data != null)
            {
                data.Visible = true;
            }
        }
        public void Hide()
        {
            if (data1image != null)
            {
                data1image.Visible = false;
            }
            if (data != null)
            {
                data.Visible = false;
            }
        }
    }
}
