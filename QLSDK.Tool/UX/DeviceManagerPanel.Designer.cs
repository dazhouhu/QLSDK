namespace QLSDK.Tool.UX
{
    partial class DeviceManagerPanel
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
            this.cbxVideoInput = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxAudioOutput = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxAudioInput = new System.Windows.Forms.ComboBox();
            this.lblAudioInput = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbxVideoInput
            // 
            this.cbxVideoInput.DisplayMember = "DeviceName";
            this.cbxVideoInput.FormattingEnabled = true;
            this.cbxVideoInput.Location = new System.Drawing.Point(75, 90);
            this.cbxVideoInput.Name = "cbxVideoInput";
            this.cbxVideoInput.Size = new System.Drawing.Size(209, 20);
            this.cbxVideoInput.TabIndex = 5;
            this.cbxVideoInput.ValueMember = "DeviceHandle";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "视频输入:";
            // 
            // cbxAudioOutput
            // 
            this.cbxAudioOutput.DisplayMember = "DeviceName";
            this.cbxAudioOutput.FormattingEnabled = true;
            this.cbxAudioOutput.Location = new System.Drawing.Point(75, 64);
            this.cbxAudioOutput.Name = "cbxAudioOutput";
            this.cbxAudioOutput.Size = new System.Drawing.Size(209, 20);
            this.cbxAudioOutput.TabIndex = 6;
            this.cbxAudioOutput.ValueMember = "DeviceHandle";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "音频输出:";
            // 
            // cbxAudioInput
            // 
            this.cbxAudioInput.DisplayMember = "DeviceName";
            this.cbxAudioInput.FormattingEnabled = true;
            this.cbxAudioInput.Location = new System.Drawing.Point(75, 38);
            this.cbxAudioInput.Name = "cbxAudioInput";
            this.cbxAudioInput.Size = new System.Drawing.Size(209, 20);
            this.cbxAudioInput.TabIndex = 7;
            this.cbxAudioInput.ValueMember = "DeviceHandle";
            // 
            // lblAudioInput
            // 
            this.lblAudioInput.AutoSize = true;
            this.lblAudioInput.Location = new System.Drawing.Point(17, 41);
            this.lblAudioInput.Name = "lblAudioInput";
            this.lblAudioInput.Size = new System.Drawing.Size(59, 12);
            this.lblAudioInput.TabIndex = 4;
            this.lblAudioInput.Text = "音频输入:";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Image = global::QLSDK.Tool.Properties.Resources.cancel24;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(179, 124);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 40);
            this.btnCancel.TabIndex = 8;
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
            this.btnOK.Location = new System.Drawing.Point(37, 124);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 40);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "确定  ";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(13, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 14);
            this.label3.TabIndex = 10;
            this.label3.Text = "设备管理：";
            // 
            // DeviceManagerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbxVideoInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbxAudioOutput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbxAudioInput);
            this.Controls.Add(this.lblAudioInput);
            this.Name = "DeviceManagerPanel";
            this.Size = new System.Drawing.Size(300, 180);
            this.Load += new System.EventHandler(this.DeviceManagerPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbxVideoInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxAudioOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxAudioInput;
        private System.Windows.Forms.Label lblAudioInput;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label3;
    }
}
