using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections.Generic;

namespace ToDoManager_MUI.Common
{
    /// <summary>
    /// ファイルパス群
    /// </summary>
    public static class FilePath
    {
        /// <summary>
        /// 基底ディレクトリ（C:\Users\UserName\AppData\Roaming）
        /// </summary>
        public static string BaseDir = String.Format("{0}/ToDoManager_MUI/xml", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        /// <summary>
        /// ToDo一覧XMLファイルパス
        /// </summary>
        public static string XmlPathToDo = "ToDo.xml";
    }

    /// <summary>
    /// ToDoクラス
    /// </summary>
    [XmlRoot("ToDo")]
    public class ToDo : INotifyPropertyChanged
    {
        // イベントだけ実装しておく。OnPropertyChangedは使わない
        public event PropertyChangedEventHandler PropertyChanged;

        [XmlIgnore]
        private int _id;
        /// <summary>
        /// ToDoのID
        /// </summary>
        [XmlElement("ID")]
        public int ID
        {
            get { return _id; }
            set
            {
                if (Equals(_id, value))
                {
                    return;
                }
                _id = value;
                PropertyChanged.Raise(() => ID);
            }
        }

        [XmlIgnore]
        private int _ignoreDate;
        /// <summary>
        /// 期限なしフラグ（1：期限なし）
        /// </summary>
        [XmlElement("IgnoreDate")]
        public int IgnoreDate
        {
            get { return _ignoreDate; }
            set
            {
                if (Equals(_ignoreDate, value))
                {
                    return;
                }
                _ignoreDate = value;
                PropertyChanged.Raise(() => IgnoreDate);
            }
        }

        [XmlIgnore]
        private DateTime? _today;
        /// <summary>
        /// 基準日
        /// </summary>
        [XmlIgnore]
        public DateTime? Today
        {
            get { return _today; }
            set
            {
                if (Equals(_today, value))
                {
                    return;
                }
                _today = value;
                PropertyChanged.Raise(() => Today);
            }
        }

        [XmlIgnore]
        private DateTime? _date;
        /// <summary>
        /// 日付
        /// </summary>
        [XmlElement("Date")]
        public DateTime? Date
        {
            get { return _date; }
            set
            {
                if (Equals(_date, value))
                {
                    return;
                }
                _date = value;
                PropertyChanged.Raise(() => Date);
            }
        }

        [XmlIgnore]
        private string _title;
        /// <summary>
        /// ToDoタイトル
        /// </summary>
        [XmlElement("Title")]
        public string Title
        {
            get { return _title; }
            set
            {
                if (Equals(_title, value))
                {
                    return;
                }
                _title = value;
                PropertyChanged.Raise(() => Title);
            }
        }

        [XmlIgnore]
        private string _detail;
        /// <summary>
        /// ToDo詳細
        /// </summary>
        [XmlElement("Detail")]
        public string Detail
        {
            get { return _detail; }
            set
            {
                if (Equals(_detail, value))
                {
                    return;
                }
                _detail = value;
                PropertyChanged.Raise(() => Detail);
            }
        }
    }

    public class ToDoComparer : IComparer<ToDo>
    {
        public int Compare(ToDo x, ToDo y)
        {
            int ret = 0;

            // nullより大きい
            if (y is null)
            {
                ret = 1;
            }

            // 違う型とは比較できない
            if (x.GetType() != y.GetType())
            {
                throw new ArgumentException("別の型とは比較できません。", "y");
            }

            // まず日付で比較する
            // 古い方が先
            if (x.IgnoreDate == 0 && y.IgnoreDate == 0)
            {
                if (x.Date < (DateTime)y.Date)
                {
                    // このオブジェクト < 比較先
                    ret = -1;
                }
                else if (x.Date > (DateTime)y.Date)
                {
                    // このオブジェクト > 比較先
                    ret = 1;
                }
                else
                {
                    // このオブジェクト == 比較先
                    // IDで比較
                    ret = x.ID - y.ID;
                }
            }

            // 無期限のもの同士はIDでソート
            if (x.IgnoreDate == 1 && y.IgnoreDate == 1)
            {
                ret = x.ID - y.ID;
            }
            else if (x.IgnoreDate == 1)
            {
                ret = 1;
            }
            else if (y.IgnoreDate == 1)
            {
                ret = -1;
            }

           return ret;
        }
    }
}
