﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using System.Collections.Specialized;

namespace QLSDK.Core
{
    internal partial class QLCallView : UserControl
    {
        #region Field
        private ILog log = LogUtil.GetLogger("QLSDK.QLCallView");
        private QLConfigManager qlConfig = null;
        private QLCallManager callManager = null;
        private QLCall _currentCall = null;
        private Dictionary<QLChannel, ChannelView> channelViews = new Dictionary<QLChannel, ChannelView>();
        private Control ownerContainer;
        private QLChannel localChannel;
        private QLChannel contentChannel;
        #endregion
        #region Constructors
        private static readonly object lockObj = new object();
        private static QLCallView instance = null;
        private QLCallView()
        {
            InitializeComponent();
        }
        internal static QLCallView GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new QLCallView();
                    }
                }
            }
            return instance;
        }
        #endregion

        #region 设置显示的呼叫
        /// <summary>
        /// 设置显示的呼叫
        /// </summary>
        /// <param name="call">呼叫</param>
        private void SetCurrentCall(QLCall call)
        {
            if (null != _currentCall)
            {
                _currentCall.PropertyChanged -= OnCallPropertyChangedHandle;
                _currentCall.Channels.CollectionChanged -= OnChannelsCllectionChangedHandle;
            }
            this.Controls.Clear();
            foreach (var channelView in channelViews)
            {
                //  channelView.Value.Dispose();
            }
            channelViews.Clear();
            this.Controls.Clear();

            _currentCall = call;
            if (null != _currentCall)
            {
                _currentCall.PropertyChanged += OnCallPropertyChangedHandle;
                _currentCall.Channels.CollectionChanged += OnChannelsCllectionChangedHandle;
            }
        }
        #region CallBack
        private void OnCallPropertyChangedHandle(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case "CallHandle": break;
                case "CallName": break;
                case "CallState": break;
                case "CurrentChannel":
                    {
                        ViewRender();
                    }
                    break;
            }

        }
        private void OnChannelsCllectionChangedHandle(object sender, NotifyCollectionChangedEventArgs args)
        {
            #region ChannelView
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (var item in args.NewItems)
                        {
                            var channel = item as QLChannel;
                            if (null != channel)
                            {
                                var channelView = new ChannelView(channel);
                                channelViews.Add(channel, channelView);
                                this.Controls.Add(channelView);
                                switch (channel.MediaType)
                                {
                                    case MediaType.LOCAL: localChannel = channel; break;
                                    case MediaType.CONTENT: contentChannel = channel; break;
                                }
                            }
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (var item in args.OldItems)
                        {
                            var channel = item as QLChannel;
                            if (channelViews.ContainsKey(channel))
                            {
                                var channelView = channelViews[channel];
                                if (null != channelView)
                                {
                                    this.Controls.Remove(channelView);
                                    channelView.Dispose();
                                }
                                channelViews.Remove(channel);
                                switch (channel.MediaType)
                                {
                                    case MediaType.LOCAL: localChannel = null; break;
                                    case MediaType.CONTENT: contentChannel = null; break;
                                }
                            }
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Reset: break;
                case NotifyCollectionChangedAction.Move: break;
                case NotifyCollectionChangedAction.Replace: break;
            }
            #endregion

            ViewRender();
        }
        #endregion

        #endregion

        #region BindPanel
        internal void AttachViewContainer(Control container)
        {
            if (null == container)
            {
                throw new Exception("父控件必须");
            }
            container.Controls.Add(this);
            ownerContainer = container;

            this.Dock = DockStyle.Fill;
            //this.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            //this.Width = ownerContainer.Width;
            //this.Height = ownerContainer.Height - 80;
            ownerContainer.SizeChanged += (sender, args) =>
            {
                //this.Width = ownerContainer.Width;
                //this.Height = ownerContainer.Height - 80;
                ViewRender();
            };
        }
        #endregion

        #region Messages
        public void ShowMessage(bool isModal, string msg, MessageBoxButtonsType btnType, MessageBoxIcon boxIcon
            , Action okAction = null, Action cancelAction = null, Action noAction = null)
        {
            if (null != ownerContainer)
            {
                 UXMessageMask.ShowMessage(ownerContainer, isModal, msg, btnType, boxIcon, okAction, cancelAction, noAction);
            }
        }
        public void HideMessage()
        {
            if (null != ownerContainer)
            {
                UXMessageMask.HideMessage(ownerContainer);
            }
        }
        #endregion

        #region 渲染视图
        /// <summary>
        /// 渲染视图
        /// </summary>
        internal void ViewRender()
        {
            if (null == _currentCall || channelViews.Count <= 0)
            {
                return;
            }
            var layoutType = qlConfig.GetProperty(PropertyKey.LayoutType);

            var viewWidth = this.Width;
            var viewHeight = this.Height;
            var ratioWidth = 320;
            var ratioHeight = 240;
            var cellWidth = ratioWidth;
            var cellHeight = ratioHeight;

            var cols = viewWidth / cellWidth;
            var rows = viewHeight / cellHeight;

            var activeChannel = contentChannel;
            #region ActiveChannel
            if (null == activeChannel)
            {
                activeChannel = contentChannel;
            }
            if (null == activeChannel && null != _currentCall.CurrentChannel)
            {
                activeChannel = _currentCall.CurrentChannel;
            }
            if (null == activeChannel)
            {
                activeChannel = _currentCall.Channels.FirstOrDefault();
            }
            if (null == activeChannel)
            {
                return;
            }
            #endregion

            switch (layoutType)
            {
                case "VAS":
                    {
                        #region VAS
                        var activeView = channelViews.Where(cv => cv.Key == activeChannel).Select(cv => cv.Value).FirstOrDefault();
                        if (null != activeView)
                        {
                            activeView.Location = new Point(0, 0);
                            activeView.Size = new Size(viewWidth, viewHeight);
                            activeView.IsShowBar = false;
                            activeView.SendToBack();
                        }

                        LocateChannel(rows, cols, viewWidth, viewHeight, cellWidth, cellHeight
                            , channelViews.Where(cv => cv.Key != activeChannel).Select(cv => cv.Value).ToList());
                        #endregion
                    }
                    break;
                case "ContinuousPresence":
                    {
                        #region ContinuousPresence
                        var cRows = 0;
                        var cCols = 0;
                        if (rows <= cols)
                        {
                            switch (channelViews.Count)
                            {
                                case 0: cRows = 0; cCols = 0; break;
                                case 1: cRows = 1; cCols = 1; break;
                                case 2: cRows = 1; cCols = 2; break;
                                case 3: cRows = 2; cCols = 2; break;
                                case 4: cRows = 2; cCols = 2; break;
                                case 5: cRows = 2; cCols = 3; break;
                                case 6: cRows = 2; cCols = 3; break;
                                case 7: cRows = 3; cCols = 3; break;
                                case 8: cRows = 3; cCols = 3; break;
                                case 9: cRows = 3; cCols = 3; break;
                                case 10: cRows = 3; cCols = 4; break;
                                case 11: cRows = 3; cCols = 4; break;
                                case 12: cRows = 3; cCols = 4; break;
                                case 13: cRows = 4; cCols = 4; break;
                                case 14: cRows = 4; cCols = 4; break;
                                case 15: cRows = 4; cCols = 4; break;
                                case 16: cRows = 4; cCols = 4; break;
                            }
                        }
                        else
                        {
                            switch (channelViews.Count)
                            {
                                case 0: cRows = 0; cCols = 0; break;
                                case 1: cRows = 1; cCols = 1; break;
                                case 2: cRows = 2; cCols = 1; break;
                                case 3: cRows = 2; cCols = 2; break;
                                case 4: cRows = 2; cCols = 2; break;
                                case 5: cRows = 3; cCols = 2; break;
                                case 6: cRows = 3; cCols = 2; break;
                                case 7: cRows = 3; cCols = 3; break;
                                case 8: cRows = 3; cCols = 3; break;
                                case 9: cRows = 3; cCols = 3; break;
                                case 10: cRows = 4; cCols = 3; break;
                                case 11: cRows = 4; cCols = 3; break;
                                case 12: cRows = 4; cCols = 3; break;
                                case 13: cRows = 4; cCols = 4; break;
                                case 14: cRows = 4; cCols = 4; break;
                                case 15: cRows = 4; cCols = 4; break;
                                case 16: cRows = 4; cCols = 4; break;
                            }
                        }
                        var x = 0;
                        int y = 0;
                        var i = 0;
                        var cWidth = viewWidth / cCols;
                        var cHeight = viewHeight / cRows;
                        foreach (var view in channelViews.Values)
                        {
                            view.Location = new Point(x, y);
                            view.Size = new Size(cWidth, cHeight);
                            view.IsShowBar = true;
                            view.BringToFront();
                            x = x + cWidth;
                            i++;
                            if (i % cCols == 0)
                            {
                                x = 0;
                                y = y + cHeight;
                            }
                        }
                        #endregion
                    }
                    break;
                case "Presentation":
                    {
                        #region ContinuousPresence
                        var locate = LocateChannel(rows, cols, viewWidth, viewHeight, cellWidth, cellHeight
                            , channelViews.Where(cv => cv.Key != activeChannel).Select(cv => cv.Value).ToList());
                        var activeView = channelViews.Where(cv => cv.Key == activeChannel).Select(cv => cv.Value).FirstOrDefault();
                        if (null != activeView)
                        {
                            activeView.Location = new Point(0, 0);
                            activeView.Size = new Size(locate.X, locate.Y);
                            activeView.IsShowBar = false;
                            activeView.BringToFront();
                        }
                        #endregion
                    }
                    break;
                case "Single":
                    {
                        #region Single
                        var activeView = channelViews.Where(cv => cv.Key == activeChannel).Select(cv => cv.Value).FirstOrDefault();
                        if (null != activeView)
                        {
                            activeView.Location = new Point(0, 0);
                            activeView.Size = new Size(viewWidth, viewHeight);
                            activeView.IsShowBar = false;
                            activeView.SendToBack();
                        }
                        foreach(var view in  channelViews.Where(cv => cv.Key != activeChannel))
                        {
                            view.Value.Size = new Size(0, 0);
                        }
                        #endregion
                    }
                    break;
            }
            var maskPnl = ownerContainer.Controls.Find("msgPnl", true).FirstOrDefault();
            if(null != maskPnl)
            {
                maskPnl.BringToFront();
                if(maskPnl.Controls.Count>0)
                {
                    maskPnl.Controls[0].BringToFront();
                }
            }
        }
        private Point LocateChannel(int rows, int cols, int x, int y, int cellWidth, int cellHeight, IList<ChannelView> channelViews)
        {
            if (channelViews.Count <= 0 || rows <= 0 || cols <= 0)
            {
                return new Point(x, y);
            }
            if (rows <= cols)
            {
                if (channelViews.Count <= rows)
                {
                    var i = 0;
                    foreach (var view in channelViews)
                    {
                        i++;
                        view.Location = new Point(x - cellWidth, y - i * cellHeight);
                        view.Size = new Size(cellWidth, cellHeight);
                        view.IsShowBar = true;
                        view.BringToFront();
                    }
                    return new Point(x - cellWidth, y);
                }
                else if (channelViews.Count <= cols)
                {
                    var i = 0;
                    foreach (var view in channelViews)
                    {
                        i++;
                        view.Location = new Point(x - i * cellWidth, y - cellHeight);
                        view.Size = new Size(cellWidth, cellHeight);
                        view.IsShowBar = true;
                        view.BringToFront();
                    }
                    return new Point(x, y - cellHeight);
                }
                else if (channelViews.Count < cols + rows)
                {
                    var i = 0;
                    foreach (var view in channelViews)
                    {
                        i++;
                        var rowindex = 1;
                        var colIndex = 1;
                        if (i > rows)
                        {
                            colIndex = i - rows;
                            rowindex = 1;
                        }
                        else
                        {
                            colIndex = 1;
                            rowindex = i;
                        }
                        view.Location = new Point(x - colIndex * cellWidth, y - rowindex * cellHeight);
                        view.Size = new Size(cellWidth, cellHeight);
                        view.IsShowBar = true;
                        view.BringToFront();
                    }
                    return new Point(x - cellWidth, y - cellHeight);
                }
                else
                {
                    return LocateChannel(rows - 1, cols - 1, x - cellWidth, y - cellHeight, cellWidth, cellHeight, channelViews.Skip(rows + cols - 1).ToList());
                }
            }
            else
            {
                if (channelViews.Count <= cols)
                {
                    var i = 0;
                    foreach (var view in channelViews)
                    {
                        i++;
                        view.Location = new Point(x - i * cellWidth, y - cellHeight);
                        view.Size = new Size(cellWidth, cellHeight);
                        view.IsShowBar = true;
                        view.BringToFront();
                    }
                    return new Point(x, y - cellHeight);
                }
                else if (channelViews.Count <= rows)
                {
                    var i = 0;
                    foreach (var view in channelViews)
                    {
                        i++;
                        view.Location = new Point(x - cellWidth, y - i * cellHeight);
                        view.Size = new Size(cellWidth, cellHeight);
                        view.IsShowBar = true;
                        view.BringToFront();
                    }
                    return new Point(x - cellWidth, y);
                }
                else if (channelViews.Count < cols + rows)
                {
                    var i = 0;
                    foreach (var view in channelViews)
                    {
                        i++;
                        var rowindex = 1;
                        var colIndex = 1;
                        if (i > cols)
                        {
                            colIndex = 1;
                            rowindex = i - cols;
                        }
                        else
                        {
                            colIndex = i;
                            rowindex = 1;
                        }
                        view.Location = new Point(x - colIndex * cellWidth, y - rowindex * cellHeight);
                        view.Size = new Size(cellWidth, cellHeight);
                        view.BringToFront();
                    }
                    return new Point(x - cellWidth, y - cellHeight);
                }
                else
                {
                    return LocateChannel(rows - 1, cols - 1, x - cellWidth, y - cellHeight, cellWidth, cellHeight, channelViews.Skip(rows + cols - 1).ToList());
                }
            }
        }
        #endregion

        /// <summary>
        /// 加载成功处理器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QLCallView_Load(object sender, EventArgs e)
        {
            qlConfig = QLConfigManager.GetInstance();
            callManager = QLCallManager.GetInstance();
            callManager.CurrentCallChanged+=() =>
            {
                SetCurrentCall(callManager.CurrentCall);
            };
        }

    }
}
