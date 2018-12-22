namespace QLSDK.Core
{
    /// <summary>
    /// FECC 处理事件
    /// </summary>
    public enum FECCKey
    {
        /// <summary>
        /// 未知
        /// </summary>
        UNKNOWN = 0,
        /// <summary>
        /// 向上
        /// </summary>
        UP,                 /**< Up control. */
        /// <summary>
        /// 向下
        /// </summary>
        DOWN,				/**< Down control. */
        /// <summary>
        /// 向左
        /// </summary>
        LEFT,				/**< Left control. */
        /// <summary>
        /// 向右
        /// </summary>
        RIGHT,				/**< Right control. */
        /// <summary>
        /// 缩小
        /// </summary>
        ZOOM_IN,			/**< Zoom in control. */
        /// <summary>
        /// 放大
        /// </summary>
        ZOOM_OUT,			/**< Zoom out control. */
        MAX
    }
}
