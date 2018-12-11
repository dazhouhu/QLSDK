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
    public partial class FECCPanel : UserControl
    {
        #region Fields
        private QLManager qLManager = QLManager.GetInstance();
        private Timer upTimer = new Timer() { Interval = 200 };
        private Timer leftTimer = new Timer() { Interval = 200 };
        private Timer rightTimer = new Timer() { Interval = 200 };
        private Timer downTimer = new Timer() { Interval = 200 };
        private Timer zoominTimer = new Timer() { Interval = 200 };
        private Timer zoomoutTimer = new Timer() { Interval = 200 };
        #endregion
        #region Constructors
        public FECCPanel()
        {
            InitializeComponent();
            upTimer.Tick += (sender, e) => { qLManager.SendFECC(null, FECCKey.UP, FECCAction.CONTINUE); };
            leftTimer.Tick += (sender, e) => { qLManager.SendFECC(null, FECCKey.LEFT, FECCAction.CONTINUE); };
            rightTimer.Tick += (sender, e) => { qLManager.SendFECC(null, FECCKey.RIGHT, FECCAction.CONTINUE); };
            downTimer.Tick += (sender, e) => { qLManager.SendFECC(null, FECCKey.DOWN, FECCAction.CONTINUE); };
            zoominTimer.Tick += (sender, e) => { qLManager.SendFECC(null, FECCKey.ZOOM_IN, FECCAction.CONTINUE); };
            zoomoutTimer.Tick += (sender, e) => { qLManager.SendFECC(null, FECCKey.ZOOM_OUT, FECCAction.CONTINUE); };
        }
        ~FECCPanel(){
            upTimer.Stop();
            leftTimer.Stop();
            rightTimer.Stop();
            downTimer.Stop();
            zoominTimer.Stop();
            zoomoutTimer.Stop();
        }
        #endregion
        /// <summary>
        /// 关闭
        /// </summary>
        public Action OnCancel { get; set; }
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnCancel?.Invoke();
            this.Dispose();
        }

        #region Up
        private void btnUp_MouseDown(object sender, MouseEventArgs e)
        {
            qLManager.SendFECC(null, FECCKey.UP, FECCAction.START);
            upTimer.Start();
        }

        private void btnUp_MouseUp(object sender, MouseEventArgs e)
        {
            upTimer.Stop();
            qLManager.SendFECC(null, FECCKey.UP, FECCAction.STOP);
        }
        #endregion
        #region Left
        private void btnLeft_MouseDown(object sender, MouseEventArgs e)
        {
            qLManager.SendFECC(null, FECCKey.LEFT, FECCAction.START);
            leftTimer.Start();
        }

        private void btnLeft_MouseUp(object sender, MouseEventArgs e)
        {
            leftTimer.Stop();
            qLManager.SendFECC(null, FECCKey.LEFT, FECCAction.STOP);
        }
        #endregion
        #region Right
        private void btnRight_MouseDown(object sender, MouseEventArgs e)
        {
            qLManager.SendFECC(null, FECCKey.RIGHT, FECCAction.START);
            rightTimer.Start();
        }

        private void btnRight_MouseUp(object sender, MouseEventArgs e)
        {
            rightTimer.Stop();
            qLManager.SendFECC(null, FECCKey.RIGHT, FECCAction.STOP);
        }
        #endregion
        #region Down
        private void btnDown_MouseDown(object sender, MouseEventArgs e)
        {
            qLManager.SendFECC(null, FECCKey.DOWN, FECCAction.START);
            downTimer.Start();
        }

        private void btnDown_MouseUp(object sender, MouseEventArgs e)
        {
            downTimer.Stop();
            qLManager.SendFECC(null, FECCKey.DOWN, FECCAction.STOP);
        }
        #endregion
        #region ZoomIn
        private void btnZoomIn_MouseDown(object sender, MouseEventArgs e)
        {
            qLManager.SendFECC(null, FECCKey.ZOOM_IN, FECCAction.START);
            zoominTimer.Start();
        }

        private void btnZoomIn_MouseUp(object sender, MouseEventArgs e)
        {
            zoominTimer.Stop();
            qLManager.SendFECC(null, FECCKey.ZOOM_IN, FECCAction.STOP);
        }
        #endregion
        #region ZoomOut
        private void btnZoomOut_MouseDown(object sender, MouseEventArgs e)
        {
            qLManager.SendFECC(null, FECCKey.ZOOM_OUT, FECCAction.START);
            zoomoutTimer.Start();
        }

        private void btnZoomOut_MouseUp(object sender, MouseEventArgs e)
        {
            zoomoutTimer.Stop();
            qLManager.SendFECC(null, FECCKey.ZOOM_OUT, FECCAction.STOP);
        }
        #endregion
    }
}
