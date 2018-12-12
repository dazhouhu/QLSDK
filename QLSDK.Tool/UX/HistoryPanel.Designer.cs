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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.grdCalls = new System.Windows.Forms.DataGridView();
            this.CallName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CallType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CallMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConnectedTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CallState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Reason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAnswer = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btntHold = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnResume = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnHangup = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnAudioCall = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnVideoCall = new System.Windows.Forms.DataGridViewButtonColumn();
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
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdCalls.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.grdCalls.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdCalls.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CallName,
            this.CallType,
            this.CallMode,
            this.StartTime,
            this.ConnectedTime,
            this.CallState,
            this.Reason,
            this.btnAnswer,
            this.btntHold,
            this.btnResume,
            this.btnHangup,
            this.btnAudioCall,
            this.btnVideoCall});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdCalls.DefaultCellStyle = dataGridViewCellStyle8;
            this.grdCalls.GridColor = System.Drawing.SystemColors.ControlLight;
            this.grdCalls.Location = new System.Drawing.Point(0, 48);
            this.grdCalls.Margin = new System.Windows.Forms.Padding(0);
            this.grdCalls.Name = "grdCalls";
            this.grdCalls.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdCalls.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.grdCalls.RowHeadersVisible = false;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.White;
            this.grdCalls.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.grdCalls.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.grdCalls.RowTemplate.Height = 23;
            this.grdCalls.Size = new System.Drawing.Size(700, 452);
            this.grdCalls.TabIndex = 13;
            this.grdCalls.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdCalls_CellClick);
            this.grdCalls.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdCalls_CellFormatting);
            this.grdCalls.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grdCalls_CellPainting);
            // 
            // CallName
            // 
            this.CallName.DataPropertyName = "CallName";
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.CallName.DefaultCellStyle = dataGridViewCellStyle7;
            this.CallName.Frozen = true;
            this.CallName.HeaderText = "呼叫者";
            this.CallName.Name = "CallName";
            this.CallName.ReadOnly = true;
            this.CallName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CallName.Width = 120;
            // 
            // CallType
            // 
            this.CallType.DataPropertyName = "CallType";
            this.CallType.HeaderText = "呼叫类型";
            this.CallType.Name = "CallType";
            this.CallType.ReadOnly = true;
            this.CallType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CallType.Width = 80;
            // 
            // CallMode
            // 
            this.CallMode.DataPropertyName = "CallMode";
            this.CallMode.HeaderText = "呼叫模式";
            this.CallMode.Name = "CallMode";
            this.CallMode.ReadOnly = true;
            this.CallMode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CallMode.Width = 80;
            // 
            // StartTime
            // 
            this.StartTime.DataPropertyName = "StartTime";
            this.StartTime.HeaderText = "呼叫时间";
            this.StartTime.Name = "StartTime";
            this.StartTime.ReadOnly = true;
            this.StartTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.StartTime.Width = 80;
            // 
            // ConnectedTime
            // 
            this.ConnectedTime.DataPropertyName = "ConnectedTime";
            this.ConnectedTime.HeaderText = "通话时长";
            this.ConnectedTime.Name = "ConnectedTime";
            this.ConnectedTime.ReadOnly = true;
            this.ConnectedTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ConnectedTime.Width = 80;
            // 
            // CallState
            // 
            this.CallState.DataPropertyName = "CallState";
            this.CallState.HeaderText = "呼叫状态";
            this.CallState.Name = "CallState";
            this.CallState.ReadOnly = true;
            this.CallState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CallState.Width = 80;
            // 
            // Reason
            // 
            this.Reason.DataPropertyName = "Reason";
            this.Reason.HeaderText = "呼叫备注";
            this.Reason.Name = "Reason";
            this.Reason.ReadOnly = true;
            this.Reason.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btnAnswer
            // 
            this.btnAnswer.HeaderText = "接听";
            this.btnAnswer.Name = "btnAnswer";
            this.btnAnswer.ReadOnly = true;
            this.btnAnswer.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnAnswer.Text = "接听";
            this.btnAnswer.Width = 40;
            // 
            // btntHold
            // 
            this.btntHold.HeaderText = "保持";
            this.btntHold.Name = "btntHold";
            this.btntHold.ReadOnly = true;
            this.btntHold.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btntHold.Text = "保持";
            this.btntHold.Width = 40;
            // 
            // btnResume
            // 
            this.btnResume.HeaderText = "恢复";
            this.btnResume.Name = "btnResume";
            this.btnResume.ReadOnly = true;
            this.btnResume.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnResume.Text = "恢复";
            this.btnResume.Width = 40;
            // 
            // btnHangup
            // 
            this.btnHangup.HeaderText = "挂断";
            this.btnHangup.Name = "btnHangup";
            this.btnHangup.ReadOnly = true;
            this.btnHangup.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnHangup.Text = "挂断";
            this.btnHangup.Width = 40;
            // 
            // btnAudioCall
            // 
            this.btnAudioCall.HeaderText = "语音呼叫";
            this.btnAudioCall.Name = "btnAudioCall";
            this.btnAudioCall.ReadOnly = true;
            this.btnAudioCall.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnAudioCall.Text = "语音呼叫";
            this.btnAudioCall.Width = 80;
            // 
            // btnVideoCall
            // 
            this.btnVideoCall.HeaderText = "视频呼叫";
            this.btnVideoCall.Name = "btnVideoCall";
            this.btnVideoCall.ReadOnly = true;
            this.btnVideoCall.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnVideoCall.Text = "语音呼叫";
            this.btnVideoCall.Width = 80;
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
            this.Load += new System.EventHandler(this.HistoeryPanel_Load);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConnectedTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn CallState;
        private System.Windows.Forms.DataGridViewTextBoxColumn Reason;
        private System.Windows.Forms.DataGridViewButtonColumn btnAnswer;
        private System.Windows.Forms.DataGridViewButtonColumn btntHold;
        private System.Windows.Forms.DataGridViewButtonColumn btnResume;
        private System.Windows.Forms.DataGridViewButtonColumn btnHangup;
        private System.Windows.Forms.DataGridViewButtonColumn btnAudioCall;
        private System.Windows.Forms.DataGridViewButtonColumn btnVideoCall;
    }
}
