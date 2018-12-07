using log4net;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace QLSDK.Core
{
    public class QLManager
    {
        #region Fields
        private static ILog log = LogUtil.GetLoger("QLSDK.QLManager");
        #endregion

        #region Constructors        
        private static readonly object lockObj = new object();
        private static QLManager instance = null;
        private QLManager()
        {
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
        #endregion

        #region 是否注册成功
        private bool isRegisted = false;
        /// <summary>
        /// 是否注册成功
        /// </summary>
        public bool IsRegisted { get { return isRegisted; } }

        private void ValideRegisted()
        {
            if(!isRegisted)
            {
                throw new Exception("未注册成功，请进行注册！");
            }
        }
        #endregion

        /// <summary>
        /// polc sdk 初始化
        /// </summary>
        /// <param name="server">服务器验证地址</param>
        /// <param name="password">注册用户名</param>
        /// <param name="password">注册用户密码</param>
        /// <param name="container">显示组件的容器控件</param>
        /// <param name="qlConfig">初始化配置  获取默认配置：QlConfig.GetDefaultConfig()</param>
        public void Initialize(string server, string username, string password, Control container, IDictionary<PropertyKey, string> qlConfig = null)
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
            if (null == container)
            {
                throw new Exception("显示承载容器必须");
            }
            #endregion
            
            var postData = new Dictionary<string, string>() {
                { "secret_key",username },
                { "package_name",password}
            };
            var postResult= HttpUtil<AuthorizeData>.Post(server, postData);  //发送验证数据
            if(postResult.success)
            {
                #region 设置配置信息
                IDictionary<PropertyKey, string> ps = qlConfig;
                if(null == qlConfig)
                {
                    ps = QlConfig.GetInstance().GetDefaultConfig();
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
                SDKInit(ps);
            }
            else
            {
                throw new Exception("登录验证失败,"+postResult.msg);
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
        
        public static void StartEventMonitor()
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
                            //mainThreadSynContext.Post(new SendOrPostCallback(DoEvent), evt);
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
        public static void StopEventMonitor()
        {
            isRunning = false;
            DispatchEvents();
        }
        public static void DispatchEvents()
        {
            //lock (synObject)
            {
                log.Info("notify evt monitor to proceed the events");
                autoEvent.Set();
            }
        }
        public static void AddEvent(QLEvent evt)
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
                #region Device
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
                            var device = new Device(DeviceType.VIDEOINPUT, deviceHandle, deviceName);
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
                            var device = new Device(DeviceType.AUDIOINPUT, deviceHandle, deviceName);
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
                            var device = new Device(DeviceType.AUDIOOUTPUT, deviceHandle, deviceName);
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
                            var device = new Device(DeviceType.MONITOR, deviceHandle, deviceName);
                            deviceManager.AddDevice(device);
                        }
                        else
                        {
                            deviceManager.RemoveDevice(deviceHandle);
                        }
                    }
                    break;  /* from MP */
                #endregion

                #region 不处理
                #region Register
                case EventType.UNKNOWN: break;
                case EventType.SIP_REGISTER_SUCCESS:
                    {
                        RegisterAction?.Invoke();
                    }
                    break;
                case EventType.SIP_REGISTER_FAILURE:
                    {
                        UnregisterAction?.Invoke();
                    }
                    break;
                case EventType.SIP_REGISTER_UNREGISTERED: break;
                    #endregion
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

            InternalMFWEvent?.Invoke(evt);
            MFWEvent?.Invoke(evt);
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
            var device = new QLDevice(DeviceType.APPLICATIONS, appHandle.ToString(), appName);
            deviceManager.AddDevice(device);
        }


        private static void AddDeviceCallbackF(int deviceType, IntPtr deviceHandlePtr, IntPtr deviceNamePtr)
        {
            if (deviceType < (int)DeviceType.APPLICATIONS)
            {
                var deviceHandle = IntPtrHelper.IntPtrToUTF8string(deviceHandlePtr);
                var deviceName = IntPtrHelper.IntPtrToUTF8string(deviceNamePtr);
                log.Info("AddDeviceCallback: deviceType:" + deviceType + "  deviceHandle:" + deviceHandle + "  deviceName:" + deviceName);
                var device = new Device((DeviceType)deviceType, deviceHandle, deviceName);
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
                var statistics = new MediaStatistics()
                {
                    ChannelName = channelName,
                    ParticipantName = participantName,
                    RemoteSystemId = remoteSystemId,
                    CallRate = callRate,
                    PacketsLost = packetsLost,
                    PacketLoss = packetLoss,
                    VideoProtocol = videoProtocol,
                    VideoRate = videoRate,
                    VideoRateUsed = videoRateUsed,
                    VideoFrameRate = videoFrameRate,
                    VideoPacketsLost = videoPacketsLost,
                    VideoJitter = videoJitter,
                    VideoFormat = videoFormat,
                    ErrorConcealment = errorConcealment,
                    AudioProtocol = audioProtocol,
                    AudioRate = audioRate,
                    AudioPacketsLost = audioPacketsLost,
                    AudioJitter = audioJitter,
                    AudioEncrypt = audioEncrypt,
                    VideoEncrypt = videoEncrypt,
                    FeccEncrypt = feccEncrypt,
                    AudioReceivedPacket = audioReceivedPacket,
                    RoundTripTime = roundTripTime,
                    FullIntraFrameRequest = fullIntraFrameRequest,
                    IntraFrameSent = intraFrameSent,
                    PacketsCount = packetsCount,
                    OverallCPULoad = overallCPULoad,
                    ChannelNum = channelNo
                };
                callView.Invoke(new Action(() =>
                {
                    MFWCore.mediaStatistics.Add(statistics);
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

        private void SDKInit(IDictionary<PropertyKey,string> config)
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
            QlConfig.GetInstance().SetProperties(config);
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
            log.Info("        PLCM MFW  App Initialized Successful ( version: " + version + " )");
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

            //Get Devices
            var errNo = PlcmProxy.GetDevice(DeviceType.AUDIOINPUT);
            if (ErrorNumber.OK != errNo)
            {
                var errMsg = "Get audio input device failed. Error number = " + errNo;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            errNo = PlcmProxy.GetDevice(DeviceType.VIDEOINPUT);
            if (ErrorNumber.OK != errNo)
            {
                var errMsg = "Get video input device failed. Error number = " + errNo;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            errNo = PlcmProxy.GetDevice(DeviceType.AUDIOOUTPUT);
            if (ErrorNumber.OK != errNo)
            {
                var errMsg = "Get audio output device failed. Error number = " + errNo;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            errNo = PlcmProxy.GetDevice(DeviceType.MONITOR);
            if (ErrorNumber.OK != errNo)
            {
                var errMsg = "Get monitor device failed. Error number = " + errNo;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
        }
        #endregion

        /// <summary>
        /// 获取plcm核心句柄
        /// </summary>
        /// <returns></returns>
        //public intPtr GetCoreHandle();

        /// <summary>
        /// 登录绑定polc服务
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="displayName">显示名</param>
        //public void Login(string userName, string passWord, string displayName) { }

        /// <summary>
        /// 获取当前的regId
        /// </summary>
        /// <returns>当前的regId</returns>
        public string GetRegId()
        {
            return QlConfig.GetInstance().GetProperty(PropertyKey.PLCM_MFW_KVLIST_KEY_REG_ID);
        }

        /// <summary>
        /// 注销polc服务
        /// </summary>
        /// <param name="regId">polm注册的regId</param>
        /// <param name="tag"> TAG标记</param>
        public void Logout(string regId, string tag)
        {

        }

        /// <summary>
        /// 呼叫
        /// </summary>
        /// <param name="protocal">呼叫协议  默认； sip</param>
        /// <param name="dialUri">被呼叫方Uri</param>
        /// <param name="callType">呼叫模式 默认：VIDEO</param>
        /// <returns>呼叫处理器ID</returns>
        public QLCall Call(Protocal protocal, string dialUri, CallMode callMode = CallMode.VIDEO)
        {
            return null;
        }

        /// <summary>
        /// 开启铃声播放
        /// </summary>
        /// <param name="assetPath">铃声文件路径</param>
        /// <param name="isLoop">是否循环</param>
        /// <param name="interval">事件间隔</param>
        public void StartAlert(string assetPath, bool isLoop, int interval)
        {

        }

        /// <summary>
        /// 关闭铃声播放
        /// </summary>
        public void StopAlert()
        {

        }
        /// <summary>
        /// 挂断呼叫
        /// </summary>
        /// <param name="call">呼叫处理器</param>
        public void EndCall(QLCall call)
        {

        }
        /// <summary>
        /// 保持呼叫
        /// </summary>
        /// <param name="call">呼叫处理器ID</param>
        public void HoldCall(QLCall call)
        {

        }

        /// <summary>
        /// 接听/应答呼叫
        /// </summary>
        /// <param name="call">呼叫处理器</param>
        /// <param name="callMode">响应呼叫类型  默认 VIDEO</param>
        public void AnswerCall(QLCall call, CallMode callMode)
        {

        }

        /// <summary>
        /// 设置扬声器静音
        /// </summary>
        /// <param name="isMute">是否静音</param>
        public void MuteSpeaker(bool isMute)
        {

        }

        /// <summary>
        /// 切换音频视频模式
        /// </summary>
        /// <param name="call">呼叫处理器ID</param>
        /// <param name="callMode">呼叫类型</param>
        public void ChangeCallType(QLCall call, CallMode callMode)
        {

        }
        /// <summary>
        /// 切换摄像头
        /// </summary>
        //public void SwitchCamera() { }

        /// <summary>
        /// 修改扬声器的音量
        /// </summary>
        /// <param name="volume">音量</param>
        public void AdjustSpeakerVolume(int volume)
        {

        }

        /// <summary>
        /// 开启发送content
        /// </summary>
        /// <param name="call">呼叫处理器ID</param>
        /// <param name="bufType">BufType类型</param>
        //public void StartSendContent(QLCall call, BufType bufType){ }

        /// <summary>
        /// 停止发送content
        /// </summary>
        /// <param name="call">呼叫处理器</param>
        public void StopSendContent(QLCall call)
        {

        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Release()
        {

        }
        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="onQlEventListener">事件监听器</param>
        public void AddQlEventListener(OnQlEventListener onQlEventListener)
        {

        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="onQlEventListener">事件监听器</param>
        public void RemoveQlEventListener(OnQlEventListener onQlEventListener)
        {

        }

        /// <summary>
        /// 设置网络带宽HcallRate">呼叫速率</param>
        public void SetNetworkCallRate(int callRate)
        {

        }
    }
}
