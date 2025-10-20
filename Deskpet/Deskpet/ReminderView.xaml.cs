using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Deskpet
{
    public partial class ReminderView : UserControl
    {
        private DispatcherTimer _reminderTimer = null!;
        private DispatcherTimer _countdownTimer = null!;
        private Storyboard _flashStoryboard = null!;
        private int _countdownSeconds;

        public ReminderView()
        {
            InitializeComponent();
            SetupReminderTimer();
            SetupCountdownTimer();
            SetupAnimation();
            ReminderTypeComboBox.SelectionChanged += ReminderTypeComboBox_SelectionChanged;
            UpdateSelectedReminderText();
        }

        #region 初始化與設定
        private void SetupReminderTimer()
        {
            _reminderTimer = new DispatcherTimer();
            _reminderTimer.Tick += ReminderTimer_Tick;
        }

        private void SetupCountdownTimer()
        {
            _countdownTimer = new DispatcherTimer();
            _countdownTimer.Interval = TimeSpan.FromSeconds(1);
            _countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void SetupAnimation()
        {
            ColorAnimation flashAnimation = new ColorAnimation
            {
                From = Colors.Black,
                To = Colors.Red,
                Duration = TimeSpan.FromSeconds(0.5),
                AutoReverse = true,
                RepeatBehavior = new RepeatBehavior(3)
            };
            var brush = new SolidColorBrush(Colors.Black);
            ReminderText.Foreground = brush;
            _flashStoryboard = new Storyboard();
            _flashStoryboard.Children.Add(flashAnimation);
            Storyboard.SetTarget(flashAnimation, brush);
            Storyboard.SetTargetProperty(flashAnimation, new PropertyPath(SolidColorBrush.ColorProperty));
        }
        #endregion

        #region 按鈕點擊事件
        private void StartReminder_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(HourTextBox.Text, out int hours) || hours < 0 || hours > 24) { MessageBox.Show("請輸入 0 ~ 24 的整數（小時）"); return; }
            if (!int.TryParse(MinuteTextBox.Text, out int minutes) || minutes < 0 || minutes > 59) { MessageBox.Show("請輸入 0 ~ 59 的整數（分鐘）"); return; }
            if (!int.TryParse(SecondTextBox.Text, out int seconds) || seconds < 0 || seconds > 59) { MessageBox.Show("請輸入 0 ~ 59 的整數（秒）"); return; }

            _countdownSeconds = hours * 3600 + minutes * 60 + seconds;
            if (_countdownSeconds <= 0) { MessageBox.Show("提醒時間必須大於 0 秒"); return; }

            string reminderType = (ReminderTypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "提醒";
            CountdownText.Text = $"倒數{_countdownSeconds} 秒";
            ReminderText.Text = $"提醒已啟動：{reminderType}，每 {_countdownSeconds} 秒一次";

            _reminderTimer.Interval = TimeSpan.FromSeconds(_countdownSeconds);
            _reminderTimer.Start();
            _countdownTimer.Start();
        }

        private void StopReminder_Click(object sender, RoutedEventArgs e)
        {
            _reminderTimer.Stop();
            _countdownTimer.Stop();
            ReminderText.Text = "提醒已停止";
            CountdownText.Text = string.Empty;
        }
        #endregion

        #region 計時器與UI更新
        private void ReminderTimer_Tick(object? sender, EventArgs e)
        {
            string reminderType = (ReminderTypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "提醒";
            if (ReminderText.Foreground is SolidColorBrush) { _flashStoryboard.Begin(); }

            string message = $"該 {reminderType} 了！";
            var notification = new NotificationWindow(message);
            notification.Show();

            _countdownSeconds = (int)_reminderTimer.Interval.TotalSeconds;
            _countdownTimer.Start();
        }

        private void CountdownTimer_Tick(object? sender, EventArgs e)
        {
            _countdownSeconds--;
            if (_countdownSeconds < 0)
            {
                _countdownTimer.Stop();
                CountdownText.Text = "倒數結束！";
                return;
            }
            CountdownText.Text = $"倒數：{_countdownSeconds} 秒";
        }

        private void ReminderTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectedReminderText();
        }

        private void UpdateSelectedReminderText()
        {
            if (ReminderTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                SelectedReminderText.Text = $"目前選擇: {selectedItem.Content}";
            }
        }
        #endregion
    }
}