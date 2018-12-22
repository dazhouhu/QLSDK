namespace QLSDK.Core
{
    /// <summary>
    /// 媒体流信号统计信息
    /// </summary>
    public class QLMediaStatistics
    {
        /// <summary>
        /// 通道名称
        /// </summary>
        public string ChannelName { get; set; }
        /// <summary>
        /// 与会者
        /// </summary>
        public string ParticipantName { get; set; }
        /// <summary>
        /// 远端系统Id
        /// </summary>
        public string RemoteSystemId { get; set; }
        /// <summary>
        /// 呼叫速率
        /// </summary>
        public string CallRate { get; set; }
        /// <summary>
        /// 已丢失的数据包
        /// </summary>
        public string PacketsLost { get; set; }
        /// <summary>
        /// 数据包丢失量
        /// </summary>
        public string PacketLoss { get; set; }
        /// <summary>
        /// 视频协议
        /// </summary>
        public string VideoProtocol { get; set; }
        /// <summary>
        /// 音频速率
        /// </summary>
        public string VideoRate { get; set; }
        /// <summary>
        /// 视频使用的速率
        /// </summary>
        public string VideoRateUsed { get; set; }
        /// <summary>
        /// 视频帧速率
        /// </summary>
        public string VideoFrameRate { get; set; }
        /// <summary>
        /// 已丢失的视频数据包
        /// </summary>
        public string VideoPacketsLost { get; set; }
        /// <summary>
        /// 视频抖动
        /// </summary>
        public string VideoJitter { get; set; }
        /// <summary>
        /// 视频格式
        /// </summary>
        public string VideoFormat { get; set; }
        /// <summary>
        /// 错误隐藏
        /// </summary>
        public string ErrorConcealment { get; set; }
        /// <summary>
        /// 视频协议
        /// </summary>
        public string AudioProtocol { get; set; }
        /// <summary>
        /// 音频速率
        /// </summary>
        public string AudioRate { get; set; }
        /// <summary>
        /// 已丢失的音频数据包
        /// </summary>
        public string AudioPacketsLost { get; set; }
        /// <summary>
        /// 音频抖动
        /// </summary>
        public string AudioJitter { get; set; }
        /// <summary>
        /// 音频已加密
        /// </summary>
        public string AudioEncrypt { get; set; }
        /// <summary>
        /// 视频已加密
        /// </summary>
        public string VideoEncrypt { get; set; }
        /// <summary>
        /// 运程控制已加密
        /// </summary>
        public string FeccEncrypt { get; set; }
        /// <summary>
        /// 已接收到的音频包
        /// </summary>
        public string AudioReceivedPacket { get; set; }
        /// <summary>
        /// 往返时间
        /// </summary>
        public string RoundTripTime { get; set; }
        /// <summary>
        /// 全帧请求
        /// </summary>
        public string FullIntraFrameRequest { get; set; }
        /// <summary>
        /// 帧内发送
        /// </summary>
        public string IntraFrameSent { get; set; }
        /// <summary>
        /// 包总量
        /// </summary>
        public string PacketsCount { get; set; }
        /// <summary>
        /// CPU负载
        /// </summary>
        public string OverallCPULoad { get; set; }
        /// <summary>
        /// ssrc
        /// </summary>
        public int ChannelNum { get; set; }
    }
}
