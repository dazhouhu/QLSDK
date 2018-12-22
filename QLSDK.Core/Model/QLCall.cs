using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;

namespace QLSDK.Core
{
    public class QLCall : BaseModel
    {
        private ILog log = LogUtil.GetLogger("QLSDK.QLCall");

        #region Constructors
        public QLCall(int callHandle)
        {
            this.CallHandle = callHandle;
        }
        ~QLCall()
        {
            shareTimer?.Dispose();
        }
        #endregion

        #region 网络地址
        private string _networkIP;
        /// <summary>
        /// 网络地址
        /// </summary>
        public string NetworkIP
        {
            get { return _networkIP; }
            set
            {
                _networkIP = value;
                NotifyPropertyChanged("NetworkIP");
            }
        }
        #endregion
        #region 是否只能音频
        private bool _isAudioOnly = false;

        /// <summary>
        /// 是否只能音频
        /// </summary>
        [JsonIgnore]
        public bool IsAudioOnly
        {
            get { return _isAudioOnly; }
            set
            {
                if (_isAudioOnly != value)
                {
                    _isAudioOnly = value;
                    NotifyPropertyChanged("IsAudioOnly");
                }
            }
        }
        #endregion
        #region 是否静音
        private bool _isMute;

        /// <summary>
        /// 是否静音
        /// </summary>
        [JsonIgnore]
        public bool IsMute
        {
            get { return _isMute; }
            set
            {
                if (_isMute != value)
                {
                    _isMute = value;
                    NotifyPropertyChanged("IsMute");
                }
            }
        }
        #endregion
        #region 打开/关闭 本地视频
        private bool _muteVideo;
        /// <summary>
        /// 打开/关闭 本地视频
        /// </summary>
        public bool MuteVideo
        {
            get { return _muteVideo; }
            set
            {
                if (_muteVideo != value)
                {
                    _muteVideo = value;
                    NotifyPropertyChanged("MuteVideo");
                }
            }
        }
        #endregion

        #region 当前活动通道
        private QLChannel _currentChannel;
        /// <summary>
        /// 当前活动通道
        /// </summary>
        [JsonIgnore]
        internal QLChannel CurrentChannel
        {
            get { return _currentChannel; }
            set
            {

                if (_currentChannel != value)
                {
                    if (null != _currentChannel)
                    {
                        _currentChannel.IsActive = false;
                    }
                    _currentChannel = value;
                    NotifyPropertyChanged("CurrentChannel");
                }
            }
        }
        #endregion
        #region Channels
        private ObservableCollection<QLChannel> _channels = new ObservableCollection<QLChannel>();
        /// <summary>
        /// 通道集合
        /// </summary>
        [JsonIgnore]
        internal ObservableCollection<QLChannel> Channels { get { return _channels; } }

        internal QLChannel GetChannel(int channelID)
        {
            return _channels.FirstOrDefault(c => c.ChannelID == channelID);
        }
        internal void AddChannel(int channelID, MediaType mediaType)
        {
            if (null == GetChannel(channelID))
            {
                var isActive = channelID == ActiveSpeakerId;
                var channel = new QLChannel(this, channelID, mediaType, isActive);
                _channels.Add(channel);
                if (isActive)
                {
                    CurrentChannel = channel;
                }
            }
        }
        internal void AddChannel(QLChannel channel)
        {
            if (null == GetChannel(channel.ChannelID))
            {
                var isActive = channel.IsActive;
                _channels.Add(channel);
                if (isActive)
                {
                    CurrentChannel = channel;
                }
            }
        }
        internal void RemoveChannel(int channelID)
        {
            var channel = GetChannel(channelID);
            if (null != channel)
            {
                _channels.Remove(channel);
                if (CurrentChannel == channel)
                {
                    CurrentChannel = _channels.LastOrDefault();
                }
            }
        }
        internal void ClearRemoteChannels()
        {
            var channels = _channels.Where(c => c.MediaType == MediaType.REMOTE).ToList();
            foreach (var c in channels)
            {
                _channels.Remove(c);
            }
            CurrentChannel = channels.FirstOrDefault();
        }
        internal void SetChannelName(int channelID, string channelName)
        {
            var channel = GetChannel(channelID);
            if (null != channel)
            {
                channel.ChannelName = channelName;
            }
        }

        internal QLChannel GetLocalChannel()
        {
            return _channels.FirstOrDefault(c => c.MediaType == MediaType.LOCAL);
        }
        internal QLChannel GetContentChannel()
        {
            return _channels.FirstOrDefault(c => c.MediaType == MediaType.CONTENT);
        }
        #endregion
        #region ChannelNumber
        private int _channelNumber;

        [JsonIgnore]
        internal int ChannelNumber
        {
            get { return _channelNumber; }
            set
            {
                _channelNumber = value;
                NotifyPropertyChanged("ChannelNumber");
            }
        }
        #endregion
        #region ActiveSpeakerId
        private int _activeSpeakerId = 0;

        [JsonIgnore]
        internal int ActiveSpeakerId
        {
            get { return _activeSpeakerId; }
            set
            {
                _activeSpeakerId = value;
                var channel = GetChannel(_activeSpeakerId);
                if (null != channel)
                {
                    channel.IsActive = true;
                    CurrentChannel = channel;
                    NotifyPropertyChanged("ActiveSpeakerId");
                }
            }
        }
        #endregion
               

        #region 呼叫ID
        private int _callHandle = -1;
        /// <summary>
        /// 呼叫ID
        /// </summary>
        public int CallHandle
        {
            get { return _callHandle; }
            set
            {
                _callHandle = value;
                NotifyPropertyChanged("CallHandle");
            }
        }
        #endregion
        #region 呼叫名
        private string _callName;
        /// <summary>
        /// 呼叫名
        /// </summary>
        public string CallName
        {
            get { return _callName; }
            set
            {
                _callName = value;
                NotifyPropertyChanged("CallName");
            }
        }
        #endregion
        #region 呼叫模式
        private CallMode _callMode;
        /// <summary>
        /// 呼叫模式
        /// </summary>
        public CallMode CallMode
        {
            get { return _callMode; }
            set
            {
                _callMode = value;
                NotifyPropertyChanged("CallMode");
            }
        }
        #endregion
        #region 呼叫状态
        private CallState _callState;
        /// <summary>
        /// 呼叫状态
        /// </summary>
        public CallState CallState
        {
            get { return _callState; }
            set
            {
                _callState = value;
                NotifyPropertyChanged("CallState");
            }
        }
        #endregion
        #region 是否共享内容支持
        private bool _isContentSupported = false;
        /// <summary>
        /// 是否共享内容支持
        /// </summary>
        [JsonIgnore]
        public bool IsContentSupported
        {
            get { return _isContentSupported; }
            set
            {
                if (_isContentSupported != value)
                {
                    _isContentSupported = value;
                    NotifyPropertyChanged("IsContentSupported");
                }
            }
        }
        #endregion
        #region 是否共享内容空闲
        private bool _isContentIdle = false;
        /// <summary>
        /// 是否共享内容空闲
        /// </summary>
        [JsonIgnore]
        public bool IsContentIdle
        {
            get { return _isContentIdle; }
            set
            {
                if (_isContentIdle != value)
                {
                    _isContentIdle = value;
                    base.NotifyPropertyChanged("IsContentIdle");
                }
            }
        }
        #endregion
        #region 呼叫结束原因
        private string _reason;
        /// <summary>
        /// 呼叫结束原因
        /// </summary>
        public string Reason
        {
            get { return _reason; }
            set
            {
                _reason = value;
                NotifyPropertyChanged("Reason");
            }
        }
        #endregion
        
        #region 开始时间
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        #endregion
        #region 终止时间
        /// <summary>
        /// 终止时间
        /// </summary>
        public DateTime? StopTime { get; set; }
        #endregion
        #region 通话开始时间 
        /// <summary>
        /// 通话开始时间
        /// </summary>
        public DateTime? ConnectedTime { get; set; }
        #endregion
        #region 通话结束时间
        /// <summary>
        /// 通话结束时间
        /// </summary>
        public DateTime? UnconnectedTime { get; set; }
        #endregion
        #region 呼叫类型
        /// <summary>
        /// 呼叫类型
        /// </summary>
        public CallType CallType { get; set; }
        #endregion


        #region Methods        

        /// <summary>
        /// 是否活动呼叫
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsActive()
        {
            switch (this.CallState)
            {
                case CallState.SIP_UNKNOWN:
                case CallState.SIP_CALL_CLOSED:
                case CallState.SIP_OUTGOING_FAILURE:
                    return false;
                case CallState.SIP_INCOMING_INVITE:
                case CallState.SIP_INCOMING_CONNECTED:
                case CallState.SIP_CALL_HOLD:
                case CallState.SIP_CALL_HELD:
                case CallState.SIP_CALL_DOUBLE_HOLD:
                case CallState.SIP_OUTGOING_TRYING:
                case CallState.SIP_OUTGOING_RINGING:
                case CallState.SIP_OUTGOING_CONNECTED:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取呼叫状态文本
        /// </summary>
        /// <returns>状态文本</returns>
        public string CallStateText()
        {
            var msg = string.Empty;
            switch (this.CallState)
            {
                case CallState.SIP_UNKNOWN:
                    break;
                case CallState.SIP_OUTGOING_FAILURE:
                    msg = string.Format("【{0}】呼出失败", this.CallName);
                    break;
                case CallState.SIP_CALL_CLOSED:
                    msg = string.Format("【{0}】呼出关闭", this.CallName);
                    break;
                case CallState.SIP_INCOMING_INVITE:
                    msg = string.Format("【{0}】正在呼入响铃中", this.CallName);
                    break;
                case CallState.SIP_INCOMING_CONNECTED:
                    msg = string.Format("【{0}】正在呼入通话中", this.CallName);
                    break;
                case CallState.SIP_CALL_HOLD:
                    msg = string.Format("【{0}】正在主动保持连接中", this.CallName);
                    break;
                case CallState.SIP_CALL_HELD:
                    msg = string.Format("【{0}】正在被动保持连接中", this.CallName);
                    break;
                case CallState.SIP_CALL_DOUBLE_HOLD:
                    msg = string.Format("【{0}】正在双方保持连接中", this.CallName);
                    break;
                case CallState.SIP_OUTGOING_TRYING:
                    msg = string.Format("【{0}】正在尝试呼出中", this.CallName);
                    break;
                case CallState.SIP_OUTGOING_RINGING:
                    msg = string.Format("【{0}】正在呼出响铃中", this.CallName);
                    break;
                case CallState.SIP_OUTGOING_CONNECTED:
                    msg = string.Format("【{0}】正在呼出通话中", this.CallName);
                    break;
            }
            return msg;
        }
        #endregion

        #region 呼叫操作
        /// <summary>
        /// 接听/应答呼叫
        /// </summary>
        /// <param name="callMode">响应呼叫类型  默认 VIDEO</param>
        public void AnswerCall(CallMode callMode = CallMode.VIDEO)
        {
            var errno = PlcmProxy.AnswerCall(this, callMode);
            if (ErrorNumber.OK != errno)
            {
                var errMsg = "接听应答呼叫失败，ErrorNo=" + errno;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            log.Info(string.Format("呼叫{0}接听成功。{1}", CallName, callMode));
        }

        /// <summary>
        /// 拒绝接听
        /// </summary>
        public void RejectCall()
        {
            var errno = PlcmProxy.RejectCall(this.CallHandle);
            if (ErrorNumber.OK != errno)
            {
                var errMsg = "拒绝接听失败，errNo=" + errno;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            log.Info(string.Format("拒绝接听{0}", CallName));
        }

        /// <summary>
        /// 挂断呼叫
        /// </summary>
        public void HangUpCall()
        {
            var errno = PlcmProxy.TerminateCall(this.CallHandle);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("挂断呼叫失败，ErrorNo=" + errno);
            }
            log.Info(string.Format("挂断呼叫{0}", CallName));
        }

        /// <summary>
        /// 保持呼叫
        /// </summary>
        public void HoldCall()
        {
            var errno = PlcmProxy.HoldCall(this.CallHandle);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("保持呼叫失败，ErrorNo=" + errno);
            }
            log.Info(string.Format("呼叫{0}保持", CallName));
        }
        /// <summary>
        /// 恢复保持的呼叫
        /// </summary>
        public void ResumeCall()
        {
            var errno = PlcmProxy.ResumeCall(this.CallHandle);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("恢复呼叫失败，ErrorNo=" + errno);
            }
            log.Info(string.Format("呼叫{0}恢复", CallName));
        }

        
        /// <summary>
        /// 切换音频视频模式
        /// </summary>
        /// <param name="call">呼叫处理器,为空时为当前呼叫</param>
        /// <param name="callMode">呼叫类型</param>
        public void ChangeCallType(CallMode callMode)
        {
            var errno = PlcmProxy.ChangeCallMode(this.CallHandle, callMode);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("切换音频视频模式失败，ErrorNo=" + errno);
            }
            log.Info(string.Format("呼叫{0}改变模式{1}", CallName,callMode));
        }


        /// <summary>
        /// 开启发送共享内容
        /// </summary>
        /// <param name="monitorHandle">显示器句柄</param>
        /// <param name="appHandle">应用程序句柄</param>
        public void StartSendContent(string monitorHandle, IntPtr appHandle)
        {
            var errno = PlcmProxy.StartShareContent(this.CallHandle, monitorHandle, appHandle);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("内容共享失败，ErrorNo=" + errno);
            }
            log.Info(string.Format("呼叫{0}开始发送共享内容", CallName));
        }

        /// <summary>
        /// 停止发送共享内容
        /// </summary>
        public void StopSendContent()
        {
            shareTimer?.Stop();
            var errno = PlcmProxy.StopShareContent(this.CallHandle);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("停止内容共享失败，ErrorNo=" + errno);
            }
            log.Info(string.Format("呼叫{0}结束发送共享内容", CallName));
        }

        #region BFCP共享
        private Timer shareTimer = null;
        /// <summary>
        /// 开始发送双流共享内容
        /// </summary>
        public void StartBFCPContent(ImageFormat imageFormat)
        {
            var errno = PlcmProxy.StartBFCPContent(this.CallHandle);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("内容共享失败，ErrorNo=" + errno);
            }
            log.Info(string.Format("呼叫{0}开始发送BFCP共享内容", CallName));

            shareTimer?.Dispose();
            shareTimer = new Timer() { Interval = 1000 };
            shareTimer.Tick += (sender, args) =>
            {
                //var width = Screen.PrimaryScreen.Bounds.Width;
                //var height = Screen.PrimaryScreen.Bounds.Height;
                var width = 1280;
                var height = 720;
                errno = PlcmProxy.SetContentBuffer(imageFormat, width, height);
                if (ErrorNumber.OK != errno)
                {
                    log.Error("SetContentBuffer 失败, ex:" + errno);
                }
                log.Info(string.Format("SetContentBuffer({0},{1},{2}", imageFormat, width, height));
            };
            shareTimer.Start();
        }
        #endregion

        /// <summary>
        /// 发送DTMF key
        /// </summary>
        /// <param name="key">DTMFkey</param>
        public void SendDTMFKey(DTMFKey key)
        {
            var errno = PlcmProxy.SendDTMFKey(this.CallHandle, key);
            if (errno != ErrorNumber.OK)
            {
                var errMsg = string.Format("发送DTMF Key({0})失败，errno={1}" ,key,errno);
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            log.Info(string.Format("发送DTMF Key({0})成功.", key));
        }

        #region FECC

        private Timer feccTimer = null;
        private FECCKey feccKey = FECCKey.UNKNOWN;
        /// <summary>
        /// 发送FECC
        /// </summary>
        /// <param name="key">FECC Key</param>
        public void StartSendFECC(FECCKey key)
        {
            var errno = PlcmProxy.SendFECCKey(this.CallHandle, key,  FECCAction.START);
            if (errno != ErrorNumber.OK)
            {
                var errMsg = string.Format("发送FECC({0}) Start失败，errno={1}" ,key, errno);
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            log.Info(string.Format("发送FECC({0}) Start成功.", key));

            feccKey = key;

            feccTimer?.Dispose();
            feccTimer = new Timer() { Interval = 200 };
            feccTimer.Tick += (sender, args) =>
            {
                errno = PlcmProxy.SendFECCKey(this.CallHandle, feccKey, FECCAction.CONTINUE);
                if (errno != ErrorNumber.OK)
                {
                    var errMsg = string.Format("发送FECC({0}) Continue失败，errno={1}",key, errno);
                    log.Error(errMsg);
                    throw new Exception(errMsg);
                }
                log.Info(string.Format("发送FECC({0}) Continue成功",key));
            };
            feccTimer.Start();
        }
        /// <summary>
        /// 结束FECC发送
        /// </summary>
        public void StopSendFECC()
        {
            feccTimer?.Stop();
            var errno = PlcmProxy.SendFECCKey(this.CallHandle, feccKey, FECCAction.STOP);
            if (errno != ErrorNumber.OK)
            {
                var errMsg = string.Format("发送FECC({0}) Stop，errno={1}", feccKey, errno);
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            log.Info(string.Format("发送FECC({0}) Stop失败", feccKey));
        }
        #endregion


        #endregion

        #region 设备相关
        /// <summary>
        /// 打开/关闭本地视频
        /// </summary>
        /// <param name="isMute">关闭：true, 打开：false</param>
        public void MuteLocalVideo(bool isMute)
        {
            var errno = PlcmProxy.MuteLocalVideo(isMute);
            if (errno != ErrorNumber.OK)
            {
                var errMsg = string.Format("打开/关闭本地视频设置{0}失败,errno={1}", isMute, errno);
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            MuteVideo = isMute;
            log.Info(string.Format("打开/关闭本地视频设置{0}成功", isMute));
        }
        /// <summary>
        /// 打开/关闭麦克风
        /// </summary>
        /// <param name="isMute">关闭：true, 打开：false</param>
        public void MuteMic(bool isMute)
        {
            var errno = PlcmProxy.MuteMic(this.CallHandle, isMute);
            if (ErrorNumber.OK != errno)
            {
                var errMsg = string.Format("麦克风静音设置{0}失败,errno={1}", isMute, errno);
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            log.Info(string.Format("麦克风静音设置{0}成功", isMute));
        }
        #endregion
    }
}
