using Assignment8.ViewModels;

namespace Assignment8
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new ProfilePage();
        }
    }
}
