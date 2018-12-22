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
        private bool isRegiested = false;
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
            this.Visible = false;
            var loginWin = new LoginWindow();
            if (loginWin.ShowDialog() == DialogResult.Cancel)
            {
                if (!this.isRegiested)
                {
                    Application.Exit();
                }
            }

            qlManager.QLEvent += (evt) =>
            {
                switch (evt.EventType)
                {          
                    #region Register
                    case EventType.UNKNOWN: break;
                    case EventType.SIP_REGISTER_SUCCESS:
                        {
                            isRegiested = true;
                            this.Visible = true;
                        }
                        break;
                    case EventType.SIP_REGISTER_FAILURE:
                        {
                            this.Visible = false;
                            isRegiested = false;
                            var loginWindow = new LoginWindow();
                            if (loginWindow.ShowDialog() == DialogResult.Cancel)
                            {
                                if (!this.isRegiested)
                                {
                                    Application.Exit();
                                }
                            }
                        }
                        break;
                    case EventType.SIP_REGISTER_UNREGISTERED:
                        {
                            this.Visible = false;
                            isRegiested = false;
                            var loginWindow = new LoginWindow();
                            if (loginWindow.ShowDialog() == DialogResult.Cancel)
                            {
                                if (!this.isRegiested)
                                {
                                    Application.Exit();
                                }
                            }
                        }
                        break;
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
            if(!isRegiested)
            {
                e.Cancel = false;
                return;
            }

            var currentCall = callManager.CurrentCall;
            if (null != currentCall && currentCall.IsActive())
            {
                var stateText= currentCall.CallStateText();
                stateText += '\n' + "确定要挂断当前通话。";
                if (MessageBox.Show(this, stateText, "确认信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    currentCall.HangUpCall();
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = false;
            }
        }
    }
}

