using System.Windows;

namespace Hitomi
{
    public partial class NotifyIcon
    {
        public NotifyIcon()
        {
            InitializeComponent();
        }

        private void TaskbarIcon_OnTrayDoubleClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Activate();
        }
    }
}
