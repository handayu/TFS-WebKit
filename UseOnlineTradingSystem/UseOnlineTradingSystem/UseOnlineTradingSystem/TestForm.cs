using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace UseOnlineTradingSystem
{
    public partial class TestForm : Form
    {

        private ArrayList list1 = new ArrayList();
        //private BindingList<> list2 = new BindingList();
        private ArrayList list3 = new ArrayList();

        public TestForm()
        {
            InitializeComponent();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            #region 测试GDI-1(画板上pen画基本图线)
            //Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            //Pen p = new Pen(Color.Blue, 2);//定义了一个蓝色,宽度为的画笔

            //g.DrawLine(p, 10, 10, 100, 100);//在画板上画直线,起始坐标为(10,10),终点坐标为(100,100)
            //g.DrawRectangle(p, 10, 10, 100, 100);//在画板上画矩形,起始坐标为(10,10),宽为,高为
            //g.DrawEllipse(p, 10, 10, 100, 100);//在画板上画椭圆,起始坐标为(10,10),外接矩形的宽为,高为
            #endregion

            #region 测试GDI-2(设置笔的各类属性)
            //Graphics g = e.Graphics;//获取创建画布

            //Pen p = new Pen(Color.Blue, 5);//创建笔，并设置笔的粗细为,颜色为蓝色及各类属性
            ////画虚线
            //p.DashStyle = DashStyle.Dot;//定义虚线的样式为点

            //g.DrawLine(p, 10, 10, 200, 10);

            ////自定义虚线
            //p.DashPattern = new float[] { 2, 1 };//设置短划线和空白部分的数组
            //g.DrawLine(p, 10, 20, 200, 20);

            ////画箭头,只对不封闭曲线有用
            //p.DashStyle = DashStyle.Solid;//恢复实线
            //p.EndCap = LineCap.ArrowAnchor;//定义线尾的样式为箭头
            //g.DrawLine(p, 10, 30, 200, 30);

            //g.Dispose();
            //p.Dispose();
            #endregion

            #region 测试GDI-3(设置矩形，并用图片，颜色等填充)
            //Graphics g =e.Graphics;
            //Rectangle rect = new Rectangle(10, 10, 50, 50);//定义矩形,参数为起点横纵坐标以及其长和宽

            ////单色填充
            //SolidBrush b1 = new SolidBrush(Color.Blue);//定义单色画刷          
            //g.FillRectangle(b1, rect);//填充这个矩形

            ////字符串
            //g.DrawString("字符串", new Font("宋体", 10), b1, new PointF(90, 10));

            //用图片填充
            //TextureBrush b2 = new TextureBrush(Image.FromFile(@"e:\picture\1.jpg"));
            //rect.Location = new Point(10, 70);//更改这个矩形的起点坐标
            //rect.Width = 200;//更改这个矩形的宽来
            //rect.Height = 200;//更改这个矩形的高
            //g.FillRectangle(b2, rect);

            //用渐变色填充
            //rect.Location = new Point(10, 290);
            //LinearGradientBrush b3 = new LinearGradientBrush(rect, Color.Yellow, Color.Black, LinearGradientMode.Horizontal);
            //g.FillRectangle(b3, rect);
            #endregion

            #region GDI测试4






            #endregion


        }

        private void Form_Load(object sender, EventArgs e)
        {
            this.radGridView_TradeInfo.ThemeName = this.visualStudio2012DarkTheme1.ThemeName;

            SetPreferences();
            //BindToSubObjects();
        }


        private void SetPreferences()
        {
            this.radGridView_TradeInfo.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            this.radGridView_TradeInfo.ShowGroupPanel = false;
            this.radGridView_TradeInfo.MasterTemplate.EnableGrouping = false;
            this.radGridView_TradeInfo.EnableHotTracking = true;
            //this.radRadioDataReader.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
        }

        /// <summary>
        /// SubObj
        /// </summary>
        private void BindToSubObjects()
        {
            this.radGridView_TradeInfo.MasterTemplate.AllowAddNewRow = false;
            this.radGridView_TradeInfo.MasterTemplate.AutoGenerateColumns = false;
            this.radGridView_TradeInfo.DataSource = null;

            this.radGridView_TradeInfo.TableElement.BeginUpdate();

            this.radGridView_TradeInfo.MasterTemplate.Columns.Clear();
            this.radGridView_TradeInfo.MasterTemplate.Columns.Add(new GridViewTextBoxColumn("时间", "TransTimeDateTime"));
            this.radGridView_TradeInfo.MasterTemplate.Columns.Add(new GridViewTextBoxColumn("品牌", "CommBrandName"));
            this.radGridView_TradeInfo.MasterTemplate.Columns.Add(new GridViewTextBoxColumn("等级", "CommLevelName"));
            this.radGridView_TradeInfo.MasterTemplate.Columns.Add(new GridViewTextBoxColumn("参考合约", "Contract"));
            this.radGridView_TradeInfo.MasterTemplate.Columns.Add(new GridViewTextBoxColumn("仓库", "WarehouseName"));
            this.radGridView_TradeInfo.MasterTemplate.Columns.Add(new GridViewTextBoxColumn("买/卖", "TransTypeName"));
            this.radGridView_TradeInfo.MasterTemplate.Columns.Add(new GridViewTextBoxColumn("升贴水", "Premium"));
            this.radGridView_TradeInfo.MasterTemplate.Columns.Add(new GridViewTextBoxColumn("绝对价格", "FixedPrice"));
            this.radGridView_TradeInfo.MasterTemplate.Columns.Add(new GridViewTextBoxColumn("委托量", "CommAvailableQuantity"));
            this.radGridView_TradeInfo.MasterTemplate.Columns.Add(new GridViewTextBoxColumn("可撤", "CommAvailableQuantity"));
            this.radGridView_TradeInfo.MasterTemplate.Columns.Add(new GridViewTextBoxColumn("备注", "Remarks"));

            for (int i = 0; i < radGridView_TradeInfo.MasterTemplate.Columns.Count; i++)
            {
                this.radGridView_TradeInfo.MasterTemplate.Columns[i].Width = 150;
            }
            this.radGridView_TradeInfo.TableElement.EndUpdate(false);

            if (list3.Count == 0)
            {
                list3.Add(new SelfListed()
                {
                    TransTime = "20171108",
                    TransType = "0",
                    TransTypeName = "购买"
                }
                );
            }
            this.radGridView_TradeInfo.DataSource = list3;
            this.radGridView_TradeInfo.Columns[1].Width = 200;
        }

        //public class Customer
        //{
        //    int id;
        //    string customerName;
        //    string companyName;
        //    Address address;
        //    public int ID
        //    {
        //        get { return this.id; }
        //        set { this.id = value; }
        //    }
        //    public string CustomerName
        //    {
        //        get { return this.customerName; }
        //        set { this.customerName = value; }
        //    }
        //    public string CompanyName
        //    {
        //        get { return this.companyName; }
        //        set { this.companyName = value; }
        //    }
        //    public Address Address
        //    {
        //        get { return this.address; }
        //    }
        //    public Customer()
        //    {
        //        this.address = new Address();
        //    }
        //    public Customer(int id, string customerName, string companyName, string country, string city, string postalCode, string phone)
        //    {
        //        this.id = id;
        //        this.customerName = customerName;
        //        this.companyName = companyName;
        //        this.address = new Address(country, city, postalCode, phone);
        //    }
        //}

        //public class Address
        //{
        //    string country;
        //    string city;
        //    ContactDetails contactDetails;
        //    public string Country
        //    {
        //        get { return country; }
        //        set { country = value; }
        //    }
        //    public string City
        //    {
        //        get { return city; }
        //        set { city = value; }
        //    }
        //    public ContactDetails ContactDetails
        //    {
        //        get { return this.contactDetails; }
        //    }
        //    public Address()
        //    {
        //        this.contactDetails = new ContactDetails();
        //    }
        //    public Address(string country, string city, string postalCode, string phone)
        //    {
        //        this.country = country;
        //        this.city = city;
        //        this.contactDetails = new ContactDetails(postalCode, phone);
        //    }
        //}

        //public class ContactDetails
        //{
        //    string postalCode;
        //    string phone;
        //    public string PostalCode
        //    {
        //        get { return postalCode; }
        //        set { postalCode = value; }
        //    }
        //    public string Phone
        //    {
        //        get { return phone; }
        //        set { phone = value; }
        //    }
        //    public ContactDetails()
        //    {

        //    }
        //    public ContactDetails(string postalCode, string phone)
        //    {
        //        this.postalCode = postalCode;
        //        this.phone = phone;
        //    }
        //}
    }
}
