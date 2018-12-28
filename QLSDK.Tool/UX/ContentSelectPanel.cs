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

        public void BindAppData(IEnumerable<QLApp> apps)
        {
            cbxApp.DataSource = apps.ToList();
        }

        public Func<ImageFormat,ContentType,object,bool> OKAction { get; set; }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if(null != OKAction)
            {
                var imageFormat = ImageFormat.RGBA;
                if (cbxFormat.SelectedIndex > 0)
                {
                    imageFormat = (ImageFormat)cbxFormat.SelectedIndex;
                }

                var contentType = rdoMonitor.Checked ? ContentType.Monitor:ContentType.Application;
                
                object contentHandle=null;
                if (rdoMonitor.Checked)
                {
                    if (cbxMonitor.SelectedIndex >= 0)
                    {
                        contentHandle = cbxMonitor.SelectedValue;
                    }
                }
                else
                {
                    if (cbxApp.SelectedIndex >= 0)
                    {
                        contentHandle = cbxApp.SelectedValue;
                    }
                }
                var result = OKAction(imageFormat,contentType, contentHandle);
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

        private void rdoMonitor_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoMonitor.Checked)
            {
                cbxMonitor.Enabled = true;
                cbxApp.Enabled = false;
            }
            else
            {
                cbxMonitor.Enabled = false;
                cbxApp.Enabled = true;
            }
        }

        private void rdoApp_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoApp.Checked)
            {
                cbxMonitor.Enabled = false;
                cbxApp.Enabled = true;
            }
            else
            {
                cbxMonitor.Enabled = true;
                cbxApp.Enabled = false;
            }
        }
    }
}
