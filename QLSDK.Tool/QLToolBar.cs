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
        private QLCallManager callManager = QLCallManager.GetInstance();
        private QLDeviceManager deviceManager = QLDeviceManager.GetInstance();
        #endregion

        #region Constructors
        public QLToolBar()
        {
            InitializeComponent();
        }
        #endregion

        #region BindPanel
        public void BindPanel(Control container)
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
        private void MFWToolBar_Load(object sender, EventArgs e)
        {
            bindDeviceStatus();
            deviceManager.DevicesChanged += (obj, args) => {
                bindDeviceStatus();
            };
            callManager.PropertyChanged += (obj, args) =>
            {
                switch (args.PropertyName)
                {
                    case "CurrentCall":
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
                            bindDeviceStatus();
                        }
                        break;
                }
            };
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
                        //bindDeviceStatus();
                    }
                    break;
            }
        }

        private void bindDeviceStatus()
        {
            #region Device Init
            if (null != deviceManager.CurrentAudioInputDevice)
            {
                this.btnMic.Enabled = true;
                this.btnMic.Image = Properties.Resources.mic;
                var volume = QLManager.GetInstance().GetMicVolume();
                this.tbMicVolume.Value = volume;
                this.tbMicVolume.LostFocus += (obj, args) => { this.tbMicVolume.Hide(); };
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
                var volume = QLManager.GetInstance().GetSpeakerVolume();
                this.tbSpeakerVolume.Value = volume;
                this.tbSpeakerVolume.LostFocus += (obj, args) => { this.tbSpeakerVolume.Hide(); };
            }
            else
            {
                this.btnSpeaker.Enabled = false;
                this.btnSpeaker.Image = Properties.Resources.speaker_mute;
                this.tbSpeakerVolume.Value = 0;
            }
            if (null != deviceManager.CurrentVideoInputDevice && null != callManager.CurrentCall)
            {
                this.btnCamera.Enabled = true;
                switch (callManager.CurrentCall.CallMode)
                {
                    case CallMode.VIDEO:
                        {
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
                this.btnCamera.Image = Properties.Resources.camera_mute;
                this.btnShare.Enabled = false;
                this.btnShare.Image = Properties.Resources.share_mute;
            }
            #endregion
        }
        private void btnSignal_Click(object sender, EventArgs e)
        {
            if (null == _currentCall)
                return;
            try
            {
                var signalWin = new SignalPanel();
                UXMessageMask.ShowForm(ownerContainer, signalWin);
            }
            catch (Exception ex)
            {
                UXMessageMask.ShowMessage(ownerContainer, false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMic_Click(object sender, EventArgs e)
        {
            tbMicVolume.Show();
            tbMicVolume.Focus();
            tbMicVolume.BringToFront();
            tbMicVolume.Left = btnMic.Left + 10;
            tbMicVolume.Top = this.Top - 118;
        }

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
        private void btnCamera_Click(object sender, EventArgs e)
        {
            if (true == MuteCamera)
            {
                try
                {
                    QLManager.GetInstance().StartCamera();
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
                    QLManager.GetInstance().StopCamera();
                    MuteCamera = true;
                }
                catch (Exception ex)
                {
                    UXMessageMask.ShowMessage(ownerContainer, false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnShare_Click(object sender, EventArgs e)
        {
            if (null == _currentCall)
                return;
            var contentSelectWin = new ContentSelectPanel()
            {
                Monitors = deviceManager.GetDevicesByType(DeviceType.MONITOR),
                Apps = deviceManager.GetDevicesByType(DeviceType.APPLICATIONS),
                OKAction = (type, format, monitor, app) => {
                    try
                    {
                        switch (type)
                        {
                            case "Monitor":
                                {
                                    QLManager.GetInstance().StartShareContent(monitor, app);
                                }
                                break;
                            case "BFCP":
                                {
                                    var width = Screen.PrimaryScreen.Bounds.Width;
                                    var height = Screen.PrimaryScreen.Bounds.Height;
                                    QLManager.GetInstance().SetContentBuffer(format, width, height);
                                    QLManager.GetInstance().StartBFCPContent();
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
            UXMessageMask.ShowForm(ownerContainer, contentSelectWin);
        }

        private void btnAttender_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "实现中", "消息框", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnMore_Click(object sender, EventArgs e)
        {
            moreMenu.Show(btnMore, new Point(0, 0), ToolStripDropDownDirection.AboveRight);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }
        #endregion
        #region Menu
        private void menuItemDTMF_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "暂时不实现", "消息框", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void menuItemFECC_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "暂时不实现", "消息框", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void menuItemP_Click(object sender, EventArgs e)
        {
            QLManager.GetInstance().SetLayout(LayoutType.Presentation);
        }

        private void menuItemVAS_Click(object sender, EventArgs e)
        {
            QLManager.GetInstance().SetLayout(LayoutType.VAS);
        }

        private void menuItemCP_Click(object sender, EventArgs e)
        {
            QLManager.GetInstance().SetLayout(LayoutType.ContinuousPresence);
        }
        #endregion

        private void tbMicVolume_ValueChanged(object sender, EventArgs e)
        {
            if (null == _currentCall)
                return;
            var volume = tbMicVolume.Value;

            try
            {
                QLManager.GetInstance().SetMicVolume(volume);
                if (0 == volume)
                {
                    QLManager.GetInstance().MuteMic(true);
                    this.btnMic.Image = Properties.Resources.mic_mute;
                }
                else
                {
                    QLManager.GetInstance().MuteMic(false);
                    this.btnMic.Image = Properties.Resources.mic;
                }

            }
            catch (Exception ex)
            {
                Action okAction = () =>
                {
                    volume = QLManager.GetInstance().GetMicVolume();
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
                QLManager.GetInstance().SetSpeakerVolume(volume);
                if (0 == volume)
                {
                    QLManager.GetInstance().MuteSpeaker(true);
                    this.btnSpeaker.Image = Properties.Resources.speaker_mute;

                }
                else
                {
                    QLManager.GetInstance().MuteSpeaker(false);
                    this.btnSpeaker.Image = Properties.Resources.speaker;
                }
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(this, ex.Message, "消息框", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    volume = QLManager.GetInstance().GetSpeakerVolume();
                    this.tbSpeakerVolume.Value = volume;
                }
            }
        }
    }
}
