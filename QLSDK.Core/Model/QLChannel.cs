using System.Drawing;

namespace QLSDK.Core
{
    /// <summary>
    /// 呼叫通道
    /// </summary>
    internal class QLChannel : BaseModel
    {
        #region Constructors
        internal QLChannel(QLCall call, int id, MediaType mediaType, bool isActive = false)
        {
            this._call = call;
            this.ChannelID = id;
            this._mediaType = mediaType;
            this.IsActive = isActive;
        }
        #endregion

        #region 所属呼叫
        private QLCall _call;
        /// <summary>
        /// 所属呼叫
        /// </summary>
        public QLCall Call { get { return _call; } }
        #endregion

        #region 通道ID
        private int _channelID = 0;
        /// <summary>
        /// 通道ID
        /// </summary>
        public int ChannelID
        {
            get { return _channelID; }
            set
            {
                _channelID = value;
                NotifyPropertyChanged("ChannelID");
            }
        }
        #endregion

        #region 通道名
        private string _channelName;
        /// <summary>
        /// 通道名
        /// </summary>
        public string ChannelName
        {
            get { return _channelName; }
            set
            {
                if (_channelName != value)
                {
                    _channelName = value;
                    NotifyPropertyChanged("ChannelName");
                }
            }
        }
        #endregion

        #region 是否活动
        private bool _isActive;
        /// <summary>
        /// 是否活动
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    NotifyPropertyChanged("IsActive");
                }
            }
        }
        #endregion

        #region 是否视频通道
        private bool _isVideo;
        /// <summary>
        /// 是否视频通道
        /// </summary>
        public bool IsVideo
        {
            get { return _isVideo; }
            set
            {
                if (_isVideo != value)
                {
                    _isVideo = value;
                    NotifyPropertyChanged("IsVideo");
                }
            }
        }
        #endregion

        #region 是否音频通道
        private bool _isAudio;
        /// <summary>
        /// 是否音频通道
        /// </summary>
        public bool IsAudio
        {
            get { return _isAudio; }
            set
            {
                _isAudio = value;
                NotifyPropertyChanged("IsAudio");
            }
        }
        #endregion

        #region 通道流类型
        private MediaType _mediaType;
        /// <summary>
        /// 通道流类型
        /// </summary>
        public MediaType MediaType
        {
            get { return _mediaType; }
        }
        #endregion

        #region 本地共享图像
        private Image _localContentImage;
        internal Image LocalContentImage
        {
            get { return _localContentImage; }
            set
            {
                if (_localContentImage != value)
                {
                    _localContentImage = value;
                    base.NotifyPropertyChanged("LocalContentImage");
                }
            }
        }
        #endregion

        #region 区域大小
        private Size _size = new Size(400, 300);
        /// <summary>
        /// 区域大小
        /// </summary>
        public Size Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    IsVideo = true;
                    NotifyPropertyChanged("Size");
                }
            }
        }
        #endregion
    }
}
