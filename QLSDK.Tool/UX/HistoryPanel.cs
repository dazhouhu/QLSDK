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
    public partial class HistoryPanel : UserControl
    {
        public HistoryPanel()
        {
            InitializeComponent();
        }
        public Action OnClose { get; set; }
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke();
            this.Dispose();
        }
        public void BindData(IEnumerable<QLCall> calls)
        {
            this.grdCalls.DataSource = null;
            this.grdCalls.DataSource = calls;
        }

        private void SignalPanel_Load(object sender, EventArgs e)
        {
        }
    }
}
