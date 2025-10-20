using System;
using System.Windows;
using System.Windows.Threading;

namespace Deskpet
{
    public partial class NotificationWindow : Window
    {
        private DispatcherTimer _closeTimer;

        public NotificationWindow(string message)
        {
            InitializeComponent();
            MessageText.Text = message;

            // 設定一個計時器，3秒後自動關閉這個視窗
            _closeTimer = new DispatcherTimer();
            _closeTimer.Interval = TimeSpan.FromSeconds(3);
            _closeTimer.Tick += CloseTimer_Tick;
            _closeTimer.Start();
        }

        private void CloseTimer_Tick(object? sender, EventArgs e)
        {
            _closeTimer.Stop();
            this.Close();
        }
    }
}