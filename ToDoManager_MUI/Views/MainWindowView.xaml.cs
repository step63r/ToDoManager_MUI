using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using ToDoManager_MUI.Common;

namespace ToDoManager_MUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : ModernWindow
    {
        private RelayCommand setFocusCommand;
        public RelayCommand SetFocusCommand
        {
            get
            {
                if (setFocusCommand == null)
                {
                    setFocusCommand = new RelayCommand(SetFocus, CanSetFocus);
                }
                return setFocusCommand;
            }
        }

        public MainWindowView()
        {
            InitializeComponent();

            // Settings の値をウィンドウに反映
            Left = Properties.Settings.Default.MainWindow_Left;
            Top = Properties.Settings.Default.MainWindow_Top;
            Width = Properties.Settings.Default.MainWindow_Width;
            Height = Properties.Settings.Default.MainWindow_Height;

            // 最初は通知領域を非表示で起動
            tbMain.Visibility = Visibility.Collapsed;
        }

        ~MainWindowView()
        {
            // 通知領域のアイコンを消す
            tbMain.Icon = null;
        }

        public void SetFocus()
        {
            if (dgToDo.SelectedIndex != -1)
            {
                dgToDo.Focus();
            }
        }
        public bool CanSetFocus()
        {
            return true;
        }

        private void ModernWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (WindowState == WindowState.Normal)
                {
                    // ウィンドウの値を Settings に格納
                    Properties.Settings.Default.MainWindow_Left = Left;
                    Properties.Settings.Default.MainWindow_Top = Top;
                    Properties.Settings.Default.MainWindow_Width = Width;
                    Properties.Settings.Default.MainWindow_Height = Height;
                    // ファイルに保存
                    Properties.Settings.Default.Save();
                }

                //閉じるのをキャンセルする
                e.Cancel = true;

                //ウィンドウを非可視にする
                Visibility = Visibility.Collapsed;
                ShowInTaskbar = false;

                // 通知領域のアイコンを表示する
                tbMain.Visibility = Visibility.Visible;
            }
            catch { }

        }

        private void ShowMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ウィンドウを可視化
                Visibility = Visibility.Visible;
                WindowState = WindowState.Normal;

                ShowInTaskbar = true;

                // 通知領域のアイコンを非表示にする
                tbMain.Visibility = Visibility.Collapsed;
            }
            catch { }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // ウィンドウの値を Settings に格納
                Properties.Settings.Default.MainWindow_Left = Left;
                Properties.Settings.Default.MainWindow_Top = Top;
                Properties.Settings.Default.MainWindow_Width = Width;
                Properties.Settings.Default.MainWindow_Height = Height;
                // ファイルに保存
                Properties.Settings.Default.Save();

                System.Windows.Application.Current.Shutdown();
            }
            catch { }
        }

        //private void DataGridClick(object parameter)
        //{
        //    if (dgToDo.SelectedIndex != -1)
        //    {
        //        dgToDo.Focus();
        //    }
        //}
    }
}
