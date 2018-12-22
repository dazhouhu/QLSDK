using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLSDK.Core;

namespace QLSDK.Tool.UX
{
    public partial class DeviceManagerPanel : UserControl
    {
        private static QLDeviceManager deviceManager = QLDeviceManager.GetInstance();
        public DeviceManagerPanel()
        {
            InitializeComponent();
        }

        private void DeviceManagerPanel_Load(object sender, EventArgs e)
        {
            //音频输入设备
            var audioInputDevices = deviceManager.GetDevicesByType(DeviceType.AUDIOINPUT);
            cbxAudioInput.DataSource = audioInputDevices;
            cbxAudioInput.SelectedItem = deviceManager.CurrentAudioInputDevice;
            //音频输出设备
            var audioOutputDevices = deviceManager.GetDevicesByType(DeviceType.AUDIOOUTPUT);
            cbxAudioOutput.DataSource = audioOutputDevices;
            cbxAudioOutput.SelectedItem = deviceManager.CurrentAudioOutputDevice;
            //视频输入设备
            var deviceInputDevices = deviceManager.GetDevicesByType(DeviceType.VIDEOINPUT);
            cbxVideoInput.DataSource = deviceInputDevices;
            cbxVideoInput.SelectedItem = deviceManager.CurrentVideoInputDevice;
        }
        public Action OnCancel { get; set; }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancel?.Invoke();
            this.Dispose();
        }
        public Action OKAction { get; set; }
        private void btnOK_Click(object sender, EventArgs e)
        {
            deviceManager.CurrentAudioInputDevice = (QLDevice)cbxAudioInput.SelectedItem;
            deviceManager.CurrentAudioOutputDevice = (QLDevice)cbxAudioOutput.SelectedItem;
            deviceManager.CurrentVideoInputDevice = (QLDevice)cbxVideoInput.SelectedItem;

            OKAction?.Invoke();
            this.Dispose();
        }
    }
}
