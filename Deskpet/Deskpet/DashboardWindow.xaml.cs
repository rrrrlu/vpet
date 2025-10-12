using System.Windows;

namespace Deskpet // 確保命名空間正確
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent(); // 連結正確後，這個錯誤就會消失
        }
    }
}