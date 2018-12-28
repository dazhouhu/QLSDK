using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSDK.Core
{
    internal partial class ContentRegion : Form
    {
        #region Fields
        private ILog log = LogUtil.GetLogger("QLSDK:ContentRegion");

        private QLCall call = null;
        private QLChannel channel = null;
        private Timer shareTimer = null;
        #endregion

        #region Constructors
        public ContentRegion(QLCall call,QLChannel channel)
        {
            InitializeComponent();

            if (null == call || !call.IsActive())
            {
                throw new Exception("呼叫不存在或处理非活动状态");
            }
            this.call = call;
            this.channel = channel;
            this.call.PropertyChanged += OnCallPropertyChangedHandle;
            
            shareTimer = new Timer() { Interval = 1000 };
            shareTimer.Tick += onShareTimerTickHandle;
            shareTimer.Start();
        }
        private void onShareTimerTickHandle(object sender, EventArgs e)
        {
            try
            {
                log.Info(string.Format("SetContentBuffer({0},{1},{2}) ", call.ContentImageFormat, call.ContentType, call.ContentHandle));
                #region Image
                Bitmap image = null;
                var rect = new Rectangle();
                if (null == call.ContentHandle) return;
                if (call.ContentType == ContentType.Monitor)
                {
                    image = ImageHelper.CaptureScreen(call.ContentHandle, ref rect);
                }
                else
                {
                    image = ImageHelper.CaptureApplication(call.ContentHandle, ref rect);
                }
                #endregion
                if (null != image)
                {

                    log.Info(string.Format("SetContentBuffer({0},{1},{2})", call.ContentImageFormat, image.Width, image.Height));
                    this.Bounds = rect;
                    
                    #region Resolution
                    Size size = new Size();
                    if(image.Width<=320 && image.Height<=240)
                    {
                        size = new Size(320, 240);
                    }
                    else if (image.Width <= 432 && image.Height <= 240)
                    {
                        size = new Size(432, 240);
                    }
                    else if (image.Width <= 640 && image.Height <= 480)
                    {
                        size = new Size(640, 480);
                    }
                    else if (image.Width <= 704 && image.Height <= 576)
                    {
                        size = new Size(704, 576);
                    }
                    else if(image.Width <= 800 && image.Height <= 600)
                    {
                        size = new Size(800, 600);
                    }
                    else if(image.Width<=1024 && image.Height<=576)
                    {
                        size = new Size(1024, 576);
                    }
                    else if (image.Width <= 1024 && image.Height <= 768)
                    {
                        size = new Size(1024, 768);
                    }
                    else 
                    {
                        size = new Size(1280, 720);
                    }
                    #endregion
                    var scaledImage = ImageHelper.ScaleImage(image, size);

                    #region Bytes
                    byte[] buffer = null;
                    if (call.ContentImageFormat == ImageFormat.YV12)
                    {
                        buffer = ImageHelper.GetYUV(scaledImage);
                    }
                    else
                    {
                        buffer = ImageHelper.GetRGBA(scaledImage);
                    }
                    #endregion
                    var errno = PlcmProxy.SetContentBuffer(call.ContentImageFormat, buffer, scaledImage.Width, scaledImage.Height);
                    if (ErrorNumber.OK != errno)
                    {
                        log.Error("SetContentBuffer 失败, ex:" + errno);
                        
                        this.Hide();
                    }
                    else
                    {
                        channel.LocalContentImage = scaledImage;
                        this.Show();
                    }
                }
                else
                {
                    throw new Exception("捕捉图像失败");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        #endregion

        #region Paint
        private void ContentRegion_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(this.BackColor);
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            ControlPaint.DrawBorder(e.Graphics, rect,
                Color.Yellow, 4, ButtonBorderStyle.Solid,
                Color.Yellow, 4, ButtonBorderStyle.Solid,
                Color.Yellow, 4, ButtonBorderStyle.Solid,
                Color.Yellow, 4, ButtonBorderStyle.Solid
            );
        }
        private void ContentRegion_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void ContentRegion_LocationChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        #endregion

        private void ContentRegion_Load(object sender, EventArgs e)
        {
        }
        #region OnCallPropertyChangedHandle
        private void OnCallPropertyChangedHandle(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case "CallState":
                case "IsAudioOnly":
                case "MuteVideo":
                case "MuteMic":
                case "CallMode":
                case "IsContentSupported":
                case "IsContentIdle":
                    {
                        SetToolsStatus();
                    }
                    break;
            }
        }

        private void SetToolsStatus()
        {
            if (null == call)
            { //当前呼叫为空时,按钮不可用

                btnMic.Enabled = false;
                btnMic.Image = Properties.Resources.mic_mute;
                
                btnVideo.Enabled = false;
                btnVideo.Image = Properties.Resources.camera_mute;

                this.Close();
            }
            else
            {
                switch (call.CallState)
                {
                    case CallState.SIP_UNKNOWN:
                    case CallState.SIP_OUTGOING_FAILURE:
                    case CallState.SIP_CALL_CLOSED:
                    case CallState.SIP_INCOMING_INVITE:
                    case CallState.SIP_OUTGOING_TRYING:
                    case CallState.SIP_OUTGOING_RINGING:
                        { //非活动状态,不可用
                            btnMic.Enabled = false;
                            btnMic.Image = Properties.Resources.mic_mute;
                            this.btnMic.Tag = "麦克风不可用";

                            btnVideo.Enabled = false;
                            btnVideo.Image = Properties.Resources.camera_mute;
                            this.btnVideo.Tag = "摄像头不可用";
                        }
                        break;
                    case CallState.SIP_CALL_HOLD:
                    case CallState.SIP_CALL_HELD:
                    case CallState.SIP_CALL_DOUBLE_HOLD:
                    case CallState.SIP_INCOMING_CONNECTED:
                    case CallState.SIP_OUTGOING_CONNECTED:
                        { //活动呼叫
                            var deviceManager = QLDeviceManager.GetInstance();
                            #region AudioInputDevice
                            if (null != deviceManager.CurrentAudioInputDevice)
                            {
                                this.btnMic.Enabled = true;
                                this.btnMic.Image = Properties.Resources.mic;
                                this.btnMic.Image = call.MuteMic ? Properties.Resources.mic_mute : Properties.Resources.mic;
                                this.btnMic.Tag = call.MuteMic ? "去打开麦克风" : "去关闭麦克风";
                            }
                            else
                            {
                                this.btnMic.Enabled = false;
                                this.btnMic.Image = Properties.Resources.mic_mute;
                                this.btnMic.Tag = "麦克风不可用";
                            }
                            #endregion
                            
                            #region VideoInputDevice
                            if (null != deviceManager.CurrentVideoInputDevice)
                            {
                                switch (call.CallMode)
                                {
                                    case CallMode.VIDEO:
                                        {
                                            this.btnVideo.Enabled = true;
                                            this.btnVideo.Image = call.MuteVideo ? Properties.Resources.camera_mute : Properties.Resources.camera;
                                            this.btnMic.Tag = call.MuteMic ? "去打开摄像头" : "去关闭摄像头";
                                        }
                                        break;
                                    case CallMode.AUDIO:
                                        {
                                            this.btnVideo.Enabled = false;
                                            this.btnVideo.Image = Properties.Resources.camera_mute;
                                            this.btnVideo.Tag = "摄像头不可用";
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                this.btnVideo.Enabled = false;
                                this.btnVideo.Image = Properties.Resources.camera_mute;
                                this.btnVideo.Tag = "摄像头不可用";
                            }
                            #endregion
                        }
                        break;
                }
            }
        }
        #endregion
        private void btnVideo_Click(object sender, EventArgs e)
        {
            if (null != call)
            {
                call.MuteLocalVideo(!call.MuteVideo);
            }
            else
            {
                this.Close();
            }
        }

        private void btnMic_Click(object sender, EventArgs e)
        {
            if (null != call)
            {
                call.MuteLocalMic(!call.MuteMic);
            }
            else
            {
                this.Close();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (null != call)
            {
                call.StopSendContent();
            }
            else
            {
                this.Close();
            }
        }

        private void ContentRegion_FormClosing(object sender, FormClosingEventArgs e)
        {
            shareTimer?.Stop();
            shareTimer?.Dispose();
            call.PropertyChanged -= OnCallPropertyChangedHandle;
        }
    }
}
