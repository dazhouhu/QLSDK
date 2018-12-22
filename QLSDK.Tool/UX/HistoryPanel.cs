using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QLSDK.Core;
using System.Drawing;

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

        private void HistoeryPanel_Load(object sender, EventArgs e)
        {
            this.grdCalls.AutoGenerateColumns = false;
        }

        public void BindData(IEnumerable<QLCall> calls)
        {
            this.grdCalls.DataSource = null;
            this.grdCalls.DataSource = calls.OrderByDescending(c=>c.StartTime).ToList();
        }

        #region gvCall
        private void grdCalls_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var dgv = (DataGridView)sender;
            if (e.ColumnIndex < 0)
            {
                return;
            }
            switch(dgv.Columns[e.ColumnIndex].Name)
            {
                #region CallType
                case "CallType":
                    {
                        var type = string.Empty;
                        var callType = (CallType)e.Value;
                        switch (callType)
                        {
                            case QLSDK.Core.CallType.UNKNOWN: break;

                            case QLSDK.Core.CallType.INCOMING:   /* UAS received INVITE, from CC */
                                type = "呼入";
                                break;
                            case QLSDK.Core.CallType.OUTGOING:   /* from CC */
                                type = "呼出";
                                break;                            
                        }
                        e.Value = type;
                        e.FormattingApplied = true;
                    }
                    break;
                #endregion
                #region CallMode
                case "CallMode":
                    {
                        var mode = string.Empty;
                        var callMode = (CallMode)e.Value;
                        switch (callMode)
                        {
                            case QLSDK.Core.CallMode.VIDEO:   /* UAS received INVITE, from CC */
                                mode = "视频通话";
                                break;
                            case QLSDK.Core.CallMode.AUDIO:   /* from CC */
                                mode = "语音通话";
                                break;
                        }
                        e.Value = mode;
                        e.FormattingApplied = true;
                    }
                    break;
                #endregion
                #region StartTime
                case "StartTime":
                    {
                        var start = string.Empty;
                        var startTime = (DateTime)e.Value;
                        var now = DateTime.Now;
                        TimeSpan timeSpan = now - startTime;
                        if(timeSpan.TotalMinutes<=5)
                        {
                            start = "刚刚";
                        }
                        else if(timeSpan.TotalHours<1)
                        {
                            start = timeSpan.Minutes + "分前";
                        }
                        else if(timeSpan.TotalDays <1)
                        {
                            start = timeSpan.Hours + "小时前";
                        }
                        else if(timeSpan.TotalDays<7)
                        {
                            start = timeSpan.Days + "天前";
                        }
                        else
                        {
                            start = startTime.ToString("MM-dd HH:mm");
                        }
                        e.Value = start;
                        e.FormattingApplied = true;
                    }
                    break;
                #endregion
                #region ConnectedTime
                    case "ConnectedTime":
                    {
                        var times = string.Empty;
                        var connectedTime = (DateTime?)e.Value;
                        if (connectedTime.HasValue)
                        {                            
                            var calls = dgv.DataSource as IEnumerable<QLSDK.Core.QLCall>;
                            if(null != calls)
                            {
                                var call = calls.Skip(e.RowIndex).FirstOrDefault();
                                if(null != call)
                                {
                                    var unconnectTime = DateTime.Now;
                                    if (call.UnconnectedTime.HasValue)
                                    {
                                        unconnectTime = call.UnconnectedTime.Value;
                                    }
                                    else if (call.StopTime.HasValue)
                                    {
                                        unconnectTime = call.StopTime.Value;
                                    }
                                    TimeSpan timeSpan = unconnectTime - connectedTime.Value;
                                    if (timeSpan.TotalHours < 1)
                                    {
                                        times = string.Format("{0}分{1}秒", timeSpan.Minutes, timeSpan.Seconds);
                                    }
                                    else if (timeSpan.TotalDays <= 1)
                                    {
                                        times = string.Format("{0}时{1}分", timeSpan.Hours, timeSpan.Minutes);
                                    }
                                    else
                                    {
                                        times = string.Format("{0}天{1}时{2}分",(int)timeSpan.TotalDays, timeSpan.Hours, timeSpan.Minutes);
                                    }
                                }
                            }
                        }
                        e.Value = times;
                        e.FormattingApplied = true;
                    }
                    break;
                #endregion

                #region CallState
                case "CallState":
                    {
                        var status = string.Empty;
                        var callState = (CallState)e.Value;
                        switch (callState)
                        {
                            case QLSDK.Core.CallState.SIP_UNKNOWN: break;

                            case QLSDK.Core.CallState.SIP_INCOMING_INVITE:   /* UAS received INVITE, from CC */
                                status = "呼入待接听";
                                break;
                            case QLSDK.Core.CallState.SIP_INCOMING_CONNECTED:   /* from CC */
                                status = "呼入通话中";
                                break;
                            case QLSDK.Core.CallState.SIP_CALL_HOLD:  /* local hold */
                                status = "主动保持";
                                break;
                            case QLSDK.Core.CallState.SIP_CALL_HELD:  /* far site hold */
                                status = "被动保持";
                                break;
                            case QLSDK.Core.CallState.SIP_CALL_DOUBLE_HOLD:  /* both far-site and local hold */
                                status = "双边保持";
                                break;

                            case QLSDK.Core.CallState.SIP_OUTGOING_TRYING:         /* UAC get 100, from CC */
                                status = "呼出中";
                                break;
                            case QLSDK.Core.CallState.SIP_OUTGOING_RINGING:       /* UAC get 180 from CC */
                                status = "呼出响铃";
                                break;                       //SIP_OUTGOING_ANSWERED,  /* UAC get 200 from CC */
                            case QLSDK.Core.CallState.SIP_OUTGOING_CONNECTED:  /* from CC */
                                status = "呼出通话中";
                                break;
                            case QLSDK.Core.CallState.SIP_CALL_CLOSED:
                                status = "通话关闭";
                                break;
                            case QLSDK.Core.CallState.SIP_OUTGOING_FAILURE:
                                status = "呼出失败";
                                break;
                        }
                        e.Value = status;
                        e.FormattingApplied = true;
                    }
                    break;
                    #endregion
            }
        }

        private void grdCalls_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            var dgv = (DataGridView)sender;
            if (e.ColumnIndex < 0)
            {
                return;
            }
            if (e.RowIndex < 0)
            {
                return;
            }
            var isHandled =true;
            
            var calls = dgv.DataSource as IEnumerable<QLSDK.Core.QLCall>;
            if (null != calls)
            {
                var call = calls.Skip(e.RowIndex).FirstOrDefault();
                if (null != call)
                {
                    var columnName = dgv.Columns[e.ColumnIndex].Name;
                    switch(columnName)
                    {
                        case "btnAnswer":
                        case "btnHold":
                        case "btnResume":
                        case "btnHangup":
                        case "btnAudioCall":
                        case "btnVideoCall":break;
                        default:isHandled = false;break;
                    }
                    switch (call.CallState)
                    {
                        case QLSDK.Core.CallState.SIP_UNKNOWN:
                        case QLSDK.Core.CallState.SIP_CALL_CLOSED:
                        case QLSDK.Core.CallState.SIP_OUTGOING_FAILURE:
                            {
                                switch(columnName)
                                {
                                    case "btnAudioCall":
                                        isHandled = false;
                                        break;
                                    case "btnVideoCall":
                                        {
                                            if(null != QLDeviceManager.GetInstance().CurrentVideoInputDevice)
                                            {
                                                isHandled = false;
                                            }
                                        }break;
                                }
                            }
                            break;
                        case QLSDK.Core.CallState.SIP_INCOMING_INVITE:
                            if ("btnAnswer" == columnName)
                            {
                                isHandled = false;
                            }
                            break;
                        case QLSDK.Core.CallState.SIP_INCOMING_CONNECTED:   /* from CC */
                            switch (columnName)
                            {
                                case "btnHold": isHandled = false; break;
                                case "btnHangup": isHandled = false; break;
                            }
                            break;
                        case QLSDK.Core.CallState.SIP_CALL_HOLD:  /* local hold */
                            switch (columnName)
                            {
                                case "btnResume": isHandled = false; break;
                                case "btnHangup": isHandled = false; break;
                            }
                            break;
                        case QLSDK.Core.CallState.SIP_CALL_HELD:  /* far site hold */
                            switch (columnName)
                            {
                                case "btnHold": isHandled = false; break;
                                case "btnHangup": isHandled = false; break;
                            }
                            break;
                        case QLSDK.Core.CallState.SIP_CALL_DOUBLE_HOLD:  /* both far-site and local hold */
                            switch (columnName)
                            {
                                case "btnResume": isHandled = false; break;
                                case "btnHangup": isHandled = false; break;
                            }
                            break;
                        case QLSDK.Core.CallState.SIP_OUTGOING_TRYING:         /* UAC get 100, from CC */
                            switch (columnName)
                            {
                                case "btnHangup": isHandled = false; break;
                            }
                            break;
                        case QLSDK.Core.CallState.SIP_OUTGOING_RINGING:       /* UAC get 180 from CC */
                            switch (columnName)
                            {
                                case "btnHangup": isHandled = false; break;
                            }
                            break;                       //SIP_OUTGOING_ANSWERED,  /* UAC get 200 from CC */
                        case QLSDK.Core.CallState.SIP_OUTGOING_CONNECTED:  /* from CC */
                            switch (columnName)
                            {
                                case "btnHold": isHandled = false; break;
                                case "btnHangup": isHandled = false; break;
                            }
                            break;
                    }
                }
            }
            
            if (isHandled)
            {
                using (Brush gridBrush = new SolidBrush(dgv.GridColor), backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                }
            }
            e.Handled = isHandled;
        }
        private void grdCalls_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = (DataGridView)sender;
            if (e.ColumnIndex < 0)
            {
                return;
            }
            if (e.RowIndex < 0)
            {
                return;
            }
            var calls = dgv.DataSource as IEnumerable<QLSDK.Core.QLCall>;
            if (null != calls)
            {
                var call = calls.Skip(e.RowIndex).FirstOrDefault();
                if (null != call)
                {
                    var columnName = dgv.Columns[e.ColumnIndex].Name;
                    switch (columnName)
                    {
                        #region Answer
                        case "btnAnswer":
                            {
                                switch(call.CallState)
                                {
                                    case QLSDK.Core.CallState.SIP_UNKNOWN:
                                    case QLSDK.Core.CallState.SIP_CALL_CLOSED:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_FAILURE:
                                        break;
                                    case QLSDK.Core.CallState.SIP_INCOMING_INVITE:
                                        {
                                            if(null == QLDeviceManager.GetInstance().CurrentVideoInputDevice)
                                            {
                                                call.AnswerCall(QLSDK.Core.CallMode.AUDIO);
                                            }
                                            else
                                            {
                                                call.AnswerCall(call.CallMode);
                                            }                                            
                                        }break;
                                    case QLSDK.Core.CallState.SIP_INCOMING_CONNECTED:
                                    case QLSDK.Core.CallState.SIP_CALL_HOLD:
                                    case QLSDK.Core.CallState.SIP_CALL_HELD:
                                    case QLSDK.Core.CallState.SIP_CALL_DOUBLE_HOLD:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_TRYING:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_RINGING:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_CONNECTED:break;
                                }
                            }
                            break;
                        #endregion
                        #region Hold
                        case "btnHold":
                            {
                                switch (call.CallState)
                                {
                                    case QLSDK.Core.CallState.SIP_UNKNOWN:
                                    case QLSDK.Core.CallState.SIP_CALL_CLOSED:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_FAILURE:
                                        break;
                                    case QLSDK.Core.CallState.SIP_INCOMING_INVITE:                                        
                                        break;
                                    case QLSDK.Core.CallState.SIP_INCOMING_CONNECTED:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_CONNECTED:
                                        {
                                            call.HoldCall();
                                        }
                                        break;
                                    case QLSDK.Core.CallState.SIP_CALL_HOLD:
                                    case QLSDK.Core.CallState.SIP_CALL_HELD:
                                    case QLSDK.Core.CallState.SIP_CALL_DOUBLE_HOLD:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_TRYING:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_RINGING:
                                        break;
                                }
                            }
                            break;
                        #endregion
                        #region Resume
                        case "btnResume":
                            {
                                switch (call.CallState)
                                {
                                    case QLSDK.Core.CallState.SIP_UNKNOWN:
                                    case QLSDK.Core.CallState.SIP_CALL_CLOSED:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_FAILURE:
                                        break;
                                    case QLSDK.Core.CallState.SIP_INCOMING_INVITE:
                                    case QLSDK.Core.CallState.SIP_INCOMING_CONNECTED:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_CONNECTED:
                                    case QLSDK.Core.CallState.SIP_CALL_HELD:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_TRYING:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_RINGING:
                                        break;
                                    case QLSDK.Core.CallState.SIP_CALL_HOLD:
                                    case QLSDK.Core.CallState.SIP_CALL_DOUBLE_HOLD:
                                        {
                                            call.ResumeCall();
                                        }
                                        break;
                                }
                            }
                            break;
                        #endregion
                        #region Hangup
                        case "btnHangup":
                            {
                                switch (call.CallState)
                                {
                                    case QLSDK.Core.CallState.SIP_UNKNOWN:
                                    case QLSDK.Core.CallState.SIP_CALL_CLOSED:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_FAILURE:
                                        break;
                                    case QLSDK.Core.CallState.SIP_INCOMING_INVITE:
                                    case QLSDK.Core.CallState.SIP_INCOMING_CONNECTED:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_CONNECTED:
                                    case QLSDK.Core.CallState.SIP_CALL_HELD:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_TRYING:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_RINGING:
                                    case QLSDK.Core.CallState.SIP_CALL_HOLD:
                                    case QLSDK.Core.CallState.SIP_CALL_DOUBLE_HOLD:
                                        {
                                            call.HangUpCall();
                                        }
                                        break;
                                }
                            }
                            break;
                        #endregion
                        #region AudioCall
                        case "btnAudioCall":
                            {
                                switch (call.CallState)
                                {
                                    case QLSDK.Core.CallState.SIP_UNKNOWN:
                                    case QLSDK.Core.CallState.SIP_CALL_CLOSED:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_FAILURE:
                                        {                                            
                                            QLCallManager.GetInstance().DialCall(call.CallName, QLSDK.Core.CallMode.AUDIO);
                                        }
                                        break;
                                    case QLSDK.Core.CallState.SIP_INCOMING_INVITE:
                                    case QLSDK.Core.CallState.SIP_INCOMING_CONNECTED:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_CONNECTED:
                                    case QLSDK.Core.CallState.SIP_CALL_HELD:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_TRYING:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_RINGING:
                                    case QLSDK.Core.CallState.SIP_CALL_HOLD:
                                    case QLSDK.Core.CallState.SIP_CALL_DOUBLE_HOLD:
                                        break;
                                }
                            }
                            break;
                        #endregion
                        #region Video Call
                        case "btnVideoCall":
                            {
                                switch (call.CallState)
                                {
                                    case QLSDK.Core.CallState.SIP_UNKNOWN:
                                    case QLSDK.Core.CallState.SIP_CALL_CLOSED:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_FAILURE:
                                        {
                                            QLCallManager.GetInstance().DialCall(call.CallName, QLSDK.Core.CallMode.VIDEO);
                                        }
                                        break;
                                    case QLSDK.Core.CallState.SIP_INCOMING_INVITE:
                                    case QLSDK.Core.CallState.SIP_INCOMING_CONNECTED:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_CONNECTED:
                                    case QLSDK.Core.CallState.SIP_CALL_HELD:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_TRYING:
                                    case QLSDK.Core.CallState.SIP_OUTGOING_RINGING:
                                    case QLSDK.Core.CallState.SIP_CALL_HOLD:
                                    case QLSDK.Core.CallState.SIP_CALL_DOUBLE_HOLD:
                                        break;
                                }
                            }
                            break;
                        #endregion
                    }
                }
            }
        }
        #endregion
    }
}
