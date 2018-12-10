using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSDK.Core
{
    /// <summary>
    /// 基类，实现属性改变通知接口
    /// </summary>
    public abstract class BaseModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 整改改变通知事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
