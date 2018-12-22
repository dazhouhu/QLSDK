namespace QLSDK.Core
{
    /// <summary>
    /// 设备
    /// </summary>
    public class QLDevice
    {

        #region Constructors
        internal QLDevice(DeviceType deviceType, string deviceHandle, string deviceName)
        {
            this._deviceType = deviceType;
            this._deviceHandle = deviceHandle;
            this._deviceName = deviceName;
        }
        #endregion

        #region　设备ID
        private string _deviceHandle;
        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceHandle { get { return this._deviceHandle; } }
        #endregion

        #region 设备名
        private string _deviceName;
        /// <summary>
        /// 设备名
        /// </summary>
        public string DeviceName { get { return this._deviceName; } }
        #endregion

        #region 设备类型
        private DeviceType _deviceType;
        /// <summary>
        /// 设备类型
        /// </summary>
        public DeviceType DeviceType { get { return this._deviceType; } }
        #endregion
    }
}
