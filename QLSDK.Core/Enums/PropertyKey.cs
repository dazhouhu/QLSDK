namespace QLSDK.Core
{
    /// <summary>
    /// 配置属性表
    /// </summary>
    public enum PropertyKey
    {
        /// <summary>
        /// 配置属性起始边界
        /// </summary>
        PLCM_MFW_KVLIST_KEY_MINSYS = 0,
        /// <summary>
        /// SIP代理服务器地址
        /// </summary>
        PLCM_MFW_KVLIST_KEY_SIP_ProxyServer,                                   /**< DNS name or IP address of the SIP Proxy Server. */
        /// <summary>
        /// SIP代理服务器协议 "UDP","TCP"，"TLS"
        /// </summary>
        PLCM_MFW_KVLIST_KEY_SIP_Transport,                                     /**< Protocol the user application uses for SIP signaling. The value can be "UDP","TCP" or "TLS". */
        /// <summary>
        /// SIP代理服务器类型 "ibm","standard","off"
        /// </summary>
        PLCM_MFW_KVLIST_KEY_SIP_ServerType,                                       /**< Determines if you need to register the user application with a SIP Server. The value can be "ibm","standard" or "off".  */
        /// <summary>
        /// SIP代理服务器注册超时时间
        /// </summary>
        PLCM_MFW_KVLIST_KEY_SIP_Register_Expires_Interval,                     /**< The expiration interval for SIP register. */
        /// <summary>
        /// SIP代理服务器注册用户名
        /// </summary>
        PLCM_MFW_KVLIST_KEY_SIP_UserName,                                     /**< User name for authentication to a Registrar Server. */
        /// <summary>
        /// SIP代理服务器域名，若服务器为DMA,则可以为空
        /// </summary>
        PLCM_MFW_KVLIST_KEY_SIP_Domain,                                           /**< Domain name for authentication to a Registrar Server. If user application uses Polycom DMA server as the SIP server,the value can be left empty. */
        /// <summary>
        /// SIP代理服务器认证名，为空时为用户名
        /// </summary>
        PLCM_MFW_KVLIST_KEY_SIP_AuthorizationName,                           /**< Authentication name when registering to a SIP Registrar Server. If the value is empty,the User Name is used for authentication.  */
        /// <summary>
        /// SIP代理服务器注册密码
        /// </summary>
        PLCM_MFW_KVLIST_KEY_SIP_Password,                                     /**< Password for authentication to a Registrar Server. */
        /// <summary>
        /// SIP Cookie头内容
        /// </summary>
        PLCM_MFW_KVLIST_KEY_SIP_CookieHead,                                       /**< Cookie head. */
        /// <summary>
        /// SIP 证书头内容
        /// </summary>
        PLCM_MFW_KVLIST_KEY_SIP_Base_Cred,                                    /**< Base credential head. */
        /// <summary>
        /// SIP 匿名标识Token
        /// </summary>
        PLCM_MFW_KVLIST_KEY_SIP_AnonymousToken_Cred,                           /**< Anonymous-Token cred. */
        /// <summary>
        /// SIP 匿名证书
        /// </summary>
        PLCM_MFW_KVLIST_KEY_SIP_Anonymous_Cred,                                /**< Anonymous cred. */

        /// <summary>
        /// 最多呼叫数
        /// </summary>
        PLCM_MFW_KVLIST_KEY_CallSettings_MaxCallNum,                          /**< Maximum number of SIP calls. */
        /// <summary>
        /// 网络调用速率
        /// </summary>
        PLCM_MFW_KVLIST_KEY_CallSettings_NetworkCallRate,                     /**< Negotiated speed (bandwidth) for the call; usually combined video and audio speeds in the call. */
        /// <summary>
        /// 是否启用AES加密 "on","off","auto"
        /// </summary>
        PLCM_MFW_KVLIST_KEY_CallSettings_AesEcription,                            /**< Determines if a user application uses AES encryption. The value can be "on","off","auto". */
        /// <summary>
        /// 默认音频起始端口，需要打开防火墙
        /// </summary>
        PLCM_MFW_KVLIST_KEY_CallSettings_DefaultAudioStartPort,                   /**< Sets the start port of audio port range. This range of ports needs to be opened in the firewall. */
        /// <summary>
        /// 默认音频结束端口，需要打开防火墙
        /// </summary>
        PLCM_MFW_KVLIST_KEY_CallSettings_DefaultAudioEndPort,                 /**< Sets the end port of audio port range. This range of ports needs to be opened in the firewall. */
        /// <summary>
        /// 默认视频起始端口，需要打开防火墙
        /// </summary>
        PLCM_MFW_KVLIST_KEY_CallSettings_DefaultVideoStartPort,                   /**< Sets the start port of video port range. This range of ports needs to be opened in the firewall. */
        /// <summary>
        /// 默认视频结束端口，需要打开防火墙
        /// </summary>
        PLCM_MFW_KVLIST_KEY_CallSettings_DefaultVideoEndPort,                  /**< Sets the end port of video port range. This range of ports needs to be opened in the firewall. */

        /// <summary>
        /// 客户端监听端口
        /// </summary>
        PLCM_MFW_KVLIST_KEY_CallSettings_SIPClientListeningPort,               /**< Local listen port for SIP. Default value is 5060. */
        /// <summary>
        /// 客户端TLS监听端口
        /// </summary>
        PLCM_MFW_KVLIST_KEY_CallSettings_SIPClientListeningTLSPort,            /**< Local listen port for SIP TLS. Default value is 5061. */

        /// <summary>
        /// 启动SVC
        /// </summary>
        PLCM_MFW_KVLIST_KEY_EnableSVC,                                            /**< Enable/Disable the SVC feature. The value can be "true" or "false". */
        /// <summary>
        /// 日志等级
        /// </summary>
        PLCM_MFW_KVLIST_KEY_LogLevel,                                         /**< Log information level. Log levels defined in Macros can be set. */
        /// <summary>
        /// 用户代理
        /// </summary>
        PLCM_MFW_KVLIST_KEY_User_Agent,                                       /**< Customer names for SIP user-agent. */

        /// <summary>
        /// ICE用户名
        /// </summary>
        PLCM_MFW_KVLIST_ICE_UserName,                                         /**< ICE username. */
        /// <summary>
        /// ICE密码
        /// </summary>
        PLCM_MFW_KVLIST_ICE_Password,                                         /**< ICE password. */
        /// <summary>
        /// ICE TCP服务器
        /// </summary>
        PLCM_MFW_KVLIST_ICE_TCPServer,                                        /**< ICE TCP server.*/
        /// <summary>
        /// ICE UDP服务器
        /// </summary>
        PLCM_MFW_KVLIST_ICE_UDPServer,                                        /**< ICE UDP server. */
        /// <summary>
        /// ICE TLS服务器
        /// </summary>
        PLCM_MFW_KVLIST_ICE_TLSServer,                                         /**< ICE TLS server. */
        /// <summary>
        /// 是否启用ICE
        /// </summary>
        PLCM_MFW_KVLIST_ICE_Enable,                                            /**< Enable/ Disable ICE. The value can be "true" or "false". */
        /// <summary>
        /// 是否启用ICE认证
        /// </summary>
        PLCM_MFW_KVLIST_ICE_AUTHTOKEN_Enable,                                  /**< Enable/Disable ICE token. The value can be "true" or "false". */
        /// <summary>
        /// ICE 初始认证Token
        /// </summary>
        PLCM_MFW_KVLIST_ICE_INIT_AUTHTOKEN,                                    /**< Authentication token for initial Binding request. */
        /// <summary>
        /// ICE RTO
        /// </summary>
        PLCM_MFW_KVLIST_ICE_RTO,                                               /**< Represents the starting interval between retransmissions which doubles after each retransmission. Unit is millisecond. Default value is 100. */
        /// <summary>
        /// ICE RC
        /// </summary>
        PLCM_MFW_KVLIST_ICE_RC,                                                /**< Number of maximum retransmissions for single request sent to the TURN server. Default value is 7. */
        /// <summary>
        /// ICE RM
        /// </summary>
        PLCM_MFW_KVLIST_ICE_RM,                                                   /**< Represents duration equal to rm times the rto has passed since the last request was sent and no response received when client will consider the transaction (connection to the TURN server) timed out and failed. The default value is 16. */

        /// <summary>
        /// QOS 服务类型 "IP_PRECEDENCE","DIFFSERV"，WINDOWS不支持
        /// </summary>
        PLCM_MFW_KVLIST_QOS_ServiceType,                                       /**< Qos service type. The value can be "IP_PRECEDENCE","DIFFSERV". Not supported on Windows. */
        /// <summary>
        /// QOS 音频值 0~255 ，WINDOWS不支持
        /// </summary>
        PLCM_MFW_KVLIST_QOS_Audio,                                             /**< Qos audio value. The value can be 0~255. Not supported on Windows. */
        /// <summary>
        /// QOS 视频值 0~255 ，WINDOWS不支持
        /// </summary>
        PLCM_MFW_KVLIST_QOS_Video,                                             /**< Qos video value. The value can be 0~255. Not supported on Windows. */
        /// <summary>
        /// QOS FECC值 0~255 ，WINDOWS不支持
        /// </summary>
        PLCM_MFW_KVLIST_QOS_Fecc,                                              /**< Qos FECC value. The value can be 0~255. Not supported on Windows. */
        /// <summary>
        /// 是否启用QOS
        /// </summary>
        PLCM_MFW_KVLIST_QOS_Enable,                                            /**< Enable/Disable Qos. The value can be "true" or "false". */

        /// <summary>
        /// 是否启用DBM
        /// </summary>
        PLCM_MFW_KVLIST_DBM_Enable,                                            /**< Enable/Disable DBM. The value can be "true" or "false". */

        /// <summary>
        /// 注册ID
        /// </summary>
        PLCM_MFW_KVLIST_KEY_REG_ID,                                            /**< Register id,the unique index of Registrar server. This value can only be added or removed,but it can not be updated. */

        /// <summary>
        /// 是否启用LPR
        /// </summary>
        PLCM_MFW_KVLIST_LPR_Enable,                                            /**< Enable/ Disable LPR. The value can be "true" or "false". */

        /// <summary>
        /// 证书文件路径
        /// </summary>
        PLCM_MFW_KVLIST_CERT_PATH,                                            /**< Sets the path of certificates. */
        /// <summary>
        /// 是否核查FQDN
        /// </summary>
        PLCM_MFW_KVLIST_CERT_CHECKFQDN,                                       /**< Whether check the FQDN of certificate. */

        /// <summary>
        /// 是否启用Http连接
        /// </summary>
        PLCM_MFW_KVLIST_HttpConnect_Enable,                                       /**< Enable/Disable Http connect. The value can be "true" or "false".  */
        /// <summary>
        /// HTTP代理服务器
        /// </summary>
        PLCM_MFW_KVLIST_SIP_HttpProxyServer,                                  /**< SIP http proxy server. */
        /// <summary>
        /// HTTP代理服务器端口
        /// </summary>
        PLCM_MFW_KVLIST_SIP_HttpProxyPort,                                    /**< SIP http proxy port.  */
        /// <summary>
        /// HTTP代理服务器用户名
        /// </summary>
        PLCM_MFW_KVLIST_SIP_HttpProxyUserName,                                    /**< SIP http proxy user name.  */
        /// <summary>
        /// HTTP代理服务器密码
        /// </summary>
        PLCM_MFW_KVLIST_SIP_HttpPassword,                                     /**< SIP http proxy password.  */
        /// <summary>
        /// ICE HTTP代理服务器
        /// </summary>
        PLCM_MFW_KVLIST_ICE_HttpProxyServer,                                  /**< ICE http proxy server. */
        /// <summary>
        /// ICE HTTP代理服务器端口
        /// </summary>
        PLCM_MFW_KVLIST_ICE_HttpProxyPort,                                    /**< ICE http proxy port.  */
        /// <summary>
        /// ICE HTTP代理服务器用户名
        /// </summary>
        PLCM_MFW_KVLIST_ICE_HttpProxyUserName,                                    /**< ICE http proxy user name.  */
        /// <summary>
        /// ICE HTTP代理服务器密码
        /// </summary>
        PLCM_MFW_KVLIST_ICE_HttpPassword,                                     /**< ICE http proxy password.  */
        /// <summary>
        /// Media HTTP代理服务器
        /// </summary>
        PLCM_MFW_KVLIST_MEDIA_HttpProxyServer,                                 /**< Media http proxy server.  */
        /// <summary>
        /// Media HTTP代理服务器端口
        /// </summary>
        PLCM_MFW_KVLIST_MEDIA_HttpProxyPort,                                   /**< Media http proxy port.  */
        /// <summary>
        /// Media HTTP代理服务器用户名
        /// </summary>
        PLCM_MFW_KVLIST_MEDIA_HttpProxyUserName,                              /**< Media http proxy user name.  */
        /// <summary>
        /// Media HTTP代理服务器密码
        /// </summary>
        PLCM_MFW_KVLIST_MEDIA_HttpPassword,                                    /**< Media http proxy password.  */
        /// <summary>
        /// 产品名
        /// </summary>
        PLCM_MFW_KVLIST_PRODUCT,                                              /**< Product name.  */
        /// <summary>
        /// 是否启用自动缩放
        /// </summary>
        PLCM_MFW_KVLIST_AutoZoom_Enable,                                       /**< Enable/ Disable auto zoom for video render. The value can be "true" or "false". */
        /// <summary>
        /// 是否启用TLS offload
        /// </summary>
        PLCM_MFW_KVLIST_TLSOffLoad_Enable,                                     /**< Enable/ Disable TLS OffLoad. The value can be "true" or "false". */
        /// <summary>
        /// 是否启用TLS offload 主机
        /// </summary>
        PLCM_MFW_KVLIST_TLSOffLoad_Host,                                       /**< TLS OffLoad host name. */
        /// <summary>
        /// 是否启用TLS offload 端口
        /// </summary>
        PLCM_MFW_KVLIST_TLSOffLoad_Port,                                      /**< TLS OffLoad port. */

        /// <summary>
        /// 是否启用Http管道
        /// </summary>
        PLCM_MFW_KVLIST_HttpTunnel_Enable,                                     /**< Enable/ Disable http tunnel. The value can be "true" or "false". */
        /// <summary>
        /// 是否启用Http管道服务器
        /// </summary>
        PLCM_MFW_KVLIST_SIP_HttpTunnelProxyServer,                             /**< SIP http tunnel proxy server. */
        /// <summary>
        /// 是否启用Http管道服务器端口
        /// </summary>
        PLCM_MFW_KVLIST_SIP_HttpTunnelProxyPort,                               /**< SIP http tunnel proxy port. */
        /// <summary>
        /// 是否启用Media Http管道服务器
        /// </summary>
        PLCM_MFW_KVLIST_MEDIA_HttpTunnelProxyServer,                           /**< Media http tunnel proxy server.  */
        /// <summary>
        /// 是否启用Media Http管道服务器端口
        /// </summary>
        PLCM_MFW_KVLIST_MEDIA_HttpTunnelProxyPort,                             /**< Media http tunnel proxy port.  */
        /// <summary>
        /// RTP模式 TCP/RTP/AVP ，RTP/AVP ， All
        /// </summary>
        PLCM_MFW_KVLIST_RTPMode,                                               /**< RTP mode. The value can be TCP/RTP/AVP or RTP/AVP or All.  */
        /// <summary>
        /// 是否启用强制TCP/BFCP
        /// </summary>
        PLCM_MFW_KVLIST_TCPBFCPForced,                                         /**< Enable/ Disable TCP/BFCP forced. The value can be "true" or "false".  */
        /// <summary>
        /// 是否启用G729B
        /// </summary>
        PLCM_MFW_KVLIST_G729B_Enable,                                          /**< Enable/Disable G729B codec. The value can be "true" or "false".  */
        /// <summary>
        /// 是否启用SAML
        /// </summary>
        PLCM_MFW_KVLIST_SAML_Enable,                                          /**< Enable/Disable SAML. The value can be "true" or "false". */
        /// <summary>
        /// iLBC编码 "20" ， "30"
        /// </summary>
        PLCM_MFW_KVLIST_iLBCFrame,                                             /**< Microsecond Frame for iLBC codec. The value can be "20" or "30".  Default value is 30. */
        /// <summary>
        /// 是否启用BFCP
        /// </summary>
        PLCM_MFW_KVLIST_BFCP_CONTENT_Enable,                                  /**< Enable/Disable BFCP content. The value can be "true" or "false".  */
        /// <summary>
        /// 是否启用 纵向模式
        /// </summary>
        PLCM_MFW_KVLIST_SUPPORT_PORTRAIT_MODE,                                /**< Enable/Disable support portrait mode.  */
        /// <summary>
        /// 远端显示名
        /// </summary>
        PLCM_MFW_KVLIST_KEY_DisplayName,                                     /**< Display name for sip call. */
        /// <summary>
        /// 是否启用 FECC
        /// </summary>
        PLCM_MFW_KVLIST_FECC_Enable,                                           /**<  Enable/Disable FECC function. */
        /// <summary>
        /// 是否启用舒适音功能
        /// </summary>
        PLCM_MFW_KVLIST_Comfortable_Noise_Enable,                            /**< Enable/Disable comfortable noise function. */
        /// <summary>
        /// 是否启用头部压缩功能
        /// </summary>
        PLCM_MFW_KVLIST_SIP_Header_Compact_Enable,                           /**< Enable/Disable SIP header compact function. */

        /// <summary>
        /// 配置结束边界
        /// </summary>
        PLCM_MFW_KVLIST_KEY_MAXSYS,

        /// <summary>
        /// 本地地址
        /// </summary>
        LocalAddr,
        /// <summary>
        /// 被呼叫地址
        /// </summary>
        CalleeAddr,
        /// <summary>
        /// 音频输入设备
        /// </summary>
        AUDIO_INPUT_DEVICE,
        /// <summary>
        /// 音频输出设备
        /// </summary>
        AUDIO_OUTPUT_DEVICE,
        /// <summary>
        /// 音频输出响铃设备
        /// </summary>
        AUDIO_OUTPUT_DEVICE_FOR_RINGTONE,
        /// <summary>
        /// 视频输入设备
        /// </summary>
        VIDEO_INPUT_DEVICE,
        /// <summary>
        /// 视频输出设备
        /// </summary>
        MONITOR_DEVICE,

        /*Sound Effects*/

        /// <summary>
        /// 呼入响铃音
        /// </summary>
        SOUND_INCOMING,
        /// <summary>
        /// 结束响铃音
        /// </summary>
        SOUND_CLOSED,
        /// <summary>
        /// 呼出响铃音
        /// </summary>
        SOUND_RINGING,
        /// <summary>
        /// 保持响铃音
        /// </summary>
        SOUND_HOLD,

        /// <summary>
        /// ICE认证Token
        /// </summary>
        ICE_AUTH_TOKEN,

        /// <summary>
        /// 静态图片
        /// </summary>
        StaticImage,  

        /// <summary>
        /// 布局类型
        /// </summary>
        LayoutType,
        
        /// <summary>
        /// Suite加密key
        /// </summary>
        CryptoSuiteType,
        /// <summary>
        /// SRTP key
        /// </summary>
        SRTPKey,
        /// <summary>
        /// 认证Token
        /// </summary>
        AuthToken
    }
}
