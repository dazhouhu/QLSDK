using log4net;
using QLSDK.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSDK.WFSample
{
    public partial class LoginWindow : Form
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            #region Valid
            var server = txtServer.Text.Trim();
            if (string.IsNullOrEmpty(server))
            {
                txtServer.Focus();
                MessageBox.Show(this, "服务器地址必须", "消息通知", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var userName = txtUserName.Text.Trim();
            if (string.IsNullOrEmpty(userName))
            {
                txtUserName.Focus();
                MessageBox.Show(this, "用户名必须", "消息通知", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var pwd = txtPassword.Text.Trim();
            if (string.IsNullOrEmpty(pwd))
            {
                txtPassword.Focus();
                MessageBox.Show(this, "密码必须", "消息通知", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion
            try
            {
                var qlConfig = new Dictionary<PropertyKey, string>();
                //qlConfig.Add(PropertyKey.PLCM_MFW_KVLIST_KEY_DisplayName, "SDKSample测试用户");
                qlConfig.Add(PropertyKey.PLCM_MFW_KVLIST_KEY_LogLevel, "Debug");
                QLManager.GetInstance().Login(server, userName, pwd, qlConfig);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "消息通知", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
