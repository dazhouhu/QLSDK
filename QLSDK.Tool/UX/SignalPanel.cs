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
    public partial class SignalPanel : UserControl
    {
        #region Fields
        #endregion
        #region Constructors
        private static SignalPanel instance = null;
        public SignalPanel()
        {
            InitializeComponent();
            grdMediaStatistics.AutoGenerateColumns = false;

            this.txtCallRate.Text = "SIP";
            this.txtCallRate.Text = QLConfig.GetInstance().GetProperty(PropertyKey.PLCM_MFW_KVLIST_KEY_CallSettings_NetworkCallRate);
        }
        private static SignalPanel GetInstamce()
        {
            if (null == instance)
            {
                instance = new SignalPanel();
            }
            return instance;
        }
        #endregion

        public Action OnClose { get; set; }
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke();
            this.Dispose();
        }
        public void BindData(IEnumerable<QLMediaStatistics> statistics)
        {
            this.grdMediaStatistics.DataSource = null;
            this.grdMediaStatistics.DataSource = statistics;
        }

        private void SignalPanel_Load(object sender, EventArgs e)
        {
        }
    }
}
