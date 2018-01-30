namespace USeFuturesSpirit
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panelTop = new System.Windows.Forms.Panel();
            this.arbitrageQuoteListControl1 = new USeFuturesSpirit.ArbitrageQuoteListControl();
            this.investorFundControl1 = new USeFuturesSpirit.InvestorFundControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panelBottomLeft = new System.Windows.Forms.Panel();
            this.arbitrageOrderListControl1 = new USeFuturesSpirit.ArbitrageOrderListControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.positionListControl1 = new USeFuturesSpirit.PositionListControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.orderBookListControl1 = new USeFuturesSpirit.OrderBookListControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tradeBookListControl1 = new USeFuturesSpirit.TradeBookListControl();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.bottomStateControl1 = new USeFuturesSpirit.BottomStateControl();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.panelMiddleLeft = new System.Windows.Forms.Panel();
            this.splitterMiddelRight = new System.Windows.Forms.Splitter();
            this.panelMiddleRight = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.arbitrageLogViewControl1 = new USeFuturesSpirit.ArbitrageLogViewControl();
            this.alarmNoticeViewControl1 = new USeFuturesSpirit.AlarmNoticeViewControl();
            this.arbitrageRunStateControl1 = new USeFuturesSpirit.ArbitrageRunStateControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExitSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHistoryArbitrage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmView = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSimpleOrderPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOption = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSystemConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.套利合约配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.默认下单参数配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAboutMe = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTop.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panelBottomLeft.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.panelStatus.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelMiddle.SuspendLayout();
            this.panelMiddleLeft.SuspendLayout();
            this.panelMiddleRight.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.arbitrageQuoteListControl1);
            this.panelTop.Controls.Add(this.investorFundControl1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 24);
            this.panelTop.Margin = new System.Windows.Forms.Padding(2);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1167, 186);
            this.panelTop.TabIndex = 0;
            // 
            // arbitrageQuoteListControl1
            // 
            this.arbitrageQuoteListControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arbitrageQuoteListControl1.Location = new System.Drawing.Point(0, 42);
            this.arbitrageQuoteListControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.arbitrageQuoteListControl1.Name = "arbitrageQuoteListControl1";
            this.arbitrageQuoteListControl1.Product = null;
            this.arbitrageQuoteListControl1.Size = new System.Drawing.Size(1167, 144);
            this.arbitrageQuoteListControl1.TabIndex = 0;
            // 
            // investorFundControl1
            // 
            this.investorFundControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.investorFundControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.investorFundControl1.Location = new System.Drawing.Point(0, 0);
            this.investorFundControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.investorFundControl1.Name = "investorFundControl1";
            this.investorFundControl1.Size = new System.Drawing.Size(1167, 42);
            this.investorFundControl1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(696, 435);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panelBottomLeft);
            this.tabPage1.Location = new System.Drawing.Point(23, 4);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(669, 427);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "组合下单监控";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panelBottomLeft
            // 
            this.panelBottomLeft.Controls.Add(this.arbitrageOrderListControl1);
            this.panelBottomLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottomLeft.Location = new System.Drawing.Point(2, 2);
            this.panelBottomLeft.Margin = new System.Windows.Forms.Padding(2);
            this.panelBottomLeft.Name = "panelBottomLeft";
            this.panelBottomLeft.Size = new System.Drawing.Size(665, 423);
            this.panelBottomLeft.TabIndex = 3;
            // 
            // arbitrageOrderListControl1
            // 
            this.arbitrageOrderListControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arbitrageOrderListControl1.Location = new System.Drawing.Point(0, 0);
            this.arbitrageOrderListControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.arbitrageOrderListControl1.Name = "arbitrageOrderListControl1";
            this.arbitrageOrderListControl1.Size = new System.Drawing.Size(665, 423);
            this.arbitrageOrderListControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.positionListControl1);
            this.tabPage2.Location = new System.Drawing.Point(23, 4);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(669, 427);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "持仓";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // positionListControl1
            // 
            this.positionListControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.positionListControl1.Location = new System.Drawing.Point(2, 2);
            this.positionListControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.positionListControl1.Name = "positionListControl1";
            this.positionListControl1.Size = new System.Drawing.Size(665, 423);
            this.positionListControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.orderBookListControl1);
            this.tabPage3.Location = new System.Drawing.Point(23, 4);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(669, 427);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "委托";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // orderBookListControl1
            // 
            this.orderBookListControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderBookListControl1.Location = new System.Drawing.Point(0, 0);
            this.orderBookListControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.orderBookListControl1.Name = "orderBookListControl1";
            this.orderBookListControl1.Size = new System.Drawing.Size(669, 427);
            this.orderBookListControl1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tradeBookListControl1);
            this.tabPage4.Location = new System.Drawing.Point(23, 4);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(669, 427);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "成交";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tradeBookListControl1
            // 
            this.tradeBookListControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tradeBookListControl1.Location = new System.Drawing.Point(0, 0);
            this.tradeBookListControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tradeBookListControl1.Name = "tradeBookListControl1";
            this.tradeBookListControl1.Size = new System.Drawing.Size(669, 427);
            this.tradeBookListControl1.TabIndex = 0;
            // 
            // panelStatus
            // 
            this.panelStatus.Controls.Add(this.bottomStateControl1);
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStatus.Location = new System.Drawing.Point(0, 647);
            this.panelStatus.Margin = new System.Windows.Forms.Padding(2);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(1167, 40);
            this.panelStatus.TabIndex = 4;
            // 
            // bottomStateControl1
            // 
            this.bottomStateControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomStateControl1.Location = new System.Drawing.Point(0, 0);
            this.bottomStateControl1.Margin = new System.Windows.Forms.Padding(2);
            this.bottomStateControl1.Name = "bottomStateControl1";
            this.bottomStateControl1.Size = new System.Drawing.Size(1167, 40);
            this.bottomStateControl1.TabIndex = 0;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.panelMiddle);
            this.panelBottom.Controls.Add(this.splitter1);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(0, 210);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(2);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1167, 437);
            this.panelBottom.TabIndex = 5;
            // 
            // panelMiddle
            // 
            this.panelMiddle.Controls.Add(this.panelMiddleLeft);
            this.panelMiddle.Controls.Add(this.splitterMiddelRight);
            this.panelMiddle.Controls.Add(this.panelMiddleRight);
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(0, 2);
            this.panelMiddle.Margin = new System.Windows.Forms.Padding(2);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(1167, 435);
            this.panelMiddle.TabIndex = 2;
            // 
            // panelMiddleLeft
            // 
            this.panelMiddleLeft.Controls.Add(this.tabControl1);
            this.panelMiddleLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddleLeft.Location = new System.Drawing.Point(0, 0);
            this.panelMiddleLeft.Margin = new System.Windows.Forms.Padding(2);
            this.panelMiddleLeft.Name = "panelMiddleLeft";
            this.panelMiddleLeft.Size = new System.Drawing.Size(696, 435);
            this.panelMiddleLeft.TabIndex = 3;
            // 
            // splitterMiddelRight
            // 
            this.splitterMiddelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitterMiddelRight.Location = new System.Drawing.Point(696, 0);
            this.splitterMiddelRight.Margin = new System.Windows.Forms.Padding(2);
            this.splitterMiddelRight.Name = "splitterMiddelRight";
            this.splitterMiddelRight.Size = new System.Drawing.Size(3, 435);
            this.splitterMiddelRight.TabIndex = 2;
            this.splitterMiddelRight.TabStop = false;
            // 
            // panelMiddleRight
            // 
            this.panelMiddleRight.Controls.Add(this.panel1);
            this.panelMiddleRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelMiddleRight.Location = new System.Drawing.Point(699, 0);
            this.panelMiddleRight.Margin = new System.Windows.Forms.Padding(2);
            this.panelMiddleRight.Name = "panelMiddleRight";
            this.panelMiddleRight.Size = new System.Drawing.Size(468, 435);
            this.panelMiddleRight.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Controls.Add(this.arbitrageRunStateControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(468, 435);
            this.panel1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.arbitrageLogViewControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.alarmNoticeViewControl1);
            this.splitContainer1.Size = new System.Drawing.Size(468, 408);
            this.splitContainer1.SplitterDistance = 183;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 2;
            // 
            // arbitrageLogViewControl1
            // 
            this.arbitrageLogViewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arbitrageLogViewControl1.Location = new System.Drawing.Point(0, 0);
            this.arbitrageLogViewControl1.Margin = new System.Windows.Forms.Padding(2);
            this.arbitrageLogViewControl1.Name = "arbitrageLogViewControl1";
            this.arbitrageLogViewControl1.Size = new System.Drawing.Size(468, 183);
            this.arbitrageLogViewControl1.TabIndex = 0;
            // 
            // alarmNoticeViewControl1
            // 
            this.alarmNoticeViewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alarmNoticeViewControl1.Location = new System.Drawing.Point(0, 0);
            this.alarmNoticeViewControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.alarmNoticeViewControl1.Name = "alarmNoticeViewControl1";
            this.alarmNoticeViewControl1.Size = new System.Drawing.Size(468, 222);
            this.alarmNoticeViewControl1.TabIndex = 2;
            // 
            // arbitrageRunStateControl1
            // 
            this.arbitrageRunStateControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.arbitrageRunStateControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.arbitrageRunStateControl1.Location = new System.Drawing.Point(0, 0);
            this.arbitrageRunStateControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.arbitrageRunStateControl1.Name = "arbitrageRunStateControl1";
            this.arbitrageRunStateControl1.Size = new System.Drawing.Size(468, 27);
            this.arbitrageRunStateControl1.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Margin = new System.Windows.Forms.Padding(2);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1167, 2);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSystem,
            this.tsmiHistoryArbitrage,
            this.tsmView,
            this.tsmiOption,
            this.tsmiHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1167, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiSystem
            // 
            this.tsmiSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiExitSystem});
            this.tsmiSystem.Name = "tsmiSystem";
            this.tsmiSystem.Size = new System.Drawing.Size(43, 20);
            this.tsmiSystem.Text = "系统";
            // 
            // tsmiExitSystem
            // 
            this.tsmiExitSystem.Name = "tsmiExitSystem";
            this.tsmiExitSystem.Size = new System.Drawing.Size(98, 22);
            this.tsmiExitSystem.Text = "退出";
            // 
            // tsmiHistoryArbitrage
            // 
            this.tsmiHistoryArbitrage.Name = "tsmiHistoryArbitrage";
            this.tsmiHistoryArbitrage.Size = new System.Drawing.Size(79, 20);
            this.tsmiHistoryArbitrage.Text = "历史套利单";
            this.tsmiHistoryArbitrage.Click += new System.EventHandler(this.tsmiHistoryArbitrage_Click);
            // 
            // tsmView
            // 
            this.tsmView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmSimpleOrderPanel});
            this.tsmView.Name = "tsmView";
            this.tsmView.Size = new System.Drawing.Size(43, 20);
            this.tsmView.Text = "视图";
            // 
            // tsmSimpleOrderPanel
            // 
            this.tsmSimpleOrderPanel.Name = "tsmSimpleOrderPanel";
            this.tsmSimpleOrderPanel.Size = new System.Drawing.Size(110, 22);
            this.tsmSimpleOrderPanel.Text = "下单板";
            this.tsmSimpleOrderPanel.Click += new System.EventHandler(this.tsmSimpleOrderPanel_Click);
            // 
            // tsmiOption
            // 
            this.tsmiOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSystemConfig,
            this.套利合约配置ToolStripMenuItem,
            this.默认下单参数配置ToolStripMenuItem});
            this.tsmiOption.Name = "tsmiOption";
            this.tsmiOption.Size = new System.Drawing.Size(43, 20);
            this.tsmiOption.Text = "选项";
            // 
            // tsmiSystemConfig
            // 
            this.tsmiSystemConfig.Name = "tsmiSystemConfig";
            this.tsmiSystemConfig.Size = new System.Drawing.Size(170, 22);
            this.tsmiSystemConfig.Text = "系统配置";
            this.tsmiSystemConfig.Click += new System.EventHandler(this.tsmiSystemConfig_Click);
            // 
            // 套利合约配置ToolStripMenuItem
            // 
            this.套利合约配置ToolStripMenuItem.Name = "套利合约配置ToolStripMenuItem";
            this.套利合约配置ToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.套利合约配置ToolStripMenuItem.Text = "套利合约配置";
            this.套利合约配置ToolStripMenuItem.Click += new System.EventHandler(this.tsmiArbitrageInstrument_Click);
            // 
            // 默认下单参数配置ToolStripMenuItem
            // 
            this.默认下单参数配置ToolStripMenuItem.Name = "默认下单参数配置ToolStripMenuItem";
            this.默认下单参数配置ToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.默认下单参数配置ToolStripMenuItem.Text = "默认下单参数配置";
            this.默认下单参数配置ToolStripMenuItem.Click += new System.EventHandler(this.tsmiArbitrageOrderSettings_Click);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAboutMe});
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(43, 20);
            this.tsmiHelp.Text = "帮助";
            // 
            // tsmiAboutMe
            // 
            this.tsmiAboutMe.Name = "tsmiAboutMe";
            this.tsmiAboutMe.Size = new System.Drawing.Size(122, 22);
            this.tsmiAboutMe.Text = "关于我们";
            this.tsmiAboutMe.Click += new System.EventHandler(this.tsmiAboutMe_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 687);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "USe 期货套利";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelTop.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panelBottomLeft.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.panelStatus.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.panelMiddle.ResumeLayout(false);
            this.panelMiddleLeft.ResumeLayout(false);
            this.panelMiddleRight.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Panel panelBottom;
        private BottomStateControl bottomStateControl1;
        private ArbitrageOrderListControl arbitrageOrderListControl1;
        private System.Windows.Forms.Panel panelBottomLeft;
        private PositionListControl positionListControl1;
        private OrderBookListControl orderBookListControl1;
        private TradeBookListControl tradeBookListControl1;
        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.Panel panelMiddleRight;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitterMiddelRight;
        private System.Windows.Forms.Panel panelMiddleLeft;
        private System.Windows.Forms.Panel panel1;
        private ArbitrageLogViewControl arbitrageLogViewControl1;
        private ArbitrageRunStateControl arbitrageRunStateControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiSystem;
        private InvestorFundControl investorFundControl1;
        private System.Windows.Forms.ToolStripMenuItem tsmiHistoryArbitrage;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiExitSystem;
        private System.Windows.Forms.ToolStripMenuItem tsmiAboutMe;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private AlarmNoticeViewControl alarmNoticeViewControl1;
        private System.Windows.Forms.ToolStripMenuItem tsmiOption;
        private System.Windows.Forms.ToolStripMenuItem tsmiSystemConfig;
        private System.Windows.Forms.ToolStripMenuItem tsmView;
        private System.Windows.Forms.ToolStripMenuItem tsmSimpleOrderPanel;
        private System.Windows.Forms.ToolStripMenuItem 套利合约配置ToolStripMenuItem;
        private ArbitrageQuoteListControl arbitrageQuoteListControl1;
        private System.Windows.Forms.ToolStripMenuItem 默认下单参数配置ToolStripMenuItem;
    }
}

