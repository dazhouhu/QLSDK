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

        public static void ShowForm(Control ownerContainer, Control container)
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

            var x = (ownerContainer.Width - container.Width) / 2;
            var y = (ownerContainer.Height - container.Height) / 2;
            container.Location = new Point(x, y);
            msgPnl.Controls.Add(container);
            container.Disposed += (obj, args) => {
                HideMessage(ownerContainer);
            };
        }

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
        None = 0,
        OK,
        OKCancel,
        YesNo,
        YesNoCancel,
        Answer,
        Hangup,
        AnswerHangup
    }
}
