namespace QLSDK.Core
{
    public class QLManager
    {
        #region Constructors
        private static readonly object lockObj = new object();
        private static QLManager instance = null;
        private QLManager()
        {
        }
        public static QLManager GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new QLManager();
                    }
                }
            }
            return instance;
        }
        #endregion

        /// <summary>
        /// polc sdk 初始化
        /// </summary>
        /// <param name="password">注册用户名</param>
        /// <param name="password">注册用户密码</param>
        /// <param name="qlConfig">初始化配置  获取默认配置：QlConfig.GetDefaultConfig()</param>
        public void Initialize(string username,string password,QlConfig qlConfig)
        {

        }

        /// <summary>
        /// 获取plcm核心句柄
        /// </summary>
        /// <returns></returns>
        //public intPtr GetCoreHandle();

        /// <summary>
        /// 登录绑定polc服务
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="displayName">显示名</param>
        //public void Login(string userName, string passWord, string displayName) { }

        /// <summary>
        /// 获取当前的regId
        /// </summary>
        /// <returns>当前的regId</returns>
        public string GetRegId()
        {
            return string.Empty;
        }

        /// <summary>
        /// 注销polc服务
        /// </summary>
        /// <param name="regId">polm注册的regId</param>
        /// <param name="tag"> TAG标记</param>
        public void Logout(string regId, string tag)
        {

        }

        /// <summary>
        /// 呼叫
        /// </summary>
        /// <param name="protocal">呼叫协议  默认； sip</param>
        /// <param name="dialUri">被呼叫方Uri</param>
        /// <param name="callType">呼叫模式 默认：VIDEO</param>
        /// <returns>呼叫处理器ID</returns>
        public QLCallHandle Call(Protocal protocal, string dialUri, CallMode callMode = CallMode.VIDEO)
        {
            return null;
        }

        /// <summary>
        /// 开启铃声播放
        /// </summary>
        /// <param name="assetPath">铃声文件路径</param>
        /// <param name="isLoop">是否循环</param>
        /// <param name="interval">事件间隔</param>
        public void startAlert(string assetPath, bool isLoop, int interval)
        {

        }

        /// <summary>
        /// 关闭铃声播放
        /// </summary>
        public void StopAlert()
        {

        }
        /// <summary>
        /// 挂断呼叫
        /// </summary>
        /// <param name="callHandle">呼叫处理器</param>
        public void EndCall(QLCallHandle callHandle)
        {

        }
        /// <summary>
        /// 保持呼叫
        /// </summary>
        /// <param name="callHandle">呼叫处理器ID</param>
        public void HoldCall(QLCallHandle callHandle)
        {

        }

        /// <summary>
        /// 接听/应答呼叫
        /// </summary>
        /// <param name="callHandle">呼叫处理器</param>
        /// <param name="callMode">响应呼叫类型  默认 VIDEO</param>
        public void AnswerCall(QLCallHandle callHandle, CallMode callMode)
        {

        }

        /// <summary>
        /// 设置扬声器静音
        /// </summary>
        /// <param name="isMute">是否静音</param>
        public void MuteSpeaker(bool isMute)
        {

        }

        /// <summary>
        /// 切换音频视频模式
        /// </summary>
        /// <param name="callHandle">呼叫处理器ID</param>
        /// <param name="callMode">呼叫类型</param>
        public void changeCallType(QLCallHandle callHandle, CallMode callMode)
        {

        }
        /// <summary>
        /// 切换摄像头
        /// </summary>
        public void switchCamera()
        {

        }

        /// <summary>
        /// 修改扬声器的音量
        /// </summary>
        /// <param name="volume">音量</param>
        public void AdjustSpeakerVolume(int volume)
        {

        }

        /// <summary>
        /// 开启发送content
        /// </summary>
        /// <param name="callHandle">呼叫处理器ID</param>
        /// <param name="bufType">BufType类型</param>
        //public void StartSendContent(QLCallHandle callHandle, BufType bufType){ }

        /// <summary>
        /// 停止发送content
        /// </summary>
        /// <param name="callHandle">呼叫处理器</param>
        public void StopSendContent(QLCallHandle callHandle)
        {

        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Release()
        {

        }
        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="onQlEventListener">事件监听器</param>
        public void AddQlEventListener(OnQlEventListener onQlEventListener)
        {

        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="onQlEventListener">事件监听器</param>
        public void RemoveQlEventListener(OnQlEventListener onQlEventListener)
        {

        }

        /// <summary>
        /// 设置网络带宽HcallRate">呼叫速率</param>
        public void SetNetworkCallRate(int callRate)
        {

        }
    }
}
