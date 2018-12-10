using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSDK.Core
{
    public class QLDeviceManager : BaseModel
    {
        #region Fields
        private ILog log = LogUtil.GetLogger("QLSDK.QLDeviceManager");
        private QLConfig qlConfig = QLConfig.GetInstance();
        private ObservableCollection<QLDevice> devices = new ObservableCollection<QLDevice>();
        #endregion

        #region Constructors
        private static readonly object lockObj = new object();
        private static QLDeviceManager _instance = null;
        private QLDeviceManager() { }

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
        public event NotifyCollectionChangedEventHandler DevicesChanged;
        #endregion

        #region CurrentDevice
        private QLDevice _currentAudioInputDevice;
        public QLDevice CurrentAudioInputDevice
        {
            get { return _currentAudioInputDevice; }
            set
            {
                if (_currentAudioInputDevice != value)
                {
                    _currentAudioInputDevice = value;
                    qlConfig.SetProperty(PropertyKey.AUDIO_INPUT_DEVICE, _currentAudioInputDevice?.DeviceHandle);
                    NotifyPropertyChanged("CurrentAudioInputDevice");
                }
            }
        }
        private QLDevice _currentAudioOutputDevice;
        public QLDevice CurrentAudioOutputDevice
        {
            get { return _currentAudioOutputDevice; }
            set
            {
                if (_currentAudioOutputDevice != value)
                {
                    _currentAudioOutputDevice = value;
                    qlConfig.SetProperty(PropertyKey.AUDIO_OUTPUT_DEVICE, _currentAudioOutputDevice?.DeviceHandle);
                    NotifyPropertyChanged("CurrentAudioOutputDevice");
                }
            }
        }
        private QLDevice _currentVideoInputDevice;
        public QLDevice CurrentVideoInputDevice
        {
            get { return _currentVideoInputDevice; }
            set
            {
                if (_currentVideoInputDevice != value)
                {
                    _currentVideoInputDevice = value;
                    qlConfig.SetProperty(PropertyKey.VIDEO_INPUT_DEVICE, _currentVideoInputDevice?.DeviceHandle);
                    NotifyPropertyChanged("CurrentVideoInputDevice");
                }
            }
        }

        #endregion

        #region Methods
        public void AddDevice(QLDevice device)
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
                                PlcmProxy.SetAudioDevice(inputHandle, outputHandle);
                                PlcmProxy.SetAudioDeviceForRingtone(outputHandle);
                            }
                            if (null == CurrentAudioOutputDevice && null != outputHandle)
                            {
                                CurrentAudioOutputDevice = audioOutput;
                                PlcmProxy.SetAudioDevice(inputHandle, outputHandle);
                                PlcmProxy.SetAudioDeviceForRingtone(outputHandle);
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
                                    PlcmProxy.SetVideoDevice(videoHandle);
                                }
                            }
                        }
                        break;
                }
            }
        }
        public void RemoveDevice(string deviceHandle)
        {
            var device = GetDevice(deviceHandle);
            if (null != device)
            {
                RemoveDevice(device);
            }
        }
        public void RemoveDevice(QLDevice device)
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

        public QLDevice GetDevice(string deviceHandle)
        {
            return devices.FirstOrDefault(d => d.DeviceHandle == deviceHandle);
        }
        public bool ContainDevice(string deviceHandle)
        {
            return null != GetDevice(deviceHandle);
        }

        public List<QLDevice> GetDevicesByType(DeviceType deviceType)
        {
            return devices.Where(d => d.DeviceType == deviceType).ToList();
        }
        #endregion

        #region Sound

        public void PlaySound(string filePath, bool isLoop, int interval)
        {
            log.Info("startPlayingAlert filePath:" + filePath);
            var errno = PlcmProxy.StartPlayAlert(filePath, isLoop, interval);
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("开启铃声播放失败，ErrorNo=" + errno);
            }
        }
        public void StopSound()
        {
            var errno = PlcmProxy.StopPlayAlert();
            if (ErrorNumber.OK != errno)
            {
                throw new Exception("关闭铃声播放失败，ErrorNo=" + errno);
            }
        }
        #endregion
    }
}
