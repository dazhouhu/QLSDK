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
    public partial class DTMFPanel : UserControl
    {
        #region Fields
        private static QLManager qlManager = QLManager.GetInstance();
        #endregion
        public DTMFPanel()
        {
            InitializeComponent();
        }
        #region Call
        private void btnC_Click(object sender, EventArgs e)
        {
            try
            {
                qlManager.SendDTMFKey(null, DTMFKey.STAR);
                txtCallee.Text = txtCallee.Text + "*";
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                qlManager.SendDTMFKey(null, DTMFKey.STAR);
                txtCallee.Text = txtCallee.Text + "#";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            try
            {
                qlManager.SendDTMFKey(null, DTMFKey.STAR);
                txtCallee.Text = txtCallee.Text + "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            try
            {
                qlManager.SendDTMFKey(null, DTMFKey.STAR);
                txtCallee.Text = txtCallee.Text + "1";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            try
            {
                qlManager.SendDTMFKey(null, DTMFKey.STAR);
                txtCallee.Text = txtCallee.Text + "2";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            try
            {
                qlManager.SendDTMFKey(null, DTMFKey.STAR);
                txtCallee.Text = txtCallee.Text + "3";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            try
            {
                qlManager.SendDTMFKey(null, DTMFKey.STAR);
                txtCallee.Text = txtCallee.Text + "4";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            try
            {
                qlManager.SendDTMFKey(null, DTMFKey.STAR);
                txtCallee.Text = txtCallee.Text + "5";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            try
            {
                qlManager.SendDTMFKey(null, DTMFKey.STAR);
                txtCallee.Text = txtCallee.Text + "6";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btm7_Click(object sender, EventArgs e)
        {
            try
            {
                qlManager.SendDTMFKey(null, DTMFKey.STAR);
                txtCallee.Text = txtCallee.Text + "7";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            try
            {
                qlManager.SendDTMFKey(null, DTMFKey.STAR);
                txtCallee.Text = txtCallee.Text + "8";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            try
            {
                qlManager.SendDTMFKey(null, DTMFKey.STAR);
                txtCallee.Text = txtCallee.Text + "9";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
