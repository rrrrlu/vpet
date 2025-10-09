// Pet.WPF/MainWindow.xaml.cs

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

// ====== 【確認點】確保命名空間與您專案中所有其他 .cs 檔案一致 ======
namespace Deskpet
{
    public partial class MainWindow : Window
    {
        // ====== 【修正點 1】直接在這裡初始化變數，解決所有 Null 錯誤 ======
        private readonly DatabaseService _dbService = new DatabaseService();
        private PetStatus _currentPetStatus = new PetStatus();

        public MainWindow()
        {
            InitializeComponent();
        }

        // ====== 視窗載入事件，用於初始化和讀取資料 (您的邏輯是正確的) ======
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _dbService.InitializeDatabase();
            _currentPetStatus = _dbService.LoadPetStatus();
        }

        // ====== 視窗關閉事件，用於儲存資料 (您的邏輯是正確的) ======
        protected override void OnClosing(CancelEventArgs e)
        {
            // 在實際應用中，您會在這裡根據寵物當前的狀態更新 _currentPetStatus 的值
            // 例如: _currentPetStatus.Mood = 80;
            //       _currentPetStatus.Hunger = 90;

            _dbService.SavePetStatus(_currentPetStatus);
            base.OnClosing(e);
        }

        private void UpdateStatusDisplay()
        {
            if (StatusText != null && _currentPetStatus != null)
            {
                StatusText.Text = $"心情: {_currentPetStatus.Mood}, 飽食度: {_currentPetStatus.Hunger}";
            }
        }
        #region 拖曳功能
        // ====== 【修正點 2】使用 WPF 內建的 DragMove() 方法，這是最標準、最簡單的作法 ======
        private void PetImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 只要滑鼠左鍵按下，就呼叫這個方法來拖動整個視窗
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        #endregion
        private void FeedButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. 檢查 _currentPetStatus 是否存在
            if (_currentPetStatus != null)
            {
                // 2. 修改飽食度的值 (例如每次+10，最多不超過100)
                _currentPetStatus.Hunger += 10;
                if (_currentPetStatus.Hunger > 100)
                {
                    _currentPetStatus.Hunger = 100;
                }

                // 3. (可選) 顯示提示，讓使用者知道發生了什麼事
                MessageBox.Show($"你餵了寵物！\n目前飽食度: {_currentPetStatus.Hunger}");
                // ====== 在這裡新增儲存指令 ======
                _dbService.SavePetStatus(_currentPetStatus);

                UpdateStatusDisplay();
            }
        }
    }
}