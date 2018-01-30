namespace MarketDataStore
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblMarketDataFileStorage_ErrorStoreCount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblMarketDataFileStorage_StoreCount = new System.Windows.Forms.Label();
            this.lblMarketDataFileStorage_UnStoreCount = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gridKLineStore = new System.Windows.Forms.DataGridView();
            this.Column4 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblProcessor_ProcessCount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblReceive_LatestMarketDataTime = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblMonitorExchange = new System.Windows.Forms.Label();
            this.lblReceive_ReceiveCount = new System.Windows.Forms.Label();
            this.lblReceive_InstrumentCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.errorNoticeViewControl1 = new MarketDataStore.ErrorNoticeViewControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridKLineStore)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(471, 503);
            this.panel1.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnStart);
            this.panel3.Controls.Add(this.btnStop);
            this.panel3.Controls.Add(this.btnTest);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 453);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(471, 50);
            this.panel3.TabIndex = 65;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(93, 8);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 32);
            this.btnStart.TabIndex = 61;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(173, 8);
            this.btnStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 32);
            this.btnStop.TabIndex = 62;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(12, 8);
            this.btnTest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 32);
            this.btnTest.TabIndex = 63;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.lblMarketDataFileStorage_ErrorStoreCount);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.pictureBox1);
            this.groupBox5.Controls.Add(this.lblMarketDataFileStorage_StoreCount);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.lblMarketDataFileStorage_UnStoreCount);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.ForeColor = System.Drawing.Color.Blue;
            this.groupBox5.Location = new System.Drawing.Point(9, 109);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox5.Size = new System.Drawing.Size(456, 74);
            this.groupBox5.TabIndex = 64;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "行情文件存储";
            // 
            // lblMarketDataFileStorage_ErrorStoreCount
            // 
            this.lblMarketDataFileStorage_ErrorStoreCount.BackColor = System.Drawing.Color.White;
            this.lblMarketDataFileStorage_ErrorStoreCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMarketDataFileStorage_ErrorStoreCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMarketDataFileStorage_ErrorStoreCount.Location = new System.Drawing.Point(92, 46);
            this.lblMarketDataFileStorage_ErrorStoreCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMarketDataFileStorage_ErrorStoreCount.Name = "lblMarketDataFileStorage_ErrorStoreCount";
            this.lblMarketDataFileStorage_ErrorStoreCount.Size = new System.Drawing.Size(80, 22);
            this.lblMarketDataFileStorage_ErrorStoreCount.TabIndex = 67;
            this.lblMarketDataFileStorage_ErrorStoreCount.Text = "cu1701";
            this.lblMarketDataFileStorage_ErrorStoreCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(27, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 22);
            this.label4.TabIndex = 66;
            this.label4.Text = "错误数";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MarketDataStore.Properties.Resources.green;
            this.pictureBox1.Location = new System.Drawing.Point(7, 20);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(25, 23);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 65;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // lblMarketDataFileStorage_StoreCount
            // 
            this.lblMarketDataFileStorage_StoreCount.BackColor = System.Drawing.Color.White;
            this.lblMarketDataFileStorage_StoreCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMarketDataFileStorage_StoreCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMarketDataFileStorage_StoreCount.Location = new System.Drawing.Point(252, 20);
            this.lblMarketDataFileStorage_StoreCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMarketDataFileStorage_StoreCount.Name = "lblMarketDataFileStorage_StoreCount";
            this.lblMarketDataFileStorage_StoreCount.Size = new System.Drawing.Size(80, 22);
            this.lblMarketDataFileStorage_StoreCount.TabIndex = 64;
            this.lblMarketDataFileStorage_StoreCount.Text = "cu1701";
            this.lblMarketDataFileStorage_StoreCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMarketDataFileStorage_UnStoreCount
            // 
            this.lblMarketDataFileStorage_UnStoreCount.BackColor = System.Drawing.Color.White;
            this.lblMarketDataFileStorage_UnStoreCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMarketDataFileStorage_UnStoreCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMarketDataFileStorage_UnStoreCount.Location = new System.Drawing.Point(92, 20);
            this.lblMarketDataFileStorage_UnStoreCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMarketDataFileStorage_UnStoreCount.Name = "lblMarketDataFileStorage_UnStoreCount";
            this.lblMarketDataFileStorage_UnStoreCount.Size = new System.Drawing.Size(80, 22);
            this.lblMarketDataFileStorage_UnStoreCount.TabIndex = 56;
            this.lblMarketDataFileStorage_UnStoreCount.Text = "cu1701";
            this.lblMarketDataFileStorage_UnStoreCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Location = new System.Drawing.Point(27, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 22);
            this.label10.TabIndex = 2;
            this.label10.Text = "未存储";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.gridKLineStore);
            this.groupBox3.ForeColor = System.Drawing.Color.Blue;
            this.groupBox3.Location = new System.Drawing.Point(7, 239);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(458, 199);
            this.groupBox3.TabIndex = 59;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "K线存储";
            // 
            // gridKLineStore
            // 
            this.gridKLineStore.AllowUserToAddRows = false;
            this.gridKLineStore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridKLineStore.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridKLineStore.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridKLineStore.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridKLineStore.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.Column3,
            this.Column5,
            this.Column1,
            this.Column2});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridKLineStore.DefaultCellStyle = dataGridViewCellStyle5;
            this.gridKLineStore.Location = new System.Drawing.Point(9, 19);
            this.gridKLineStore.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridKLineStore.Name = "gridKLineStore";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridKLineStore.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.gridKLineStore.RowHeadersWidth = 5;
            this.gridKLineStore.RowTemplate.Height = 24;
            this.gridKLineStore.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridKLineStore.Size = new System.Drawing.Size(430, 175);
            this.gridKLineStore.TabIndex = 67;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Visible = false;
            this.Column4.Width = 30;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "Name";
            this.Column3.HeaderText = "存储器";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 80;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "ErrorStoreCount";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column5.HeaderText = "错误";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 60;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "UnstoreCount";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column1.HeaderText = "未存储";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 65;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "StoreCount";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column2.HeaderText = "已存储";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 65;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lblProcessor_ProcessCount);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.ForeColor = System.Drawing.Color.Blue;
            this.groupBox2.Location = new System.Drawing.Point(8, 184);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(457, 51);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "K线处理";
            // 
            // lblProcessor_ProcessCount
            // 
            this.lblProcessor_ProcessCount.BackColor = System.Drawing.Color.White;
            this.lblProcessor_ProcessCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProcessor_ProcessCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblProcessor_ProcessCount.Location = new System.Drawing.Point(92, 21);
            this.lblProcessor_ProcessCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProcessor_ProcessCount.Name = "lblProcessor_ProcessCount";
            this.lblProcessor_ProcessCount.Size = new System.Drawing.Size(80, 22);
            this.lblProcessor_ProcessCount.TabIndex = 56;
            this.lblProcessor_ProcessCount.Text = "cu1701";
            this.lblProcessor_ProcessCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(35, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 22);
            this.label6.TabIndex = 2;
            this.label6.Text = "处理数";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.Controls.Add(this.lblReceive_LatestMarketDataTime);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.lblMonitorExchange);
            this.groupBox1.Controls.Add(this.lblReceive_ReceiveCount);
            this.groupBox1.Controls.Add(this.lblReceive_InstrumentCount);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Location = new System.Drawing.Point(9, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(456, 103);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CTP行情接收";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::MarketDataStore.Properties.Resources.green;
            this.pictureBox2.Location = new System.Drawing.Point(5, 73);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(25, 23);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 66;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Visible = false;
            // 
            // lblReceive_LatestMarketDataTime
            // 
            this.lblReceive_LatestMarketDataTime.BackColor = System.Drawing.Color.White;
            this.lblReceive_LatestMarketDataTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblReceive_LatestMarketDataTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblReceive_LatestMarketDataTime.Location = new System.Drawing.Point(252, 74);
            this.lblReceive_LatestMarketDataTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReceive_LatestMarketDataTime.Name = "lblReceive_LatestMarketDataTime";
            this.lblReceive_LatestMarketDataTime.Size = new System.Drawing.Size(80, 22);
            this.lblReceive_LatestMarketDataTime.TabIndex = 60;
            this.lblReceive_LatestMarketDataTime.Text = "HH:mm:ss";
            this.lblReceive_LatestMarketDataTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(176, 74);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(73, 22);
            this.label11.TabIndex = 59;
            this.label11.Text = "最新行情";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMonitorExchange
            // 
            this.lblMonitorExchange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMonitorExchange.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lblMonitorExchange.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMonitorExchange.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMonitorExchange.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblMonitorExchange.Location = new System.Drawing.Point(7, 20);
            this.lblMonitorExchange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMonitorExchange.Name = "lblMonitorExchange";
            this.lblMonitorExchange.Size = new System.Drawing.Size(437, 22);
            this.lblMonitorExchange.TabIndex = 58;
            this.lblMonitorExchange.Text = "SFE";
            this.lblMonitorExchange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReceive_ReceiveCount
            // 
            this.lblReceive_ReceiveCount.BackColor = System.Drawing.Color.White;
            this.lblReceive_ReceiveCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblReceive_ReceiveCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblReceive_ReceiveCount.Location = new System.Drawing.Point(92, 75);
            this.lblReceive_ReceiveCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReceive_ReceiveCount.Name = "lblReceive_ReceiveCount";
            this.lblReceive_ReceiveCount.Size = new System.Drawing.Size(80, 22);
            this.lblReceive_ReceiveCount.TabIndex = 57;
            this.lblReceive_ReceiveCount.Text = "12345678";
            this.lblReceive_ReceiveCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblReceive_InstrumentCount
            // 
            this.lblReceive_InstrumentCount.BackColor = System.Drawing.Color.White;
            this.lblReceive_InstrumentCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblReceive_InstrumentCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblReceive_InstrumentCount.Location = new System.Drawing.Point(92, 49);
            this.lblReceive_InstrumentCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReceive_InstrumentCount.Name = "lblReceive_InstrumentCount";
            this.lblReceive_InstrumentCount.Size = new System.Drawing.Size(80, 22);
            this.lblReceive_InstrumentCount.TabIndex = 56;
            this.lblReceive_InstrumentCount.Text = "496";
            this.lblReceive_InstrumentCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(35, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 22);
            this.label2.TabIndex = 3;
            this.label2.Text = "接收数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(33, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "合约数";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(471, 0);
            this.splitter1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 503);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // errorNoticeViewControl1
            // 
            this.errorNoticeViewControl1.Location = new System.Drawing.Point(477, 0);
            this.errorNoticeViewControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.errorNoticeViewControl1.Name = "errorNoticeViewControl1";
            this.errorNoticeViewControl1.Size = new System.Drawing.Size(470, 503);
            this.errorNoticeViewControl1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(181, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 22);
            this.label7.TabIndex = 63;
            this.label7.Text = "已存储";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 503);
            this.Controls.Add(this.errorNoticeViewControl1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(893, 422);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KLine数据存储";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridKLineStore)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMonitorExchange;
        private System.Windows.Forms.Label lblProcessor_ProcessCount;
        private System.Windows.Forms.Label lblReceive_LatestMarketDataTime;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblReceive_ReceiveCount;
        private System.Windows.Forms.Label lblReceive_InstrumentCount;
        private System.Windows.Forms.GroupBox groupBox3;
        private ErrorNoticeViewControl errorNoticeViewControl1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblMarketDataFileStorage_StoreCount;
        private System.Windows.Forms.Label lblMarketDataFileStorage_UnStoreCount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.DataGridView gridKLineStore;
        private System.Windows.Forms.Label lblMarketDataFileStorage_ErrorStoreCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewImageColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Label label7;
    }
}