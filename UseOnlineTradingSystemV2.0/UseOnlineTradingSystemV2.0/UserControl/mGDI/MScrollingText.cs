using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;

namespace mPaint
{
	/// <summary>
	/// 跑马灯
	/// </summary>
	public class MScrollingText : System.Windows.Forms.Control
	{
        public List<MScrollingItem> Items = new List<MScrollingItem>(); //滚动内容
        private bool timerEnable = false;               // 用于文本动画的计时器控制。						
        private string text = "Text";					// 滚动文本
		private float staticTextPos = 0;				// 滚动文本的X轴像素点
        private float yPos = 0;							// 滚动文本的Y轴像素点
        private VerticleTextPosition verticleTextPosition = VerticleTextPosition.Center;	// 将文本垂直放置的方式
        private int scrollPixelDistance = 2;			// 每个定时器事件的文本卷轴多远
		private bool showBorder = false;					// 是否显示边界
        private bool stopScrollOnMouseOver = true;		// 鼠标悬停在文本上是否停止滚动
		private bool scrollOn = true;					// 是否滚动
        private Brush foregroundBrush = null;			// 允许用户自定义笔刷设置为文本的字体
        private Brush backgroundBrush = null;			// 允许用户自定义笔刷设置为背景
        private Color borderColor = Color.Black;		// 允许用户设置的颜色控制边界

		public MScrollingText()
		{
            // 设置默认属性ScrollingText控制
            this.Name = "ScrollingText";
            this.Size = new System.Drawing.Size(216, 40);
            this.MouseClick +=new MouseEventHandler(MScrollingText_MouseClick);
            this.staticTextPos = this.ClientSize.Width;
            this.MouseEnter +=new EventHandler(MScrollingText_MouseEnter);
            this.MouseLeave += new EventHandler(MScrollingText_MouseLeave);

            // 开启内部双缓冲技术的定制GDI +绘画
            this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);

            // 设置定时器
            timerEnable = true;
            Thread timer = new Thread(new ThreadStart(Tick));
            timer.IsBackground = true;
            timer.Start();
            timer = null;
            //timer.Interval = 25;	// 默认的时间间隔
            //timer.Enabled = true;
            //timer.Tick += new EventHandler(Tick);			
        }

        private void MScrollingText_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button== MouseButtons.Left)
            {
                foreach (var v in Items)
                {
                    if(v.Contain(e.Location))
                    {
                        TextClicked?.Invoke(v);
                        return;
                    }
                }
            }
        }

        private void MScrollingText_MouseLeave(object sender, EventArgs e)
        {
            scrollOn = true;
            this.Cursor = Cursors.Default;
        }

        private void MScrollingText_MouseEnter(object sender, EventArgs e)
        {
            if (stopScrollOnMouseOver)
            {
                scrollOn = false;
            }
            this.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// 资源清理
        /// </summary>
        protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				// 清理刷子
				if (foregroundBrush != null)
					foregroundBrush.Dispose();

				if (backgroundBrush != null)
					backgroundBrush.Dispose();

                // 清理定时器
                timerEnable = false;
				//if (timer != null)
				//	timer.Dispose();
			}
			base.Dispose( disposing );
		}

        /// <summary>
        /// 控制文本的动画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Tick()
		{
            // 更新矩形
            // lastKnownRect.X -= 10;
            // lastKnownRect.Width += 20;			
            //lastKnownRect.Inflate(10, 5);

            // 创建基于更新的矩形区域
            //Region updateRegion = new Region(lastKnownRect);
            while (timerEnable)
            {
                if (scrollOn)
                {
                    // 重新油漆的控制
                    Invalidate(this.ClientRectangle);
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate { Update(); });
                    }
                    else
                    {
                        Update();
                    }         
                    staticTextPos -= scrollPixelDistance;
                }
                Thread.Sleep(25);
            }
        }

		/// <summary>
		/// 重绘
		/// </summary>
		/// <param name="pe"></param>
		protected override void OnPaint(PaintEventArgs pe)
		{
            // 重绘文本新位置
            //填充用户的背景画刷
            if (backgroundBrush != null)
            {
                pe.Graphics.FillRectangle(backgroundBrush, 0, 0, this.ClientSize.Width, this.ClientSize.Height);
            }
            else
            {
                pe.Graphics.Clear(this.BackColor);
            }
            // 画出边框
            if (showBorder)
            {
                using (Pen borderPen = new Pen(borderColor))
                {
                    pe.Graphics.DrawRectangle(borderPen, 0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1);
                }
            }
            bool flag = false;
            float nowTextPos = staticTextPos;
            //计算需要绘制的文字
            for (int i = 0; i < Items.Count; i++)
            {
                if (nowTextPos > this.ClientSize.Width)
                {
                    break;
                }
                // 测量位置计算字符串的大小
                Items[i].stringLoactionX = nowTextPos;
                Items[i].stringLoactionY = yPos;
                Items[i].stringSize = pe.Graphics.MeasureString(Items[i].Text, this.Font);
                //只有可见的才绘制
                if (nowTextPos + Items[i].stringSize.Width > 0)
                {
                    // 绘制位图的文本字符串在内存中
                    if (foregroundBrush == null)
                    {
                        using (Brush tempForeBrush = new System.Drawing.SolidBrush(this.ForeColor))
                        {
                            pe.Graphics.DrawString(Items[i].Text, this.Font, tempForeBrush, nowTextPos, yPos);
                        }
                    }
                    else
                    {
                        pe.Graphics.DrawString(Items[i].Text, this.Font, foregroundBrush, nowTextPos, yPos);
                    }
                    //绘制过
                    flag = true;
                }
                //下一个文本的位置
                nowTextPos += Items[i].stringSize.Width;
                nowTextPos += 10;
            }
            if (!flag)
            {
                //重置
                staticTextPos = this.ClientSize.Width;
            }
            //计算滚动
            base.OnPaint(pe);
		}

		public event Action<MScrollingItem>  TextClicked;

		#region Properties
		//[Browsable(true),
		//CategoryAttribute("滚动内容"),
  //      Description("定时器间隔决定控制多久重绘一次")]
		//public int TextScrollSpeed
		//{
		//	set
		//	{
		//		timer.Interval = value;
		//	}
		//	get
		//	{
		//		return timer.Interval;
		//	}
		//}

		[
		Browsable(true),
        CategoryAttribute("滚动内容"),
		Description("滚动文字出现的位置(px)")
		]
		public int TextScrollDistance
		{
			set
			{
				scrollPixelDistance = value;
			}
			get
			{
				return scrollPixelDistance;
			}
		}

		[
		Browsable(true),
		CategoryAttribute("滚动内容"),
        Description("滚动的文本在那控制")
		]
		public string ScrollText
		{
			set
			{
				text = value;
				this.Invalidate();
				this.Update();
			}
			get
			{
				return text;
			}
		}

		[
		Browsable(true),
        CategoryAttribute("滚动内容"),
        Description("垂直对齐方式")
		]
		public VerticleTextPosition VerticleTextPosition
		{
			set
			{
				verticleTextPosition = value;
			}
			get
			{
				return verticleTextPosition;
			}
		}

		[
		Browsable(true),
        CategoryAttribute("滚动内容"),
        Description("开启或关闭边框")
		]
		public bool ShowBorder
		{
			set
			{
				showBorder = value;
			}
			get
			{
				return showBorder;
			}
		}

		[
		Browsable(true),
        CategoryAttribute("滚动内容"),
		Description("边框的颜色")
		]
		public Color BorderColor
		{
			set
			{
				borderColor = value;
			}
			get
			{
				return borderColor;
			}
		}

		[
		Browsable(true),
        CategoryAttribute("滚动内容"),
        Description("鼠标悬停于文本上是否停止滚动")
		]
		public bool StopScrollOnMouseOver
		{
			set
			{
				stopScrollOnMouseOver = value;
			}
			get
			{
				return stopScrollOnMouseOver;
			}
		}

		[
		Browsable(false)
		]
		public Brush ForegroundBrush
		{
			set
			{
				foregroundBrush = value;
			}
			get
			{
				return foregroundBrush;
			}
		}
		
		[
		ReadOnly(true)
		]
		public Brush BackgroundBrush
		{
			set
			{
				backgroundBrush = value;
			}
			get
			{
				return backgroundBrush;
			}
		}
		#endregion		
	}

    public class MScrollingItem
    {
        public string Text;
        public float stringLoactionX;
        public float stringLoactionY;
        public SizeF stringSize;
        public object Tag;
        public bool Contain(Point p)
        {
            if (p.X > stringLoactionX - 1 && p.Y > stringLoactionY - 1
                && p.X < stringLoactionX + stringSize.Width + 1 && p.Y < stringLoactionY + stringSize.Height + 1)
            {
                return true;
            }
            return false;
        }
    }

    public enum ScrollDirection
	{
		RightToLeft
		,LeftToRight
		,Bouncing
	}

	public enum VerticleTextPosition
	{
		Top
		,Center
		,Botom
	}
}
