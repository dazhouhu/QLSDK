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
    public class QLManager
    {
        #region Fields
        private static ILog log = LogUtil.GetLogger("QLSDK.QLManager");
        private static Action<ObservableCollection<QLMediaStatistics>> mediaStatisticsCallBack;
        private static Action<ObservableCollection<QLDevice>> appCallBack;
        private ObservableCollection<QLMediaStatistics> mediaStatistics = new ObservableCollection<QLMediaStatistics>();
        private ObservableCollection<QLDevice> apps = new ObservableCollection<QLDevice>();
        private static QLConfig qlConfig = QLConfig.GetInstance();
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
        /// 析构释放资源
        /// </summary>
        ~QLManager()
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
        public event QLEventHandle QLEvent;
        internal static event QLEventHandle InternalQLEvent;
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
            /*
            var postResult = new HttpResullt<AuthorizeData>()
            {
                code = "200",
                data = new AuthorizeData()
                {
                    sip_addr = "58.218.201.171",
                    account = "polycomtest4@ch",
                    pass = "123456789",
                    log_level = "Debug"
                }
            };
            */
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
                    ps[PropertyKey.PLCM_MFW_KVLIST_KEY_REG_ID] = postResult.data.account + "@" + postResult.data.sip_addr;
                }
                if (string.IsNullOrEmpty(ps[PropertyKey.PLCM_MFW_KVLIST_KEY_REG_ID]))
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
            QLManager.InternalQLEvent?.Invoke(evt);
            QLManager.GetInstance().QLEvent?.Invoke(evt);
        }
        #endregion
        
        private static void AddLogCallbackF(ulong timestamp, bool expired, int funclevel, ulong pid, ulong tid, IntPtr lev, IntPtr comp, IntPtr msg, int len)
        {
            var output = string.Empty;
            var level = IntPtrHelper.IntPtrToUTF8string(lev);
            var component = IntPtrHelper.IntPtrTostring(comp);
            var message = IntPtrHelper.IntPtrTostring(msg);
            if (string.IsNullOrEmpty(component))
            {
                component = "wrapper";
            }

            output += string.Format("[PID:{0}][TID:{1}] ", pid, tid); ;

            for (int i = 0; i < funclevel; i++)
            {
                output += "  ";
            }
            output += level.ToString();

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

        private static void AddAppCallbackF(IntPtr appHandle, IntPtr appNamePtr)
        {
            var appName = IntPtrHelper.IntPtrTostring(appNamePtr);
            log.Info("AddAppCallbackF: appHandle:" + appHandle + "  appName:" + appName);
            var app = new QLDevice(DeviceType.APPLICATIONS, appHandle.ToString(), appName);
            callView.Invoke(new Action(() =>
            {
                QLManager.GetInstance().apps.Add(app);
            }));
        }


        private static void AddDeviceCallbackF(int deviceType, IntPtr deviceHandlePtr, IntPtr deviceNamePtr)
        {
            if (deviceType < (int)DeviceType.APPLICATIONS)
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
        private static void DisplayCallStatisticsCallbackF(int timeInLastCall, int totalTime, int callPlaced, int callReceived, int callConnected)
        {
            /*
            CallStatistics callStatistics = new CallStatistics(timeInLastCall, totalTime, callPlaced, callReceived, callConnected);
            callStatisticsDisplay.displayCallStatistics(callStatistics);
            */
        }

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
            #region Default Properties
            var defaultProperties = qlConfig.GetDefaultConfig();

            #endregion

            qlConfig.SetProperties(defaultProperties);
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

            PlcmProxy.SetProperty(PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_ProxyServer, "58.218.201.171");
            PlcmProxy.SetProperty(PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_UserName, "polycomtest4@ch");
            PlcmProxy.SetProperty(PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_UserName, "123456789");

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
        #region 不提供的接口
        /// <summary>
        /// 获取plcm核心句柄
        /// </summary>
        /// <returns></returns>
        //public IntPtr GetCoreHandle();

        /// <summary>
        /// 登录绑定polc服务
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="displayName">显示名</param>
        //public void Login(string userName, string passWord, string displayName) { }

        /// <summary>
        /// 切换摄像头
        /// </summary>
        //public void SwitchCamera() { }
        #endregion

        /// <summary>
        /// 获取当前的regId
        /// </summary>
        /// <returns>当前的regId</returns>
        public string GetRegId()
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
        /// 呼叫
        /// </summary>
        /// <param name="dialUri">被呼叫方Uri</param>
        /// <param name="callType">呼叫模式 默认：VIDEO</param>
        /// <returns>呼叫处理器ID</returns>
        public QLCall DialCall(string dialUri, CallMode callMode = CallMode.VIDEO)
        {
            int callHandle = -1;
            qlConfig.SetProperty(PropertyKey.CalleeAddr, dialUri);
            var errno= PlcmProxy.PlaceCall(dialUri, ref callHandle, callMode);
            if(ErrorNumber.OK== errno)
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
                callManager.AddCall(call);
                callManager.CurrentCall = call;
                return call;
            }
            else
            {
                throw new Exception("呼叫失败，ErrorNo="+errno);
            }
        }

        /// <summary>
        /// 开启铃声播放
        /// </summary>
        /// <param name="assetPath">铃声文件路径</param>
        /// <param name="isLoop">是否循环</param>
        /// <param name="interval">事件间隔</param>
        public void StartAlert(string assetPath, bool isLoop, int interval)
        {
            deviceManager.PlaySound(assetPath, isLoop, interval);
        }

        /// <summary>
        /// 关闭铃声播放
        /// </summary>
        public void StopAlert()
        {
            deviceManager.StopSound();
        }
        /// <summary>
        /// 挂断呼叫
        /// </summary>
        /// <param name="call">呼叫处理器,为空时为当前呼叫</param>
        public void EndCall(QLCall call=null)
        {
            if (null == call) call = GetCurrentCall();
            if (null == call) return;  //throw new Exception("呼叫不存在，不能挂断");
            var errno = PlcmProxy.TerminateCall(call.CallHandle);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("挂断呼叫失败，ErrorNo=" + errno);
            }
        }
        /// <summary>
        /// 保持呼叫
        /// </summary>
        /// <param name="call">呼叫处理器,为空时为当前呼叫</param>
        public void HoldCall(QLCall call = null)
        {
            if (null == call) call = GetCurrentCall();
            if (null == call) throw new Exception("呼叫不存在，不能保持呼叫");
            var errno = PlcmProxy.HoldCall(call.CallHandle);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("保持呼叫失败，ErrorNo=" + errno);
            }
        }
        /// <summary>
        /// 恢复保持的呼叫
        /// </summary>
        /// <param name="call">呼叫处理器,为空时为当前呼叫</param>
        public void ResumeCall(QLCall call = null)
        {
            if (null == call) call = GetCurrentCall();
            if (null == call) throw new Exception("呼叫不存在，不能恢复");
            var errno = PlcmProxy.ResumeCall(call.CallHandle);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("恢复呼叫失败，ErrorNo=" + errno);
            }
        }

        /// <summary>
        /// 接听/应答呼叫
        /// </summary>
        /// <param name="call">呼叫处理器,为空时为当前呼叫</param>
        /// <param name="callMode">响应呼叫类型  默认 VIDEO</param>
        public void AnswerCall(QLCall call = null, CallMode callMode=CallMode.VIDEO)
        {
            if (null == call) call = GetCurrentCall();
            if (null == call) throw new Exception("呼叫不存在，不能接听");
            var errno=PlcmProxy.AnswerCall(call, callMode);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("接听应答呼叫失败，ErrorNo=" + errno);
            }
        }

        /// <summary>
        /// 切换音频视频模式
        /// </summary>
        /// <param name="call">呼叫处理器,为空时为当前呼叫</param>
        /// <param name="callMode">呼叫类型,默认为Video</param>
        public void ChangeCallType(QLCall call=null, CallMode callMode=CallMode.VIDEO)
        {
            if (null == call) call = GetCurrentCall();
            if (null == call) throw new Exception("呼叫不存在，不能切换音频视频模式"); 
            var errno = PlcmProxy.ChangeCallMode(call.CallHandle, callMode);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("切换音频视频模式失败，ErrorNo=" + errno);
            }
        }

        /// <summary>
        /// 修改扬声器的音量
        /// </summary>
        /// <param name="volume">音量</param>
        public void AdjustSpeakerVolume(int volume)
        {
            var errno = PlcmProxy.SetSpeakerVolume(volume);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("修改扬声器的音量失败，ErrorNo=" + errno);
            }
        }

        /// <summary>
        /// 开启发送content
        /// </summary>
        /// <param name="call">呼叫处理器,为空时为当前呼叫</param>
        /// <param name="bufType">BufType类型</param>
        public void StartSendContent(QLCall call, string monitorHandle, string appHandle)
        {
            if (null == call) call = GetCurrentCall();
            if (null == call) throw new Exception("呼叫不存在，不能内容共享");
            var errno = PlcmProxy.StartShareContent(call.CallHandle, monitorHandle,new IntPtr(int.Parse(appHandle)));
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("内容共享失败，ErrorNo=" + errno);
            }
        }

        /// <summary>
        /// 停止发送content
        /// </summary>
        /// <param name="call">呼叫处理器,为空时为当前呼叫</param>
        public void StopSendContent(QLCall call=null)
        {
            if (null == call) call = GetCurrentCall();
            if (null == call) return; //throw new Exception("呼叫不存在，不能停止内容共享");
            var errno = PlcmProxy.StopShareContent(call.CallHandle);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("停止内容共享失败，ErrorNo=" + errno);
            }
        }

        /*
        public void StartShareContent(string deviceHandle, IntPtr appWndHandle)
        {
            if (null != callManager.CurrentCall)
            {
                var errno = PlcmProxy.StartShareContent(callManager.CurrentCall.CallHandle, deviceHandle, appWndHandle);
                if (ErrorNumber.OK != errno)
                {
                    throw new Exception("开始共享内容失败,errno=" + errno);
                }
            }
            else
            {
                throw new Exception("当前呼叫为空，不能共享内容");
            }
        }
        public void StartBFCPContent()
        {
            if (null != callManager.CurrentCall)
            {
                var errno = PlcmProxy.StartBFCPContent(callManager.CurrentCall.CallHandle);
                if (ErrorNumber.OK != errno)
                {
                    throw new Exception("开始共享内容失败,errno=" + errno);
                }
            }
            else
            {
                throw new Exception("当前呼叫为空，不能共享内容");
            }
        }

        public void StopShareContent()
        {
            if (null != callManager.CurrentCall)
            {
                var errno = PlcmProxy.StopShareContent(callManager.CurrentCall.CallHandle);
                if (ErrorNumber.OK != errno)
                {
                    throw new Exception("结束共享内容失败,errno=" + errno);
                }
            }
            else
            {
                throw new Exception("当前呼叫为空，不能结束共享内容");
            }
        }

        public void SetContentBuffer(ImageFormat format, int width, int height)
        {
            var errno = PlcmProxy.SetContentBuffer(format, width, height);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("开始共享内容失败,errno=" + errno);
            }
        }
        */
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Release()
        {
            PlcmProxy.UnregisterClient();
        }

        /// <summary>
        /// 设置网络带宽HcallRate">呼叫速率</param>
        public void SetNetworkCallRate(int callRate)
        {
            qlConfig.SetProperty(PropertyKey.PLCM_MFW_KVLIST_KEY_CallSettings_NetworkCallRate, callRate.ToString());
        }


        #region Device Setting
        public void MuteLocalVideo(bool isMute)
        {
            var errno = PlcmProxy.MuteLocalVideo(isMute);
            if (errno != ErrorNumber.OK)
            {
                log.Error("mute local video failed. ErrorNum = " + errno);
            }
        }
        public void MuteMic(bool isMute)
        {
            if (null != callManager.CurrentCall)
            {
                var errno = PlcmProxy.MuteMic(callManager.CurrentCall.CallHandle, isMute);
                if (ErrorNumber.OK != errno)
                {
                    throw new Exception("麦克风静音设置失败,errno=" + errno);
                }
            }
            else
            {
                throw new Exception("当前呼叫为空，不能进行麦克风静音设置");
            }
        }

        public void MuteSpeaker(bool isMute)
        {
            var errno = PlcmProxy.MuteSpeaker(isMute);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("扬声器静音设置失败,errno=" + errno);
            }
        }

        public void SetMicVolume(int volume)
        {
            var errno = PlcmProxy.SetMicVolume(volume);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("麦克风音量设置失败,errno=" + errno);
            }
        }

        public int GetMicVolume()
        {
            return PlcmProxy.GetMicVolume();
        }

        public void SetSpeakerVolume(int volume)
        {
            var errno = PlcmProxy.SetSpeakerVolume(volume);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("扬声器音量设置失败,errno=" + errno);
            }
        }

        public int GetSpeakerVolume()
        {
            return PlcmProxy.GetSpeakerVolume();
        }
        public void StartCamera()
        {
            var errno = PlcmProxy.StartCamera();
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("开启摄像头失败,errno=" + errno);
            }
        }

        public void StopCamera()
        {
            var errno = PlcmProxy.StopCamera();
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("关闭摄像头失败,errno=" + errno);
            }
        }

        #endregion
        #region 获取统计信息
        public void GetMediaStatistics(Action<ObservableCollection<QLMediaStatistics>> callBack)
        {
            mediaStatistics.Clear();
            if (null !=callManager.CurrentCall)
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
        public void GetApps(Action<ObservableCollection<QLDevice>> callback)
        {
            apps.Clear();
            if (null != callManager.CurrentCall)
            {
                appCallBack = callback;
                var errno = PlcmProxy.GetApplicationInfo();
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
        #endregion

        #region ViewLayout
        public void SetLayout(LayoutType layout)
        {
            qlConfig.SetProperty(PropertyKey.LayoutType, layout.ToString());
            callView.ViewRender();
        }
        #endregion

        #region Call
        /// <summary>
        /// 获取当前呼叫
        /// </summary>
        /// <returns></returns>
        public QLCall GetCurrentCall()
        {
            return callManager.CurrentCall;
        }

        /// <summary>
        /// 发送DTMF key
        /// </summary>
        /// <param name="call">呼叫，为空时默认为当前呼叫</param>
        /// <param name="key">DTMFkey</param>
        public void SendDTMFKey(QLCall call,DTMFKey key)
        {
            if (null == call) call = GetCurrentCall();
            if (null == call) throw new Exception("呼叫不存在，不能发送DTMF Key");

            var errno = PlcmProxy.SendDTMFKey(call.CallHandle, key);
            if (errno != ErrorNumber.OK)
            {
                var errMsg = "发送DTMF Key失败，errno=" + errno;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            log.Info(string.Format("发送DTMF Key({0})成功.",key));
        }

        /// <summary>
        /// 发送FECC
        /// </summary>
        /// <param name="call">呼叫，为空时默认为当前呼叫<</param>
        /// <param name="key">FECC Key</param>
        /// <param name="action">FECC Action</param>
        public void SendFECC(QLCall call,FECCKey key,FECCAction action)
        {
            if (null == call) call = GetCurrentCall();
            if (null == call) throw new Exception("呼叫不存在，不能发送FECC");

            var errno = PlcmProxy.SendFECCKey(call.CallHandle, key,action);
            if (errno != ErrorNumber.OK)
            {
                var errMsg = "发送FECC失败，errno=" + errno;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            log.Info(string.Format("发送FECC({0})成功.", key));
        }
        #endregion
    }
}
