using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using USeFuturesSpirit.ViewModel;
using USe.Common;
using USe.TradeDriver.Common;
using USe.Common.AppLogger;

namespace USeFuturesSpirit
{
    public partial class PositionListControl : USeUserControl
    {
        #region DataSource数据源
        /// <summary>
        /// 持仓
        /// </summary>
        private BindingList<PositionDataViewModel> m_position_data_source = new BindingList<PositionDataViewModel>();

        /// <summary>
        /// 持仓明细
        /// </summary>
        private BindingList<PositionDetailDataViewModel> m_positionDetail_data_source = new BindingList<PositionDetailDataViewModel>();

        #endregion

        #region List控件初始化
        public PositionListControl()
        {
            InitializeComponent();
        }
        #endregion

        #region 业务数据初始化
        public override void Initialize()
        {
            PositionDetailGridInitialize();
            PositionGridInitialize();
        }


        private void PositionGridInitialize()
        {
            this.gridPosition.Visible = true;
            this.gridPosition.Dock = DockStyle.Fill;

            this.gridPosition.AutoGenerateColumns = false;
            this.gridPosition.DataSource = m_position_data_source;

            USeManager.Instance.OrderDriver.OnPositionChanged += OrderDriver_OnPositionChanged;

            //查询持仓
            List<USePosition> position_book = USeManager.Instance.OrderDriver.QueryPositions();
            if (position_book != null && position_book.Count > 0)
            {
                foreach (USePosition position in position_book)
                {
                    UpdatePositionBook(position);
                }
            }

            //订阅市场行情更新计算
            USeManager.Instance.QuoteDriver.OnMarketDataChanged += QuoteDriver_OnMarketDataChanged;
        }


        private void PositionDetailGridInitialize()
        {
            //默认初始化选中Sum充满并隐藏detail
            this.rbnView_PositionSum.Checked = true;
            this.gridPositionDetail.Visible = false;
            this.gridPositionDetail.AutoGenerateColumns = false;
            this.gridPositionDetail.DataSource = m_positionDetail_data_source;

            USeManager.Instance.OrderDriver.OnPositionDetailChanged += OrderDriver_OnPositionDetailChanged;

            //查询持仓
            List<USePositionDetail> positionDetail_book = USeManager.Instance.OrderDriver.QueryPositionDetail();
            if (positionDetail_book != null && positionDetail_book.Count > 0)
            {
                foreach (USePositionDetail positionDetail in positionDetail_book)
                {
                    UpdatePositionDetailBook(positionDetail);
                }
            }


        }

        #endregion

        #region PositionDetail数据更新
        private void OrderDriver_OnPositionDetailChanged(object sender, USePositionDetailChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<USePositionDetailChangedEventArgs>(OrderDriver_OnPositionDetailChanged), sender, e);
                return;
            }

            UpdatePositionDetailBook(e.PositionDetail);
        }


        private void UpdatePositionDetailBook(USePositionDetail positionDetail)
        {
            PositionDetailDataViewModel PositionDetail_Model = m_positionDetail_data_source.FirstOrDefault(p => (p.InstrumentCode == positionDetail.Instrument.InstrumentCode) && (p.Direction == positionDetail.Direction));

            if (PositionDetail_Model != null)
            {
                m_positionDetail_data_source.Remove(PositionDetail_Model);
            }
            else
            {
                PositionDetailDataViewModel positionDetail_model = PositionDetailDataViewModel.Creat(positionDetail);
                m_positionDetail_data_source.Add(positionDetail_model);
            }
        }
        #endregion

        #region Position数据更新
        private void OrderDriver_OnPositionChanged(object sender, USePositionChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<USePositionChangedEventArgs>(OrderDriver_OnPositionChanged), sender, e);
                return;
            }

            UpdatePositionBook(e.Position);
        }

        private void UpdatePositionBook(USePosition position)
        {
            PositionDataViewModel Position_Model = m_position_data_source.FirstOrDefault(p => (p.InstrumentCode == position.InstrumentCode) && (p.Direction == position.Direction));

            if (Position_Model != null)
            {
                if (position.NewPosition + position.OldPosition == 0)
                {
                    m_position_data_source.Remove(Position_Model);
                }
                else
                {
                    Position_Model.Update(position);

                    if (position.NewPosition < position.NewFrozonPosition)
                    {

                        string tmpTestText = string.Format(@"[HanyuListUpdate],DateTime:{0} InstrumentCode:{1} Direction:{2} OpenQty：{3},NewAvaliablePosition:{4} ,NewFrozonPosition:{5}，NewPosition{6}," +
                            "OldAvaliablePosition:{7}，OldFrozonPosition:{8},OldPosition:{9}",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), position.InstrumentCode, position.Direction, position.OpenQty, position.NewAvaliablePosition,
                            position.NewFrozonPosition, position.NewPosition, position.OldAvaliablePosition, position.OldFrozonPosition, position.OldPosition);

                        USeManager.Instance.EventLogger.WriteAudit(tmpTestText);
                    }

                }
            }
            else
            {
                if (position.NewPosition + position.OldPosition == 0)
                {
                    m_position_data_source.Remove(Position_Model);
                }
                else
                {
                    PositionDataViewModel position_model = PositionDataViewModel.Creat(position);
                    m_position_data_source.Add(position_model);
                }
            }
        }
        #endregion

        #region OnMarketDataChange计算浮动盈亏
        private void QuoteDriver_OnMarketDataChanged(object sender, USeMarketDataChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<USeMarketDataChangedEventArgs>(QuoteDriver_OnMarketDataChanged), sender, e);
                return;
            }

            ////找到对应ins的合约根据多空方向计算浮动盈亏
            //PositionDataViewModel Position_Model = m_position_data_source.FirstOrDefault(p => (p.InstrumentCode == e.MarketData.Instrument.InstrumentCode));

            //Position_Model

            //if (Position_Model != null)
            //{
            //    marketModel.Update(e.MarketData);
            //}
        }
        #endregion

        #region radioButton事件
        private void rbnView_PositionSum_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbnView_PositionSum.Checked)
            {
                //选中Sum
                this.gridPositionDetail.Visible = false;
                this.gridPosition.Visible = true;
                this.gridPosition.Dock = DockStyle.Fill;
            }
        }

        private void rbnView_PositionDetail_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbnView_PositionDetail.Checked)
            {
                //选中detail
                this.gridPosition.Visible = false;
                this.gridPositionDetail.Visible = true;
                this.gridPositionDetail.Dock = DockStyle.Fill;
            }
        }

        #endregion
    }
}
