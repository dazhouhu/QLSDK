namespace QLSDK.Core
{
    /// <summary>
    /// FECC操作
    /// </summary>
    public enum FECCAction
    {
        /// <summary>
        /// 未知
        /// </summary>
        UNKNOWN = 0,
        /// <summary>
        /// 开始
        /// </summary>
        START,              /**< Initiates a start action for FECC. */

        /// <summary>
        /// 持续操作
        /// </summary>
        CONTINUE,           /**< Initiates a continue action for FECC; used between the start and stop actions. */

        /// <summary>
        /// 停止结束
        /// </summary>
        STOP,               /**< Initiates a stop action for FECC corresponding to the start action.*/

        MAX
    }
}
