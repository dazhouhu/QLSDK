
namespace QLSDK.Core
{
    /// <summary>
    /// 呼叫状态
    /// </summary>
    public enum CallState
    {
        /// <summary>
        /// 未知状态
        /// </summary>
        SIP_UNKNOWN,
        /// <summary>
        /// 呼入待接听中
        /// </summary>
        SIP_INCOMING_INVITE,
        /// <summary>
        /// 呼入通话中
        /// </summary>
        SIP_INCOMING_CONNECTED,
        /// <summary>
        /// 呼叫主动保持
        /// </summary>
        SIP_CALL_HOLD,
        /// <summary>
        /// 呼叫被动保持
        /// </summary>
        SIP_CALL_HELD, 
        /// <summary>
        /// 呼叫双向保持
        /// </summary>
        SIP_CALL_DOUBLE_HOLD,
        /// <summary>
        /// 尝试呼出中
        /// </summary>
        SIP_OUTGOING_TRYING,
        /// <summary>
        /// 呼出响铃中
        /// </summary>
        SIP_OUTGOING_RINGING, 
        /// <summary>
        /// 呼出通话中
        /// </summary>
        SIP_OUTGOING_CONNECTED,
        /// <summary>
        /// 呼出失败
        /// </summary>
        SIP_OUTGOING_FAILURE,
        /// <summary>
        /// 呼叫关闭结束
        /// </summary>
        SIP_CALL_CLOSED
    }

}
