
using System;
namespace mPaint
{
    public class MControlBase
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public MControlBase()
        {

        }

        private string name = null;

        /// <summary>
        /// 获取或设置控件名字
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 图层
        /// </summary>
        public int Layer = 0;

        private String text;
        /// <summary>
        /// 获取或设置控件文字
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                TextChangeEvent?.Invoke(this, value);
            }
        }

        public event Action<object, string> TextChangeEvent;

        private FONT font = new FONT("SimSun", 14,false,false,false);

        /// <summary>
        /// 获取或设置文字字体
        /// </summary>
        public FONT Font
        {
            get { return font; }
            set { font = value; }
        }

        private int foreColor = COLOR.RGB(0, 0, 0);

        /// <summary>
        /// 获取或设置字体颜色
        /// </summary>
        public int ForeColor
        {
            get { return foreColor; }
            set { foreColor = value; }
        }

        private int mouseforeColor = COLOR.RGB(0, 0, 0);

        /// <summary>
        /// 获取或设置鼠标悬浮字体颜色
        /// </summary>
        public int MouseForeColor
        {
            get { return mouseforeColor; }
            set { mouseforeColor = value; }
        }

        private RECT rectangle;
        /// <summary>
        /// 获取或设置控件区域
        /// </summary>
        public RECT Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        private int borderColor = COLOR.RGB(255, 0, 0);
        /// <summary>
        /// 获取或设置边框颜色
        /// </summary>
        public int BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }

        private int backColor;
        private int mousebackColor;
        private int mouseClickBackColor
;
        /// <summary>
        /// 获取或设置背景颜色
        /// </summary>
        public int BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }
        /// <summary>
        /// 获取或设置鼠标悬浮背景颜色
        /// </summary>
        public int MouseBackColor
        {
            get { return mousebackColor; }
            set { mousebackColor = value; }
        }
        /// <summary>
        /// 获取或设置鼠标点击背景颜色
        /// </summary>
        public int MouseClickBackColor
        {
            get { return mouseClickBackColor; }
            set { mouseClickBackColor = value; }
        }
        private bool visible = true;
        /// <summary>
        /// 获取或设置控件是否可见
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        private bool enable = true;
        /// <summary>
        /// 获取或设置控件是否启用
        /// </summary>
        public bool Enable
        {
            get { return enable; }
            set
            {
                enable = value;
            }
        }

        private int zOrder;

        /// <summary>
        /// 获取或设置排序
        /// </summary>
        public int ZOrder
        {
            get { return zOrder; }
            set { zOrder = value; }
        }

        private bool redraw=true;

        ////是否重绘
        //public bool Redraw
        //{
        //    get { return redraw; }
        //    set { redraw = value; }
        //}

        private object parent;

        /// <summary>
        /// 获取或设置父对象
        /// </summary>
        public object Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private Object tag;
        /// <summary>
        /// 获取或设置对象标签
        /// </summary>
        public Object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        private bool isHand = false;
        /// <summary>
        /// 获取或设置鼠标经过时是否变为手型
        /// </summary>
        public bool IsHand
        {
            get { return isHand; }
            set
            {
                isHand = value;
            }
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        public event MMouseEventHandler Click;
        /// <summary>
        /// 移动事件
        /// </summary>
        public event MMouseEventHandler MouseMove;
        /// <summary>
        /// 按下事件
        /// </summary>
        public event MMouseEventHandler MouseDown;
        /// <summary>
        /// 抬起事件
        /// </summary>
        public event MMouseEventHandler MouseUp;
        /// <summary>
        /// 进入事件
        /// </summary>
        public event MMouseEventHandler MouseEnter;
        /// <summary>
        /// 离开事件
        /// </summary>
        public event MMouseEventHandler MouseLeave;
        /// <summary>
        /// 鼠标滚轮
        /// </summary>
        public event MMouseEventHandler MouseWheel;
        /// <summary>
        /// 绘制事件
        /// </summary>
        public event MPaintEventHandler Paint;

        /// <summary>
        /// 执行点击事件的方法
        /// </summary>
        /// <param name="e"></param>
        public void DoClick(MMouseEventArgs e)
        {
            this.OnClick(e);
            if (this.Click != null)
            {
                this.Click(this, e);
            }
        }

        /// <summary>
        /// 执行移动事件
        /// </summary>
        /// <param name="e"></param>
        public void DoMouseMove(MMouseEventArgs e)
        {
            this.OnMouseMove(e);
            this.MouseMove?.Invoke(this, e);
        }

        /// <summary>
        /// 执行按下事件
        /// </summary>
        /// <param name="e"></param>
        public void DoMouseDown(MMouseEventArgs e)
        {
            this.OnMouseDown(e);
            this.MouseDown?.Invoke(this, e);
        }

        /// <summary>
        /// 执行抬起事件
        /// </summary>
        /// <param name="e"></param>
        public void DoMouseUp(MMouseEventArgs e)
        {
            this.OnMouseUp(e);
            this.MouseUp?.Invoke(this, e);
        }

        /// <summary>
        /// 执行进入事件
        /// </summary>
        /// <param name="e"></param>
        public void DoMouseEnter(MMouseEventArgs e)
        {
            this.OnMouseEnter(e);
            this.MouseEnter?.Invoke(this, e);
        }

        /// <summary>
        /// 执行离开事件
        /// </summary>
        /// <param name="e"></param>
        public void DoMouseLeave(MMouseEventArgs e)
        {
            this.OnMouseLeave(e);
            this.MouseLeave?.Invoke(this, e);
        }

        /// <summary>
        /// 执行鼠标滚动
        /// </summary>
        /// <param name="e"></param>
        public void DoMouseWheel(MMouseEventArgs e)
        {
            this.OnMouseWheel(e);
            this.MouseWheel?.Invoke(this, e);
        }

        /// <summary>
        /// 执行绘制事件
        /// </summary>
        /// <param name="e"></param>
        public void DoPaint(MPaintEventArgs e)
        {
            this.OnPaint(e);
            this.Paint?.Invoke(this, e);
        }

        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClick(MMouseEventArgs e){}

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseMove(MMouseEventArgs e){}

        /// <summary>
        /// 按下
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseDown(MMouseEventArgs e){}

        /// <summary>
        /// 抬起
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseUp(MMouseEventArgs e){}

        /// <summary>
        /// 进入
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseEnter(MMouseEventArgs e){}

        /// <summary>
        /// 离开
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseLeave(MMouseEventArgs e){}

        /// <summary>
        /// 鼠标滚动
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseWheel(MMouseEventArgs e){}

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPaint(MPaintEventArgs e){}

        /// <summary>
        /// 把控件抬到最前端
        /// </summary>
        public void BringToFront(){}

        /// <summary>
        /// 把控件放到最后端
        /// </summary>
        public void SendToBack(){}

        /// <summary>
        /// 销毁方法
        /// </summary>
        public virtual void Dispose(){}
    }
}
