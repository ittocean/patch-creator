using System.Windows;

namespace PatchCreator2
{
    /// <summary>
    /// Interaction logic for ConfirmWindow.xaml
    /// </summary>
    public partial class ConfirmWindow : Window
    {
        public ConfirmWindow(string content)
        {
            InitializeComponent();
            warningTextBox.Text = content;
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
