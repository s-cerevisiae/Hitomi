using System;
using System.Windows;
using System.Windows.Input;

namespace Hitomi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MainWindow_OnDeactivated(object sender, EventArgs e)
        {
            ((Window) sender).Topmost = true;
        }
    }
}
