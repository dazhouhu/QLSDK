namespace QLSDK.Core
{
    /// <summary>
    /// 证书类型
    /// </summary>
    public enum CertificateChoiceType
    {
        /// <summary>
        /// 不可信任
        /// </summary>
        NOT_TRUST = 0,
        /// <summary>
        /// 信任一次
        /// </summary>
        TRUST_ONCE,
        /// <summary>
        /// 总是信任
        /// </summary>
        TRUST_ALWAYS
    }
}
