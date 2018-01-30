using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Diagnostics;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    class ArbitrageOrderTradeView
    {
        #region
        private const float m_taskDotDiameter = 10f;
        private const float m_itemInterval = 30f;
        private Font m_font = new Font("Arial", 9);
        private Font m_negativeFont = new Font("Arial", 8);
        private Font m_fontTitle = new Font("Arial", 16, FontStyle.Bold);

        private Brush m_taskInExecuteBrush = Brushes.Yellow;
        private Brush m_taskFinishBrush = Brushes.Green;
        private Brush m_taskNoExceuctBrush = Brushes.Red;
        private Brush m_negativeTaskBrush = new SolidBrush(Color.FromArgb(192, 0, 0));

        private Color m_taskInExecuteColor = Color.Yellow;
        private Color m_taskFinishColor = Color.Green;
        private Color m_taskNoExceuctColor = Color.Red;

        private Color m_negativeTaskColor = Color.FromArgb(192, 0, 0);
        private StringFormat m_centerStringFormat = null;

        private ArbitrageTaskGroup m_taskGroup = null;
        #endregion

        #region construction
        public ArbitrageOrderTradeView()
        {
            m_centerStringFormat = new StringFormat();
            m_centerStringFormat.Alignment = StringAlignment.Center;
            m_centerStringFormat.LineAlignment = StringAlignment.Center;
        }
        #endregion

        #region 设置数据源
        public void SetData(ArbitrageTaskGroup taskGroup)
        {
            m_taskGroup = taskGroup;
        }
        #endregion

        public void Paint(Graphics g)
        {
            if (m_taskGroup != null)
            {
                DrawTaskTitle(g);
                DrawTaskList(g);
            }
        }

        /// <summary>
        /// 绘制任务标题。
        /// </summary>
        /// <param name="g"></param>
        private void DrawTaskTitle(Graphics g)
        {
            float x = 10f;
            float y = 25f;
            float secondOffset = 30;
            if (m_taskGroup.PreferentialSide == USeOrderSide.Buy)
            {
                g.DrawString("B", m_fontTitle, Brushes.Red, x, y);
                g.DrawString("S", m_fontTitle, Brushes.Green, x, y + secondOffset);
            }
            else if (m_taskGroup.PreferentialSide == USeOrderSide.Sell)
            {
                g.DrawString("S", m_fontTitle, Brushes.Green, x, y);
                g.DrawString("B", m_fontTitle, Brushes.Red, x, y + secondOffset);
            }
        }

        /// <summary>
        /// 获取子任务Brush.
        /// </summary>
        /// <param name="subTask"></param>
        /// <returns></returns>

        private Brush GetSubTaskBrush(ArbitrageSubTask subTask)
        {
            Brush taskBrush = m_taskNoExceuctBrush;
            if (subTask.TradeQty == subTask.PlanOrderQty)
            {
                taskBrush = m_taskFinishBrush;
            }
            else if (subTask.OrderQty > 0)
            {
                taskBrush = m_taskInExecuteBrush;
            }
            else
            {
                taskBrush = m_taskNoExceuctBrush;
            }

            return taskBrush;
        }

        ///// <summary>
        ///// 绘制任务。
        ///// </summary>
        ///// <param name="g"></param>
        //private void DrawTaskList_2(Graphics g)
        //{
        //    ArbitrageTaskGroup taskGroup = m_taskGroup;
        //    float xBegin = 35.0f;
        //    float yBegin = 10.0f;
        //    List<ArbitrageTask> taskList = taskGroup.TaskList;
        //    if (taskList != null && taskList.Count > 0)
        //    {
        //        for (int i = 0; i < taskList.Count; i++)
        //        {
        //            ArbitrageTask task = taskList[i];
        //            float x = xBegin + (i * m_itemInterval);
        //            float y = yBegin;

        //            ArbitrageSubTask firstSubTask = task.FirstSubTask;
        //            ArbitrageSubTask secondSubTask = task.SecondSubTask;

        //            //优先任务 
        //            Brush firstTaskBrush = m_taskNoExceuctBrush;
        //            if (firstSubTask.TradeQty == firstSubTask.PlanOrderQty)
        //            {
        //                firstTaskBrush = m_taskFinishBrush;
        //            }
        //            else if (firstSubTask.OrderQty > 0)
        //            {
        //                firstTaskBrush = m_taskInExecuteBrush;
        //            }
        //            else
        //            {
        //                firstTaskBrush = m_taskNoExceuctBrush;
        //            }
        //            g.FillEllipse(firstTaskBrush, x, y, m_taskDotWidth, m_taskDotHeight);
        //            g.DrawString(string.Format("{0}/{1}", firstSubTask.TradeQty, firstSubTask.PlanOrderQty),
        //                m_font, firstTaskBrush, new RectangleF(x + m_taskDotWidth / 2 - 15, y + 13, 30, 10), m_centerStringFormat);

        //            //反手任务
        //            Brush secondTaskBrush = m_taskNoExceuctBrush;
        //            if (secondSubTask.TradeQty == secondSubTask.PlanOrderQty)
        //            {
        //                secondTaskBrush = m_taskFinishBrush;
        //            }
        //            else if (secondSubTask.OrderQty > 0)
        //            {
        //                secondTaskBrush = m_taskInExecuteBrush;
        //            }
        //            else
        //            {
        //                secondTaskBrush = m_taskNoExceuctBrush;
        //            }
        //            //反手任务
        //            g.FillEllipse(secondTaskBrush, x, y + 30, m_taskDotWidth, m_taskDotHeight);
        //            g.DrawString(string.Format("{0}/{1}", secondSubTask.TradeQty, secondSubTask.PlanOrderQty),
        //                m_font, secondTaskBrush, new RectangleF(x + m_taskDotWidth / 2 - 15, y + 30 + 13, 30, 10), m_centerStringFormat);
        //        }
        //    }
        //}

        private void DrawTaskList(Graphics g)
        {
            float xBegin = 45.0f;   // 起始x坐标
            float yBegin = 10.0f;   // 起始y坐标

            float y_f_NegativeQtyOffset = 0;     // 优先任务反手量偏移位置
            float y_f_taskOffset = 13f;         // 优先任务主图标偏移位置
            float y_f_QtyOffset = 26f;    // 优先任务正手量偏移位置

            float y_s_taskOffset = 45f;   // 反手任务主图标偏移位置
            float y_s_QtyOffset = 58f;    // 反手任务正手量偏移位置

            ArbitrageTaskGroup taskGroup = m_taskGroup;

            List<ArbitrageTask> taskList = taskGroup.TaskList;
            if (taskList == null || taskList.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < taskList.Count; i++)
            {
                ArbitrageTask task = taskList[i];
                float x = xBegin + (i * m_itemInterval);
                float y = yBegin;

                ArbitrageSubTask firstSubTask = task.FirstSubTask;
                ArbitrageSubTask secondSubTask = task.SecondSubTask;
                Brush firstTaskBrush = GetSubTaskBrush(firstSubTask);
                Brush secondTaskBrush = GetSubTaskBrush(secondSubTask);

                Pen tmpPen = new Pen(Color.Red);

                //优先合约反向委托量
                if (firstSubTask.NegativeOrderQty > 0)
                    //if (i == 2)
                {
                    float rectWidth = 30f;
                    float rectHeight = 10f;
                    RectangleF rect = new RectangleF(x - (rectWidth / 2), y + y_f_NegativeQtyOffset, rectWidth, rectHeight);
                    string text = string.Format("{0}/{1}", firstSubTask.NegativeTradeQty , firstSubTask.NegativeOrderQty);
                    g.DrawString(text, m_negativeFont, m_negativeTaskBrush, rect, m_centerStringFormat);
                }

                //优先任务点
                g.FillEllipse(firstTaskBrush, x - m_taskDotDiameter / 2, y + y_f_taskOffset, m_taskDotDiameter, m_taskDotDiameter);

                //g.DrawRectangle(tmpPen, x, y + f_y_taskOffset, m_taskDotWidth, m_taskDotHeight);
                {
                    float rectWidth = 30f;
                    float rectHeight = 10f;
                    RectangleF rect = new RectangleF(x - rectWidth / 2, y + y_f_QtyOffset, rectWidth, rectHeight);
                    string text = string.Format("{0}/{1}", firstSubTask.TradeQty, firstSubTask.PlanOrderQty);
                    g.DrawString(text, m_font, firstTaskBrush, rect, m_centerStringFormat);
                }

                //反手任务点
                g.FillEllipse(secondTaskBrush, x - m_taskDotDiameter / 2, y + y_s_taskOffset, m_taskDotDiameter, m_taskDotDiameter);

                //g.DrawRectangle(tmpPen, x, y + s_y_taskOffset, m_taskDotWidth, m_taskDotHeight);

                {
                    float rectWidth = 30f;
                    float rectHeight = 10f;
                    RectangleF rect = new RectangleF(x - rectWidth / 2, y + y_s_QtyOffset, rectWidth, rectHeight);
                    string text = string.Format("{0}/{1}", secondSubTask.TradeQty, secondSubTask.PlanOrderQty);
                    g.DrawString(text, m_font, secondTaskBrush, rect, m_centerStringFormat);
                }
            }
        }
    }
}
