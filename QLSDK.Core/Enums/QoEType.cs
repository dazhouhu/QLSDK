namespace QLSDK.Core
{
    /// <summary>
    /// QoE类型
    /// </summary>
    public enum QoEType
    {
        /// <summary>
        /// 未知
        /// </summary>
        UNKNOWN					= 0X00000000,
        /// <summary>
        /// 视频输入原数据
        /// </summary>
        VIDEO_CAPTURE_RAWONLY	= 0X00000001,   /**< QoE type is video input raw data. */
        /// <summary>
        /// 视频输入压缩数据
        /// </summary>
        VIDEO_CAPTURE_SCALED    = 0X00000002,   /**< QoE type is video input scaled data. */
        /// <summary>
        /// 视频输出数据
        /// </summary>
        VIDEO_RENDER            = 0X00000004,   /**< QoE type is video output. */
        /// <summary>
        /// 音频输入数据
        /// </summary>
        AUDIO_RECORD            = 0X00000008,   /**< QoE type is audio input. */
        /// <summary>
        /// 音频输出数据
        /// </summary>
        AUDIO_PLAYING           = 0X00000010,   /**< QoE type is audio output. */
        /// <summary>
        /// 视频H264编码数据
        /// </summary>
        ENCODED_VIDEO           = 0X00000020,   /**< QoE type is H264 video stream data input. */
        /// <summary>
        /// 共享内容H264编码数据
        /// </summary>
        ENCODED_CONTENT         = 0X00000040,   /**< QoE type is H264 content stream data input. */
        /// <summary>
        /// 视频H264解码数据
        /// </summary>
        DECODED_VIDEO           = 0X00000080,   /**< QoE type is H264 video stream data output. */
        /// <summary>
        /// 共享内容H264解码数据
        /// </summary>
        DECODED_CONTENT         = 0X00000100,   /**< QoE type is H264 content stream data output. */
        /// <summary>
        /// RTP视频输出数据
        /// </summary>
        RTP_RECEIVED_VIDEO      = 0X00000200,   /**< QoE type is RTP video data output. */
        /// <summary>
        /// RTP音频输出数据
        /// </summary>
        RTP_RECEIVED_AUDIO      = 0X00000400,   /**< QoE type is RTP audio data output. */
        /// <summary>
        /// RTP共享内容输出数据
        /// </summary>
        RTP_RECEIVED_CONTENT    = 0X00000800,   /**< QoE type is RTP content data output. */
        /// <summary>
        /// RTP视频输入数据
        /// </summary>
        RTP_SENT_VIDEO          = 0X00001000,   /**< QoE type is RTP video data input. */
        /// <summary>
        /// RTP音频输入数据
        /// </summary>
        RTP_SENT_AUDIO          = 0X00002000,   /**< QoE type is RTP audio data input. */
        /// <summary>
        /// RTP共享内容输入数据
        /// </summary>
        RTP_SENT_CONTENT        = 0X00004000,   /**< QoE type is RTP content data input. */
        /// <summary>
        /// 全部
        /// </summary>
        ALL                     = 0X00007FFF   /**< QoE type is all above. */
    }
}
