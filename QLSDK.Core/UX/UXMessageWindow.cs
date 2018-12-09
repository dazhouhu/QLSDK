﻿using System;
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
    public partial class UXMessageWindow : Form
    {
        #region Constructors
        public UXMessageWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        public MessageBoxButtonsType MessageBoxButtonsType
        {
            set
            {
                switch (value)
                {
                    case MessageBoxButtonsType.None:
                        {
                            this.btnOK.Visible = false;
                            this.btnCancel.Visible = false;
                            this.btnNo.Visible = false;
                        }
                        break;
                    case MessageBoxButtonsType.OK:
                        {
                            this.btnOK.Image = Properties.Resources.ok24;
                            this.btnOK.Left = 115;
                            this.btnOK.Text = "确定  ";
                            this.btnOK.Visible = true;

                            this.btnCancel.Visible = false;
                            this.btnNo.Visible = false;
                        }
                        break;
                    case MessageBoxButtonsType.OKCancel:
                        {
                            this.btnOK.Image =  Properties.Resources.ok24;
                            this.btnOK.Left = 30;
                            this.btnOK.Text = "确定  ";
                            this.btnOK.Visible = true;


                            this.btnCancel.Image = Properties.Resources.cancel24;
                            this.btnCancel.Left = 200;
                            this.btnCancel.Text = "取消  ";
                            this.btnCancel.Visible = true;

                            this.btnNo.Visible = false;
                        }
                        break;
                    case MessageBoxButtonsType.YesNo:
                        {
                            this.btnOK.Image = Properties.Resources.ok24;
                            this.btnOK.Left = 30;
                            this.btnOK.Text = "是    ";
                            this.btnOK.Visible = true;


                            this.btnNo.Image = Properties.Resources.cancel24;
                            this.btnNo.Left = 200;
                            this.btnNo.Text = "否    ";
                            this.btnNo.Visible = true;

                            this.btnCancel.Visible = false;
                        }
                        break;
                    case MessageBoxButtonsType.YesNoCancel:
                        {
                            this.btnOK.Image = Properties.Resources.ok24;
                            this.btnOK.Left = 10;
                            this.btnOK.Text = "是    ";
                            this.btnOK.Visible = true;


                            this.btnNo.Image = Properties.Resources.cancel24;
                            this.btnNo.Left = 115;
                            this.btnNo.Text = "否    ";
                            this.btnNo.Visible = true;

                            this.btnCancel.Image = Properties.Resources.cancel24;
                            this.btnCancel.Left = 220;
                            this.btnCancel.Text = "取消  ";
                            this.btnCancel.Visible = true;
                        }
                        break;
                    case MessageBoxButtonsType.Answer:
                        {
                            this.btnOK.Image = Properties.Resources.call24;
                            this.btnOK.Left = 115;
                            this.btnOK.Text = "接听  ";
                            this.btnOK.Visible = true;

                            this.btnCancel.Visible = false;
                            this.btnNo.Visible = false;
                        }
                        break;
                    case MessageBoxButtonsType.Hangup:
                        {
                            this.btnOK.Image = Properties.Resources.hangup24;
                            this.btnOK.Left = 115;
                            this.btnOK.Text = "挂断  ";
                            this.btnOK.Visible = true;

                            this.btnCancel.Visible = false;
                            this.btnNo.Visible = false;
                        }
                        break;
                    case MessageBoxButtonsType.AnswerHangup:
                        {
                            this.btnOK.Image = Properties.Resources.call24;
                            this.btnOK.Left = 30;
                            this.btnOK.Text = "接听  ";
                            this.btnOK.Visible = true;


                            this.btnCancel.Image = Properties.Resources.hangup24;
                            this.btnCancel.Left = 200;
                            this.btnCancel.Text = "挂断  ";
                            this.btnCancel.Visible = true;

                            this.btnNo.Visible = false;
                        }
                        break;
                    default:
                        {
                            this.btnOK.Visible = false;
                            this.btnCancel.Visible = false;
                            this.btnNo.Visible = false;
                        }
                        break;
                }
            }
        }

        public MessageBoxIcon MessageBoxIcon
        {
            set
            {
                switch (value)
                {
                    case MessageBoxIcon.None:
                        {
                            msgIcon.Visible = false;
                        }
                        break;
                    case MessageBoxIcon.Error:
                        {
                            msgIcon.Visible = true;
                            msgIcon.Image = Properties.Resources.error;
                        }
                        break;
                    case MessageBoxIcon.Information:
                        {
                            msgIcon.Visible = true;
                            msgIcon.Image = Properties.Resources.info;
                        }
                        break;
                    case MessageBoxIcon.Question:
                        {
                            msgIcon.Visible = true;
                            msgIcon.Image = Properties.Resources.question;
                        }
                        break;
                    case MessageBoxIcon.Warning:
                        {
                            msgIcon.Visible = true;
                            msgIcon.Image = Properties.Resources.alert;
                        }
                        break;
                    default:
                        {
                            msgIcon.Visible = false;
                        }
                        break;
                }
            }
        }

        public string Message
        {
            set
            {
                this.lblMsg.Text = value;
            }
        }

        #endregion

        #region Actions
        public Action OKAction { get; set; }
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            OKAction?.Invoke();
            this.Close();
        }

        public Action CancelAction { get; set; }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            CancelAction?.Invoke();
            this.Close();
        }
        public Action NoAction { get; set; }
        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            NoAction?.Invoke();
            this.Close();
        }
        #endregion
    }
}
