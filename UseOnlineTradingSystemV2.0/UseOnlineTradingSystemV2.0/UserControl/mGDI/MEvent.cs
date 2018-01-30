using System.Windows.Forms;
namespace mPaint
{
    /// <summary>
    /// 鼠标事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MMouseEventHandler(object sender, MMouseEventArgs e);

    /// <summary>
    /// 绘画委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MPaintEventHandler(object sender, MPaintEventArgs e);

    /// <summary>
    /// 鼠标事件参数类
    /// </summary>
    public class MMouseEventArgs
    {
        #region 构造方法

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="args"></param>
        /// <param name="board"></param>
        /// <param name="control"></param>
        public MMouseEventArgs(MouseEventArgs args, MBorad board, MControlBase control)
        {
            this.mouseEventArgs = args;
            this.board = board;
            this.control = control;
        }

        #endregion

        #region 属性

        private MouseEventArgs mouseEventArgs = null;
        /// <summary>
        /// 获取或设置鼠标参数
        /// </summary>
        public MouseEventArgs MouseEventArgs
        {
            get { return mouseEventArgs; }
            set { mouseEventArgs = value; }
        }

        private MBorad board = null;
        /// <summary>
        /// 获取或设置画板
        /// </summary>
        public MBorad Board
        {
            get { return board; }
            set { board = value; }
        }

        private MControlBase control = null;
        /// <summary>
        /// 获取或设置控件对象
        /// </summary>
        public MControlBase Control
        {
            get { return control; }
            set { control = value; }
        }

        #endregion
    }

    /// <summary>
    /// 绘画参数类
    /// </summary>
    public class MPaintEventArgs
    {
        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="g"></param>
        /// <param name="advancedSearch"></param>
        public MPaintEventArgs(MPaint cp, MBorad advancedSearch)
        {
            this.cPaint = cp;
            this.board = advancedSearch;
        }

        #endregion

        #region 属性

        private MPaint cPaint = null;
        /// <summary>
        /// 画板
        /// </summary>
        public MPaint CPaint
        {
            get { return cPaint; }
            set { cPaint = value; }
        }

        private MBorad board = null;
        /// <summary>
        /// 主窗体对象
        /// </summary>
        public MBorad Board
        {
            get { return board; }
            set { board = value; }
        }

        #endregion
    }
}
