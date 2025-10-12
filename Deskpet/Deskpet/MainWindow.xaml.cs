using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Deskpet
{
    public partial class MainWindow : Window
    {
        private readonly DatabaseService _dbService = new DatabaseService();
        private PetStatus _currentPetStatus = new PetStatus();
        private ToolbarWindow? toolbar;
        private DispatcherTimer? statusUpdateTimer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _dbService.InitializeDatabase();
            _currentPetStatus = _dbService.LoadPetStatus();

            // 設定計時器
            statusUpdateTimer = new DispatcherTimer();
            statusUpdateTimer.Interval = TimeSpan.FromSeconds(5);
            // 修正點 #4：在 object 後面加上 ?
            statusUpdateTimer.Tick += StatusUpdateTimer_Tick;
            statusUpdateTimer.Start();
        }

        // 修正點 #4：在 object 後面加上 ?
        private void StatusUpdateTimer_Tick(object? sender, EventArgs e)
        {
            if (_currentPetStatus != null)
            {
                _currentPetStatus.Hunger -= 1;
                if (_currentPetStatus.Hunger < 0) _currentPetStatus.Hunger = 0;

                _currentPetStatus.Mood -= 1;
                if (_currentPetStatus.Mood < 0) _currentPetStatus.Mood = 0;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_dbService != null && _currentPetStatus != null)
            {
                _dbService.SavePetStatus(_currentPetStatus);
            }
            base.OnClosing(e);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void PetImage_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (toolbar == null)
            {
                toolbar = new ToolbarWindow(_currentPetStatus, _dbService);
                toolbar.Owner = this;
            }

            var petScreenPos = PointToScreen(new Point(0, 0));
            toolbar.Left = petScreenPos.X + (this.ActualWidth - toolbar.Width) / 2;
            toolbar.Top = petScreenPos.Y + this.ActualHeight + 5;
            toolbar.ShowToolbar();
        }
    }
}