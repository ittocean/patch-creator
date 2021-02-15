using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PatchCreator2
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        private static readonly string CONTENT = @"This version of patch creator is not
up to date.
Please click the following button to be
redirected to the current version.";

        public UpdateWindow()
        {
            InitializeComponent();
            contentLabel.Content = CONTENT;
        }

        private void ellipseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ellipseButton.Stroke = new SolidColorBrush(Color.FromRgb(126, 180, 234));
        }

        private void ellipseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ellipseButton.Stroke = Brushes.Black;
        }

        private void ellipseButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
