using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace mPaint
{
    public class MCommonData
    {
        public static  FONT dFont = new FONT("微软雅黑", 16, true, false, false);//行情，我的资金，基础管理，纵向：行情，供需发布，K线，资讯
        public static FONT d2Font = new FONT("微软雅黑", 14, true, false, false);//快捷按钮，买入基价，卖出基价，列头：标记，序号，采购/销售，等级，升贴水，绝对价格，数量，仓库，公司，备注，发布时间;登录按钮
        public static FONT d3Font = new FONT("微软雅黑", 24, true, false, false);//买卖基价具体值
        public static FONT d4Font = new FONT("微软雅黑", 14, false, false, false);//表格行数据
        public static Color LineColor = Color.FromArgb(51, 51, 51);//普通线颜色
        public static Color fontColor0 = Color.FromArgb(7, 8, 8);//整体背景
        public static Color fontColor1 = Color.FromArgb(153, 153, 153);//前景色：行情，我的资金，基础管理，纵向：行情，供需发布，K线，资讯
        public static Color fontColor2 = Color.FromArgb(255, 135, 0);//鼠标悬浮：行情，我的资金，基础管理，纵向：行情，供需发布，K线，资讯；买卖数据分割线背景颜色,普通状态下登录按钮
        public static Color fontColor3 = Color.FromArgb(36, 48, 43);//选中背景色：快捷按钮
        public static Color fontColor4 = Color.FromArgb(255, 255, 255);//前景色：快捷按钮，列头：标记，序号，采购/销售，等级，升贴水，绝对价格，数量，仓库，公司，备注，发布时间;买卖数据颜色
        public static Color fontColor5 = Color.FromArgb(36, 38, 43);//背景色：第一行背景色，纵向：行情，供需发布，K线，资讯，列头：标记，序号，采购/销售，等级，升贴水，绝对价格，数量，仓库，公司，备注，发布时间
        public static Color fontColor6 = Color.FromArgb(25, 26, 28);//背景色：表格数据区，表格数据背景色
        public static Color fontColor7 = Color.FromArgb(76, 194, 28);//数据行：卖字颜色
        public static Color fontColor8 = Color.FromArgb(102, 102, 102);//数据行：时间颜色
        public static Color fontColor9 = Color.FromArgb(255, 9, 19);//数据行：买字颜色
        public static Color fontColor10 = Color.FromArgb(43, 46, 51);//数据行鼠标悬浮颜色
        public static Color fontColor11 = Color.FromArgb(255, 160, 30);//鼠标悬浮 登录按钮
        public static Color fontColor12 = Color.FromArgb(204, 108, 0);//鼠标点击 登录按钮
        public static Color fontColor13 = Color.FromArgb(247, 247, 247);//鼠标悬浮 选择栏
        public static Color fontColor14 = Color.FromArgb(48, 49, 51);//鼠标悬浮 滚动条滑块
        public static Color fontColor15 = Color.FromArgb(104, 107, 109);//普通状态和滚动状态 滚动条滑块
    }
}
