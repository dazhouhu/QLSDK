namespace QLSDK.Core
{
    /// <summary>
    /// 事件类型
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// 未知
        /// </summary>
        UNKNOWN,

        /// <summary>
        /// 终端注册成功
        /// </summary>
        SIP_REGISTER_SUCCESS,                       /*1 with SIP server from CC*/
        /// <summary>
        /// 终端注册失败
        /// </summary>
        SIP_REGISTER_FAILURE,                       /*2 from CC */
        /// <summary>
        /// 终端未注册
        /// </summary>
        SIP_REGISTER_UNREGISTERED,                  /*3 Unregister client. */
        /// <summary>
        /// 呼入待接听中
        /// </summary>
        SIP_CALL_INCOMING,                          /*4 UAS received INVITE, from CC */
        /// <summary>
        /// 尝试呼出中
        /// </summary>
        SIP_CALL_TRYING,                            /*5< A SIP call is trying. */
        /// <summary>
        /// 呼出响铃中
        /// </summary>
        SIP_CALL_RINGING,                           /*6 UAC get 180 from CC */
        /// <summary>
        /// 呼出失败
        /// </summary>
        SIP_CALL_FAILURE,                           /*7 from CC */
        /// <summary>
        /// 呼叫结束关闭
        /// </summary>
        SIP_CALL_CLOSED,                            /* 8UAS get terminated from CC */
        /// <summary>
        /// 呼叫主动保持
        /// </summary>
        SIP_CALL_HOLD,                              /* 9local hold */
        /// <summary>
        /// 呼叫被动保持
        /// </summary>
        SIP_CALL_HELD,                              /* 10far site hold */
        /// <summary>
        /// 呼叫双边保持
        /// </summary>
        SIP_CALL_DOUBLE_HOLD,                       /* 11both far-site and local hold */
        /// <summary>
        /// 呼入连接通话中
        /// </summary>
        SIP_CALL_UAS_CONNECTED,                     /* 12from CC */
        /// <summary>
        /// 呼出连接通话中
        /// </summary>
        SIP_CALL_UAC_CONNECTED,                     /* 13from CC */

        /// <summary>
        /// 内容共享呼出中
        /// </summary>
        SIP_CONTENT_INCOMING,                       /*14from MP*/
        /// <summary>
        /// 内容共享结束
        /// </summary>
        SIP_CONTENT_CLOSED,                         /*15*/
        /// <summary>
        /// 内容共享发出中
        /// </summary>
        SIP_CONTENT_SENDING,                        /*16*/
        /// <summary>
        /// 内容共享空闲
        /// </summary>
        SIP_CONTENT_IDLE,                           /*17Indicates SIP content is idle & not sent now*/
        /// <summary>
        /// 内容共享不支持
        /// </summary>
        SIP_CONTENT_UNSUPPORTED,                    /*18*/

        /// <summary>
        /// 视频输入设备改变
        /// </summary>
        DEVICE_VIDEOINPUTCHANGED,                   /* 19from MP */
        /// <summary>
        /// 音频输入设备改变
        /// </summary>
        DEVICE_AUDIOINPUTCHANGED,                   /* 20from MP */
        /// <summary>
        /// 音频输出设备改变
        /// </summary>
        DEVICE_AUDIOOUTPUTCHANGED,                  /* 21from MP */
        /// <summary>
        /// 音量改变
        /// </summary>
        DEVICE_VOLUMECHANGED,                       /* 22from MP */
        /// <summary>
        /// 显示器改变
        /// </summary>
        DEVICE_MONITORINPUTSCHANGED,                /* 23from MP */

        /// <summary>
        /// 本地视频分辨率改变
        /// </summary>
        STREAM_VIDEO_LOCAL_RESOLUTIONCHANGED,       /*24from MP*/
        /// <summary>
        /// 远端视频分辨率改变
        /// </summary>
        STREAM_VIDEO_REMOTE_RESOLUTIONCHANGED,      /*25from MP*/
        /// <summary>
        /// 网络改变
        /// </summary>
        NETWORK_CHANGED,                            /* 26when network is changed or lost. */
        /// <summary>
        /// 处理超时
        /// </summary>
        MFW_INTERNAL_TIME_OUT,                      /* 27if receive this notification, there is an fatal error in mfw, app should show warning message then exit app. */
        /// <summary>
        /// 当前讲话都改变
        /// </summary>
        REFRESH_ACTIVE_SPEAKER,                     /* 28SVC msg */
        /// <summary>
        /// 视频更新
        /// </summary>
        REMOTE_VIDEO_REFRESH,                       /* 29SVC msg */
        /// <summary>
        /// 远端视频流通道状态改变
        /// </summary>
        REMOTE_VIDEO_CHANNELSTATUS_CHANGED,         /* 30SVC msg */
        /// <summary>
        /// 远端显示名改变
        /// </summary>
        REMOTE_VIDEO_DISPLAYNAME_UPDATE,            /* 31Remote video status is changed. */
        /// <summary>
        /// 呼叫模式改变
        /// </summary>
        SIP_CALL_MODE_CHANGED,                      /*32Indicates call mode is changed. */
        /// <summary>
        /// 呼叫模式升级改变
        /// </summary>
        SIP_CALL_MODE_UPGRADE_REQ,                  /*33Indicates receiving an audio-video call upgrade request. */
        /// <summary>
        /// 讲话状态变更
        /// </summary>
        IS_TALKING_STATUS_CHANGED,                  /*34*/
        /// <summary>
        /// 证书验证
        /// </summary>
        CERTIFICATE_VERIFY,                         /*35 Certifate needs user trust.*/
        /// <summary>
        /// 转码完成
        /// </summary>
        TRANSCODER_FINISH,                          /*36 transcoding finish*/
        /// <summary>
        /// ICE状态改变
        /// </summary>
        ICE_STATUS_CHANGED,                         /*37 Notify ICE status. */
        /// <summary>
        /// SUTLite 呼入
        /// </summary>
        SUTLITE_INCOMING_CALL,                      /*38 Notify SUTLite incoming call. */
        /// <summary>
        /// SUTLite 结束
        /// </summary>
        SUTLITE_TERMINATE_CALL,                     /*39 Notify SUTLite terminate call. */

        /// <summary>
        /// 不支持视频编码
        /// </summary>
        NOT_SUPPORT_VIDEOCODEC,                     /*40 Video call is not supported for video codec is not supported. */
        /// <summary>
        /// 网络带宽受限
        /// </summary>
        BANDWIDTH_LIMITATION,                       /*41 Notify network bandwidth is limitation status. */
        /// <summary>
        /// 媒体地址变更
        /// </summary>
        MEDIA_ADDRESS_UPDATED,                      /*42 Notify media IP address updated. */
        /// <summary>
        /// 发现状态变更
        /// </summary>
        AUTODISCOVERY_STATUS_CHANGED                /*43 Notify http tunnel auto discovery status changed. */
                                                   //MORE...
    }
}
