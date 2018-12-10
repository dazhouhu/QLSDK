using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSDK.Tool.UX
{
    public partial class DTMFPanel : UserControl
    {
        public DTMFPanel()
        {
            InitializeComponent();
        }
        #region Call
        private void btnC_Click(object sender, EventArgs e)
        {
            txtCallee.Text = txtCallee.Text + "*";
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            txtCallee.Text = txtCallee.Text + "#";
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

        #endregion
        public Action OnCancel { get; set; }
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnCancel?.Invoke();
            this.Dispose();
        }
    }
}
