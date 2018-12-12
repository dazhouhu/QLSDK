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
using QLSDK.Tool.UX;

namespace QLSDK.Tool
{
    public partial class QLToolBar : UserControl
    {
        #region Fields
        private Control ownerContainer;
        private QLCall _currentCall;
        private QLManager qlManager = QLManager.GetInstance();
        private QLCallManager callManager = QLCallManager.GetInstance();
        private QLDeviceManager deviceManager = QLDeviceManager.GetInstance();
        #endregion

        #region Constructors
        public QLToolBar()
        {
            InitializeComponent();
        }
        #endregion

        #region AttachViewContainer
        public void AttachViewContainer(Control container)
        {
            if (null == container)
            {
                throw new Exception("父控件必须");
            }
            container.Controls.Add(this);
            ownerContainer = container;

            this.Dock = DockStyle.Bottom;
            //this.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            this.Width = ownerContainer.Width;
            ownerContainer.SizeChanged += (sender, args) =>
            {
                this.Width = ownerContainer.Width;
            };
            ownerContainer.Controls.Add(tbMicVolume);
            ownerContainer.Controls.Add(tbSpeakerVolume);
        }
        #endregion

        #region ToolBar
        private void QLToolBar_Load(object sender, EventArgs e)
        {
            deviceManager.PropertyChanged += OnDevicePropertyChangedHandle;
            callManager.CurrentCallChanged +=() =>
            {
                if (null != _currentCall)
                {
                    _currentCall.PropertyChanged -= OnCallPropertyChangedHandle;
                }
                _currentCall = callManager.CurrentCall;
                if (null != _currentCall)
                {
                    _currentCall.PropertyChanged += OnCallPropertyChangedHandle;
                }
                SetToolsStatus();
            };

            this.tbMicVolume.LostFocus += (obj, args) => { this.tbMicVolume.Hide(); };
            this.tbSpeakerVolume.LostFocus += (obj, args) => { this.tbSpeakerVolume.Hide(); };

            SetToolsStatus();
        }
        #region BtnStatus
        private void OnDevicePropertyChangedHandle(object sender,PropertyChangedEventArgs args)
        {
            switch(args.PropertyName)
            {
                case "CurrentAudioInputDevice": //音频输入设备
                    {
                        SetToolsStatus();
                    }
                    break;
                case "CurrentAudioOutputDevice": //音频输出设备
                    {
                        SetToolsStatus();
                    }
                    break;
                case "CurrentVideoInputDevice": //视频输入设备
                    {
                        SetToolsStatus();
                    }
                    break;
            }
        }
        private void OnCallPropertyChangedHandle(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case "CallState":
                case "IsAudioOnly":
                case "CallMode":
                case "IsContentSupported":
                    {
                        SetToolsStatus();
                    }
                    break;
            }
        }

        private void SetToolsStatus()
        {
            if (null == callManager.CurrentCall)
            { //当前呼叫为空时,按钮不可用
                btnSignal.Enabled = false;
                btnSignal.Image = Properties.Resources.signal0;

                btnMic.Enabled = false;
                btnMic.Image = Properties.Resources.mic_mute;

                btnSpeaker.Enabled = false;
                btnSpeaker.Image = Properties.Resources.speaker_mute;

                btnCamera.Enabled = false;
                this.MuteCamera = true;
                btnCamera.Image = Properties.Resources.camera_mute;

                btnShare.Enabled = false;
                btnShare.Image = Properties.Resources.share_mute;

                btnCall.Image = Properties.Resources.call24;
                btnCall.Text = "呼叫";

                menuItemDTMF.Enabled = false;
                menuItemFECC.Enabled = false;
                menuItemLayout.Enabled = false;
            }
            else
            {
                switch (callManager.CurrentCall.CallState)
                {
                    case CallState.SIP_UNKNOWN:
                    case CallState.SIP_OUTGOING_FAILURE:
                    case CallState.SIP_CALL_CLOSED:
                    case CallState.NULL_CALL:
                        { //非活动状态,不可用
                            btnSignal.Enabled = false;
                            btnSignal.Image = Properties.Resources.signal0;

                            btnMic.Enabled = false;
                            btnMic.Image = Properties.Resources.mic_mute;

                            btnSpeaker.Enabled = false;
                            btnSpeaker.Image = Properties.Resources.speaker_mute;

                            btnCamera.Enabled = false;
                            this.MuteCamera = true;
                            btnCamera.Image = Properties.Resources.camera_mute;

                            btnShare.Enabled = false;
                            btnShare.Image = Properties.Resources.share_mute;

                            btnCall.Image = Properties.Resources.call24;
                            btnCall.Text = "呼叫";

                            menuItemDTMF.Enabled = false;
                            menuItemFECC.Enabled = false;
                            menuItemLayout.Enabled = false;
                        }
                        break;
                    case CallState.SIP_INCOMING_INVITE:
                    case CallState.SIP_OUTGOING_TRYING:
                    case CallState.SIP_CALL_HOLD:
                    case CallState.SIP_CALL_HELD:
                    case CallState.SIP_CALL_DOUBLE_HOLD:
                    case CallState.SIP_OUTGOING_RINGING:
                    case CallState.SIP_INCOMING_CONNECTED:
                    case CallState.SIP_OUTGOING_CONNECTED:
                        { //活动呼叫
                            btnSignal.Enabled = true;
                            btnSignal.Image = Properties.Resources.signal6;

                            btnCall.Image = Properties.Resources.hangup24;
                            btnCall.Text = "挂断";

                            menuItemDTMF.Enabled = true;
                            menuItemFECC.Enabled = true;
                            menuItemLayout.Enabled = true;

                            if (null != deviceManager.CurrentAudioInputDevice)
                            {
                                this.btnMic.Enabled = true;
                                this.btnMic.Image = Properties.Resources.mic;
                                var volume = qlManager.GetMicVolume();
                                this.tbMicVolume.Value = volume;
                            }
                            else
                            {
                                this.btnMic.Enabled = false;
                                this.btnMic.Image = Properties.Resources.mic_mute;
                                this.tbMicVolume.Value = 0;
                            }
                            if (null != deviceManager.CurrentAudioOutputDevice)
                            {
                                this.btnSpeaker.Enabled = true;
                                this.btnSpeaker.Image = Properties.Resources.speaker;
                                var volume = qlManager.GetSpeakerVolume();
                                this.tbSpeakerVolume.Value = volume;
                            }
                            else
                            {
                                this.btnSpeaker.Enabled = false;
                                this.btnSpeaker.Image = Properties.Resources.speaker_mute;
                                this.tbSpeakerVolume.Value = 0;
                            }
                            if (null != deviceManager.CurrentVideoInputDevice)
                            {
                                switch (callManager.CurrentCall.CallMode)
                                {
                                    case CallMode.VIDEO:
                                        {
                                            this.btnCamera.Enabled = true;
                                            this.MuteCamera = false;
                                            this.btnCamera.Image = Properties.Resources.camera;
                                            if (callManager.CurrentCall.IsContentSupported)
                                            {
                                                this.btnShare.Enabled = true;
                                                this.btnShare.Image = Properties.Resources.share;
                                            }
                                            else
                                            {
                                                this.btnShare.Enabled = false;
                                                this.btnShare.Image = Properties.Resources.share_mute;
                                            }
                                        }
                                        break;
                                    case CallMode.AUDIO:
                                        {
                                            this.btnCamera.Enabled = false;
                                            this.MuteCamera = true;
                                            this.btnCamera.Image = Properties.Resources.camera_mute;
                                            this.btnShare.Enabled = false;
                                            this.btnShare.Image = Properties.Resources.share_mute;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                this.btnCamera.Enabled = false;
                                this.MuteCamera = true;
                                this.btnCamera.Image = Properties.Resources.camera_mute;
                                this.btnShare.Enabled = false;
                                this.btnShare.Image = Properties.Resources.share_mute;
                            }
                        }
                        break;
                }
            }
        }
        #endregion
        /// <summary>
        /// 查看信号信息
        /// </summary>
        private void btnSignal_Click(object sender, EventArgs e)
        {
            if (null == _currentCall)
                return;
            try
            {
                var signalWin = new SignalPanel();
                UXMessageMask.ShowForm(ownerContainer, signalWin);
                qlManager.GetMediaStatistics((mediaStatistics)=> {
                    signalWin.BindData(mediaStatistics);
                });
            }
            catch (Exception ex)
            {
                UXMessageMask.ShowMessage(ownerContainer, false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 麦克风
        /// </summary>
        private void btnMic_Click(object sender, EventArgs e)
        {
            tbMicVolume.Show();
            tbMicVolume.Focus();
            tbMicVolume.BringToFront();
            tbMicVolume.Left = btnMic.Left + 10;
            tbMicVolume.Top = this.Top - 118;
        }

        /// <summary>
        /// 扬声器
        /// </summary>
        private void btnSpeaker_Click(object sender, EventArgs e)
        {
            tbSpeakerVolume.Show();
            tbSpeakerVolume.Focus();
            tbSpeakerVolume.BringToFront();
            tbSpeakerVolume.Left = btnSpeaker.Left + 10;
            tbSpeakerVolume.Top = this.Top - 118;
        }
        private bool _muteCamera = true;
        public bool MuteCamera
        {
            get { return _muteCamera; }
            set
            {
                if (_muteCamera != value)
                {
                    _muteCamera = value;
                    btnCamera.Image = _muteCamera ? Properties.Resources.camera_mute : Properties.Resources.camera;
                }
            }
        }

        /// <summary>
        /// 摄像头
        /// </summary>
        private void btnCamera_Click(object sender, EventArgs e)
        {
            if (true == MuteCamera)
            {
                try
                {
                    qlManager.StartCamera();
                    MuteCamera = false;
                }
                catch (Exception ex)
                {
                    UXMessageMask.ShowMessage(ownerContainer, false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    qlManager.StopCamera();
                    MuteCamera = true;
                }
                catch (Exception ex)
                {
                    UXMessageMask.ShowMessage(ownerContainer, false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 内容共享
        /// </summary>
        private void btnShare_Click(object sender, EventArgs e)
        {
            if (null == _currentCall)
                return;
            var contentSelectWin = new ContentSelectPanel()
            {
                OKAction = (type, format, monitor, app) => {
                    try
                    {
                        switch (type)
                        {
                            case "Monitor":
                                {
                                    qlManager.StartSendContent(qlManager.GetCurrentCall(), monitor, app);
                                }
                                break;
                            case "BFCP":
                                {
                                    var width = Screen.PrimaryScreen.Bounds.Width;
                                    var height = Screen.PrimaryScreen.Bounds.Height;
                                    //qlManager.SetContentBuffer(format, width, height);
                                    //qlManager.StartBFCPContent();
                                }
                                break;
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "消息框", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                },
                OnCancel = () => { }
            };
            qlManager.GetApps((apps) => {
                contentSelectWin.BindAppData(apps);
            });
            UXMessageMask.ShowForm(ownerContainer, contentSelectWin);
        }

        /// <summary>
        /// 联系人
        /// </summary>
        private void btnAttender_Click(object sender, EventArgs e)
        {
            try
            {
                var pnl = new HistoryPanel();
                UXMessageMask.ShowForm(ownerContainer, pnl);
                callManager.GetHistoryCalls((calls) => {
                    pnl.BindData(calls);
                });
            }
            catch (Exception ex)
            {
                UXMessageMask.ShowMessage(ownerContainer, false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 呼叫/挂断
        /// </summary>
        private void btnCall_Click(object sender, EventArgs e)
        {
            if(btnCall.Text=="呼叫")
            {
                var callWin = new CallPanel()
                {                    
                    OKAction = () => {},
                    OnCancel = () => { }
                };
                UXMessageMask.ShowForm(ownerContainer, callWin);
            }
            else{
                Action hangupAction = () =>
                {
                    qlManager.EndCall();
                };
                UXMessageMask.ShowMessage(ownerContainer,false, "确认要挂断当前通话吗？", MessageBoxButtonsType.OKCancel, MessageBoxIcon.Question
                                            , hangupAction);
            }
        }

        /// <summary>
        /// 更多
        /// </summary>
        private void btnMore_Click(object sender, EventArgs e)
        {
            moreMenu.Show(btnMore, new Point(0, 0), ToolStripDropDownDirection.AboveRight);
        }
        #endregion

        #region Menu
        private void menuItemDTMF_Click(object sender, EventArgs e)
        {
            var win = new DTMFPanel()
            {
                OnCancel = () => { }
            };
            UXMessageMask.ShowForm(ownerContainer, win);
        }

        private void menuItemFECC_Click(object sender, EventArgs e)
        {
            var win = new FECCPanel()
            {
                OnCancel = () => { }
            };
            UXMessageMask.ShowForm(ownerContainer, win);
        }

        private void menuItemDeviceManager_Click(object sender, EventArgs e)
        {
            var deviceWin = new DeviceManagerPanel()
            {
                OKAction = () => {

                },
                OnCancel = () => { }
            };
            UXMessageMask.ShowForm(ownerContainer, deviceWin);
        }

        private void menuItemP_Click(object sender, EventArgs e)
        {
            qlManager.SetLayout(LayoutType.Presentation);
        }

        private void menuItemVAS_Click(object sender, EventArgs e)
        {
            qlManager.SetLayout(LayoutType.VAS);
        }

        private void menuItemCP_Click(object sender, EventArgs e)
        {
            qlManager.SetLayout(LayoutType.ContinuousPresence);
        }

        private void menuItemSingle_Click(object sender, EventArgs e)
        {
            qlManager.SetLayout(LayoutType.Single);
        }
        #endregion

        private void tbMicVolume_ValueChanged(object sender, EventArgs e)
        {
            if (null == _currentCall)
                return;
            var volume = tbMicVolume.Value;

            try
            {
                qlManager.SetMicVolume(volume);
                if (0 == volume)
                {
                    qlManager.MuteMic(true);
                    this.btnMic.Image = Properties.Resources.mic_mute;
                }
                else
                {
                    qlManager.MuteMic(false);
                    this.btnMic.Image = Properties.Resources.mic;
                }

            }
            catch (Exception ex)
            {
                Action okAction = () =>
                {
                    volume = qlManager.GetMicVolume();
                    this.tbMicVolume.Value = volume;
                };
                UXMessageMask.ShowMessage(ownerContainer, false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error
                                            , okAction);
            }
        }

        private void tbSpeakerVolume_ValueChanged(object sender, EventArgs e)
        {
            var volume = tbSpeakerVolume.Value;
            try
            {
                qlManager.SetSpeakerVolume(volume);
                if (0 == volume)
                {
                    qlManager.MuteSpeaker(true);
                    this.btnSpeaker.Image = Properties.Resources.speaker_mute;

                }
                else
                {
                    qlManager.MuteSpeaker(false);
                    this.btnSpeaker.Image = Properties.Resources.speaker;
                }
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(this, ex.Message, "消息框", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    volume = qlManager.GetSpeakerVolume();
                    this.tbSpeakerVolume.Value = volume;
                }
            }
        }

    }
}
