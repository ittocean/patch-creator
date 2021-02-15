﻿using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.IO;

namespace PatchCreator2
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		static App()
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);
		}

		private static Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			Assembly thisAssembly = Assembly.GetExecutingAssembly();

			//Get the Name of the AssemblyFile
			var name = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";

			//Load form Embedded Resources - This Function is not called if the Assembly is in the Application Folder
			var resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(name));
			if (resources.Count() > 0)
			{
				var resourceName = resources.First();
				using (Stream stream = thisAssembly.GetManifestResourceStream(resourceName))
				{
					if (stream == null) return null;
					var block = new byte[stream.Length];
					stream.Read(block, 0, block.Length);
					return Assembly.Load(block);
				}
			}
			return null;
		}
	}
}
