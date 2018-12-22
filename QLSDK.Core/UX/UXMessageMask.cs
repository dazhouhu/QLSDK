using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSDK.Core
{
    /// <summary>
    /// 遮罩层显示控件
    /// </summary>
    public partial class UXMessageMask : Panel
    {
        private UXMessageMask()
        {
            InitializeComponent();
            this.BackColor = Color.Transparent;
            SetStyle(ControlStyles.Opaque | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            /*
            Color drawColor = Color.FromArgb(127, this.BackColor);
            //// 定义画笔
            Pen labelBorderPen = new Pen(drawColor, 0);
            SolidBrush labelBackColorBrush = new SolidBrush(drawColor);
            //// 绘制背景色
            pe.Graphics.DrawRectangle(labelBorderPen, 0, 0, Size.Width, Size.Height);
            pe.Graphics.FillRectangle(labelBackColorBrush, 0, 0, Size.Width, Size.Height);
            */

            base.OnPaint(pe);
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="ownerContainer">所属容器</param>
        /// <param name="isModal">是否模态框显示</param>
        /// <param name="msg">消息</param>
        /// <param name="btnType">按钮类型</param>
        /// <param name="boxIcon">图标</param>
        /// <param name="okAction">ok回调</param>
        /// <param name="cancelAction">cancel回调</param>
        /// <param name="noAction">no回调</param>
        public static void ShowMessage(Control ownerContainer, bool isModal, string msg, MessageBoxButtonsType btnType, MessageBoxIcon boxIcon
            , Action okAction = null, Action cancelAction = null, Action noAction = null)
        {
            HideMessage(ownerContainer);

            var msgPnl = new UXMessageMask()
            {
                Name = "msgPnl"
            };
            msgPnl.Left = 0;
            msgPnl.Top = 0;
            msgPnl.Width = ownerContainer.Width;
            msgPnl.Height = ownerContainer.Height;
            msgPnl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            ownerContainer.Controls.Add(msgPnl);
            msgPnl.BringToFront();

            if (isModal)
            {
                var win = new UXMessageWindow()
                {
                    Message = msg,
                    MessageBoxButtonsType = btnType,
                    MessageBoxIcon = boxIcon,
                    OKAction = okAction,
                    CancelAction = cancelAction,
                    NoAction = noAction,
                    Owner = ownerContainer.FindForm()
                };
                win.FormClosed += (sender, args) => { HideMessage(ownerContainer); };
                win.ShowDialog();
            }
            else
            {
                var msgBox = new UXMessagePanel()
                {
                    Message = msg,
                    MessageBoxButtonsType = btnType,
                    MessageBoxIcon = boxIcon,
                    OKAction = okAction,
                    CancelAction = cancelAction,
                    NoAction = noAction
                };
                var x = (ownerContainer.Width - msgBox.Width) / 2;
                var y = (ownerContainer.Height - msgBox.Height) / 2;
                msgBox.Location = new Point(x, y);
                msgBox.Disposed += (obj, args) => { HideMessage(ownerContainer); };
                msgPnl.Controls.Add(msgBox);
            }
        }

        /// <summary>
        /// 隐藏消息
        /// </summary>
        /// <param name="ownerContainer">容器控件</param>
        public static void HideMessage(Control ownerContainer)
        {
            if (null == ownerContainer)
            {
                return;
            }
            if (ownerContainer.Controls.ContainsKey("msgPnl"))
            {
                ownerContainer.Controls.RemoveByKey("msgPnl");
            }
        }

        /// <summary>
        /// 显示表单
        /// </summary>
        /// <param name="ownerContainer">所属容器</param>
        /// <param name="form">表单控件</param>
        public static void ShowForm(Control ownerContainer, Control form)
        {
            HideMessage(ownerContainer);

            var msgPnl = new UXMessageMask()
            {
                Name = "msgPnl"
            };
            msgPnl.Left = 0;
            msgPnl.Top = 0;
            msgPnl.Width = ownerContainer.Width;
            msgPnl.Height = ownerContainer.Height;
            msgPnl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            ownerContainer.Controls.Add(msgPnl);
            msgPnl.BringToFront();

            var x = (ownerContainer.Width - form.Width) / 2;
            var y = (ownerContainer.Height - form.Height) / 2;
            form.Location = new Point(x, y);
            msgPnl.Controls.Add(form);
            form.Disposed += (obj, args) => {
                HideMessage(ownerContainer);
            };
        }

        /// <summary>
        /// 大小改变时，重绘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UXMessageMask_SizeChanged(object sender, EventArgs e)
        {
            if(this.Controls.Count>0)
            {
                foreach (var control in this.Controls)
                {
                    var pnl = control as UserControl;
                    if (null != pnl)
                    {
                        var x = (this.Width - pnl.Width) / 2;
                        var y = (this.Height - pnl.Height) / 2;
                        pnl.Location = new Point(x, y);
                    }
                }
            }
        }
    }

    public enum MessageBoxButtonsType
    {
        /// <summary>
        /// 无按钮
        /// </summary>
        None = 0,
        /// <summary>
        /// 确认按钮
        /// </summary>  
        OK,
        /// <summary>
        /// 确认，取消按钮
        /// </summary>
        OKCancel,
        /// <summary>
        /// 是，否按钮
        /// </summary>
        YesNo,
        /// <summary>
        /// 是，否，取消按钮 
        /// </summary>
        YesNoCancel,
        /// <summary>
        /// 接听按钮
        /// </summary>
        Answer,
        /// <summary>
        /// 挂断按钮 
        /// </summary>
        Hangup,
        /// <summary>
        /// 接听，挂断按钮
        /// </summary>
        AnswerHangup
    }
}
