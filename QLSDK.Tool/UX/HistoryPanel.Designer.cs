namespace QLSDK.Tool.UX
{
    partial class HistoryPanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.grdCalls = new System.Windows.Forms.DataGridView();
            this.CallName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CallType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CallMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CallStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CallEndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CallRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Reason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Opt = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdCalls)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.Location = new System.Drawing.Point(13, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(85, 19);
            this.lblTitle.TabIndex = 12;
            this.lblTitle.Text = "呼叫历史";
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = global::QLSDK.Tool.Properties.Resources.close;
            this.btnClose.Location = new System.Drawing.Point(661, 13);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(25, 23);
            this.btnClose.TabIndex = 14;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grdCalls
            // 
            this.grdCalls.AllowUserToAddRows = false;
            this.grdCalls.AllowUserToDeleteRows = false;
            this.grdCalls.BackgroundColor = System.Drawing.Color.Azure;
            this.grdCalls.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdCalls.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdCalls.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdCalls.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CallName,
            this.CallType,
            this.CallMode,
            this.CallStartTime,
            this.CallEndTime,
            this.CallRate,
            this.Reason,
            this.Opt});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdCalls.DefaultCellStyle = dataGridViewCellStyle3;
            this.grdCalls.GridColor = System.Drawing.SystemColors.ControlLight;
            this.grdCalls.Location = new System.Drawing.Point(0, 48);
            this.grdCalls.Margin = new System.Windows.Forms.Padding(0);
            this.grdCalls.Name = "grdCalls";
            this.grdCalls.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdCalls.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.grdCalls.RowHeadersVisible = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            this.grdCalls.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.grdCalls.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.grdCalls.RowTemplate.Height = 23;
            this.grdCalls.Size = new System.Drawing.Size(700, 452);
            this.grdCalls.TabIndex = 13;
            // 
            // CallName
            // 
            this.CallName.DataPropertyName = "CallName";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.CallName.DefaultCellStyle = dataGridViewCellStyle2;
            this.CallName.HeaderText = "呼叫者";
            this.CallName.Name = "CallName";
            this.CallName.ReadOnly = true;
            // 
            // CallType
            // 
            this.CallType.DataPropertyName = "CallType";
            this.CallType.HeaderText = "呼叫类型";
            this.CallType.Name = "CallType";
            this.CallType.ReadOnly = true;
            // 
            // CallMode
            // 
            this.CallMode.DataPropertyName = "CallMode";
            this.CallMode.HeaderText = "呼叫模式";
            this.CallMode.Name = "CallMode";
            this.CallMode.ReadOnly = true;
            // 
            // CallStartTime
            // 
            this.CallStartTime.DataPropertyName = "CallStartTime";
            this.CallStartTime.HeaderText = "呼叫时间";
            this.CallStartTime.Name = "CallStartTime";
            this.CallStartTime.ReadOnly = true;
            // 
            // CallEndTime
            // 
            this.CallEndTime.DataPropertyName = "CallEndTime";
            this.CallEndTime.HeaderText = "通话时长";
            this.CallEndTime.Name = "CallEndTime";
            this.CallEndTime.ReadOnly = true;
            // 
            // CallRate
            // 
            this.CallRate.DataPropertyName = "CallRate";
            this.CallRate.HeaderText = "呼叫状态";
            this.CallRate.Name = "CallRate";
            this.CallRate.ReadOnly = true;
            // 
            // Reason
            // 
            this.Reason.DataPropertyName = "Reason";
            this.Reason.HeaderText = "呼叫备注";
            this.Reason.Name = "Reason";
            this.Reason.ReadOnly = true;
            // 
            // Opt
            // 
            this.Opt.HeaderText = "操作";
            this.Opt.Name = "Opt";
            this.Opt.ReadOnly = true;
            this.Opt.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Opt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // HistoryPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grdCalls);
            this.Controls.Add(this.lblTitle);
            this.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Name = "HistoryPanel";
            this.Size = new System.Drawing.Size(700, 500);
            this.Load += new System.EventHandler(this.SignalPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdCalls)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView grdCalls;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn CallName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CallType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CallMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn CallStartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn CallEndTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn CallRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Reason;
        private System.Windows.Forms.DataGridViewButtonColumn Opt;
    }
}
