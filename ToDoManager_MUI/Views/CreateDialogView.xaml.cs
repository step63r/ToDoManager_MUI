using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDoManager_MUI.Views
{
    /// <summary>
    /// Interaction logic for CreateDialogView.xaml
    /// </summary>
    public partial class CreateDialogView : ModernDialog
    {
        public CreateDialogView()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton, this.CancelButton };
            this.OkButton.Content = "OK";
            this.CancelButton.Content = "キャンセル";

            // ダイアログボタンのバインドはこちらで行う
            var bindOK = new Binding("CreateDialog_OK");
            this.OkButton.SetBinding(Button.CommandProperty, bindOK);
            
            // キャンセルボタンは明示的に実装しなくてもいいのでは？
            //var bindCancel = new Binding("CreateDialog_Cancel");
            //this.CancelButton.SetBinding(Button.CommandProperty, bindCancel);
        }
    }
}
