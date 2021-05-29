using System;
using System.Windows;
using Hitomi.Core;

namespace Hitomi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            Activated += StartElmish;
        }

        private void StartElmish(object sender, EventArgs e)
        {
            Activated -= StartElmish;
            Program.main(MainWindow);
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Current.Shutdown();
        }
    }
}
