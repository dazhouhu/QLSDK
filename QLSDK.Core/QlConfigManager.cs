using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QLSDK.Core
{
    /// <summary>
    /// 配置处理器
    /// </summary>
    public class QLConfigManager
    {
        #region Fields
        private IDictionary<PropertyKey, string> properties;
        private ILog log = LogUtil.GetLogger("QLSDK.QLConfig");
        #endregion

        #region Constructors
        private static readonly object lockObj = new object();
        private static QLConfigManager instance = null;
        private QLConfigManager()
        {
            properties = new Dictionary<PropertyKey, string>();            
        }
        public static QLConfigManager GetInstance()
        {
            if (null == instance)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new QLConfigManager();
                    }
                }
            }
            return instance;
        }
        #endregion

        #region Set 
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="key">属性名</param>
        /// <param name="value">属性值</param>
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
                errno = PlcmProxy.UpdateConfig();
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
                            string audioOutput = null;
                            if(properties.ContainsKey(PropertyKey.AUDIO_OUTPUT_DEVICE))
                            {
                                audioOutput = properties[PropertyKey.AUDIO_OUTPUT_DEVICE];
                            }
                            PlcmProxy.SetAudioDevice(value, audioOutput);
                        }
                        break;
                    case PropertyKey.AUDIO_OUTPUT_DEVICE:
                        {
                            string audioInput = null;
                            if (properties.ContainsKey(PropertyKey.AUDIO_INPUT_DEVICE))
                            {
                                audioInput = properties[PropertyKey.AUDIO_INPUT_DEVICE];
                            }
                            PlcmProxy.SetAudioDevice(audioInput, value);
                            PlcmProxy.SetAudioDeviceForRingtone(value);
                        }
                        break;
                    case PropertyKey.AUDIO_OUTPUT_DEVICE_FOR_RINGTONE:
                        {

                        }
                        break;
                    case PropertyKey.VIDEO_INPUT_DEVICE:
                        {
                            PlcmProxy.SetVideoDevice(value);
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
                    case PropertyKey.StaticImage: {
                            #region StaticImage
                             if (!string.IsNullOrWhiteSpace(value))
                            {
                                var filePath = Path.Combine(Application.StartupPath,value);
                                if (File.Exists(filePath))
                                {
                                    var img = new Bitmap(filePath);
                                    if (null != img)
                                    {
                                        int w = img.Width;
                                        int h = img.Height;
                                        var buffer = new byte[w*h*4];
                                        var idx = 0;
                                        for (var row = 0; row < h; row++)
                                        {
                                            for (var col = 0; col < w; col++)
                                            {
                                                var color = img.GetPixel(col,row);
                                                //转换为RGBA模式
                                                buffer[idx] = color.R;
                                                idx++;
                                                buffer[idx] = color.G;
                                                idx++;
                                                buffer[idx] = color.B;
                                                idx++;
                                                buffer[idx] = color.A;
                                                idx++;
                                            }
                                        }
                                        int length = buffer.Length;

                                        var intPtrBuffer = IntPtrHelper.IntPtrFromBytes(buffer);
                                        var errno = PlcmProxy.SetStaticImage(intPtrBuffer, length, w, h);
                                        if (ErrorNumber.OK != errno)
                                        {
                                            var errMsg = "setStaticImage failed,errno=" + errno;
                                            log.Error(errMsg);
                                            //throw new Exception(errMsg);
                                        }
                                    }
                                }
                            }
                            #endregion
                        } break;
                    case PropertyKey.LayoutType:break;
                    case PropertyKey.CryptoSuiteType:break;
                    case PropertyKey.SRTPKey:break;
                    case PropertyKey.AuthToken:break;
                }
            }
        }
        /// <summary>
        /// 批量设置属性
        /// </summary>
        /// <param name="properties">属性集合</param>
        public void SetProperties(IDictionary<PropertyKey, string> properties)
        {
            if (null != properties && properties.Count > 0)
            {
                foreach (var propertyKV in properties)
                {
                    SetProperty(propertyKV.Key, propertyKV.Value);                    
                }
                /*
                errno = PlcmProxy.UpdateConfig();
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
        /// <summary>
        /// 获取所有属性
        /// </summary>
        /// <returns>属性集合</returns>
        public IDictionary<PropertyKey, string> GetProerpties()
        {
            log.Info("GetProerpties");
            return properties;
        }
        /// <summary>
        /// 获取指定属性
        /// </summary>
        /// <param name="key">属性名</param>
        /// <returns>属性值</returns>
        public string GetProperty(PropertyKey key)
        {
            log.Info(string.Format("GetProperty:{0}:{1}", key, properties[key]));
            return properties[key];
        }
        /// <summary>
        /// 获取默认配置属性
        /// </summary>
        /// <returns>属性集合</returns>
        public IDictionary<PropertyKey,string> GetDefaultConfig()
        {
            return  new Dictionary<PropertyKey, string>()
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
                {PropertyKey.PLCM_MFW_KVLIST_KEY_LogLevel,"DEBUG"},
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
                {PropertyKey.PLCM_MFW_KVLIST_CERT_PATH,"./TLS Certificate/"},
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
                {PropertyKey.StaticImage,"static_img_640_480.png" },
                {PropertyKey.LayoutType,"Presentation" },
                {PropertyKey.CryptoSuiteType,"AES_CM_128_HMAC_SHA1_80" },
                {PropertyKey.SRTPKey,"HfVGG79oW5XStt9DewUYrdngYlV/QqDBGIDNFB7m" },
                {PropertyKey.AuthToken,"AApzdG1lZXRpbmcxAAdzdHVzZXIxAAABPcJe1o4CsXgvirq1RQys3JCU0U8RvJ4uoA==" }
            };
        }
        #endregion
    }
}
