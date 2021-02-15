using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PatchCreator2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly PatchCreatorLogic r_logic = new PatchCreatorLogic();

		public MainWindow()
		{
			InitializeComponent();
			itemListBox.ItemsSource = Logic.FilePaths;
		}

		public PatchCreatorLogic Logic
		{
			get
			{
				return r_logic;
			}
		}

		private void closeButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Window_StateChanged(object sender, EventArgs e)
		{
			this.WindowState = System.Windows.WindowState.Normal;
		}

		private void aboutButton_Click(object sender, RoutedEventArgs e)
		{
			Window about = new AboutWindow();
			about.Owner = this;
			about.ShowDialog();
		}

		private void itemListBox_DragEnter(object sender, DragEventArgs e)
		{
			e.Effects = DragDropEffects.Copy;
		}

		private void itemListBox_Drop(object sender, DragEventArgs e)
		{
			Logic.AddFiles((string[])e.Data.GetData(DataFormats.FileDrop));
			ValidateListBox(itemListBox);
		}

		private void grid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left && !(e.OriginalSource is Ellipse))
			{
				DragMove();
			}
		}

		private void numericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			char c = Convert.ToChar(e.Text);
			e.Handled = !char.IsNumber(c);
			OnPreviewTextInput(e);
		}

		private void ellipseButton_MouseEnter(object sender, MouseEventArgs e)
		{
			((Ellipse) e.OriginalSource).Stroke = new SolidColorBrush(Color.FromRgb(126, 180, 234));
		}

		private void ellipseButton_MouseLeave(object sender, MouseEventArgs e)
		{
			((Ellipse) e.OriginalSource).Stroke = Brushes.White;
		}

		private void ellipseButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			createButton.Focus();
			if (ValidateForm())
			{
                bool? proceedWithPatchCreation = true;
                FilesStats fileStats = Logic.GetFilesStats();
                if(!fileStats.AllClear)
                {
                    Window confirm = new ConfirmWindow(BuildConfirmMessage(fileStats));
                    confirm.Owner = this;
                    proceedWithPatchCreation = confirm.ShowDialog();
                }

                if (proceedWithPatchCreation.GetValueOrDefault(false))
                {
                    string pathToPatch = Logic.createPatch(commonBaseTextBox.Text, prodTextBox.Text, buildTextBox.Text, descriptionTextBox.Text);
					if (pathToPatch != null)
					{
						Process.Start("explorer.exe", string.Format("/select,\"{0}\"", pathToPatch));
					}
				}
			}
		}

		private void itemListBox_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key.Equals(Key.Delete))
			{
				IList selected = new ArrayList(itemListBox.SelectedItems);
				foreach (FilePath item in selected)
				{
					Logic.FilePaths.Remove(item);
				}
			}
        }

		private void commonBaseTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			ValidateCommonBase();
		}
        
		private void freeTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			ValidateFreeText((TextBox)sender);
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
			if (r_logic.isOutOfDate())
			{
				Window update = new UpdateWindow();
				update.Owner = this;
				if (update.ShowDialog() == true)
				{
					r_logic.openRemoteVersionLocation();
					Close();
				}
			}
			else
			{
				Window tips = new TipsWindow();
				tips.Owner = this;
				tips.ShowDialog();
			}
        }

        private void prodLabel_Click(object sender, RoutedEventArgs e)
        {
            prodLabel.Content = r_logic.getNextProjectName() + " #";
        }

		private void numericTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			ValidateNumeric((TextBox)sender);
		}

        private string BuildConfirmMessage(FilesStats fileStats)
        {
            StringBuilder confirmWindowMessage = new StringBuilder();
            if (fileStats.MissingCount.Count > 0)
            {
                confirmWindowMessage.AppendLine("The following files cannot be found and will be skipped:");
                foreach (FilePath filePath in fileStats.MissingCount)
                {
                    confirmWindowMessage.Append(" - ").AppendLine(filePath.Path);
                }
                confirmWindowMessage.AppendLine();
            }

            if (fileStats.OutdatedCount.Count > 0)
            {
                confirmWindowMessage.AppendLine("The following files were last modified more than 30 minutes ago:");
                foreach (FilePath filePath in fileStats.OutdatedCount)
                {
                    confirmWindowMessage.Append(" - ").AppendLine(filePath.Path);
                }
                confirmWindowMessage.AppendLine();
            }

            if (fileStats.NotCompiledCount.Count > 0)
            {
                confirmWindowMessage.AppendLine("The following files are uncompiled JAVA files:");
                foreach (FilePath filePath in fileStats.NotCompiledCount)
                {
                    confirmWindowMessage.Append(" - ").AppendLine(filePath.Path);
                }
                confirmWindowMessage.AppendLine();
            }

            return confirmWindowMessage.Append("Would you like to proceed anyway?").ToString();
        }

		private bool ValidateForm()
		{
			bool result = true;
			result &= ValidateNumeric(prodTextBox);
			result &= ValidateNumeric(buildTextBox);
			result &= ValidateFreeText(descriptionTextBox);
			result &= ValidateListBox(itemListBox);
			result &= ValidateCommonBase();
			return result;
		}

		private bool ValidateCommonBase()
		{
			List<FilePath> filePaths = new List<FilePath>(Logic.FilePaths);
			bool result = !commonBaseTextBox.Text.Equals(string.Empty) && !filePaths.Exists((FilePath filePath) =>
				{
					return !filePath.Path.Contains("\\" + commonBaseTextBox.Text + "\\");
				});
			commonBaseTextBox.BorderBrush = result ? new SolidColorBrush(Color.FromRgb(67, 67, 70)) : Brushes.Red;

			return result;
		}

		private bool ValidateListBox(ListBox i_ListBox)
		{
			bool result = i_ListBox.HasItems;
			i_ListBox.BorderBrush = result ? new SolidColorBrush(Color.FromRgb(67, 67, 70)) : Brushes.Red;

			return result;
		}

		private bool ValidateFreeText(TextBox i_TextBox)
		{
			bool result = !i_TextBox.Text.Equals(string.Empty);
			i_TextBox.BorderBrush = result ? new SolidColorBrush(Color.FromRgb(67, 67, 70)) : Brushes.Red;

			return result;
		}

		private bool ValidateNumeric(TextBox i_TextBox)
		{
			bool result = Regex.IsMatch(i_TextBox.Text, "^\\d+$");
			i_TextBox.BorderBrush = result ? new SolidColorBrush(Color.FromRgb(67, 67, 70)) : Brushes.Red;

			return result;
		}

		private void sendButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			sendButton.Focus();
			Window sendPatch = new SendPatchWindow();
			sendPatch.Owner = this;
			sendPatch.ShowDialog();
		}
	}
}
