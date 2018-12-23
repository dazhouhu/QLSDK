using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace QLSDK.Core
{
    /// <summary>
    /// 设备管理器
    /// </summary>
    public class QLDeviceManager : BaseModel
    {
        #region Fields
        private ILog log = LogUtil.GetLogger("QLSDK.QLDeviceManager");
        private QLConfigManager qlConfig = QLConfigManager.GetInstance();
        private ObservableCollection<QLDevice> devices = new ObservableCollection<QLDevice>();
        #endregion

        #region Constructors
        private static readonly object lockObj = new object();
        private static QLDeviceManager _instance = null;
        private QLDeviceManager() {
            devices.CollectionChanged += (obj, args) =>
            {
                DevicesChanged?.Invoke(_instance, args);
            };
        }

        public static QLDeviceManager GetInstance()
        {
            if (null == _instance)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new QLDeviceManager();
                    }
                }
            }
            return _instance;
        }
        #endregion

        #region Events
        /// <summary>
        /// 设备集合改变事件
        /// </summary>
        public event NotifyCollectionChangedEventHandler DevicesChanged;
        #endregion

        #region 当前设备
        private QLDevice _currentAudioInputDevice;
        /// <summary>
        /// 当前音频输入设备
        /// </summary>
        public QLDevice CurrentAudioInputDevice
        {
            get { return _currentAudioInputDevice; }
            set
            {
                if (_currentAudioInputDevice != value)
                {
                    _currentAudioInputDevice = value;
                    qlConfig.SetProperty(PropertyKey.AUDIO_INPUT_DEVICE, _currentAudioInputDevice?.DeviceHandle,true);
                    NotifyPropertyChanged("CurrentAudioInputDevice");
                }
            }
        }
        private QLDevice _currentAudioOutputDevice;
        /// <summary>
        /// 当前音频输出设备
        /// </summary>
        public QLDevice CurrentAudioOutputDevice
        {
            get { return _currentAudioOutputDevice; }
            set
            {
                if (_currentAudioOutputDevice != value)
                {
                    _currentAudioOutputDevice = value;
                    qlConfig.SetProperty(PropertyKey.AUDIO_OUTPUT_DEVICE, _currentAudioOutputDevice?.DeviceHandle,true);
                    NotifyPropertyChanged("CurrentAudioOutputDevice");
                }
            }
        }
        private QLDevice _currentVideoInputDevice;
        /// <summary>
        /// 当前视频输入设备
        /// </summary>
        public QLDevice CurrentVideoInputDevice
        {
            get { return _currentVideoInputDevice; }
            set
            {
                if (_currentVideoInputDevice != value)
                {
                    _currentVideoInputDevice = value;
                    qlConfig.SetProperty(PropertyKey.VIDEO_INPUT_DEVICE, _currentVideoInputDevice?.DeviceHandle,true);
                    NotifyPropertyChanged("CurrentVideoInputDevice");
                }
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="device">设备</param>
        internal void AddDevice(QLDevice device)
        {
            if (device.DeviceName.Contains("none"))
            {
                return;
            }
            if (!ContainDevice(device.DeviceHandle))
            {
                devices.Add(device);
                switch (device.DeviceType)
                {
                    case DeviceType.AUDIOINPUT:
                    case DeviceType.AUDIOOUTPUT:
                        {
                            var audioInput = GetDevicesByType(DeviceType.AUDIOINPUT).FirstOrDefault();
                            var audioOutput = GetDevicesByType(DeviceType.AUDIOOUTPUT).FirstOrDefault();
                            var inputHandle = audioInput?.DeviceHandle;
                            var outputHandle = audioOutput?.DeviceHandle;
                            if (null == CurrentAudioInputDevice && null != audioInput)
                            {
                                CurrentAudioInputDevice = audioInput;
                            }
                            if (null == CurrentAudioOutputDevice && null != outputHandle)
                            {
                                CurrentAudioOutputDevice = audioOutput;
                            }
                        }
                        break;
                    case DeviceType.VIDEOINPUT:
                        {
                            if (null == CurrentAudioOutputDevice)
                            {
                                var video = GetDevicesByType(DeviceType.VIDEOINPUT).FirstOrDefault();
                                var videoHandle = video?.DeviceHandle;
                                if (null != videoHandle)
                                {
                                    CurrentVideoInputDevice = video;
                                }
                            }
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// 移除设备
        /// </summary>
        /// <param name="deviceHandle">设备ID</param>
        internal void RemoveDevice(string deviceHandle)
        {
            var device = GetDevice(deviceHandle);
            if (null != device)
            {
                RemoveDevice(device);
            }
        }
        /// <summary>
        /// 移除设备
        /// </summary>
        /// <param name="device">设备</param>
        internal void RemoveDevice(QLDevice device)
        {
            if (null != device)
            {
                devices.Remove(device);
                switch(device.DeviceType)
                {
                    case DeviceType.AUDIOINPUT:
                        CurrentAudioInputDevice = GetDevicesByType(DeviceType.AUDIOINPUT).FirstOrDefault();
                        break;
                    case DeviceType.AUDIOOUTPUT:
                        CurrentAudioInputDevice = GetDevicesByType(DeviceType.AUDIOOUTPUT).FirstOrDefault();
                        break;
                    case DeviceType.VIDEOINPUT:
                        CurrentAudioInputDevice = GetDevicesByType(DeviceType.VIDEOINPUT).FirstOrDefault();
                        break;
                }
            }
        }


        /// <summary>
        /// 获取设备
        /// </summary>
        /// <param name="deviceHandle">设备ID</param>
        /// <returns>设备</returns>
        public QLDevice GetDevice(string deviceHandle)
        {
            return devices.FirstOrDefault(d => d.DeviceHandle == deviceHandle);
        }

        /// <summary>
        /// 是否包含设备
        /// </summary>
        /// <param name="deviceHandle">设备ID</param>
        /// <returns>包含:true ,不包含：false</returns>
        public bool ContainDevice(string deviceHandle)
        {
            return null != GetDevice(deviceHandle);
        }

        /// <summary>
        /// 获取指定类型的设备列表
        /// </summary>
        /// <param name="deviceType">设备类型</param>
        /// <returns>设备列表</returns>
        public List<QLDevice> GetDevicesByType(DeviceType deviceType)
        {
            return devices.Where(d => d.DeviceType == deviceType).ToList();
        }
        #endregion

        #region 声音播放
        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="filePath">声音文件路径</param>
        /// <param name="isLoop">是否循环播放</param>
        /// <param name="interval">播放间隔</param>
        public void PlaySound(string filePath, bool isLoop, int interval)
        {
            if (null != CurrentAudioOutputDevice)
            {
                var errno = PlcmProxy.StartPlayAlert(filePath, isLoop, interval);
                if (ErrorNumber.OK != errno)
                {
                    var errMsg = "开启铃声播放失败，ErrorNo=" + errno;
                    log.Error(errMsg);
                    throw new Exception(errMsg);
                }
                log.Info("开启铃声播放成功 filePath:" + filePath);
            }
        }
        /// <summary>
        /// 停止播放声音
        /// </summary>
        public void StopSound()
        {
            if (null != CurrentAudioOutputDevice)
            {
                var errno = PlcmProxy.StopPlayAlert();
                if (ErrorNumber.OK != errno)
                {
                    var errMsg = "关闭铃声播放失败，ErrorNo=" + errno;
                    log.Error(errMsg);
                    throw new Exception(errMsg);
                }
                log.Info("关闭铃声播放成功");
            }
        }
        #endregion

        #region 设备设置        
        /// <summary>
        /// 设置扬声器音量值
        /// </summary>
        /// <param name="volume">音量值</param>
        public void SetMicVolume(int volume)
        {
            var errno = PlcmProxy.SetMicVolume(volume);
            if (ErrorNumber.OK != errno)
            {
                var errMag = "麦克风音量设置失败,errno=" + errno;
                log.Error(errMag);
                throw new Exception(errMag);
            }
            log.Info("麦克风音量设置成功");
        }
        /// <summary>
        /// 获取麦克风音量
        /// </summary>
        /// <returns>音量值</returns>
        public int GetMicVolume()
        {
            log.Info("获取麦克风音量");
            return PlcmProxy.GetMicVolume();
        }

        /// <summary>
        /// 静音扬声器
        /// </summary>
        /// <param name="isMute">是否静音</param>
        public void MuteSpeaker(bool isMute)
        {
            var errno = PlcmProxy.MuteSpeaker(isMute);
            if (ErrorNumber.OK != errno)
            {
                var errMsg = "扬声器静音设置失败,errno=" + errno;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            log.Info("扬声器静音设置成功");
        }
        /// <summary>
        /// 设置扬声器音量值
        /// </summary>
        /// <param name="volume">音量值</param>
        public void SetSpeakerVolume(int volume)
        {
            var errno = PlcmProxy.SetSpeakerVolume(volume);
            if (ErrorNumber.OK != errno)
            {
                var errMsg = "扬声器音量设置失败,errno=" + errno;
                log.Error(errMsg);
                throw new Exception(errMsg);
            }
            log.Info("扬声器音量设置成功");
        }
        /// <summary>
        /// 获取扬声器音量
        /// </summary>
        /// <returns>音量值</returns>
        public int GetSpeakerVolume()
        {
            log.Info("获取扬声器音量");
            return PlcmProxy.GetSpeakerVolume();
        }


        /// <summary>
        /// 打开摄像头
        /// </summary>
        public void StartCamera()
        {
            var errno = PlcmProxy.StartCamera();
            if (ErrorNumber.OK != errno)
            {
                var errMag = "开启摄像头失败,errno=" + errno;
                log.Error(errMag);
                throw new Exception(errMag);
            }
            log.Info("开启摄像头成功");
        }
        /// <summary>
        /// 关闭摄像头
        /// </summary>
        public void StopCamera()
        {
            var errno = PlcmProxy.StopCamera();
            if (ErrorNumber.OK != errno)
            {
                var errMag = "关闭摄像头失败,errno=" + errno;
                log.Error(errMag);
                throw new Exception(errMag);
            }
            log.Info("关闭摄像头成功");
        }

        #endregion
    }
}
