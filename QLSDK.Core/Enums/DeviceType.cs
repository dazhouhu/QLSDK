namespace QLSDK.Core
{
    /// <summary>
    /// 设备类型
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// 未知
        /// </summary>
        UNKNOWN = 0,
        /// <summary>
        /// 音频输入设备，如麦克风
        /// </summary>
        AUDIOINPUT,
        /// <summary>
        /// 音频输出设备,如扬声器
        /// </summary>
        AUDIOOUTPUT,
        /// <summary>
        /// 视频输入设备，如摄像头
        /// </summary>
        VIDEOINPUT,
        /// <summary>
        /// 视频输出设备，如显示器
        /// </summary>
        MONITOR
    }
}
