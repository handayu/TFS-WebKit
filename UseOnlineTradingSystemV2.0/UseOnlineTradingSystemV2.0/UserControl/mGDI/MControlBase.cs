
using System;
namespace mPaint
{
    public class MControlBase
    {
        /// <summary>
        /// ���췽��
        /// </summary>
        public MControlBase()
        {

        }

        private string name = null;

        /// <summary>
        /// ��ȡ�����ÿؼ�����
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// ͼ��
        /// </summary>
        public int Layer = 0;

        private String text;
        /// <summary>
        /// ��ȡ�����ÿؼ�����
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
        /// ��ȡ��������������
        /// </summary>
        public FONT Font
        {
            get { return font; }
            set { font = value; }
        }

        private int foreColor = COLOR.RGB(0, 0, 0);

        /// <summary>
        /// ��ȡ������������ɫ
        /// </summary>
        public int ForeColor
        {
            get { return foreColor; }
            set { foreColor = value; }
        }

        private int mouseforeColor = COLOR.RGB(0, 0, 0);

        /// <summary>
        /// ��ȡ�������������������ɫ
        /// </summary>
        public int MouseForeColor
        {
            get { return mouseforeColor; }
            set { mouseforeColor = value; }
        }

        private RECT rectangle;
        /// <summary>
        /// ��ȡ�����ÿؼ�����
        /// </summary>
        public RECT Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        private int borderColor = COLOR.RGB(255, 0, 0);
        /// <summary>
        /// ��ȡ�����ñ߿���ɫ
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
        /// ��ȡ�����ñ�����ɫ
        /// </summary>
        public int BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }
        /// <summary>
        /// ��ȡ�������������������ɫ
        /// </summary>
        public int MouseBackColor
        {
            get { return mousebackColor; }
            set { mousebackColor = value; }
        }
        /// <summary>
        /// ��ȡ�����������������ɫ
        /// </summary>
        public int MouseClickBackColor
        {
            get { return mouseClickBackColor; }
            set { mouseClickBackColor = value; }
        }
        private bool visible = true;
        /// <summary>
        /// ��ȡ�����ÿؼ��Ƿ�ɼ�
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        private bool enable = true;
        /// <summary>
        /// ��ȡ�����ÿؼ��Ƿ�����
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
        /// ��ȡ����������
        /// </summary>
        public int ZOrder
        {
            get { return zOrder; }
            set { zOrder = value; }
        }

        private bool redraw=true;

        ////�Ƿ��ػ�
        //public bool Redraw
        //{
        //    get { return redraw; }
        //    set { redraw = value; }
        //}

        private object parent;

        /// <summary>
        /// ��ȡ�����ø�����
        /// </summary>
        public object Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private Object tag;
        /// <summary>
        /// ��ȡ�����ö����ǩ
        /// </summary>
        public Object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        private bool isHand = false;
        /// <summary>
        /// ��ȡ��������꾭��ʱ�Ƿ��Ϊ����
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
        /// ����¼�
        /// </summary>
        public event MMouseEventHandler Click;
        /// <summary>
        /// �ƶ��¼�
        /// </summary>
        public event MMouseEventHandler MouseMove;
        /// <summary>
        /// �����¼�
        /// </summary>
        public event MMouseEventHandler MouseDown;
        /// <summary>
        /// ̧���¼�
        /// </summary>
        public event MMouseEventHandler MouseUp;
        /// <summary>
        /// �����¼�
        /// </summary>
        public event MMouseEventHandler MouseEnter;
        /// <summary>
        /// �뿪�¼�
        /// </summary>
        public event MMouseEventHandler MouseLeave;
        /// <summary>
        /// ������
        /// </summary>
        public event MMouseEventHandler MouseWheel;
        /// <summary>
        /// �����¼�
        /// </summary>
        public event MPaintEventHandler Paint;

        /// <summary>
        /// ִ�е���¼��ķ���
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
        /// ִ���ƶ��¼�
        /// </summary>
        /// <param name="e"></param>
        public void DoMouseMove(MMouseEventArgs e)
        {
            this.OnMouseMove(e);
            this.MouseMove?.Invoke(this, e);
        }

        /// <summary>
        /// ִ�а����¼�
        /// </summary>
        /// <param name="e"></param>
        public void DoMouseDown(MMouseEventArgs e)
        {
            this.OnMouseDown(e);
            this.MouseDown?.Invoke(this, e);
        }

        /// <summary>
        /// ִ��̧���¼�
        /// </summary>
        /// <param name="e"></param>
        public void DoMouseUp(MMouseEventArgs e)
        {
            this.OnMouseUp(e);
            this.MouseUp?.Invoke(this, e);
        }

        /// <summary>
        /// ִ�н����¼�
        /// </summary>
        /// <param name="e"></param>
        public void DoMouseEnter(MMouseEventArgs e)
        {
            this.OnMouseEnter(e);
            this.MouseEnter?.Invoke(this, e);
        }

        /// <summary>
        /// ִ���뿪�¼�
        /// </summary>
        /// <param name="e"></param>
        public void DoMouseLeave(MMouseEventArgs e)
        {
            this.OnMouseLeave(e);
            this.MouseLeave?.Invoke(this, e);
        }

        /// <summary>
        /// ִ��������
        /// </summary>
        /// <param name="e"></param>
        public void DoMouseWheel(MMouseEventArgs e)
        {
            this.OnMouseWheel(e);
            this.MouseWheel?.Invoke(this, e);
        }

        /// <summary>
        /// ִ�л����¼�
        /// </summary>
        /// <param name="e"></param>
        public void DoPaint(MPaintEventArgs e)
        {
            this.OnPaint(e);
            this.Paint?.Invoke(this, e);
        }

        /// <summary>
        /// ���
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClick(MMouseEventArgs e){}

        /// <summary>
        /// ����ƶ�
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseMove(MMouseEventArgs e){}

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseDown(MMouseEventArgs e){}

        /// <summary>
        /// ̧��
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseUp(MMouseEventArgs e){}

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseEnter(MMouseEventArgs e){}

        /// <summary>
        /// �뿪
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseLeave(MMouseEventArgs e){}

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseWheel(MMouseEventArgs e){}

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPaint(MPaintEventArgs e){}

        /// <summary>
        /// �ѿؼ�̧����ǰ��
        /// </summary>
        public void BringToFront(){}

        /// <summary>
        /// �ѿؼ��ŵ�����
        /// </summary>
        public void SendToBack(){}

        /// <summary>
        /// ���ٷ���
        /// </summary>
        public virtual void Dispose(){}
    }
}
