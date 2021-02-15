using PatchCreator.logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PatchCreator2
{
    /// <summary>
    /// Interaction logic for SendPatchWindow.xaml
    /// </summary>
    public partial class SendPatchWindow : Window
    {
        public SendPatchWindow()
        {
            InitializeComponent();
            LoadPatchFilesPaths();
        }

        private void LoadPatchFilesPaths()
        {
            chooseFileComboBox.Items.Clear();
            string destinationFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            chooseFileComboBox.SelectedValuePath = "Key";
            chooseFileComboBox.DisplayMemberPath = "Value";
            foreach (string filePath in Directory.GetFiles(destinationFolder, "Patch.*.zip"))
            {
                string fileName = Path.GetFileName(filePath);
                chooseFileComboBox.Items.Add(new KeyValuePair<string, string>(filePath, fileName));
            }
            chooseFileComboBox.Background = Brushes.Black;
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                var previousCursor = Mouse.OverrideCursor;
                Mouse.OverrideCursor = Cursors.Wait;
                StatusLabel.Foreground = Brushes.White;
                StatusLabel.Content = "Transfering patch to host...";
                await Task.Run(async () => await Task.Delay(20));
                var logic = new ServerIntegrationLogic(hostnameTextBox.Text, usernameTextBox.Text, passwordTextBox.Password);
                Status status = logic.CopyPatchToServer(chooseFileComboBox.SelectedValue.ToString(), pathTextBox.Text);
                if (status == Status.Successful && commandTextBox.Text.Length > 0)
                {
                    StatusLabel.Content = "Executing command on host...";
                    await Task.Run(async () => await Task.Delay(20));
                    status = logic.ExecuteCommandOnServer(commandTextBox.Text);
                }
                if (status == Status.Successful)
                {
                    StatusLabel.Foreground = Brushes.Lime;
                    StatusLabel.Content = "Done. All is good.";
                }
                else
                {
                    StatusLabel.Foreground = Brushes.Red;
                    switch (status)
                    {
                        case Status.Failed_Connect:
                            StatusLabel.Content = "Connection to host failed.";
                            break;
                        case Status.Failed_Transfer:
                            StatusLabel.Content = "Patch transfer to path failed.";
                            break;
                        case Status.Failed_Command:
                            StatusLabel.Content = "Command execution failed.";
                            break;
                        case Status.Unknown:
                            StatusLabel.Content = "Something is wrong... *shrug*";
                            break;
                    }
                }
                Mouse.OverrideCursor = previousCursor;
            }
        }

        private bool ValidateForm()
        {
            bool result = true;
            result &= ValidateNonEmpty(chooseFileComboBox);
            result &= ValidateNonEmpty(hostnameTextBox);
            result &= ValidateNonEmpty(usernameTextBox);
            result &= ValidateNonEmpty(passwordTextBox);
            result &= ValidateNonEmpty(pathTextBox);
            return result;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateNonEmpty((TextBox) sender);
        }

        private void passwordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateNonEmpty((PasswordBox) sender);
        }

        private void comboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateNonEmpty((ComboBox)sender);
        }

        private bool ValidateNonEmpty(TextBox i_TextBox)
        {
            bool isNonEmpty = i_TextBox.Text.Length > 0;
            i_TextBox.BorderBrush = isNonEmpty ? new SolidColorBrush(Color.FromRgb(67, 67, 70)) : Brushes.Red;

            return isNonEmpty;
        }

        private bool ValidateNonEmpty(PasswordBox i_PasswordBox)
        {
            bool isNonEmpty = i_PasswordBox.Password.Length > 0;
            i_PasswordBox.BorderBrush = isNonEmpty ? new SolidColorBrush(Color.FromRgb(67, 67, 70)) : Brushes.Red;

            return isNonEmpty;
        }

        private bool ValidateNonEmpty(ComboBox i_ComboBox)
        {
            bool isNonEmpty = i_ComboBox.SelectedItem != null;
            i_ComboBox.BorderBrush = isNonEmpty ? new SolidColorBrush(Color.FromRgb(67, 67, 70)) : Brushes.Red;

            return isNonEmpty;
        }
    }
}
