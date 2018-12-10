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
            var audioInputDevices = deviceManager.GetDevicesByType(DeviceType.AUDIOINPUT);
            cbxAudioInput.DataSource = audioInputDevices;
            if (audioInputDevices.Count > 0)
            {
                cbxAudioInput.SelectedIndex = 0;
            }
            var audioOutputDevices = deviceManager.GetDevicesByType(DeviceType.AUDIOOUTPUT);
            cbxAudioOutput.DataSource = audioOutputDevices;
            if (audioOutputDevices.Count > 0)
            {
                cbxAudioOutput.SelectedIndex = 0;
            }
            var deviceInputDevices = deviceManager.GetDevicesByType(DeviceType.VIDEOINPUT);
            cbxVideoInput.DataSource = deviceInputDevices;
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
            OKAction?.Invoke();
            this.Dispose();
        }
    }
}
