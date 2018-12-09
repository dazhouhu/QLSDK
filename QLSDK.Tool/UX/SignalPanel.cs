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
        }
        private  static SignalPanel GetInstamce()
        {
            if(null == instance)
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
        public void BindSignals(IEnumerable<QLMediaStatistics> statistics)
        {
            var ds = statistics.ToList();
           
            this.grdMediaStatistics.DataSource = ds;
            this.grdMediaStatistics.Refresh();
        }

        private void SignalPanel_Load(object sender, EventArgs e)
        {
            grdMediaStatistics.AutoGenerateColumns = false;

            QLSDKCore.GetMediaStatistics(BindSignals);
            
            this.txtCallRate.Text = "SIP";
            this.txtCallRate.Text = QlConfig.GetInstance().GetProperty(PropertyKey.PLCM_MFW_KVLIST_KEY_CallSettings_NetworkCallRate);
        }
        
    }
}
