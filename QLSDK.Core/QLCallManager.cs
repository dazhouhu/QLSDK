using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSDK.Core
{
    /// <summary>
    /// 呼叫管理器
    /// </summary>
    public class QLCallManager : BaseModel,IDisposable
    {
        #region Fields
        private ILog log = LogUtil.GetLogger("QLSDK.QLCallManager");
        private QLDeviceManager deviceManager = null;
        private QLConfigManager qlConfig = null;
        private QLCallView callView = null;

        private ObservableCollection<QLCall> _callList = null;
        private ContentRegion _contentRegion = null;
        #endregion
        #region Constructors
        private static readonly object lockObj = new object();
        private static QLCallManager instance = null;
        private QLCallManager()
        {
            deviceManager = QLDeviceManager.GetInstance();
            qlConfig = QLConfigManager.GetInstance();
            callView = QLCallView.GetInstance();
            _callList = new ObservableCollection<QLCall>();
        }
        public static QLCallManager GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new QLCallManager();
                    }
                }
            }
            return instance;
        }
        
        public void Dispose()
        {
            if (_callList.Count > 0)
            {
                #region 结束呼叫
                //保存通话记录
                foreach (var call in _callList)
                {
                    if(!call.IsActive())
                    {
                        PlcmProxy.TerminateCall(call.CallHandle);
                        call.StopTime = DateTime.Now;
                        call.CallState = CallState.SIP_CALL_CLOSED;
                        call.Reason = "关闭程序，结束通话";
                    }
                }
                #endregion
                #region 保存呼叫
                GetHistoryCalls((calls) =>
                {
                    var dicPath = Path.Combine(Application.StartupPath, "History");
                    if(!Directory.Exists(dicPath))
                    {
                        Directory.CreateDirectory(dicPath);
                    }
                    var filePath= Path.Combine(dicPath, string.Format("history_{0}.log", qlConfig.GetProperty(PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_UserName)));
                    using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        using (var sw = new StreamWriter(fs))
                        {
                            var str = SerializerUtil.SerializeJson(calls);
                            sw.Write(str);
                        }
                    }
                });
                #endregion
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// 当前呼叫改变事件
        /// </summary>
        public event Action CurrentCallChanged;
        #endregion

        #region 当前呼叫
        private QLCall _currentCall;
        /// <summary>
        /// 当前呼叫
        /// </summary>
        public QLCall CurrentCall
        {
            get { return _currentCall; }
            set
            {
                if (_currentCall != value)
                {
                    _currentCall = value;
                    NotifyPropertyChanged("CurrentCall");
                    CurrentCallChanged?.Invoke();
                }
            }
        }
        #endregion


        #region QLEventHandle
        /// <summary>
        /// 处理Poly核心事件
        /// </summary>
        /// <param name="evt">事件</param>
        internal void QLEventHandle(QLEvent evt)
        {
            log.Debug("evt:" + evt.EventType);
            try
            {
                switch (evt.EventType)
                {
                    #region Register
                    case EventType.UNKNOWN: break;
                    case EventType.SIP_REGISTER_SUCCESS: break;
                    case EventType.SIP_REGISTER_FAILURE:
                        {                            
                            CurrentCall = null;
                            _contentRegion?.Close();
                            throw new Exception("注册失败");
                        }
                        break;
                    case EventType.SIP_REGISTER_UNREGISTERED:
                        {
                            CurrentCall = null;
                            _contentRegion?.Close();
                            throw new Exception("未注册");
                        }
                        break;
                    #endregion
                    #region QLCall
                    case EventType.SIP_CALL_INCOMING:
                        {
                            #region Sound
                            try
                            {
                                //播放呼入响铃
                                deviceManager.StopSound();
                                var incomingSound = qlConfig.GetProperty(PropertyKey.SOUND_INCOMING);
                                if (!string.IsNullOrWhiteSpace(incomingSound))
                                {
                                    deviceManager.PlaySound(incomingSound, true, 2000);
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                            #endregion

                            var call = GetCall(evt.CallHandle, true, evt);
                            call.CallName = evt.CallerName;
                            call.CallType = CallType.INCOMING;
                            call.CallState = CallState.SIP_INCOMING_INVITE;
                            evt.Call = call;

                            var msg = string.Format("【{0}】呼入中，是否接听？", evt.CallerName);
                            if (null != this.CurrentCall && this.CurrentCall.IsActive())
                            {
                                var callStateText = this.CurrentCall.CallStateText();
                                if (!string.IsNullOrEmpty(callStateText))
                                {
                                    msg += '\n' + callStateText;
                                    msg += '\n' + "接听将挂断当前通话。";
                                }
                            }
                            Action answerAction = () =>
                            {
                                try
                                {
                                    if (null != this.CurrentCall && this.CurrentCall.IsActive())
                                    {
                                        this.CurrentCall.HangUpCall();
                                    }
                                    var callMode = call.CallMode;
                                    if (null == deviceManager.CurrentVideoInputDevice)
                                    { //当前视频设备为空时，只能语音通话
                                        callMode = CallMode.AUDIO;
                                    }
                                    call.AnswerCall(callMode);
                                }
                                catch(Exception ex)
                                {
                                    log.Error(ex.Message);
                                    call.RejectCall();
                                    callView.ShowMessage(false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
                                }
                            };
                            Action hangupAction = () =>
                            {
                                try
                                {
                                    call.RejectCall();
                                }
                                catch(Exception ex)
                                {
                                    log.Error(ex.Message);
                                    callView.ShowMessage(false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
                                }
                            };
                            callView.ShowMessage(true, msg, MessageBoxButtonsType.AnswerHangup, MessageBoxIcon.Question
                                                        , answerAction, hangupAction);

                        }
                        break;
                    case EventType.SIP_CALL_TRYING:
                        {
                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");

                            call.CallType = CallType.OUTGOING;
                            call.CallState = CallState.SIP_OUTGOING_TRYING;
                            evt.Call = call;
                            if (call == this.CurrentCall)
                            {
                                var msg = string.Format("尝试呼出【{0}】连接中...", call.CallName);

                                Action hangupAction = () =>
                                {
                                    try
                                    {
                                        call.HangUpCall();
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex.Message);
                                        callView.ShowMessage(false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
                                    }
                                };
                                callView.ShowMessage(false, msg, MessageBoxButtonsType.Hangup, MessageBoxIcon.Information
                                                            , hangupAction);
                            }

                        }
                        break;
                    case EventType.SIP_CALL_RINGING:
                        {
                            #region Sound
                            try
                            {
                                //播放呼叫响铃
                                deviceManager.StopSound();
                                var ringingSound = qlConfig.GetProperty(PropertyKey.SOUND_RINGING);
                                if (!string.IsNullOrWhiteSpace(ringingSound))
                                {
                                    deviceManager.PlaySound(ringingSound, true, 2000);
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                            #endregion

                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");

                            call.CallType = CallType.OUTGOING;
                            call.CallState = CallState.SIP_OUTGOING_RINGING;
                            evt.Call = call;

                            if (call == this.CurrentCall)
                            {
                                var msg = string.Format("呼出【{0}】响铃中...", call.CallName);

                                Action hangupAction = () =>
                                {
                                    try
                                    {
                                        call.HangUpCall();
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex.Message);
                                        callView.ShowMessage(false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
                                    }
                                };
                                callView.ShowMessage(false, msg, MessageBoxButtonsType.Hangup, MessageBoxIcon.Information
                                                            , hangupAction);
                            }
                        }
                        break;
                    
                    case EventType.SIP_CALL_HOLD:
                        {
                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");

                            evt.Call = call;
                            call.CallState = CallState.SIP_CALL_HOLD;
                            if (call == CurrentCall)
                            {
                                var msg = string.Format("呼叫【{0}】中断保持，是否需要恢复通话？", call.CallName);
                                Action yesAction = () =>
                                {
                                    try
                                    {
                                        call.ResumeCall();
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex.Message);
                                        callView.ShowMessage(false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
                                    }
                                };
                                callView.ShowMessage(false, msg, MessageBoxButtonsType.YesNoCancel, MessageBoxIcon.Question
                                    , yesAction);
                            }
                        }
                        break;
                    case EventType.SIP_CALL_HELD:
                        {
                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");

                            evt.Call = call;
                            call.CallState = CallState.SIP_CALL_HELD;
                            if (call == CurrentCall)
                            {
                                var msg = string.Format("呼叫【{0}】被保持", call.CallName);
                                Action hangupAction = () =>
                                {
                                    try
                                    {
                                        call.HangUpCall();
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex.Message);
                                        callView.ShowMessage(false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
                                    }
                                };
                                callView.ShowMessage(false, msg, MessageBoxButtonsType.Hangup, MessageBoxIcon.Information, hangupAction);
                            }
                        }
                        break;
                    case EventType.SIP_CALL_DOUBLE_HOLD:
                        {
                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");

                            evt.Call = call;
                            call.CallState = CallState.SIP_CALL_DOUBLE_HOLD;
                            if (call == CurrentCall)
                            {
                                var msg = string.Format("呼叫【{0}】双方中断保持，是否需要恢复通话？", call.CallName);
                                var yesAction = new Action(() =>
                                {
                                    try
                                    {
                                        call.ResumeCall();
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex.Message);
                                        callView.ShowMessage(false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
                                    }
                                });
                                callView.ShowMessage(false, msg, MessageBoxButtonsType.YesNo, MessageBoxIcon.Question
                                    , yesAction);
                            }
                        }
                        break;
                    case EventType.SIP_CALL_UAS_CONNECTED:
                        {
                            #region Sound
                            try
                            {
                                deviceManager.StopSound();
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                            #endregion

                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");

                            evt.Call = call;
                            log.Info(string.Format("呼入{0}接听通话中", call.CallName));

                            call.ConnectedTime = DateTime.Now;
                            call.CallType = CallType.INCOMING;
                            call.CallState = CallState.SIP_INCOMING_CONNECTED;

                            this.CurrentCall = call;
                            callView.HideMessage();
                        }
                        break;
                    case EventType.SIP_CALL_UAC_CONNECTED:
                        {
                            #region Sound
                            try
                            {
                                deviceManager.StopSound();
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                            #endregion

                            var call = GetCall(evt.CallHandle, true, evt);
                            log.Info(string.Format("呼入{0}接听通话中", call.CallName));
                            evt.Call = call;

                            log.Info(string.Format("呼出{0}接听通话中", call.CallName));
                            call.ConnectedTime = DateTime.Now;
                            call.CallType = CallType.OUTGOING;
                            call.CallState = CallState.SIP_OUTGOING_CONNECTED;

                            this.CurrentCall = call;
                            callView.HideMessage();
                        }
                        break;

                    case EventType.SIP_CALL_FAILURE:
                        {
                            #region Sound
                            try
                            {
                                deviceManager.StopSound();
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                            #endregion

                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");

                            call.StopTime = DateTime.Now;
                            call.CallType = CallType.OUTGOING;
                            call.CallState = CallState.SIP_OUTGOING_FAILURE;
                            evt.Call = call;

                            call.Reason = string.IsNullOrEmpty(evt.Reason) ? "unknown reason" : evt.Reason;
                            if (call == CurrentCall)
                            {
                                var msg = string.Format("呼出【{0}】失败,原因:{1}", call.CallName, call.Reason);
                                log.Error(msg);
                                callView.ShowMessage(false, msg, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
                                CurrentCall = null;
                            }
                        }
                        break;
                    case EventType.SIP_CALL_CLOSED:
                        {
                            #region Sound
                            try
                            {
                                deviceManager.StopSound();
                                var closedSound = qlConfig.GetProperty(PropertyKey.SOUND_CLOSED);
                                if (!string.IsNullOrWhiteSpace(closedSound))
                                {
                                    deviceManager.PlaySound(closedSound, false, 0);
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                            #endregion

                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");

                            evt.Call = call;
                            call.Reason = string.IsNullOrEmpty(evt.Reason) ? "unknown reason" : evt.Reason;
                            call.UnconnectedTime = DateTime.Now;
                            call.StopTime = DateTime.Now;
                            call.CallState = CallState.SIP_CALL_CLOSED;

                            if (call == CurrentCall)
                            {
                                var msg = string.Format("呼出【{0}】关闭,原因:{1}", call.CallName, call.Reason);
                                log.Info(msg);
                                callView.ShowMessage(false, msg, MessageBoxButtonsType.OK, MessageBoxIcon.Information);
                                CurrentCall = null;
                            }
                        }
                        break;
                    case EventType.SIP_CALL_MODE_UPGRADE_REQ:
                        {
                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");
                            evt.Call = call;

                            var msg = string.Format("【{0}】请求中，是否接听？", evt.CallMode == CallMode.VIDEO ? "视频通话" : "语言通话");
                            Action yesAction = () =>
                            {
                                try
                                {
                                    if (null != this.CurrentCall && this.CurrentCall.IsActive())
                                    {
                                        this.CurrentCall.HangUpCall();
                                    }
                                    var callMode = evt.CallMode;
                                    if (null == deviceManager.CurrentVideoInputDevice)
                                    { //当前视频设备为空时，只能语音通话
                                        callMode = CallMode.AUDIO;
                                    }
                                    call.AnswerCall(callMode);
                                    call.CallMode = callMode;
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex.Message);
                                    callView.ShowMessage(false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
                                }
                            };
                            Action noAction = () =>
                            {
                                try
                                {
                                    if (null != this.CurrentCall && this.CurrentCall.IsActive())
                                    {
                                        this.CurrentCall.HangUpCall();
                                    }
                                    var callMode = call.CallMode;
                                    if (null == deviceManager.CurrentVideoInputDevice)
                                    { //当前视频设备为空时，只能语音通话
                                        callMode = CallMode.AUDIO;
                                    }
                                    call.AnswerCall(callMode);
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex.Message);
                                    callView.ShowMessage(false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
                                }
                            };
                            callView.ShowMessage(true, msg, MessageBoxButtonsType.YesNo, MessageBoxIcon.Question
                                                        , yesAction, noAction);
                        }
                        break;
                    #endregion
                    #region Content
                    case EventType.SIP_CONTENT_INCOMING:
                        {
                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");
                            evt.Call = call;

                            #region ContentChannel
                            var contentChannel = call.GetContentChannel();
                            if (null != contentChannel)
                            {
                                call.RemoveChannel(contentChannel.ChannelID);
                            }
                            contentChannel = new QLChannel(call, evt.StreamId, MediaType.CONTENT);
                            contentChannel.ChannelName = "远端共享流";
                            call.AddChannel(contentChannel);
                            contentChannel.Size = new Size(evt.WndWidth, evt.WndHeight);
                            contentChannel.IsVideo = true;
                            #endregion

                            call.IsContentSupported = true;
                            call.IsContentIdle = false;

                            _contentRegion?.Close();
                        }
                        break;
                    case EventType.SIP_CONTENT_SENDING:
                        {
                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");
                            evt.Call = call;

                            #region ContentChannel
                            var contentChannel = call.GetLocalContentChannel();
                            if (null != contentChannel)
                            {
                                call.RemoveChannel(contentChannel.ChannelID);
                            }
                            contentChannel = new QLChannel(call, -1, MediaType.LOCALCONTENT);
                            contentChannel.ChannelName = "本地共享流";
                            call.AddChannel(contentChannel);
                            contentChannel.IsVideo = true;
                            #endregion

                            call.IsContentSupported = true;
                            call.IsContentIdle = false;
                            /*
                            Action unSharedAction = () =>
                            {
                                _currentCall?.StopSendContent();
                            };
                            
                            callView.ShowMessage(false, "共享内容中，确认停止共享吗？", MessageBoxButtonsType.OK, MessageBoxIcon.Question
                                                        , unSharedAction);
                            */
                            #region 显示共享区域
                            _contentRegion?.Close();
                            _contentRegion = new ContentRegion(_currentCall, contentChannel);
                            #endregion
                        }
                        break;
                    case EventType.SIP_CONTENT_IDLE:
                        {
                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");
                            evt.Call = call;
                            call.IsContentSupported = true;
                            call.IsContentIdle = true;

                            _contentRegion?.Close();
                        }
                        break;
                    case EventType.SIP_CONTENT_UNSUPPORTED:
                        {
                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");
                            evt.Call = call;
                            call.IsContentSupported = false;
                            call.IsContentIdle = false;

                            _contentRegion?.Close();
                        }
                        break;
                    case EventType.SIP_CONTENT_CLOSED:
                        {
                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");
                            evt.Call = call;

                            #region ContentChannel
                            var contentChannel = call.GetContentChannel();
                            if (null != contentChannel)
                            {
                                call.RemoveChannel(contentChannel.ChannelID);
                            }

                            var localContentChannel = call.GetLocalContentChannel();
                            if (null != localContentChannel)
                            {
                                call.RemoveChannel(localContentChannel.ChannelID);
                            }
                            #endregion

                            call.IsContentSupported = true;
                            call.IsContentIdle = true;

                            _contentRegion?.Close();
                            callView.HideMessage();
                        }
                        break;
                    
                    #endregion

                    #region Device
                    /*
                    case EventType.DEVICE_VIDEOINPUTCHANGED: break;  
                    case EventType.DEVICE_AUDIOINPUTCHANGED: break; 
                    case EventType.DEVICE_AUDIOOUTPUTCHANGED: break; 
                    case EventType.DEVICE_VOLUMECHANGED: break;  
                    case EventType.DEVICE_MONITORINPUTSCHANGED:break;  
                    */
                    #endregion

                    #region Stream
                    case EventType.STREAM_VIDEO_LOCAL_RESOLUTIONCHANGED:
                        {
                            var call = CurrentCall;
                            if (null == call) throw new Exception("呼叫不存在");
                            evt.Call = call;
                            var localChannel = call.GetLocalChannel();
                            if (null == localChannel)
                            {
                                localChannel = new QLChannel(call, 0, MediaType.LOCAL, false);
                                localChannel.ChannelName = "本地视频";
                                call.AddChannel(localChannel);
                            }
                            localChannel.Size = new Size(evt.WndWidth, evt.WndHeight);
                            localChannel.IsVideo = true;
                            callView.HideMessage();
                        }
                        break;
                    case EventType.STREAM_VIDEO_REMOTE_RESOLUTIONCHANGED:
                        {
                            var call = CurrentCall;
                            if (null == call) throw new Exception("呼叫不存在");
                            evt.Call = call;
                            var channel = call.GetChannel(evt.StreamId);
                            if (null == channel)
                            {
                                channel = new QLChannel(call, evt.StreamId, MediaType.REMOTE,false);
                                channel.ChannelName = evt.RemoteChannelDisplayName;
                                call.AddChannel(channel);
                            }
                            channel.Size = new Size(evt.WndWidth, evt.WndHeight);
                            channel.IsVideo = true;

                            callView.HideMessage();
                        }
                        break;
                    #endregion

                    case EventType.NETWORK_CHANGED: break;
                    case EventType.MFW_INTERNAL_TIME_OUT: break;

                    #region Remote Refresh
                    case EventType.REFRESH_ACTIVE_SPEAKER:
                        {
                            var call = GetCall(evt.CallHandle, true, evt);
                            evt.Call = call;
                            call.ActiveSpeakerId = evt.ActiveSpeakerStreamId;
                            if (call == CurrentCall)
                            {
                                callView.HideMessage();
                            }
                        }
                        break;
                    case EventType.REMOTE_VIDEO_REFRESH:
                        {
                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");
                            evt.Call = call;
                            call.ClearRemoteChannels();
                            call.ChannelNumber = evt.RemoteVideoChannelNum;
                            call.ActiveSpeakerId = evt.ActiveSpeakerStreamId;
                            if (call == CurrentCall)
                            {
                                callView.HideMessage();
                            }
                        }
                        break;
                    case EventType.REMOTE_VIDEO_CHANNELSTATUS_CHANGED:
                        {
                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");
                            evt.Call = call;
                            call.AddChannel(evt.StreamId, MediaType.REMOTE);
                            if (call.ActiveSpeakerId <= 0)
                            {
                                call.ActiveSpeakerId = evt.StreamId;
                            }
                            if (call == CurrentCall)
                            {
                                callView.HideMessage();
                            }
                        }
                        break;
                    case EventType.REMOTE_VIDEO_DISPLAYNAME_UPDATE:
                        {
                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");
                            evt.Call = call;
                            call.SetChannelName(evt.StreamId, evt.RemoteChannelDisplayName);
                            if (call == CurrentCall)
                            {
                                callView.HideMessage();
                            }
                        }
                        break;
                    case EventType.SIP_CALL_MODE_CHANGED:
                        {
                            var call = GetCall(evt.CallHandle, true, evt);
                            if (null == call) throw new Exception("呼叫不存在");
                            evt.Call = call;
                            call.CallMode = evt.CallMode;
                        }
                        break;
                    #endregion

                    
                    case EventType.IS_TALKING_STATUS_CHANGED: break;
                    case EventType.CERTIFICATE_VERIFY: break;
                    case EventType.TRANSCODER_FINISH: break;
                    case EventType.ICE_STATUS_CHANGED: break;
                    case EventType.SUTLITE_INCOMING_CALL: break;
                    case EventType.SUTLITE_TERMINATE_CALL: break;
                    case EventType.NOT_SUPPORT_VIDEOCODEC: break;

                    case EventType.BANDWIDTH_LIMITATION: break;
                    case EventType.MEDIA_ADDRESS_UPDATED: break;
                    case EventType.AUTODISCOVERY_STATUS_CHANGED: break;
                }
            }
            catch(Exception ex)
            {
                log.Error(string.Format("Event 异常，ex={0},{1}",ex.Message,ex.StackTrace));
                callView.ShowMessage(false, ex.Message, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 获取呼叫
        /// <summary>
        /// 获取呼叫
        /// </summary>
        /// <param name="callHandle">呼叫ID</param>
        /// <param name="isCreate">无时是否需要创建</param>
        /// <param name="evt">事件</param>
        /// <returns>呼叫</returns>
        internal QLCall GetCall(int callHandle, bool isCreate = false, QLEvent evt = null)
        {
            var call = _callList.FirstOrDefault(c => c.CallHandle == callHandle && c.IsActive());
            if (null == call)
            {
                if (isCreate)
                {
                    call = new QLCall(callHandle)
                    {
                        CallHandle = callHandle,
                        CallMode = evt.CallMode,
                        ActiveSpeakerId = evt.ActiveSpeakerStreamId,
                        Reason = evt.Reason,
                        ChannelNumber = evt.RemoteVideoChannelNum,
                        CallState = CallState.SIP_UNKNOWN,
                        CallType = CallType.UNKNOWN,
                        StartTime = DateTime.Now,
                    };
                    _callList.Add(call);
                }
            }
            return call;
        }

        #endregion

        #region CUD 操作
        /// <summary>
        /// 添加呼叫
        /// </summary>
        /// <param name="call">呼叫</param>
        internal void AddCall(QLCall call)
        {
            if (call.CallHandle != -1)
            {
                var c = GetCall(call.CallHandle);
                if (null == c)
                {
                    _callList.Add(call);
                }
            }
        }
        /// <summary>
        /// 移除呼叫
        /// </summary>
        /// <param name="call">呼叫</param>
        internal void RemoveCall(QLCall call)
        {
            if (Contains(call.CallHandle))
            {
                _callList.Remove(call);
                CurrentCall = _callList.FirstOrDefault();
            }
        }

        /// <summary>
        /// 呼叫是否已经存在
        /// </summary>
        /// <param name="callHandle">呼叫ID</param>
        /// <returns>存在：true ,不存在:false</returns>
        internal bool Contains(int callHandle)
        {
            return _callList.Any(c => c.CallHandle == callHandle);
        }
        #endregion

        #region 获取历史呼叫
        /// <summary>
        /// 获取历史呼叫
        /// </summary>
        /// <param name="callback"></param>
        public void GetHistoryCalls(Action<IEnumerable<QLCall>> callback)
        {
            if (null != callback)
            {
                var calls = new List<QLCall>();
                var dicPath = Path.Combine(Application.StartupPath, "History");
                if (!Directory.Exists(dicPath))
                {
                    Directory.CreateDirectory(dicPath);
                }
                var filePath = Path.Combine(dicPath, string.Format("history_{0}.log", qlConfig.GetProperty(PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_UserName)));
                if (File.Exists(filePath))
                {
                    using(var fs =new FileStream(filePath, FileMode.Open))
                    {
                        using (var sr = new StreamReader(fs))
                        {
                            var str = sr.ReadToEnd();
                            if(!string.IsNullOrWhiteSpace(str))
                            {
                                calls = SerializerUtil.DeSerializeJson<List<QLCall>>(str);
                            }
                        }
                    }
                }
                calls.AddRange(this._callList);
                callback.Invoke(calls);
            }
        }
        #endregion

        #region 呼叫
        /// <summary>
        /// 呼叫
        /// </summary>
        /// <param name="dialUri">被呼叫方Uri</param>
        /// <param name="callType">呼叫模式 默认：VIDEO</param>
        public void DialCall(string dialUri, CallMode callMode = CallMode.VIDEO)
        {
            if(null != CurrentCall && CurrentCall.IsActive())
            {
                var msg = string.Format("确定要呼叫{0}吗？", dialUri);

                var callStateText = CurrentCall.CallStateText();
                if (!string.IsNullOrEmpty(callStateText))
                {
                    msg += '\n' + callStateText;
                    msg += '\n' + "接听将挂断当前通话。";
                }
                Action okpAction = () =>
                {
                    log.Info(string.Format("挂断呼叫{0}", CurrentCall.CallName));
                    CurrentCall.HangUpCall();
                    diallCallDiret(dialUri,callMode);
                };
                callView.ShowMessage(true, msg, MessageBoxButtonsType.OKCancel, MessageBoxIcon.Question
                                            , okpAction);
            }
            else
            {
                diallCallDiret(dialUri, callMode);
            }
        }    
        private void diallCallDiret(string dialUri, CallMode callMode = CallMode.VIDEO)
        {
            int callHandle = -1;
            qlConfig.SetProperty(PropertyKey.CalleeAddr, dialUri,true);
            var errno = PlcmProxy.PlaceCall(dialUri, ref callHandle, callMode);
            if (ErrorNumber.OK == errno)
            {
                var call = new QLCall(callHandle)
                {
                    CallHandle = callHandle,
                    CallName = dialUri,
                    CallMode = callMode,
                    ActiveSpeakerId = 0,
                    CallState = CallState.SIP_UNKNOWN,
                    CallType = CallType.UNKNOWN,
                    StartTime = DateTime.Now,
                };
                this.AddCall(call);
                this.CurrentCall = call;
                log.Info("呼叫成功！");
            }
            else
            {
                var msg = "呼叫失败，ErrorNo=" + errno;
                callView.ShowMessage(false, msg, MessageBoxButtonsType.OK, MessageBoxIcon.Error);
                throw new Exception(msg);
            }
        }
        #endregion
    }
}

