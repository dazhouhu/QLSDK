namespace QLSDK.Tool.UX
{
    partial class ContentSelectPanel
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
            this.cbxFormat = new System.Windows.Forms.ComboBox();
            this.cbxMonitor = new System.Windows.Forms.ComboBox();
            this.cbxApp = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.rdoMonitor = new System.Windows.Forms.RadioButton();
            this.rdoApp = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // cbxFormat
            // 
            this.cbxFormat.FormattingEnabled = true;
            this.cbxFormat.Items.AddRange(new object[] {
            "RGBA",
            "YUV"});
            this.cbxFormat.Location = new System.Drawing.Point(138, 25);
            this.cbxFormat.Name = "cbxFormat";
            this.cbxFormat.Size = new System.Drawing.Size(163, 20);
            this.cbxFormat.TabIndex = 9;
            // 
            // cbxMonitor
            // 
            this.cbxMonitor.DisplayMember = "DeviceName";
            this.cbxMonitor.FormattingEnabled = true;
            this.cbxMonitor.Location = new System.Drawing.Point(138, 78);
            this.cbxMonitor.Name = "cbxMonitor";
            this.cbxMonitor.Size = new System.Drawing.Size(163, 20);
            this.cbxMonitor.TabIndex = 9;
            this.cbxMonitor.ValueMember = "DeviceHandle";
            // 
            // cbxApp
            // 
            this.cbxApp.DisplayMember = "AppName";
            this.cbxApp.Enabled = false;
            this.cbxApp.FormattingEnabled = true;
            this.cbxApp.Location = new System.Drawing.Point(138, 104);
            this.cbxApp.Name = "cbxApp";
            this.cbxApp.Size = new System.Drawing.Size(163, 20);
            this.cbxApp.TabIndex = 9;
            this.cbxApp.ValueMember = "AppHandle";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Image = global::QLSDK.Tool.Properties.Resources.cancel24;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(186, 146);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 40);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消  ";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOK.Image = global::QLSDK.Tool.Properties.Resources.ok24;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(56, 146);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 40);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "确定  ";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "共享图像格式:";
            // 
            // rdoMonitor
            // 
            this.rdoMonitor.AutoSize = true;
            this.rdoMonitor.Checked = true;
            this.rdoMonitor.Location = new System.Drawing.Point(56, 79);
            this.rdoMonitor.Name = "rdoMonitor";
            this.rdoMonitor.Size = new System.Drawing.Size(71, 16);
            this.rdoMonitor.TabIndex = 12;
            this.rdoMonitor.TabStop = true;
            this.rdoMonitor.Text = "显示器：";
            this.rdoMonitor.UseVisualStyleBackColor = true;
            this.rdoMonitor.CheckedChanged += new System.EventHandler(this.rdoMonitor_CheckedChanged);
            // 
            // rdoApp
            // 
            this.rdoApp.AutoSize = true;
            this.rdoApp.Location = new System.Drawing.Point(56, 105);
            this.rdoApp.Name = "rdoApp";
            this.rdoApp.Size = new System.Drawing.Size(83, 16);
            this.rdoApp.TabIndex = 13;
            this.rdoApp.Text = "应用程序：";
            this.rdoApp.UseVisualStyleBackColor = true;
            this.rdoApp.CheckedChanged += new System.EventHandler(this.rdoApp_CheckedChanged);
            // 
            // ContentSelectPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Controls.Add(this.rdoApp);
            this.Controls.Add(this.rdoMonitor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbxApp);
            this.Controls.Add(this.cbxMonitor);
            this.Controls.Add(this.cbxFormat);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "ContentSelectPanel";
            this.Size = new System.Drawing.Size(320, 204);
            this.Load += new System.EventHandler(this.ContentSelectPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cbxFormat;
        private System.Windows.Forms.ComboBox cbxMonitor;
        private System.Windows.Forms.ComboBox cbxApp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdoMonitor;
        private System.Windows.Forms.RadioButton rdoApp;
    }
}