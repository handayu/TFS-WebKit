using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USe.TradeDriver.Common;

namespace USeFuturesSpirit
{
    public partial class TaskGroupViewControl : UserControl
    {
        public TaskGroupViewControl()
        {
            InitializeComponent();
            this.gridTask.AutoGenerateColumns = false;
            this.gridOrderBook.AutoGenerateColumns = false;
        }

        public void SetDataSource(ArbitrageTaskGroup taskGroup)
        {
            SetTaskDataSource(taskGroup);
            SetOrderBookDataSource(taskGroup);
        }

        private void SetTaskDataSource(ArbitrageTaskGroup taskGroup)
        {
            BindingList<ArbitrageSubTaskViewModel> list = new BindingList<ArbitrageSubTaskViewModel>();

            if (taskGroup != null && taskGroup.TaskList != null)
            {
                foreach (ArbitrageTask task in taskGroup.TaskList)
                {
                    ArbitrageSubTaskViewModel firstTaskVm = new ArbitrageSubTaskViewModel() {
                        TaskId = task.TaskId,
                        OrderSide = task.FirstSubTask.OrderSide,
                        Instrument = task.FirstSubTask.Instrument,
                        PlanOrderQty = task.FirstSubTask.PlanOrderQty,
                        OrderQty = task.FirstSubTask.OrderQty,
                        TradeQty = task.FirstSubTask.TradeQty
                    };
                    list.Add(firstTaskVm);

                    ArbitrageSubTaskViewModel secondTaskVm = new ArbitrageSubTaskViewModel() {
                        TaskId = task.TaskId,
                        OrderSide = task.SecondSubTask.OrderSide,
                        Instrument = task.SecondSubTask.Instrument,
                        PlanOrderQty = task.SecondSubTask.PlanOrderQty,
                        OrderQty = task.SecondSubTask.OrderQty,
                        TradeQty = task.SecondSubTask.TradeQty
                    };
                    list.Add(secondTaskVm);
                }
            }

            this.gridTask.DataSource = list;
        }


        private void SetOrderBookDataSource(ArbitrageTaskGroup taskGroup)
        {
            BindingList<ArbitrageTaskOrderBookViewModel> list = new BindingList<ArbitrageTaskOrderBookViewModel>();

            if (taskGroup != null && taskGroup.TaskList != null)
            {
                foreach (ArbitrageTask task in taskGroup.TaskList)
                {
                    foreach(USeOrderBook orderBook in task.FirstSubTask.OrderBooks)
                    {
                        list.Add(CreateOrderBookVm(orderBook,task.TaskId));
                    }
                    foreach(USeOrderBook orderBook in task.SecondSubTask.OrderBooks)
                    {
                        list.Add(CreateOrderBookVm(orderBook, task.TaskId));
                    }
                }
            }

            this.gridOrderBook.DataSource = list;
        }

        private ArbitrageTaskOrderBookViewModel CreateOrderBookVm(USeOrderBook orderBook, int taskId)
        {
            ArbitrageTaskOrderBookViewModel viewModel = new ArbitrageTaskOrderBookViewModel();
            viewModel.Update(orderBook);
            viewModel.TaskId = taskId;
            return viewModel;
        }

        private void gridTask_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            ArbitrageSubTaskViewModel model = this.gridTask.Rows[e.RowIndex].DataBoundItem as ArbitrageSubTaskViewModel;
            if(model.TaskId %2 ==0)
            {
                e.CellStyle.BackColor = Color.LightBlue;
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }

        }

        private void gridOrderBook_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            ArbitrageTaskOrderBookViewModel model = this.gridOrderBook.Rows[e.RowIndex].DataBoundItem as ArbitrageTaskOrderBookViewModel;
            if (model.TaskId % 2 == 0)
            {
                e.CellStyle.BackColor = Color.LightBlue;
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }

        }
    }
}
