using System.Windows;

namespace LeoEditor.GameProject
{
    public partial class ProjectBrowser : Window
    {
        public ProjectBrowser()
        {
            InitializeComponent();
        }

        private void OnToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(OpenProjectButton))
            {
                if (CreateProjectButton.IsChecked == true)
                {
                    CreateProjectButton.IsChecked = false;
                    BrowserContent.Margin = new Thickness(0);
                }

                OpenProjectButton.IsChecked = true;
            }
            else
            {
                if (OpenProjectButton.IsChecked == true)
                {
                    OpenProjectButton.IsChecked = false;
                    BrowserContent.Margin = new Thickness(-800, 0, 0, 0);
                }

                CreateProjectButton.IsChecked = true;
            }
        }
    }
}