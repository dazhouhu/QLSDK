namespace QLSDK.Core
{
    /// <summary>
    /// ICE状态
    /// </summary>
    public enum ICEStatus
    {
        /// <summary>
        /// 空闲
        /// </summary>
        IDLE = 0,
        /// <summary>
        /// 连接失败
        /// </summary>
        FAIL_CONN_TURN,                        /**< Connection to TURN Server failed. */
        /// <summary>
        /// 连接性检测失败
        /// </summary>
        FAIL_CONNECTIVITY_CHECK,               /**< ICE Connectivity check failure. */
        /// <summary>
        /// 认证失败
        /// </summary>
        FAIL_TOKEN_AUTHENTICATION_WITH_TURN,   /**< Token Authentication failed with TURN Server. */
        /// <summary>
        /// 分配空间超时
        /// </summary>
        FAIL_ALLOCATION_TIMEOUT,               /**< ICE ALLOCATION time-out. */
        /// <summary>
        /// 连接代理失败
        /// </summary>
        FAIL_CONN_TURN_BY_PROXY,               /**< HttpConnect feature is available for ICE but the ICE client failed to connect to TURN through the Proxy. */
        /// <summary>
        /// 激活失败
        /// </summary>
        FAIL_ACTIVE_NODE_DOWN                 /**< The active node in the running call went down in the TURN cluster. */
    }
}
