using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace PatchCreator2
{
	/// <summary>
	/// Interaction logic for AboutWindow.xaml
	/// </summary>
	public partial class AboutWindow : Window
	{
		private static readonly string CALL_OF_KTHULHU = "https://www.youtube.com/watch?v=sWGOEWdV13M";
		private static readonly string CONTENT = @"Patch Creator Version {0}
Copyright 2015 Ittai Yam";
		private static readonly string CONDITIONS = @"This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see http://www.gnu.org/licenses/.";

		private int clicks = 0;

		public AboutWindow()
		{
			InitializeComponent();
			contentLabel.Content = string.Format(CONTENT, Assembly.GetExecutingAssembly().GetName().Version.ToString());
			conditionTextBox.Text = CONDITIONS;
		}

		private void closeButton_Click(object sender, RoutedEventArgs e)
		{
		Close();
		}

		private void contentLabel_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if ((clicks = ++clicks % 6) == 0)
			{
				System.Diagnostics.Process.Start(CALL_OF_KTHULHU);
			}
		}
	}
}
