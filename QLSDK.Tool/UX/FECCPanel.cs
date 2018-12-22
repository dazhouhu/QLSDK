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
        #endregion
        #region Constructors
        public FECCPanel()
        {
            InitializeComponent();
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
            try
            {
                QLCallManager.GetInstance().CurrentCall.StartSendFECC(FECCKey.UP);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUp_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                QLCallManager.GetInstance().CurrentCall.StopSendFECC();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Left
        private void btnLeft_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                QLCallManager.GetInstance().CurrentCall.StartSendFECC(FECCKey.LEFT);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLeft_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                QLCallManager.GetInstance().CurrentCall.StopSendFECC();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Right
        private void btnRight_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                QLCallManager.GetInstance().CurrentCall.StartSendFECC(FECCKey.RIGHT);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRight_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                QLCallManager.GetInstance().CurrentCall.StopSendFECC();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Down
        private void btnDown_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                QLCallManager.GetInstance().CurrentCall.StartSendFECC(FECCKey.DOWN);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDown_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                QLCallManager.GetInstance().CurrentCall.StopSendFECC();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #region ZoomIn
        private void btnZoomIn_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                QLCallManager.GetInstance().CurrentCall.StartSendFECC(FECCKey.ZOOM_IN);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnZoomIn_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                QLCallManager.GetInstance().CurrentCall.StopSendFECC();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #region ZoomOut
        private void btnZoomOut_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                QLCallManager.GetInstance().CurrentCall.StartSendFECC(FECCKey.ZOOM_OUT);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnZoomOut_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                QLCallManager.GetInstance().CurrentCall.StopSendFECC();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
