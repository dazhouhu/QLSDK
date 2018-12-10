using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLSDK.Core;

namespace QLSDK.Tool.UX
{
    public partial class CallPanel : UserControl
    {
        public CallPanel()
        {
            InitializeComponent();
        }

        #region Call
        private void btnC_Click(object sender, EventArgs e)
        {
            txtCallee.Text = string.Empty;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            var txt = txtCallee.Text;
            if (txt.Length > 0)
            {
                txtCallee.Text = txt.Substring(0, txt.Length - 1);
            }
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtCallee.Text = txtCallee.Text + "0";
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtCallee.Text = txtCallee.Text + "1";
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtCallee.Text = txtCallee.Text + "2";
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtCallee.Text = txtCallee.Text + "3";
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtCallee.Text = txtCallee.Text + "4";
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtCallee.Text = txtCallee.Text + "5";
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtCallee.Text = txtCallee.Text + "6";
        }

        private void btm7_Click(object sender, EventArgs e)
        {
            txtCallee.Text = txtCallee.Text + "7";
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtCallee.Text = txtCallee.Text + "8";
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtCallee.Text = txtCallee.Text + "9";
        }
        public Action OKAction { get; set; }
        private void btnAudioCall_Click(object sender, EventArgs e)
        {
            var callAddr = txtCallee.Text.Trim();
            if (string.IsNullOrEmpty(callAddr))
            {
                MessageBox.Show("被呼叫方必须");
                return;
            }
            try
            {
                QLManager.GetInstance().DialCall(callAddr, CallMode.AUDIO);
                OKAction?.Invoke();
                this.Dispose();
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "消息通知", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVideoCall_Click(object sender, EventArgs e)
        {
            var callAddr = txtCallee.Text.Trim();
            if (string.IsNullOrEmpty(callAddr))
            {
                MessageBox.Show("被呼叫方必须");
                return;
            }
            try
            {
                QLManager.GetInstance().DialCall(callAddr, CallMode.VIDEO);
                OKAction?.Invoke();
                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "消息通知", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        public Action OnCancel { get; set; }
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnCancel?.Invoke();
            this.Dispose();
        }
    }
}
