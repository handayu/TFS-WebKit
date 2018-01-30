using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace mPaint
{
    /// <summary>
    /// 标题字段的属性
    /// </summary>
    public class TitleField
    {
        public TitleField(string relateSeriesField, string field,
            string displayTitle, Color fieldColor, bool onlyShow)
        {
            this.relateSeriesField = relateSeriesField;
            this.field = field;
            this.displayTitle = displayTitle;
            this.FieldColor = fieldColor;
            this.mainFlag = onlyShow;
        }

        ~TitleField()
        {
            if (fieldBrush != null) fieldBrush.Dispose();
        }

        /// <summary>
        /// 相关的线条字段的名称
        /// </summary>
        private string relateSeriesField = string.Empty;

        public string RelateSeriesField
        {
            get { return relateSeriesField; }
            set { relateSeriesField = value; }
        }

        /// <summary>
        /// 字段名
        /// </summary>
        private string field = string.Empty;

        public string Field
        {
            get { return field; }
            set { field = value; }
        }

        /// <summary>
        /// 显示的名称
        /// </summary>
        private string displayTitle = string.Empty;

        public string DisplayTitle
        {
            get { return displayTitle; }
            set { displayTitle = value; }
        }

        /// <summary>
        /// 字段的颜色
        /// </summary>
        private Color fieldColor = Color.White;

        public Color FieldColor
        {
            get { return fieldColor; }
            set
            {
                fieldColor = value;
                if (fieldBrush != null)
                {
                    fieldBrush.Dispose();
                }
                fieldBrush = new SolidBrush(value);
            }
        }

        /// <summary>
        /// 字段颜色的刷子
        /// </summary>
        private Brush fieldBrush = new SolidBrush(Color.White);

        public Brush FieldBrush
        {
            get { return fieldBrush; }
            set { fieldBrush = value; }
        }

        /// <summary>
        /// 主标题flag
        /// </summary>
        private bool mainFlag = false;

        public bool MainFlag
        {
            get { return mainFlag; }
            set { mainFlag = value; }
        }
    }
}
