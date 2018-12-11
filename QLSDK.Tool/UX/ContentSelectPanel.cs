using QLSDK.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSDK.Tool.UX
{
    public partial class ContentSelectPanel : UserControl
    {
        public ContentSelectPanel()
        {
            InitializeComponent();
        }

        public void BindAppData(IEnumerable<QLDevice> apps)
        {
            cbxApp.DataSource = apps.ToList();
        }

        private void rdoBFCP_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoBFCP.Checked)
            {
                cbxFormat.Enabled = true;
                cbxMonitor.Enabled = false;
                cbxApp.Enabled = false;
            }
            else
            {
                cbxFormat.Enabled = false;
                cbxMonitor.Enabled = true;
                cbxApp.Enabled = true;
            }
        }

        private void rdMonitor_CheckedChanged(object sender, EventArgs e)
        {
            if (rdMonitor.Checked)
            {
                cbxMonitor.Enabled = true;
                cbxApp.Enabled = true;
                cbxFormat.Enabled = false;
            }
            else
            {
                cbxMonitor.Enabled = false;
                cbxApp.Enabled = false;
                cbxFormat.Enabled = false;
            }
        }

        public Func<string, ImageFormat, string,string,bool> OKAction { get; set; }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if(null != OKAction)
            {
                var type = rdMonitor.Checked ? "Monitor" : "BFCP";
                var format = ImageFormat.YV12;
                if (cbxFormat.SelectedIndex >= 0)
                {
                    format = (ImageFormat)cbxFormat.SelectedIndex;
                }
                string monitor = null;
                if(cbxMonitor.SelectedIndex >= 0)
                {
                    monitor = cbxMonitor.SelectedValue.ToString();
                }
                string app = null;
                if(cbxApp.SelectedIndex>=0)
                {
                    app =  cbxApp.SelectedValue.ToString();
                }
                var result = OKAction(type,format,monitor, app);
                if(result)
                {
                    this.Dispose();
                }
            }
            this.Dispose();
        }

        public Action OnCancel { get; set; }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancel?.Invoke();
            this.Dispose();
        }

        private void ContentSelectPanel_Load(object sender, EventArgs e)
        {
            cbxFormat.SelectedIndex = 0;

            cbxMonitor.DataSource = QLDeviceManager.GetInstance().GetDevicesByType(DeviceType.MONITOR);
            cbxMonitor.SelectedIndex = 0;
        }
    }
}
