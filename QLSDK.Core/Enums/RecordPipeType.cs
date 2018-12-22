namespace QLSDK.Core
{
    /// <summary>
    /// 记录流类型
    /// </summary>
    public enum RecordPipeType
    {
        /// <summary>
        /// 未知
        /// </summary>
        UNKNOWN = 0,

        /// <summary>
        /// 远端音频流
        /// </summary>
        ARX,            /**< Remote audio stream.  */
        /// <summary>
        /// 本地音频流
        /// </summary>
        ATX,            /**< Local audio stream. */
        /// <summary>
        /// 远端视频流
        /// </summary>
        PVRX,           /**< Remote video stream. */
        /// <summary>
        /// 本地视频流
        /// </summary>
        PVTX,           /**< Local video stream. */
        /// <summary>
        /// 远端内容共享流
        /// </summary>
        CRX,            /**< Remote content stream. */
        /// <summary>
        /// 本地内容共享流
        /// </summary>
        CTX,            /**< Local content stream. */
        /// <summary>
        /// 远端音视频混合流
        /// </summary>
        AVRX,           /**< Mixed remote audio and video stream. */
        /// <summary>
        /// 本地音视频混合流
        /// </summary>
        AVTX,           /**< Mixed local audio and video streams. */
        /// <summary>
        /// 本地远端音频混合流
        /// </summary>
        ATRX,           /**< Mixed local and remote audio streams. */
        /// <summary>
        /// 本地远端音频和远端视频混合流
        /// </summary>
        ATRX_PVRX       /**< Mixed local and remote audio streams and remote video stream. */
    }
}
