using System;
using System.Windows;

namespace Deskpet
{
    public partial class ToolbarWindow : Window
    {
        private PetStatus _currentPetStatus;
        private DatabaseService _dbService;

        // 儲存已經建立過的模組實體
        private TodoListView? _todoListView;
        private ReminderView? _reminderView;

        public ToolbarWindow(PetStatus status, DatabaseService dbService)
        {
            InitializeComponent();
            _currentPetStatus = status;
            _dbService = dbService;
        }

        private void LoadModule(string moduleName)
        {
            object? moduleToShow = null;

            switch (moduleName)
            {
                case "TodoList":
                    if (_todoListView == null) { _todoListView = new TodoListView(); }
                    moduleToShow = _todoListView;
                    break;
                case "Reminder":
                    if (_reminderView == null) { _reminderView = new ReminderView(); }
                    moduleToShow = _reminderView;
                    break;
                case "Settings":
                    MessageBox.Show("設定模組尚未整合！");
                    break;
            }

            if (ModulePresenter.Content == moduleToShow)
            {
                ModulePresenter.Content = null;
            }
            else
            {
                ModulePresenter.Content = moduleToShow;
            }

            if (ModulePresenter.Content != null)
            {
                ModuleContainerBorder.Visibility = Visibility.Visible;
            }
            else
            {
                ModuleContainerBorder.Visibility = Visibility.Collapsed;
            }
        }

        public void ShowToolbar()
        {
            if (this.IsVisible) { this.Hide(); }
            else { this.Show(); this.Activate(); }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            ModulePresenter.Content = null;
            ModuleContainerBorder.Visibility = Visibility.Collapsed;
            this.Hide();
        }

        #region 按鈕點擊事件
        private void FeedButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPetStatus != null)
            {
                _currentPetStatus.Hunger += 10;
                if (_currentPetStatus.Hunger > 100)
                {
                    _currentPetStatus.Hunger = 100;
                }
                _dbService.SavePetStatus(_currentPetStatus);
                MessageBox.Show($"你餵了寵物！\n目前飽食度: {_currentPetStatus.Hunger}");
            }
        }

        private void Button_TodoList_Click(object sender, RoutedEventArgs e)
        {
            LoadModule("TodoList");
        }

        private void Button_Reminder_Click(object sender, RoutedEventArgs e)
        {
            LoadModule("Reminder");
        }

        private void Button_Settings_Click(object sender, RoutedEventArgs e)
        {
            LoadModule("Settings");
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion
    }
}