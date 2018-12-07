using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSDK.Core
{
    public class QlConfig
    {
        #region Fields
        private IDictionary<PropertyKey, string> properties;
        private ILog log = LogUtil.GetLoger("QLSDK.QlConfig");
        private IDictionary<PropertyKey, string> defaultProperties;
        #endregion

        #region Constructors
        private static readonly object lockObj = new object();
        private static QlConfig instance = null;
        private QlConfig()
        {
            properties = new Dictionary<PropertyKey, string>();
            var defaultProperties = new Dictionary<PropertyKey, string>()
            {
                {PropertyKey.PLCM_MFW_KVLIST_KEY_MINSYS,""},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_ProxyServer,""},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_Transport,"TCP"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_ServerType,"standard"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_Register_Expires_Interval,"300"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_UserName,""},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_Domain,""},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_AuthorizationName,"soaktestuser"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_Password,""},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_CookieHead,""},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_Base_Cred,""},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_AnonymousToken_Cred,"YWxpY2U6c2FtZXRpbWU="},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_SIP_Anonymous_Cred,"anonymous"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_CallSettings_MaxCallNum,"6"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_CallSettings_NetworkCallRate,"384"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_CallSettings_AesEcription,"off"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_CallSettings_DefaultAudioStartPort,"3230"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_CallSettings_DefaultAudioEndPort,"3550"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_CallSettings_DefaultVideoStartPort,"3230"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_CallSettings_DefaultVideoEndPort,"3550"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_CallSettings_SIPClientListeningPort,"5060"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_CallSettings_SIPClientListeningTLSPort,"5061"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_EnableSVC,"true"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_LogLevel,"ERROR"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_User_Agent,"MFW_SDK"},
                {PropertyKey.PLCM_MFW_KVLIST_ICE_UserName,""},
                {PropertyKey.PLCM_MFW_KVLIST_ICE_Password,""},
                {PropertyKey.PLCM_MFW_KVLIST_ICE_TCPServer,"0.0.0.0:3478"},
                {PropertyKey.PLCM_MFW_KVLIST_ICE_UDPServer,"0.0.0.0:3478"},
                {PropertyKey.PLCM_MFW_KVLIST_ICE_TLSServer,"0.0.0.0:3478"},
                {PropertyKey.PLCM_MFW_KVLIST_ICE_Enable,"false"},
                {PropertyKey.PLCM_MFW_KVLIST_ICE_AUTHTOKEN_Enable,"false"},
                {PropertyKey.PLCM_MFW_KVLIST_ICE_INIT_AUTHTOKEN,""},
                {PropertyKey.PLCM_MFW_KVLIST_ICE_RTO,"100"},
                {PropertyKey.PLCM_MFW_KVLIST_ICE_RC,"7"},
                {PropertyKey.PLCM_MFW_KVLIST_ICE_RM,"16"},
                {PropertyKey.PLCM_MFW_KVLIST_QOS_ServiceType,""},
                {PropertyKey.PLCM_MFW_KVLIST_QOS_Audio,""},
                {PropertyKey.PLCM_MFW_KVLIST_QOS_Video,""},
                {PropertyKey.PLCM_MFW_KVLIST_QOS_Fecc,""},
                {PropertyKey.PLCM_MFW_KVLIST_QOS_Enable,"false"},
                {PropertyKey.PLCM_MFW_KVLIST_DBM_Enable,"false"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_REG_ID,""},
                {PropertyKey.PLCM_MFW_KVLIST_LPR_Enable,"true"},
                {PropertyKey.PLCM_MFW_KVLIST_CERT_PATH,"./TLS Certificate/instance0/"},
                {PropertyKey.PLCM_MFW_KVLIST_CERT_CHECKFQDN,"false"},
                {PropertyKey.PLCM_MFW_KVLIST_HttpConnect_Enable,"false"},
                {PropertyKey.PLCM_MFW_KVLIST_SIP_HttpProxyServer,""},
                {PropertyKey.PLCM_MFW_KVLIST_SIP_HttpProxyPort,""},
                {PropertyKey.PLCM_MFW_KVLIST_SIP_HttpProxyUserName,""},
                {PropertyKey.PLCM_MFW_KVLIST_SIP_HttpPassword,""},
                {PropertyKey.PLCM_MFW_KVLIST_ICE_HttpProxyServer,""},
                {PropertyKey.PLCM_MFW_KVLIST_ICE_HttpProxyPort,""},
                {PropertyKey.PLCM_MFW_KVLIST_ICE_HttpProxyUserName,""},
                {PropertyKey.PLCM_MFW_KVLIST_ICE_HttpPassword,""},
                {PropertyKey.PLCM_MFW_KVLIST_MEDIA_HttpProxyServer,""},
                {PropertyKey.PLCM_MFW_KVLIST_MEDIA_HttpProxyPort,"80"},
                {PropertyKey.PLCM_MFW_KVLIST_MEDIA_HttpProxyUserName,""},
                {PropertyKey.PLCM_MFW_KVLIST_MEDIA_HttpPassword,""},
                {PropertyKey.PLCM_MFW_KVLIST_PRODUCT,"PLCM_MFW_IBM"},
                {PropertyKey.PLCM_MFW_KVLIST_AutoZoom_Enable,"false"},
                {PropertyKey.PLCM_MFW_KVLIST_TLSOffLoad_Enable,"false"},
                {PropertyKey.PLCM_MFW_KVLIST_TLSOffLoad_Host,""},
                {PropertyKey.PLCM_MFW_KVLIST_TLSOffLoad_Port,"0"},
                {PropertyKey.PLCM_MFW_KVLIST_HttpTunnel_Enable,"false"},
                {PropertyKey.PLCM_MFW_KVLIST_SIP_HttpTunnelProxyServer,""},
                {PropertyKey.PLCM_MFW_KVLIST_SIP_HttpTunnelProxyPort,"443"},
                {PropertyKey.PLCM_MFW_KVLIST_MEDIA_HttpTunnelProxyServer,""},
                {PropertyKey.PLCM_MFW_KVLIST_MEDIA_HttpTunnelProxyPort,"443"},
                {PropertyKey.PLCM_MFW_KVLIST_RTPMode,"RTP/AVP"},
                {PropertyKey.PLCM_MFW_KVLIST_TCPBFCPForced,"false"},
                {PropertyKey.PLCM_MFW_KVLIST_G729B_Enable,"false"},
                {PropertyKey.PLCM_MFW_KVLIST_SAML_Enable,"false"},
                {PropertyKey.PLCM_MFW_KVLIST_iLBCFrame,"30"},
                {PropertyKey.PLCM_MFW_KVLIST_BFCP_CONTENT_Enable,"true"},
                {PropertyKey.PLCM_MFW_KVLIST_SUPPORT_PORTRAIT_MODE,""},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_DisplayName,""},
                {PropertyKey.PLCM_MFW_KVLIST_FECC_Enable,"true"},
                {PropertyKey.PLCM_MFW_KVLIST_Comfortable_Noise_Enable,"true"},
                {PropertyKey.PLCM_MFW_KVLIST_SIP_Header_Compact_Enable,"false"},
                {PropertyKey.PLCM_MFW_KVLIST_KEY_MAXSYS,""},
                {PropertyKey.LocalAddr,""},
                {PropertyKey.CalleeAddr,""},
                {PropertyKey.AUDIO_INPUT_DEVICE,""},
                {PropertyKey.AUDIO_OUTPUT_DEVICE,""},
                {PropertyKey.AUDIO_OUTPUT_DEVICE_FOR_RINGTONE,""},
                {PropertyKey.VIDEO_INPUT_DEVICE,""},
                {PropertyKey.MONITOR_DEVICE,""},
                {PropertyKey.SOUND_INCOMING,"incoming.wav"},
                {PropertyKey.SOUND_CLOSED,"closed.wav"},
                {PropertyKey.SOUND_RINGING,"ringing.wav"},
                {PropertyKey.SOUND_HOLD,"hold.wav"},
                {PropertyKey.ICE_AUTH_TOKEN,""},
                {PropertyKey.LayoutType,"Presentation" }
            };
        }
        public static QlConfig GetInstance()
        {
            if (null == instance)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new QlConfig();
                    }
                }
            }
            return instance;
        }
        #endregion

        #region Set 
        public void SetProperty(PropertyKey key, string value)
        {
            log.Info(string.Format(string.Format("SetProperty:{0}={1}", key, value)));
            properties[key] = value;
            if (key <= PropertyKey.PLCM_MFW_KVLIST_KEY_MAXSYS)
            {
                var errno = PlcmProxy.SetProperty(key, value);
                if (errno != ErrorNumber.OK)
                {
                    var errMsg = string.Format("{0}设定失败,err={1}", key, errno);
                    log.Error(errMsg);
                    throw new Exception(errMsg);
                }
                /*
                errno = WrapperProxy.UpdateConfig();
                if (errno != ErrorNumber.OK)
                {
                    var errMsg = string.Format("{0}更新配置失败,err={1}", key, errno);
                    log.Error(errMsg);
                    throw new Exception(errMsg);
                }
                */
            }
            else
            {
                switch (key)
                {
                    case PropertyKey.LocalAddr: break;
                    case PropertyKey.CalleeAddr: break;
                    case PropertyKey.AUDIO_INPUT_DEVICE:
                        {

                        }
                        break;
                    case PropertyKey.AUDIO_OUTPUT_DEVICE:
                        {

                        }
                        break;
                    case PropertyKey.AUDIO_OUTPUT_DEVICE_FOR_RINGTONE:
                        {

                        }
                        break;
                    case PropertyKey.VIDEO_INPUT_DEVICE:
                        {

                        }
                        break;
                    case PropertyKey.MONITOR_DEVICE: break;

                    /*Sound Effects*/
                    case PropertyKey.SOUND_INCOMING: break;
                    case PropertyKey.SOUND_CLOSED: break;
                    case PropertyKey.SOUND_RINGING: break;
                    case PropertyKey.SOUND_HOLD: break;

                    //ICE token
                    case PropertyKey.ICE_AUTH_TOKEN: break;
                }
            }
        }
        public void SetProperties(IDictionary<PropertyKey, string> properties)
        {
            if (null != properties && properties.Count > 0)
            {
                var errno = ErrorNumber.OK;
                foreach (var propertyKV in properties)
                {
                    log.Info(string.Format(string.Format("SetProperty:{0}={1}", propertyKV.Key, propertyKV.Value)));
                    this.properties[propertyKV.Key] = propertyKV.Value;
                    if (propertyKV.Key <= PropertyKey.PLCM_MFW_KVLIST_KEY_MAXSYS)
                    {
                        errno = PlcmProxy.SetProperty(propertyKV.Key, propertyKV.Value);
                        if (errno != ErrorNumber.OK)
                        {
                            var errMsg = string.Format("{0}设定失败,err={1}", propertyKV.Key, errno);
                            log.Error(errMsg);
                            throw new Exception(errMsg);
                        }
                    }
                }
                /*
                errno = WrapperProxy.UpdateConfig();
                if (errno != ErrorNumber.OK)
                {
                    var errMsg = string.Format("更新配置失败,err={0}", errno);
                    log.Error(errMsg);
                    throw new Exception(errMsg);
                }
                */
            }
        }
        #endregion

        #region Get
        public IDictionary<PropertyKey, string> GetProerpties()
        {
            log.Info("GetProerpties");
            return properties;
        }

        public string GetProperty(PropertyKey key)
        {
            log.Info(string.Format("GetProperty:{0}:{1}", key, properties[key]));
            return properties[key];
        }
        public IDictionary<PropertyKey,string> GetDefaultConfig()
        {
            return defaultProperties;
        }
        #endregion
    }
}
