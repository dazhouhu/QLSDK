using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;

namespace QLSDK.Core
{
    internal partial class ChannelView : UserControl
    {
        #region Fields
        private ILog log = LogManager.GetLogger("ChannelView");
        private QLChannel _channel;
        #endregion

        #region Constructors
        internal ChannelView(QLChannel channel)
        {
            _channel = channel;
            InitializeComponent();
            _channel.PropertyChanged += OnPropertyChangedEventHandler;
            this.Disposed += OnDisposed;

            //防止抖动
            if (channel.MediaType == MediaType.LOCALCONTENT)
            {
                this.DoubleBuffered = true;
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
                SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            }
        }
        #endregion

        #region Properties
        public bool IsShowBar
        {
            set
            {  /*
                lblName.Visible = !value;
                lblChannelName.Visible = value;
              
                btnAudio.Visible = value;
                btnVideo.Visible = value;
                PaintView();
              

                btnAudio.Visible = false;
                btnVideo.Visible = false;  */
            }
        }
        #endregion
        private void OnPropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ChannelName":
                    {
                        this.lblName.Text = _channel.ChannelName;
                        this.lblChannelName.Text = _channel.ChannelName;
                    }
                    break;
                case "IsVideo":
                    {
                        btnVideo.Image = _channel.IsVideo ? Properties.Resources.camera : Properties.Resources.camera_mute;

                        RenderVedio();
                    }
                    break;
                case "IsAudio":
                    {
                        btnAudio.Image = _channel.IsAudio ? Properties.Resources.speaker : Properties.Resources.speaker_mute;
                    }
                    break;
                case "IsActive":
                    {
                        if (_channel.IsActive)
                        {
                            this.BorderStyle = BorderStyle.Fixed3D;
                        }
                        else
                        {
                            this.BorderStyle = BorderStyle.None;
                        }
                    }
                    break;
                case "Size":
                    {
                        if (_channel.MediaType== MediaType.LOCALCONTENT)
                        {
                            if (null != _channel.LocalContentImage)
                            {
                                pnlVideo.BackgroundImageLayout = ImageLayout.Center;
                                pnlVideo.BackgroundImage = ImageHelper.ScaleImage((Bitmap)_channel.LocalContentImage,this.Size);
                            }
                        }
                        else { 
                            PaintView();
                        }
                    }break;
                case "LocalContentImage":
                    {
                        pnlVideo.BackgroundImageLayout = ImageLayout.Center;
                        pnlVideo.BackgroundImage = ImageHelper.ScaleImage((Bitmap)_channel.LocalContentImage, this.Size);
                    }
                    break;
            }
        }

        private void ChannelView_Load(object sender, EventArgs e)
        {
            this.lblName.Text = _channel.ChannelName;
            this.lblChannelName.Text = _channel.ChannelName;

            btnVideo.Image = _channel.IsVideo ? Properties.Resources.camera : Properties.Resources.camera_mute;
            btnAudio.Image = _channel.IsAudio ? Properties.Resources.speaker : Properties.Resources.speaker_mute;

            this.IsShowBar = !_channel.IsActive;
            this.SizeChanged += (obj, args) =>
            {
                if (_channel.MediaType == MediaType.LOCALCONTENT)
                {
                    if (null != _channel.LocalContentImage)
                    {
                        pnlVideo.BackgroundImageLayout = ImageLayout.Center;
                        pnlVideo.BackgroundImage = ImageHelper.ScaleImage((Bitmap)_channel.LocalContentImage, this.Size);
                    }
                }
            };
            RenderVedio();
        }
        private void OnDisposed(object sender, EventArgs e)
        {
            try
            {
                switch (_channel.MediaType)
                {
                    case MediaType.LOCAL:
                        {
                            var errno = PlcmProxy.DetachStreamWnd(MediaType.LOCAL, _channel.ChannelID, _channel.Call.CallHandle);
                            if (ErrorNumber.OK != errno)
                            {
                                log.Error("本地视频卸载失败");
                            }
                            PlcmProxy.StopCamera();
                        }
                        break;
                    case MediaType.REMOTE:
                        {
                            var errno = PlcmProxy.DetachStreamWnd(MediaType.REMOTE, _channel.ChannelID, _channel.Call.CallHandle);
                            if (ErrorNumber.OK != errno)
                            {
                                log.Error("远程视频卸载失败");
                            }
                        }
                        break;
                    case MediaType.CONTENT:
                        {
                            var errno = PlcmProxy.DetachStreamWnd(MediaType.CONTENT, _channel.ChannelID, _channel.Call.CallHandle);
                            if (ErrorNumber.OK != errno)
                            {
                                log.Error("共享视频卸载失败");
                            }
                        }
                        break;
                    case MediaType.LOCALCONTENT: {}break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void ChannelView_SizeChanged(object sender, EventArgs e)
        {
            PaintView();
        }

        private void PaintView()
        {
            try
            {
                if (null == _channel) return;

                var streamWidth = _channel.Size.Width;
                var streamHeight = _channel.Size.Height;
                int ratio_w = 0;
                int ratio_h = 0;
                if (streamWidth * 9 == streamHeight * 16)
                {
                    log.Info("resizeResolutionChange: 16:9");
                    ratio_w = 16;
                    ratio_h = 9;
                }
                else if (streamWidth * 3 == streamHeight * 4)
                {
                    log.Info("resizeResolutionChange: 4:3");
                    ratio_w = 4;
                    ratio_h = 3;
                }
                else
                {
                    ratio_w = streamWidth;
                    ratio_h = streamHeight;
                    log.Warn("resizeResolutionChange: not normal aspect ratio.");
                }
                var hostWidth = this.Width;
                var hostHeight = this.Height - (_channel.IsActive ? 0 : 40);
                var viewHeight = hostHeight;
                var viewWidth = hostHeight * ratio_w / ratio_h;

                if (viewWidth > hostWidth)
                {
                    viewWidth = hostWidth;
                    viewHeight = viewWidth * ratio_h / ratio_w;
                }
                this.pnlVideo.Width = (int)viewWidth;
                this.pnlVideo.Height = (int)viewHeight;
                var x = (hostWidth - pnlVideo.Width) / 2;
                var y = (hostHeight - pnlVideo.Height) / 2;
                this.pnlVideo.Left = x;
                this.pnlVideo.Top = y;
            }
            catch(Exception ex)
            {
                log.Error("PaintView 异常：ex=" + ex.Message);
            }
        }


        private void RenderVedio()
        {
            try
            {
                var hwnd = pnlVideo.Handle;
                if (_channel.IsVideo)
                {
                    switch (_channel.MediaType)
                    {
                        case MediaType.LOCAL:
                            {
                                var errno = PlcmProxy.AttachStreamWnd(MediaType.LOCAL, _channel.ChannelID,_channel.Call.CallHandle, hwnd,0,0, this.pnlVideo.Width, this.pnlVideo.Height - 40);
                                if (ErrorNumber.OK!=errno)
                                {
                                    log.Error("本地视频附加失败");
                                }
                                PlcmProxy.StartCamera();
                            }
                            break;
                        case MediaType.REMOTE:
                            {
                                var errno = PlcmProxy.AttachStreamWnd(MediaType.REMOTE, _channel.ChannelID, _channel.Call.CallHandle, hwnd, 0, 0, this.pnlVideo.Width, this.pnlVideo.Height - 40);
                                if (ErrorNumber.OK != errno)
                                {
                                    log.Error("远程视频附加失败");
                                }
                            }
                            break;
                        case MediaType.CONTENT:
                            {
                                var errno = PlcmProxy.AttachStreamWnd(MediaType.CONTENT, _channel.ChannelID, _channel.Call.CallHandle, hwnd, 0, 0, this.pnlVideo.Width, this.pnlVideo.Height - 40);
                                if (ErrorNumber.OK != errno)
                                {
                                    log.Error("共享视频附加失败");
                                }
                            }
                            break;
                        case MediaType.LOCALCONTENT: { } break;
                    }
                }
                else  //音频
                {
                    switch (_channel.MediaType)
                    {
                        case MediaType.LOCAL:
                            {
                                var errno = PlcmProxy.DetachStreamWnd(MediaType.LOCAL, _channel.ChannelID, _channel.Call.CallHandle);
                                if (ErrorNumber.OK != errno)
                                {
                                    log.Error("本地视频卸载失败");
                                }
                                PlcmProxy.StopCamera();
                            }
                            break;
                        case MediaType.REMOTE:
                            {
                                var errno = PlcmProxy.DetachStreamWnd(MediaType.REMOTE, _channel.ChannelID, _channel.Call.CallHandle);
                                if (ErrorNumber.OK != errno)
                                {
                                    log.Error("远程视频卸载失败");
                                }
                            }
                            break;
                        case MediaType.CONTENT:
                            {
                                var errno = PlcmProxy.DetachStreamWnd(MediaType.CONTENT, _channel.ChannelID, _channel.Call.CallHandle);
                                if (ErrorNumber.OK != errno)
                                {
                                    log.Error("共享视频卸载失败");
                                }
                            }
                            break;
                        case MediaType.LOCALCONTENT: { } break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
    }
}

