namespace QLSDK.Core
{
    /// <summary>
    /// 媒体流类型
    /// </summary>
    internal enum MediaType
    {
        /// <summary>
        /// 未知
        /// </summary>
        UNKNOWN = 0,
        /// <summary>
        /// 本地视频
        /// </summary>
        LOCAL,         /**< Local video stream. */
        /// <summary>
        /// 远端视频
        /// </summary>
        REMOTE,        /**< Remote video stream. */
        /// <summary>
        /// 共享流视频
        /// </summary>
        CONTENT,       /**< Content stream. */

        /// <summary>
        /// 本地共享流视频
        /// </summary>
        LOCALCONTENT
    }
}
