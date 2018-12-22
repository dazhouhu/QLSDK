
using System;

namespace QLSDK.Core
{
    public class QLEvent
    {
        #region Fields
        private IntPtr _eventHandle = IntPtr.Zero;
        private int _callHandle = -1;
        private QLCall _call;
        private EventType _eventType = EventType.UNKNOWN;
        private string _callerName;
        private string _calleeName;
        private int _userCode = -1;
        private string _reason;
        private int _wndWidth = 4;
        private int _wndHeight = 3; /*the default aspect ratio is 4:3*/
        private bool _plugDeviceStatus;
        private string _plugDeviceName;
        private string _deviceHandle;
        private string _ipAddress;
        private CallMode _callMode;

        private int _streamId;
        private int _activeSpeakerStreamId;
        private int _remoteVideoChannelNum;
        private string _remoteChannelDisplayName;
        private bool _isActiveSpeaker;
        private string _placeId;
        private int _isTalkingFlag;
        private string _regID;
        private string _sipCallId;

        private string _version;
        private string _serialNumber;
        private string _notBefore;
        private string _notAfter;
        private string _issuer;
        private string _subject;
        private string _signatureAlgorithm;
        private string _fingerPrintAlgorithm;
        private string _fingerPrint;
        private string _publickey;
        private string _basicContraints;
        private string _keyUsage;
        private string _extendedKeyUsage;
        private string _subjectAlternateNames;
        private string _pemCert;
        private bool _isCertHostNameOK;
        private int _certFailReason;
        private int _certConfirmId;
        private IntPtr _transcoderTaskId;
        private string _transcoderInputFileName;
        private ICEStatus _iceStatus;
        private string _sutLiteMessage;
        private bool _isVideoOK;
        private string _mediaIPAddr;
        private AutoDiscoveryStatus _discoveryStatus;
        #endregion
        #region Constructors
        internal QLEvent(IntPtr eventHandle, int callHandle, string placeId, EventType type, string callerName,
                string calleeName, int userCode, string reason,
                int wndWidth, int wndHeight,
                bool plugDeviceStatus, string plugDeviceName, string deviceHandle, string ipAddr, CallMode callMode,
                int streamId, int activeSpeakerStreamId, int remoteVideoChannelNum, string remoteChannelDisplayName, bool isActiveSpeaker, int isTalkingFlag, string regID,
                string sipCallId, string version, string serialNumber, string notBefore, string notAfter,
                string issuer, string subject, string signatureAlgorithm, string fingerPrintAlgorithm, string fingerPrint, string publickey, string basicContraints, string keyUsage,
                string extendedKeyUsage, string subjectAlternateNames, string pemCert, bool isCertHostNameOK, int certFailReason,
                int certConfirmId, IntPtr transcoderTaskId, string transcoderInputFileName, ICEStatus iceStatus, string sutLiteMessage, bool isVideoOK, string mediaIPAddr, AutoDiscoveryStatus discoveryStatus)
        {
            this._eventHandle = eventHandle;
            this._callHandle = callHandle;

            this._placeId = placeId;
            this._eventType = type;
            this._callerName = callerName;
            this._calleeName = calleeName;
            this._userCode = userCode;
            this._reason = reason;
            this._wndWidth = wndWidth;
            this._wndHeight = wndHeight;
            this._plugDeviceStatus = plugDeviceStatus;
            this._plugDeviceName = plugDeviceName;
            this._deviceHandle = deviceHandle;
            this._ipAddress = ipAddr;
            this._callMode = callMode;

            this._streamId = streamId;
            this._activeSpeakerStreamId = activeSpeakerStreamId;
            this._remoteVideoChannelNum = remoteVideoChannelNum;
            this._remoteChannelDisplayName = remoteChannelDisplayName;
            this._isActiveSpeaker = isActiveSpeaker;
            this._isTalkingFlag = isTalkingFlag;
            this._regID = regID;
            this._sipCallId = sipCallId;
            this._version = version;
            this._serialNumber = serialNumber;
            this._notBefore = notBefore;
            this._notAfter = notAfter;
            this._issuer = issuer;
            this._subject = subject;
            this._signatureAlgorithm = signatureAlgorithm;
            this._fingerPrintAlgorithm = fingerPrintAlgorithm;
            this._fingerPrint = fingerPrint;
            this._publickey = publickey;
            this._basicContraints = basicContraints;
            this._keyUsage = keyUsage;
            this._extendedKeyUsage = extendedKeyUsage;
            this._subjectAlternateNames = subjectAlternateNames;
            this._pemCert = pemCert;
            this._isCertHostNameOK = isCertHostNameOK;
            this._certFailReason = certFailReason;
            this._certConfirmId = certConfirmId;
            this._transcoderTaskId = transcoderTaskId;
            this._transcoderInputFileName = transcoderInputFileName;
            this._iceStatus = iceStatus;
            this._sutLiteMessage = sutLiteMessage;
            this._isVideoOK = isVideoOK;
            this._mediaIPAddr = mediaIPAddr;
            this._discoveryStatus = discoveryStatus;
        }
        #endregion
        
        /// <summary>
        /// 事件类型
        /// </summary>
        public EventType EventType { get { return this._eventType; } }
        /// <summary>
        /// 事件ID
        /// </summary>
        public IntPtr EventHandle { get { return this._eventHandle; } }
        /// <summary>
        /// 呼叫ID
        /// </summary>
        public int CallHandle { get { return this._callHandle; } }
        /// <summary>
        /// 呼叫
        /// </summary>
        public QLCall Call { get { return this._call; } set { this._call = value; } }
        /// <summary>
        /// 呼叫者
        /// </summary>
        public string CallerName { get { return this._callerName; } }
        /// <summary>
        /// 被呼叫者
        /// </summary>
        public string CalleeName { get { return this._calleeName; } }
        /// <summary>
        /// 窗口宽
        /// </summary>
        public int WndWidth { get { return this._wndWidth; } }
        /// <summary>
        /// 窗口高
        /// </summary>
        public int WndHeight { get { return this._wndHeight; } }
        /// <summary>
        /// 插拔设备状态
        /// </summary>
        public bool PlugDeviceStatus { get { return this._plugDeviceStatus; } }
        /// <summary>
        /// 插拔设备名
        /// </summary>
        public string PlugDeviceName { get { return this._plugDeviceName; } }
        /// <summary>
        /// 插拔设备ID
        /// </summary>
        public string DeviceHandle { get { return this._deviceHandle; } }
        /// <summary>
        /// 呼叫结束原因
        /// </summary>
        public string Reason { get { return this._reason; } }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get { return this._ipAddress; } }
        /// <summary>
        /// 呼叫模式
        /// </summary>
        public CallMode CallMode { get { return this._callMode; } }
        /// <summary>
        /// 流ID
        /// </summary>
        public int StreamId { get { return this._streamId; } }
        /// <summary>
        /// 当前讲话流ID
        /// </summary>
        public int ActiveSpeakerStreamId { get { return this._activeSpeakerStreamId; } }
        /// <summary>
        /// 远端视频通道数
        /// </summary>
        public int RemoteVideoChannelNum { get { return this._remoteVideoChannelNum; } }
        /// <summary>
        /// 远端通道显示名
        /// </summary>
        public string RemoteChannelDisplayName { get { return this._remoteChannelDisplayName; } }
        /// <summary>
        /// 是否活动讲话
        /// </summary>
        public bool IsActiveSpeaker { get { return this._isActiveSpeaker; } }
        /// <summary>
        /// 呼出ID
        /// </summary>
        public string PlaceId { get { return this._placeId; } }
        /// <summary>
        /// 是否讲话
        /// </summary>
        public int IsTalkingFlag { get { return _isTalkingFlag; } }
        /// <summary>
        /// 注册ID
        /// </summary>
        public string RegID { get { return this._regID; } }
        /// <summary>
        /// SIP呼叫ID
        /// </summary>
        public string SipCallId { get { return this._sipCallId; } }
        /// <summary>
        /// 证书版本
        /// </summary>
        public string Version { get { return this._version; } }
        /// <summary>
        /// 证书序列号
        /// </summary>
        public string SerialNumber { get { return this._serialNumber; } }
        /// <summary>
        /// 证书认证前时间
        /// </summary>
        public string NotBefore { get { return this._notBefore; } }
        /// <summary>
        /// 证书认证后时间
        /// </summary>
        public string NotAfter { get { return this._notAfter; } }
        /// <summary>
        /// 证书发行者
        /// </summary>
        public string Issuer { get { return this._issuer; } }
        /// <summary>
        /// 证书主体
        /// </summary>
        public string Subject { get { return this._subject; } }
        /// <summary>
        /// 算法签名
        /// </summary>
        public string SignatureAlgorithm { get { return this._signatureAlgorithm; } }
        /// <summary>
        /// 指纹算法
        /// </summary>
        public string FingerPrintAlgorithm { get { return this._fingerPrintAlgorithm; } }
        /// <summary>
        /// 指纹
        /// </summary>
        public string FingerPrint { get { return this._fingerPrint; } }
        /// <summary>
        /// 公钥
        /// </summary>
        public string Publickey { get { return this._publickey; } }
        /// <summary>
        /// 基本约束
        /// </summary>
        public string BasicContraints { get { return this._basicContraints; } }
        /// <summary>
        /// 私钥
        /// </summary>
        public string KeyUsage { get { return this._keyUsage; } }
        /// <summary>
        /// 私钥扩展
        /// </summary>
        public string ExtendedKeyUsage { get { return this._extendedKeyUsage; } }
        /// <summary>
        /// 
        /// </summary>
        public string SubjectAlternateNames { get { return this._subjectAlternateNames; } }
        /// <summary>
        /// Pem证书
        /// </summary>
        public string PemCert { get { return this._pemCert; } }
        /// <summary>
        /// 认证主机是否OK
        /// </summary>
        public bool IsCertHostNameOK { get { return this._isCertHostNameOK; } }
        /// <summary>
        /// 认证确认ID
        /// </summary>
        public int CertConfirmId { get { return this._certConfirmId; } }
        /// <summary>
        /// 认证失败原因
        /// </summary>
        public int CertFailReason { get { return this._certFailReason; } }
        /// <summary>
        /// 转码输入文件名
        /// </summary>
        public string TranscoderInputFileName { get { return this._transcoderInputFileName; } }
        /// <summary>
        /// 转码任务ID
        /// </summary>
        public IntPtr TranscoderTaskId { get { return this._transcoderTaskId; } }
        /// <summary>
        /// ICE状态
        /// </summary>
        public ICEStatus ICEStatus { get { return this._iceStatus; } }
        /// <summary>
        /// SUTLite消息
        /// </summary>
        public string SUTLiteMessage { get { return this._sutLiteMessage; } }
        /// <summary>
        /// 
        /// </summary>
        public bool IsVideoOK { get { return this._isVideoOK; } }
        /// <summary>
        /// Media地址
        /// </summary>
        public string MediaIPAddr { get { return this._mediaIPAddr; } }
        /// <summary>
        /// 自动发现状态
        /// </summary>
        public AutoDiscoveryStatus AutoDiscoveryStatus { get { return this._discoveryStatus; } }
    }
}
