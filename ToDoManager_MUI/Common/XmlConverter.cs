namespace ToDoManager_MUI.Common
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// XMLシリアライザ／デシリアライザ
    /// </summary>
    public static class XmlConverter
    {
        /// <summary>
        /// 任意のオブジェクトをxmlにシリアル化する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="obj">オブジェクト</param>
        /// <param name="Path">ファイルパス</param>
        /// <returns>成功したらtrue、失敗したらfalse</returns>
        public static bool Serialize<T>(T obj, string Path) where T : class
        {
            bool ret = false;

            try
            {
                FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(fs, obj);
                fs.Close();

                ret = true;
            }
            catch
            {
                // WriteLog
            }

            return ret;
        }

        /// <summary>
        /// 任意のオブジェクトをxmlにシリアル化する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="obj">オブジェクト</param>
        /// <param name="Path">ファイルパス</param>
        /// <returns>成功したらtrue、失敗したらfalse</returns>
        public static bool SerializeFromCol<T>(ObservableCollection<T> obj, string Path) where T : class
        {
            bool ret = false;

            try
            {
                FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<T>));
                serializer.Serialize(fs, obj);
                fs.Close();

                ret = true;
            }
            catch
            {
                // WriteLog
            }

            return ret;
        }

        /// <summary>
        /// xmlファイルを任意のオブジェクトに逆シリアル化する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="Path">ファイルパス</param>
        /// <returns>オブジェクト</returns>
        public static T DeSerialize<T>(string Path) where T : class
        {
            T ret = default(T);

            try
            {
                FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                ret = (T)serializer.Deserialize(fs);
                fs.Close();
            }
            catch
            {
                // WriteLog
            }

            return ret;
        }

        /// <summary>
        /// xmlファイルを任意のオブジェクトに逆シリアル化する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="Path">ファイルパス</param>
        /// <returns>オブジェクト</returns>
        public static ObservableCollection<T> DeSerializeToCol<T>(string Path) where T : class
        {
            ObservableCollection<T> ret = default(ObservableCollection<T>);

            try
            {
                FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read);
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<T>));
                ret = (ObservableCollection<T>)serializer.Deserialize(fs);
                fs.Close();
            }
            catch
            {
                // WriteLog
            }

            return ret;
        }
    }
}
