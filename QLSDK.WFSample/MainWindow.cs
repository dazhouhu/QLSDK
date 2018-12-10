using QLSDK.Core;
using QLSDK.Tool;
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
    public partial class MainWindow : Form
    {
        #region Fields
        private static QLManager qlManager = QLManager.GetInstance();
        #endregion
        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            //设置背景
            this.BackColor = Color.FromArgb(255, 45, 58, 66);
            //显示QL组件
            qlManager.AttachViewContainer(pnlContainer);
            var qlToolBar = new QLToolBar();
            qlToolBar.AttachViewContainer(pnlContainer);
        }
        #endregion

        private void MainWindow_Load(object sender, EventArgs e)
        {
        }


        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            var msg = string.Empty;
            var currentCall = qlManager.GetCurrentCall();
            if (null != currentCall)
            {
                switch (currentCall.CallState)
                {
                    case CallState.SIP_UNKNOWN:
                    case CallState.NULL_CALL:
                        break;
                    case CallState.SIP_OUTGOING_FAILURE:
                        break;
                    case CallState.SIP_CALL_CLOSED:
                        break;
                    case CallState.SIP_INCOMING_INVITE:
                        msg = string.Format("【{0}】正在呼入响铃中", currentCall.CallName);
                        break;
                    case CallState.SIP_INCOMING_CONNECTED:
                        msg = string.Format("【{0}】正在呼入通话中", currentCall.CallName);
                        break;
                    case CallState.SIP_CALL_HOLD:
                        msg = string.Format("【{0}】正在主动保持连接中", currentCall.CallName);
                        break;
                    case CallState.SIP_CALL_HELD:
                        msg = string.Format("【{0}】正在被动保持连接中", currentCall.CallName);
                        break;
                    case CallState.SIP_CALL_DOUBLE_HOLD:
                        msg = string.Format("【{0}】正在双方保持连接中", currentCall.CallName);
                        break;
                    case CallState.SIP_OUTGOING_TRYING:
                        msg = string.Format("【{0}】正在尝试呼出中", currentCall.CallName);
                        break;
                    case CallState.SIP_OUTGOING_RINGING:
                        msg = string.Format("【{0}】正在呼出响铃中", currentCall.CallName);
                        break;
                    case CallState.SIP_OUTGOING_CONNECTED:
                        msg = string.Format("【{0}】正在呼出通话中", currentCall.CallName);
                        break;
                }
            }
            if (!string.IsNullOrEmpty(msg))
            {
                msg += '\n' + "确定要挂断当前通话。";
                var okAction = new Action(() => {
                    qlManager.EndCall();
                    this.Hide();
                    e.Cancel = true;
                });
               // callView.ShowMessage(true, msg, MessageBoxButtonsType.OKCancel, MessageBoxIcon.Question, okAction);
            }
            else
            {
                this.Hide();
                e.Cancel = true;
            }

        }
    }
}

