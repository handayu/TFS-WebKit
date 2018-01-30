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
	/// �����
	/// </summary>
	public class MScrollingText : System.Windows.Forms.Control
	{
        public List<MScrollingItem> Items = new List<MScrollingItem>(); //��������
        private bool timerEnable = false;               // �����ı������ļ�ʱ�����ơ�						
        private string text = "Text";					// �����ı�
		private float staticTextPos = 0;				// �����ı���X�����ص�
        private float yPos = 0;							// �����ı���Y�����ص�
        private VerticleTextPosition verticleTextPosition = VerticleTextPosition.Center;	// ���ı���ֱ���õķ�ʽ
        private int scrollPixelDistance = 2;			// ÿ����ʱ���¼����ı������Զ
		private bool showBorder = false;					// �Ƿ���ʾ�߽�
        private bool stopScrollOnMouseOver = true;		// �����ͣ���ı����Ƿ�ֹͣ����
		private bool scrollOn = true;					// �Ƿ����
        private Brush foregroundBrush = null;			// �����û��Զ����ˢ����Ϊ�ı�������
        private Brush backgroundBrush = null;			// �����û��Զ����ˢ����Ϊ����
        private Color borderColor = Color.Black;		// �����û����õ���ɫ���Ʊ߽�

		public MScrollingText()
		{
            // ����Ĭ������ScrollingText����
            this.Name = "ScrollingText";
            this.Size = new System.Drawing.Size(216, 40);
            this.MouseClick +=new MouseEventHandler(MScrollingText_MouseClick);
            this.staticTextPos = this.ClientSize.Width;
            this.MouseEnter +=new EventHandler(MScrollingText_MouseEnter);
            this.MouseLeave += new EventHandler(MScrollingText_MouseLeave);

            // �����ڲ�˫���弼���Ķ���GDI +�滭
            this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);

            // ���ö�ʱ��
            timerEnable = true;
            Thread timer = new Thread(new ThreadStart(Tick));
            timer.IsBackground = true;
            timer.Start();
            timer = null;
            //timer.Interval = 25;	// Ĭ�ϵ�ʱ����
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
        /// ��Դ����
        /// </summary>
        protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				// ����ˢ��
				if (foregroundBrush != null)
					foregroundBrush.Dispose();

				if (backgroundBrush != null)
					backgroundBrush.Dispose();

                // ����ʱ��
                timerEnable = false;
				//if (timer != null)
				//	timer.Dispose();
			}
			base.Dispose( disposing );
		}

        /// <summary>
        /// �����ı��Ķ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Tick()
		{
            // ���¾���
            // lastKnownRect.X -= 10;
            // lastKnownRect.Width += 20;			
            //lastKnownRect.Inflate(10, 5);

            // �������ڸ��µľ�������
            //Region updateRegion = new Region(lastKnownRect);
            while (timerEnable)
            {
                if (scrollOn)
                {
                    // ��������Ŀ���
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
		/// �ػ�
		/// </summary>
		/// <param name="pe"></param>
		protected override void OnPaint(PaintEventArgs pe)
		{
            // �ػ��ı���λ��
            //����û��ı�����ˢ
            if (backgroundBrush != null)
            {
                pe.Graphics.FillRectangle(backgroundBrush, 0, 0, this.ClientSize.Width, this.ClientSize.Height);
            }
            else
            {
                pe.Graphics.Clear(this.BackColor);
            }
            // �����߿�
            if (showBorder)
            {
                using (Pen borderPen = new Pen(borderColor))
                {
                    pe.Graphics.DrawRectangle(borderPen, 0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1);
                }
            }
            bool flag = false;
            float nowTextPos = staticTextPos;
            //������Ҫ���Ƶ�����
            for (int i = 0; i < Items.Count; i++)
            {
                if (nowTextPos > this.ClientSize.Width)
                {
                    break;
                }
                // ����λ�ü����ַ����Ĵ�С
                Items[i].stringLoactionX = nowTextPos;
                Items[i].stringLoactionY = yPos;
                Items[i].stringSize = pe.Graphics.MeasureString(Items[i].Text, this.Font);
                //ֻ�пɼ��ĲŻ���
                if (nowTextPos + Items[i].stringSize.Width > 0)
                {
                    // ����λͼ���ı��ַ������ڴ���
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
                    //���ƹ�
                    flag = true;
                }
                //��һ���ı���λ��
                nowTextPos += Items[i].stringSize.Width;
                nowTextPos += 10;
            }
            if (!flag)
            {
                //����
                staticTextPos = this.ClientSize.Width;
            }
            //�������
            base.OnPaint(pe);
		}

		public event Action<MScrollingItem>  TextClicked;

		#region Properties
		//[Browsable(true),
		//CategoryAttribute("��������"),
  //      Description("��ʱ������������ƶ���ػ�һ��")]
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
        CategoryAttribute("��������"),
		Description("�������ֳ��ֵ�λ��(px)")
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
		CategoryAttribute("��������"),
        Description("�������ı����ǿ���")
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
        CategoryAttribute("��������"),
        Description("��ֱ���뷽ʽ")
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
        CategoryAttribute("��������"),
        Description("������رձ߿�")
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
        CategoryAttribute("��������"),
		Description("�߿����ɫ")
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
        CategoryAttribute("��������"),
        Description("�����ͣ���ı����Ƿ�ֹͣ����")
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
