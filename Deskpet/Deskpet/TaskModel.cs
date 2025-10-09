using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Deskpet
{
    public class TaskModel : INotifyPropertyChanged
    {
        private int _id;

        private string _name = string.Empty;

        private bool _isCompleted;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set { _isCompleted = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayName)); }
        }

        public string DisplayName => IsCompleted ? $"{Name} ✅" : Name;


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}