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
        private static QLCallManager callManager = QLCallManager.GetInstance();
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
            qlManager.QLEvent += (evt) =>
            {
                switch (evt.EventType)
                {          
                    #region Register
                    case EventType.UNKNOWN: break;
                    case EventType.SIP_REGISTER_SUCCESS:
                        {
                        }
                        break;
                    case EventType.SIP_REGISTER_FAILURE:
                        {
                            var loginWin = new LoginWindow();
                            loginWin.ShowDialog(this);
                        }
                        break;
                    case EventType.SIP_REGISTER_UNREGISTERED: break;
                    #endregion
                    #region 不处理
                        /*
                        #region QLDevice
                        case EventType.DEVICE_VIDEOINPUTCHANGED:
                        case EventType.DEVICE_AUDIOINPUTCHANGED:
                        case EventType.DEVICE_AUDIOOUTPUTCHANGED:
                        case EventType.DEVICE_VOLUMECHANGED: 
                        case EventType.DEVICE_MONITORINPUTSCHANGED:
                        #endregion
                        #region Call
                        case EventType.SIP_CALL_INCOMING: break;
                        case EventType.SIP_CALL_TRYING:break;
                        case EventType.SIP_CALL_RINGING:break;
                        case EventType.SIP_CALL_FAILURE:break;
                        case EventType.SIP_CALL_CLOSED:break;
                        case EventType.SIP_CALL_HOLD:break;
                        case EventType.SIP_CALL_HELD:break;
                        case EventType.SIP_CALL_DOUBLE_HOLD:break;
                        case EventType.SIP_CALL_UAS_CONNECTED:break;
                        case EventType.SIP_CALL_UAC_CONNECTED:break;
                        #endregion
                        #region Content
                        case EventType.SIP_CONTENT_INCOMING:break;
                        case EventType.SIP_CONTENT_CLOSED:break;
                        case EventType.SIP_CONTENT_SENDING:break;
                        case EventType.SIP_CONTENT_IDLE:break;
                        case EventType.SIP_CONTENT_UNSUPPORTED:break;
                        #endregion



                        #region Stream
                        case EventType.STREAM_VIDEO_LOCAL_RESOLUTIONCHANGED:break;
                        case EventType.STREAM_VIDEO_REMOTE_RESOLUTIONCHANGED:break;
                        #endregion

                        case EventType.NETWORK_CHANGED: break;
                        case EventType.MFW_INTERNAL_TIME_OUT: break;

                        case EventType.REFRESH_ACTIVE_SPEAKER:break;
                        case EventType.REMOTE_VIDEO_REFRESH:break;
                        case EventType.REMOTE_VIDEO_CHANNELSTATUS_CHANGED:break;
                        case EventType.REMOTE_VIDEO_DISPLAYNAME_UPDATE:break;
                        case EventType.SIP_CALL_MODE_CHANGED:break;

                        case EventType.SIP_CALL_MODE_UPGRADE_REQ:break;
                        case EventType.IS_TALKING_STATUS_CHANGED:break;
                        case EventType.CERTIFICATE_VERIFY:break;
                        case EventType.TRANSCODER_FINISH:break;
                        case EventType.ICE_STATUS_CHANGED:break;
                        case EventType.SUTLITE_INCOMING_CALL:break;
                        case EventType.SUTLITE_TERMINATE_CALL:break;
                        case EventType.NOT_SUPPORT_VIDEOCODEC:break;

                        case EventType.BANDWIDTH_LIMITATION:break;
                        case EventType.MEDIA_ADDRESS_UPDATED:break;
                        case EventType.AUTODISCOVERY_STATUS_CHANGED:break;
                        */
                        #endregion
                }
            };
            callManager.CurrentCallChanged += () =>
            {
                if(null != callManager.CurrentCall)
                {
                    this.Text = callManager.CurrentCall.CallName;
                }
            };
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

