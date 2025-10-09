// 步驟1 複製過來的 using
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls; // UserControl 需要這個

namespace Deskpet // 確保命名空間是您自己的專案名稱
{
    public partial class TodoListView : UserControl
    {
        // 步驟2 複製過來的變數
        private readonly TaskService _taskService = new TaskService();
        private readonly ObservableCollection<TaskModel> _tasks = new ObservableCollection<TaskModel>();

        public TodoListView()
        {
            InitializeComponent();
            // 我們可以直接在這裡呼叫原本的 Loaded 邏輯
            // 或者，為了保持原樣，我們可以在 XAML 中設定 Loaded 事件
        }

        // 步驟3 複製過來的四個方法
        // 注意：我們將 Window_Loaded 改名為 UserControl_Loaded 以避免混淆
        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // listBoxTasks.ItemsSource = _tasks; // 假設 UI 元素名稱一致
            var loadedTasks = _taskService.LoadTasks();

            _tasks.Clear();
            foreach (var task in loadedTasks)
            {
                _tasks.Add(task);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // if (string.IsNullOrWhiteSpace(textBoxTask.Text)) return; // 假設 UI 元素名稱一致

            var newTask = new TaskModel
            {
                Id = _tasks.Count > 0 ? _tasks.Max(t => t.Id) + 1 : 1,
                // Name = textBoxTask.Text.Trim(), // 假設 UI 元素名稱一致
                IsCompleted = false
            };

            _tasks.Add(newTask);
            _taskService.SaveTasks(_tasks);
            // textBoxTask.Clear(); // 假設 UI 元素名稱一致
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // var selectedTask = listBoxTasks.SelectedItem as TaskModel; // 假設 UI 元素名稱一致
            // if (selectedTask == null) return;

            // _tasks.Remove(selectedTask);
            _taskService.SaveTasks(_tasks);
        }

        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {
            // var selectedTask = listBoxTasks.SelectedItem as TaskModel; // 假設 UI 元素名稱一致
            // if (selectedTask == null) return;

            // selectedTask.IsCompleted = true;
            _taskService.SaveTasks(_tasks);

            MessageBox.Show("完成任務，桌寵開心！😄", "任務完成", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}