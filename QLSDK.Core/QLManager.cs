using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Forms;

namespace QLSDK.Core
{
    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="evt"></param>
    public delegate void QLEventHandle(QLEvent evt);
    /// <summary>
    /// 核心管理器
    /// </summary>
    public class QLManager:IDisposable
    {
        #region Fields
        private static ILog log = LogUtil.GetLogger("QLSDK.QLManager");
        private static Action<ObservableCollection<QLMediaStatistics>> mediaStatisticsCallBack;
        private static Action<ObservableCollection<QLApp>> appCallBack;
        private ObservableCollection<QLMediaStatistics> mediaStatistics = new ObservableCollection<QLMediaStatistics>();
        private ObservableCollection<QLApp> apps = new ObservableCollection<QLApp>();
        private static QLConfigManager qlConfig = QLConfigManager.GetInstance();
        private static QLCallView callView = QLCallView.GetInstance();
        private static QLDeviceManager deviceManager = QLDeviceManager.GetInstance();
        private static QLCallManager callManager = QLCallManager.GetInstance();
        #endregion

        #region Constructors        
        private static readonly object lockObj = new object();
        private static QLManager instance = null;
        private QLManager()
        {
            StartEventMonitor();
            mediaStatistics.CollectionChanged += (sender, args) =>
            {
                mediaStatisticsCallBack?.Invoke(mediaStatistics);
            };
            apps.CollectionChanged += (sender, args) =>
            {
                appCallBack?.Invoke(apps);
            };

            this.InternalQLEvent += callManager.QLEventHandle;
        }
        public static QLManager GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new QLManager();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Release();
        }
        #endregion

        #region 是否注册成功
        private bool isRegisted = false;
        /// <summary>
        /// 是否注册成功
        /// </summary>
        public bool IsRegisted { get { return isRegisted; } }

        #endregion

        #region Events
        /// <summary>
        /// Poly核心事件
        /// </summary>
        public event QLEventHandle QLEvent;
        internal event QLEventHandle InternalQLEvent;
        #endregion

        /// <summary>
        /// 登录绑定polc服务
        /// </summary>
        /// <param name="server">服务器验证地址</param>
        /// <param name="password">注册用户名</param>
        /// <param name="password">注册用户密码</param>
        /// <param name="qlConfig">初始化配置  获取默认配置：QlConfig.GetDefaultConfig()</param>
        public void Login(string server, string username, string password, IDictionary<PropertyKey, string> qlConfigs = null)
        {
            #region Valid
            if (string.IsNullOrWhiteSpace(server))
            {
                throw new Exception("服务地址必须");
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new Exception("用户名必须");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("密码必须");
            }
            #endregion
            
            var postData = new Dictionary<string, string>() {
                { "secret_key",username },
                { "package_name",password}
            };
            var postResult= HttpUtil<AuthorizeData>.Post(server+ @"/api/authorization", postData);  //发送验证数据
            if (postResult.success)
            {
                #region 设置配置信息
                var ps = qlConfig.GetDefaultConfig();
                if (null != qlConfigs)
                {
                    foreach (var config in qlConfigs)
                    {
                        ps[config.Key] = config.Value;
                    }
                }
                ps[PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_ProxyServer] = postResult.data.sip_addr;
                ps[PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_UserName] = postResult.data.account;
                ps[PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_Password] = postResult.data.pass;
                if (string.IsNullOrEmpty(ps[PropertyKey.PLCM_MFW_KVLIST_KEY_REG_ID]))
                {
                   // ps[PropertyKey.PLCM_MFW_KVLIST_KEY_REG_ID] = postResult.data.account + "@" + postResult.data.sip_addr;
                }
                if (string.IsNullOrEmpty(ps[PropertyKey.PLCM_MFW_KVLIST_KEY_DisplayName]))
                {
                    ps[PropertyKey.PLCM_MFW_KVLIST_KEY_DisplayName] = postResult.data.account;
                }
                
                if(!string.IsNullOrEmpty(ps[PropertyKey.PLCM_MFW_KVLIST_KEY_LogLevel]))
                {
                    LogUtil.SetLogLevel(ps[PropertyKey.PLCM_MFW_KVLIST_KEY_LogLevel]);
                }
                #endregion
                //SDK初始化
                SDKInit(ps);
            }
            else
            {
                throw new Exception("登录验证失败,"+postResult.message);
            }
        }
        #region SDK init
        #region Callback        
        private AddEventCallback addEventCallback = new AddEventCallback(AddEventCallbackF);
        private DispatchEventsCallback dispatchEventsCallback = new DispatchEventsCallback(DispatchEventsCallbackF);
        private AddLogCallback addLogCallback = new AddLogCallback(AddLogCallbackF);
        private AddDeviceCallback addDeviceCallback = new AddDeviceCallback(AddDeviceCallbackF);
        private DisplayMediaStatisticsCallback displayMediaStatisticsCallback = new DisplayMediaStatisticsCallback(DisplayMediaStatisticsCallbackF);
        private DisplayCallStatisticsCallback displayCallStatisticsCallback = new DisplayCallStatisticsCallback(DisplayCallStatisticsCallbackF);
        private DisplayCodecCapabilities displayCodecCapabilities = new DisplayCodecCapabilities(DisplayCodecCapabilitiesF);
        private AddAppCallback addAppCallback = new AddAppCallback(AddAppCallbackF);

        /// <summary>
        /// 事件回调
        /// </summary>
        private static void AddEventCallbackF(IntPtr eventHandle, int call, IntPtr placeId, int eventType, IntPtr callerName,
                 IntPtr calleeName, int userCode, IntPtr reason, int wndWidth, int wndHeight, bool plugDeviceStatus, IntPtr plugDeviceName, IntPtr deviceHandle, IntPtr ipAddress, int callMode,
                 int streamId, int activeSpeakerStreamId, int remoteVideoChannelNum, IntPtr remoteChannelDisplayName, bool isActiveSpeaker, int isTalkingFlag, IntPtr regID, IntPtr sipCallId, IntPtr version, IntPtr serialNumber, IntPtr notBefore, IntPtr notAfter,
                 IntPtr issuer, IntPtr subject, IntPtr signatureAlgorithm, IntPtr fingerPrintAlgorithm, IntPtr fingerPrint, IntPtr publickey, IntPtr basicContraints, IntPtr keyUsage, IntPtr rxtendedKeyUsage,
                 IntPtr subjectAlternateNames, IntPtr pemCert, bool isCertHostNameOK, int certFailReason, int certConfirmId, IntPtr transcoderTaskId, IntPtr transcoderInputFileName, int iceStatus, IntPtr sutLiteMessage, bool isVideoOK, IntPtr mediaIPAddr, int discoveryStatus)
        {
            var strplaceId = IntPtrHelper.IntPtrTostring(placeId);
            var strcallerName = IntPtrHelper.IntPtrTostring(callerName);
            var strcalleeName = IntPtrHelper.IntPtrTostring(calleeName);
            var strreason = IntPtrHelper.IntPtrTostring(reason);
            var strplugDeviceName = IntPtrHelper.IntPtrTostring(plugDeviceName);
            var strdeviceHandle = IntPtrHelper.IntPtrTostring(deviceHandle);
            var stripAddress = IntPtrHelper.IntPtrTostring(ipAddress);
            var strremoteChannelDisplayName = IntPtrHelper.IntPtrTostring(remoteChannelDisplayName);
            var strregID = IntPtrHelper.IntPtrTostring(regID);
            var strsipCallId = IntPtrHelper.IntPtrTostring(sipCallId);
            var strVersion = IntPtrHelper.IntPtrTostring(version);
            var strSerialNumber = IntPtrHelper.IntPtrTostring(serialNumber);
            var strNotBefore = IntPtrHelper.IntPtrTostring(notBefore);
            var strNotAfter = IntPtrHelper.IntPtrTostring(notAfter);
            var strIssuer = IntPtrHelper.IntPtrTostring(issuer);
            var strSubject = IntPtrHelper.IntPtrTostring(subject);
            var strSignatureAlgorithm = IntPtrHelper.IntPtrTostring(signatureAlgorithm);
            var strFingerPrintAlgorithm = IntPtrHelper.IntPtrTostring(fingerPrintAlgorithm);
            var strFingerPrint = IntPtrHelper.IntPtrTostring(fingerPrint);
            var strPublickey = IntPtrHelper.IntPtrTostring(publickey);
            var strBasicContraints = IntPtrHelper.IntPtrTostring(basicContraints);
            var strKeyUsage = IntPtrHelper.IntPtrTostring(keyUsage);
            var strExtendedKeyUsage = IntPtrHelper.IntPtrTostring(rxtendedKeyUsage);
            var strSubjectAlternateNames = IntPtrHelper.IntPtrTostring(subjectAlternateNames);
            var strPemCert = IntPtrHelper.IntPtrTostring(pemCert);
            var strtranscoderInputFileName = IntPtrHelper.IntPtrTostring(transcoderInputFileName);
            var strSUTLiteMessage = IntPtrHelper.IntPtrTostring(sutLiteMessage);
            var strMediaIPAddr = IntPtrHelper.IntPtrTostring(mediaIPAddr);
            var evt = new QLEvent(eventHandle, call, strplaceId, (EventType)eventType, strcallerName,
                                strcalleeName, userCode, strreason,
                                wndWidth, wndHeight, plugDeviceStatus, strplugDeviceName, strdeviceHandle, stripAddress, (CallMode)callMode,
                                streamId, activeSpeakerStreamId, remoteVideoChannelNum, strremoteChannelDisplayName, isActiveSpeaker, isTalkingFlag, strregID, strsipCallId,
                                strVersion,
                                strSerialNumber,
                                strNotBefore,
                                strNotAfter,
                                strIssuer,
                                strSubject,
                                strSignatureAlgorithm,
                                strFingerPrintAlgorithm,
                                strFingerPrint,
                                strPublickey,
                                strBasicContraints,
                                strKeyUsage,
                                strExtendedKeyUsage,
                                strSubjectAlternateNames,
                                strPemCert,
                                isCertHostNameOK,
                                certFailReason,
                                certConfirmId,
                                transcoderTaskId,
                                strtranscoderInputFileName,
                                (ICEStatus)iceStatus,
                                strSUTLiteMessage,
                                isVideoOK,
                                strMediaIPAddr,
                                (AutoDiscoveryStatus)discoveryStatus);
            AddEvent(evt);
        }
        /// <summary>
        /// 事件释放回调
        /// </summary>
        private static void DispatchEventsCallbackF()
        {
            DispatchEvents();
        }
        #region Events

        private static Queue<QLEvent> queue = new Queue<QLEvent>();
        private static AutoResetEvent autoEvent;
        private static object synObject = new object();
        private static bool isRunning = false;
        
        private static void StartEventMonitor()
        {
            autoEvent = new AutoResetEvent(false);
            isRunning = true;
            var thread = new Thread(() =>
            {
                while (isRunning)
                {
                    log.Debug("handle the evt");
                    if (queue.Count <= 0)
                    {
                        lock (synObject)
                        {
                            log.Info("No evt, wait..");
                            autoEvent.WaitOne();
                        }
                    }
                    while (queue.Count > 0)
                    {
                        QLEvent evt = null;
                        lock (synObject)
                        {
                            evt = queue.Dequeue();
                        }
                        // dispatch QLEvent to proper modules
                        if (evt == null)
                        {
                            log.Error("QLEvent is null!");
                            continue;
                        }
                        try
                        {
                            callView.Invoke(new Action(() =>
                            {
                                DoEvent(evt);
                            }));
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }
                        PlcmProxy.FreeEvent(evt.EventHandle);
                    }
                }
            });
            thread.Start();
        }
        private static void StopEventMonitor()
        {
            isRunning = false;
            DispatchEvents();
        }
        private static void DispatchEvents()
        {
            //lock (synObject)
            {
                log.Info("notify evt monitor to proceed the events");
                autoEvent.Set();
            }
        }
        private static void AddEvent(QLEvent evt)
        {
            log.Info("addEvent, type is" + evt.EventType);
            //lock (synObject)
            {
                queue.Enqueue(evt);
                //autoEvent.Set();
            }
        }
        private static void DoEvent(object state)
        {
            var evt = state as QLEvent;
            if (null == evt)
            {
                return;
            }
            log.Info(string.Format("EventType:{0},CallHandle:{1}", evt.EventType, evt.CallHandle));
            switch (evt.EventType)
            {
                #region QLDevice
                case EventType.DEVICE_VIDEOINPUTCHANGED:
                    {
                        string deviceName = evt.PlugDeviceName;
                        string deviceHandle = evt.DeviceHandle;
                        if (string.IsNullOrWhiteSpace(deviceName)
                            || string.IsNullOrWhiteSpace(deviceHandle))
                        {
                            return;
                        }
                        if (true == evt.PlugDeviceStatus)
                        {   /*plug in a device*/
                            var device = new QLDevice(DeviceType.VIDEOINPUT, deviceHandle, deviceName);
                            deviceManager.AddDevice(device);
                        }
                        else
                        {
                            deviceManager.RemoveDevice(deviceHandle);
                        }
                    }
                    break;   /* from MP */
                case EventType.DEVICE_AUDIOINPUTCHANGED:
                    {
                        string deviceName = evt.PlugDeviceName;
                        string deviceHandle = evt.DeviceHandle;
                        if (string.IsNullOrWhiteSpace(deviceName)
                            || string.IsNullOrWhiteSpace(deviceHandle))
                        {
                            return;
                        }
                        if (true == evt.PlugDeviceStatus)
                        {   /*plug in a device*/
                            var device = new QLDevice(DeviceType.AUDIOINPUT, deviceHandle, deviceName);
                            deviceManager.AddDevice(device);
                        }
                        else
                        {
                            deviceManager.RemoveDevice(deviceHandle);
                        }
                    }
                    break;  /* from MP */
                case EventType.DEVICE_AUDIOOUTPUTCHANGED:
                    {
                        string deviceName = evt.PlugDeviceName;
                        string deviceHandle = evt.DeviceHandle;
                        if (string.IsNullOrWhiteSpace(deviceName)
                            || string.IsNullOrWhiteSpace(deviceHandle))
                        {
                            return;
                        }
                        if (true == evt.PlugDeviceStatus)
                        {   /*plug in a device*/
                            var device = new QLDevice(DeviceType.AUDIOOUTPUT, deviceHandle, deviceName);
                            deviceManager.AddDevice(device);
                        }
                        else
                        {
                            deviceManager.RemoveDevice(deviceHandle);
                        }
                    }
                    break; /* from MP */
                case EventType.DEVICE_VOLUMECHANGED: break;  /* from MP */
                case EventType.DEVICE_MONITORINPUTSCHANGED:
                    {
                        string deviceName = evt.PlugDeviceName;
                        string deviceHandle = evt.DeviceHandle;
                        if (string.IsNullOrWhiteSpace(deviceName)
                            || string.IsNullOrWhiteSpace(deviceHandle))
                        {
                            return;
                        }
                        if (true == evt.PlugDeviceStatus)
                        {   /*plug in a device*/
                            var device = new QLDevice(DeviceType.MONITOR, deviceHandle, deviceName);
                            deviceManager.AddDevice(device);
                        }
                        else
                        {
                            deviceManager.RemoveDevice(deviceHandle);
                        }
                    }
                    break;  /* from MP */
                #endregion

                #region Register
                case EventType.UNKNOWN: break;
                case EventType.SIP_REGISTER_SUCCESS:
                    {
                        QLManager.GetInstance().isRegisted = true;
                    }
                    break;
                case EventType.SIP_REGISTER_FAILURE:
                    {
                        QLManager.GetInstance().isRegisted = false;
                    }
                    break;
                case EventType.SIP_REGISTER_UNREGISTERED: break;
                    #endregion
                #region 不处理
                    /*
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
            QLManager.GetInstance().InternalQLEvent?.Invoke(evt);
            QLManager.GetInstance().QLEvent?.Invoke(evt);
        }
        #endregion
        /// <summary>
        /// 日志回调
        /// </summary>
        private static void AddLogCallbackF(long timestamp, bool expired, int funclevel, int pid, int tid, IntPtr lev, IntPtr comp, IntPtr msg, int len)
        {
            var output = string.Empty;
            var level = IntPtrHelper.IntPtrToUTF8string(lev);
            var component = IntPtrHelper.IntPtrTostring(comp);
            var message = IntPtrHelper.IntPtrTostring(msg);
            if (string.IsNullOrEmpty(component))
            {
                component = "wrapper";
            }

            output += string.Format(" [PID:{0}][TID:{1}] ", pid, tid); ;

            output += component+"  "+message;

            for (int i = 0; i < funclevel; i++)
            {
                output += "  ";
            }

            if (level == "DEBUG")
            {
                log.Debug(output);
            }
            else if (level == "INFO")
            {
                log.Info(output);
            }
            else if (level == "WARN")
            {
                log.Warn(output);
            }
            else if (level == "ERROR")
            {
                log.Error(output);
            }
            else
            {
                log.Fatal(output);
            }
        }
        /// <summary>
        /// 应用程序回调
        /// </summary>
        private static void AddAppCallbackF(IntPtr appHandle, IntPtr appNamePtr)
        {
            var appName = IntPtrHelper.IntPtrTostring(appNamePtr);
            log.Info("AddAppCallbackF: appHandle:" + appHandle + "  appName:" + appName);
            var app = new QLApp(appHandle, appName);
            callView.Invoke(new Action(() =>
            {
                QLManager.GetInstance().apps.Add(app);
            }));
        }

        /// <summary>
        /// 设备回调
        /// </summary>
        private static void AddDeviceCallbackF(int deviceType, IntPtr deviceHandlePtr, IntPtr deviceNamePtr)
        {
            if (deviceType <= (int)DeviceType.MONITOR)
            {
                var deviceHandle = IntPtrHelper.IntPtrToUTF8string(deviceHandlePtr);
                var deviceName = IntPtrHelper.IntPtrToUTF8string(deviceNamePtr);
                log.Info("AddDeviceCallback: deviceType:" + deviceType + "  deviceHandle:" + deviceHandle + "  deviceName:" + deviceName);
                var device = new QLDevice((DeviceType)deviceType, deviceHandle, deviceName);
                deviceManager.AddDevice(device);
            }
            else
            {
                throw new Exception(string.Format("DeviceType:{0} 设备不支持", deviceType));
            }
        }

        /// <summary>
        /// 统计信息回调
        /// </summary>
        private static void DisplayMediaStatisticsCallbackF(IntPtr channelNamePtr, IntPtr participantNamePtr, IntPtr remoteSystemIdPtr, IntPtr callRatePtr, IntPtr packetsLostPtr, IntPtr packetLossPtr,
                 IntPtr videoProtocolPtr, IntPtr videoRatePtr, IntPtr videoRateUsedPtr, IntPtr videoFrameRatePtr, IntPtr videoPacketsLostPtr, IntPtr videoJitterPtr,
                 IntPtr videoFormatPtr, IntPtr errorConcealmentPtr, IntPtr audioProtocolPtr, IntPtr audioRatePtr, IntPtr audioPacketsLostPtr, IntPtr audioJitterPtr,
                 IntPtr audioEncryptPtr, IntPtr videoEncryptPtr, IntPtr feccEncryptPtr, IntPtr audioReceivedPacketPtr, IntPtr roundTripTimePtr,
                 IntPtr fullIntraFrameRequestPtr, IntPtr intraFrameSentPtr, IntPtr packetsCountPtr, IntPtr overallCPULoadPtr, IntPtr channelNumPtr)
        {
            var channelName = IntPtrHelper.IntPtrTostring(channelNamePtr);
            var participantName = IntPtrHelper.IntPtrTostring(participantNamePtr);
            var remoteSystemId = IntPtrHelper.IntPtrTostring(remoteSystemIdPtr);
            var callRate = IntPtrHelper.IntPtrTostring(callRatePtr);
            var packetsLost = IntPtrHelper.IntPtrTostring(packetsLostPtr);
            var packetLoss = IntPtrHelper.IntPtrTostring(packetLossPtr);
            var videoProtocol = IntPtrHelper.IntPtrTostring(videoProtocolPtr);
            var videoRate = IntPtrHelper.IntPtrTostring(videoRatePtr);
            var videoRateUsed = IntPtrHelper.IntPtrTostring(videoRateUsedPtr);
            var videoFrameRate = IntPtrHelper.IntPtrTostring(videoFrameRatePtr);
            var videoPacketsLost = IntPtrHelper.IntPtrTostring(videoPacketsLostPtr);
            var videoJitter = IntPtrHelper.IntPtrTostring(videoJitterPtr);
            var videoFormat = IntPtrHelper.IntPtrTostring(videoFormatPtr);
            var errorConcealment = IntPtrHelper.IntPtrTostring(errorConcealmentPtr);
            var audioProtocol = IntPtrHelper.IntPtrTostring(audioProtocolPtr);
            var audioRate = IntPtrHelper.IntPtrTostring(audioRatePtr);
            var audioPacketsLost = IntPtrHelper.IntPtrTostring(audioPacketsLostPtr);
            var audioJitter = IntPtrHelper.IntPtrTostring(audioJitterPtr);
            var audioEncrypt = IntPtrHelper.IntPtrTostring(audioEncryptPtr);
            var videoEncrypt = IntPtrHelper.IntPtrTostring(videoEncryptPtr);
            var feccEncrypt = IntPtrHelper.IntPtrTostring(feccEncryptPtr);
            var audioReceivedPacket = IntPtrHelper.IntPtrTostring(audioReceivedPacketPtr);
            var roundTripTime = IntPtrHelper.IntPtrTostring(roundTripTimePtr);
            var fullIntraFrameRequest = IntPtrHelper.IntPtrTostring(fullIntraFrameRequestPtr);
            var intraFrameSent = IntPtrHelper.IntPtrTostring(intraFrameSentPtr);
            var packetsCount = IntPtrHelper.IntPtrTostring(packetsCountPtr);
            var overallCPULoad = IntPtrHelper.IntPtrTostring(overallCPULoadPtr);
            int channelNo = 0;
            if (int.TryParse(IntPtrHelper.IntPtrTostring(channelNumPtr), out channelNo))
            {
                var statistics = new QLMediaStatistics()
                {
                    ChannelName = channelName== "(null)"?string.Empty:channelName,
                    ParticipantName = participantName == "(null)" ? string.Empty : participantName,
                    RemoteSystemId = remoteSystemId == "(null)" ? string.Empty : remoteSystemId,
                    CallRate = callRate == "(null)" ? string.Empty : callRate,
                    PacketsLost = packetsLost == "(null)" ? string.Empty : packetsLost,
                    PacketLoss = packetLoss == "(null)" ? string.Empty : packetLoss,
                    VideoProtocol = videoProtocol == "(null)" ? string.Empty : videoProtocol,
                    VideoRate = videoRate == "(null)" ? string.Empty : videoRate,
                    VideoRateUsed = videoRateUsed == "(null)" ? string.Empty : videoRateUsed,
                    VideoFrameRate = videoFrameRate == "(null)" ? string.Empty : videoFrameRate,
                    VideoPacketsLost = videoPacketsLost == "(null)" ? string.Empty : videoPacketsLost,
                    VideoJitter = videoJitter == "(null)" ? string.Empty : videoJitter,
                    VideoFormat = videoFormat == "(null)" ? string.Empty : videoFormat,
                    ErrorConcealment = errorConcealment == "(null)" ? string.Empty : errorConcealment,
                    AudioProtocol = audioProtocol == "(null)" ? string.Empty : audioProtocol,
                    AudioRate = audioRate == "(null)" ? string.Empty : audioRate,
                    AudioPacketsLost = audioPacketsLost == "(null)" ? string.Empty : audioPacketsLost,
                    AudioJitter = audioJitter == "(null)" ? string.Empty : audioJitter,
                    AudioEncrypt = audioEncrypt == "(null)" ? string.Empty : audioEncrypt,
                    VideoEncrypt = videoEncrypt == "(null)" ? string.Empty : videoEncrypt,
                    FeccEncrypt = feccEncrypt == "(null)" ? string.Empty : feccEncrypt,
                    AudioReceivedPacket = audioReceivedPacket == "(null)" ? string.Empty : audioReceivedPacket,
                    RoundTripTime = roundTripTime == "(null)" ? string.Empty : roundTripTime,
                    FullIntraFrameRequest = fullIntraFrameRequest == "(null)" ? string.Empty : fullIntraFrameRequest,
                    IntraFrameSent = intraFrameSent == "(null)" ? string.Empty : intraFrameSent,
                    PacketsCount = packetsCount == "(null)" ? string.Empty : packetsCount,
                    OverallCPULoad = overallCPULoad == "(null)" ? string.Empty : overallCPULoad,
                    ChannelNum = channelNo 
                };
                callView.Invoke(new Action(() =>
                {
                    QLManager.GetInstance().mediaStatistics.Add(statistics);
                }));
            }
        }
        /// <summary>
        /// 呼叫信息回调
        /// </summary>
        private static void DisplayCallStatisticsCallbackF(int timeInLastCall, int totalTime, int callPlaced, int callReceived, int callConnected)
        {
            /*
            CallStatistics callStatistics = new CallStatistics(timeInLastCall, totalTime, callPlaced, callReceived, callConnected);
            callStatisticsDisplay.displayCallStatistics(callStatistics);
            */
        }
        /// <summary>
        /// 编码能力回调
        /// </summary>
        private static void DisplayCodecCapabilitiesF(IntPtr typePtr, IntPtr codecNamePtr)
        {
            /*
            var type = IntPtrHelper.IntPtrTostring(typePtr);
            var codecName = IntPtrHelper.IntPtrTostring(codecNamePtr);
            if (type == "audio")
            {
                codecNameDisplay.displayAudioCodec(codecName);
            }
            else
            {
                codecNameDisplay.displayVideoCodec(codecName);
            }
            */
        }
        #endregion

        /// <summary>
        /// SDK初始化
        /// </summary>
        /// <param name="config">配置信息</param>
        private void SDKInit(IDictionary<PropertyKey, string> config)
        {
            var errno = ErrorNumber.OK;
            //注册回调函数
            errno = PlcmProxy.InstallCallback(addEventCallback, dispatchEventsCallback, addLogCallback, addDeviceCallback,
                    displayMediaStatisticsCallback, displayCallStatisticsCallback, displayCodecCapabilities, addAppCallback);
            if (ErrorNumber.OK != errno)
            {
                var errMsg = "Register callback functions failed. Error number = " + errno;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            //预初始化
            errno = PlcmProxy.PreInitialize();
            if (ErrorNumber.OK != errno)
            {
                var msg = "Pre-initialization failed. Error number = " + errno.ToString();
                log.Error(msg);
                throw new Exception(msg);
            }
            var dispName = config[PropertyKey.PLCM_MFW_KVLIST_KEY_DisplayName];
            if (!string.IsNullOrWhiteSpace(dispName))
            {
                config[PropertyKey.PLCM_MFW_KVLIST_KEY_DisplayName] = string.Empty;
            }
            qlConfig.SetProperties(config);
            //初始化
            errno = PlcmProxy.Initialize();
            if (ErrorNumber.OK != errno)
            {
                var errMsg = "Initialize failed. Error number = " + errno;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            var version = PlcmProxy.GetVersion();
            log.Info("**********************************************************************");
            log.Info("        PLCM QLSDK  App Initialized Successful ( version: " + version + " )");
            log.Info("**********************************************************************");
            
            errno = PlcmProxy.UpdateConfig();
            if (errno != ErrorNumber.OK)
            {
                var errMsg = string.Format("配置失败, Errno={0}", errno);
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            errno = PlcmProxy.RegisterClient();
            if (errno != ErrorNumber.OK)
            {
                var errMsg = string.Format("Register failed, Errno={0}", errno);
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            if(!string.IsNullOrWhiteSpace(dispName))
            {
                qlConfig.SetProperty(PropertyKey.PLCM_MFW_KVLIST_KEY_DisplayName, dispName);
            }
            //获取音频输入设备信息
            var errNo = PlcmProxy.GetDevice(DeviceType.AUDIOINPUT);
            if (ErrorNumber.OK != errNo)
            {
                var errMsg = "Get audio input device failed. Error number = " + errNo;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            //获取视频输入设备信息
            errNo = PlcmProxy.GetDevice(DeviceType.VIDEOINPUT);
            if (ErrorNumber.OK != errNo)
            {
                var errMsg = "Get video input device failed. Error number = " + errNo;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            //获取音频输出设备信息
            errNo = PlcmProxy.GetDevice(DeviceType.AUDIOOUTPUT);
            if (ErrorNumber.OK != errNo)
            {
                var errMsg = "Get audio output device failed. Error number = " + errNo;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            //获取显示器信息
            errNo = PlcmProxy.GetDevice(DeviceType.MONITOR);
            if (ErrorNumber.OK != errNo)
            {
                var errMsg = "Get monitor device failed. Error number = " + errNo;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            //设置呼叫数自动
            errNo = PlcmProxy.SetRemoteVideoStreamNumber(-1, 0, 0);
            if (ErrorNumber.OK != errNo)
            {
                var errMsg = "SetRemoteVideoStreamNumber failed. Error number = " + errNo;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
        }
        #endregion

        /// <summary>
        /// 绑定显示组件
        /// </summary>
        /// <param name="container"></param>
        public void AttachViewContainer(Control container)
        {
            callView.AttachViewContainer(container);
        }
        
        /// <summary>
        /// 获取当前的regId
        /// </summary>
        /// <returns>当前的regId</returns>
        public string GetRegID()
        {
            return qlConfig.GetProperty(PropertyKey.PLCM_MFW_KVLIST_KEY_REG_ID);
        }

        /// <summary>
        /// 注销polc服务
        /// </summary>
        public void Logout()
        {
            PlcmProxy.UnregisterClient();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Release()
        {
            if (IsRegisted)
            {
                PlcmProxy.UnregisterClient();
            }
            callManager.Dispose();
            log.Info("关闭释放资源");
        }

        /// <summary>
        /// 设置网络带宽HcallRate">呼叫速率</param>
        public void SetNetworkCallRate(int callRate)
        {
            qlConfig.SetProperty(PropertyKey.PLCM_MFW_KVLIST_KEY_CallSettings_NetworkCallRate, callRate.ToString());
        }
        

        #region 获取统计信息
        /// <summary>
        /// 获取呼叫信号统计信息
        /// </summary>
        /// <param name="callBack">获取结果回调函数</param>
        public void GetMediaStatistics(Action<ObservableCollection<QLMediaStatistics>> callBack)
        {
            mediaStatistics.Clear();
            if (null != callManager.CurrentCall)
            {
                mediaStatisticsCallBack = callBack;
                var errno = PlcmProxy.GetMediaStatistics(callManager.CurrentCall.CallHandle);
                if (ErrorNumber.OK != errno)
                {
                    throw new Exception("获取信号流信息失败,errno=" + errno);
                }
            }
            else
            {
                throw new Exception("当前呼叫为空，获取信号流信息");
            }
        }
        /// <summary>
        /// 获取应该程序列表
        /// </summary>
        /// <param name="callback">获取结果回调函数</param>
        public void GetApps(Action<ObservableCollection<QLApp>> callback)
        {
            apps.Clear();

            appCallBack = callback;
            var errno = PlcmProxy.GetApplicationInfo();
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("获取信号流信息失败,errno=" + errno);
            }
        }
        #endregion

        #region ViewLayout
        /// <summary>
        /// 设置布局
        /// </summary>
        /// <param name="layout">布局类型</param>
        public void SetLayout(LayoutType layout)
        {
            qlConfig.SetProperty(PropertyKey.LayoutType, layout.ToString());
            callView.ViewRender();
        }
        #endregion

   }
}
