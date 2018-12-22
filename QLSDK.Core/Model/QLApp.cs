using System;

namespace QLSDK.Core
{
    /// <summary>
    /// 应用程序信息
    /// </summary>
    public class QLApp
    {
        #region Constructors
        internal QLApp(IntPtr appHandle, string appName)
        {
            _appHandle = appHandle;
            _appName = appName;
        }
        #endregion

        #region　应用程序句柄
        private IntPtr _appHandle;
        /// <summary>
        /// 应用程序句柄
        /// </summary>
        public IntPtr AppHandle { get { return this._appHandle; } }
        #endregion

        #region 应用程序名
        private string _appName;
        /// <summary>
        /// 应用程序名
        /// </summary>
        public string AppName { get { return this._appName; } }
        #endregion
    }
}
