using System;
using System.Windows;

namespace Deskpet
{
    public partial class ToolbarWindow : Window
    {
        private PetStatus _currentPetStatus;
        private DatabaseService _dbService;

        public ToolbarWindow(PetStatus status, DatabaseService dbService)
        {
            InitializeComponent();
            _currentPetStatus = status;
            _dbService = dbService;
        }

        // ====== 模組載入的核心邏輯 ======
        private void LoadModule(string moduleName)
        {
            // 如果點擊的是同一個已開啟的模組按鈕，則清空並收起
            if (ModulePresenter.Content?.GetType().Name == moduleName + "View")
            {
                ModulePresenter.Content = null;
            }
            else // 否則，載入對應的模組
            {
                switch (moduleName)
                {
                    case "TodoList":
                        ModulePresenter.Content = new TodoListView();
                        break;
                    case "Reminder":
                        // ModulePresenter.Content = new ReminderView(); 
                        MessageBox.Show("提醒功能模組尚未整合！");
                        break;
                    case "Settings":
                        MessageBox.Show("設定模組尚未整合！");
                        break;
                }
            }

            // 根據舞台上是否有內容，決定是否顯示下方的區塊
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
                if (_currentPetStatus.Hunger > 100) _currentPetStatus.Hunger = 100;
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