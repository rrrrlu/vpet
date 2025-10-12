// todolist.xaml.cs (最終修正版)
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Deskpet
{
    public partial class TodoListView : UserControl
    {
        private readonly TaskService _taskService = new TaskService();
        private readonly ObservableCollection<TaskModel> _tasks = new ObservableCollection<TaskModel>();

        public TodoListView()
        {
            InitializeComponent();
            this.Loaded += UserControl_Loaded;
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 將 ListBox 的資料來源與我們的 _tasks 集合綁定
            listBoxTasks.ItemsSource = _tasks;

            var loadedTasks = _taskService.LoadTasks();

            _tasks.Clear();
            foreach (var task in loadedTasks)
            {
                _tasks.Add(task);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxTask.Text)) return;

            var newTask = new TaskModel
            {
                Id = _tasks.Count > 0 ? _tasks.Max(t => t.Id) + 1 : 1,
                Name = textBoxTask.Text.Trim(),
                IsCompleted = false
            };

            _tasks.Add(newTask);
            _taskService.SaveTasks(_tasks);
            textBoxTask.Clear();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = listBoxTasks.SelectedItem as TaskModel;
            if (selectedTask == null) return;

            _tasks.Remove(selectedTask);
            _taskService.SaveTasks(_tasks);
        }

        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = listBoxTasks.SelectedItem as TaskModel;
            if (selectedTask == null) return;

            selectedTask.IsCompleted = true;
            _taskService.SaveTasks(_tasks);

            MessageBox.Show("完成任務，桌寵開心！😄", "任務完成", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}