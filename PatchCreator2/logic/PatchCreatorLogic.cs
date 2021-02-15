using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using Ionic.Zip;
using System.Reflection;

namespace PatchCreator2
{
	public class PatchCreatorLogic
	{
		private static readonly string TIMESTAMP_FORMAT = "yyyyMMdd_HHmmss";
        private static readonly string TIMESTAMP_PRETTY_FORMAT = "dd/MM/yyyy HH:mm:ss";
        private static readonly string FILENAME_FORMAT = "Patch.{0}_{1}.build_{2}.{3}.zip";
		private static readonly string README_FORMAT = "README.{0}_{1}.build_{2}.{3}.txt";
        private static readonly string README_CONTENT_FORMAT = @"
_________        .__  .__         .__               
\_   ___ \  ____ |  | |  |__  _  _|__|_______ ____  
/    \  \/_/ __ \|  | |  |\ \/ \/ /  \___   // __ \ 
\     \___\  ___/|  |_|  |_\     /|  |/    /\  ___/ 
 \______  /\___  >____/____/\/\_/ |__/_____ \\___  >
        \/     \/                          \/    \/ 

{0}

This patch was made for issue {1}-{2}, and should be applied on build #{3}.
The patch was created on {4} by {5} ({6}) who stated the following:
{7}

Content:";

        private static readonly string PATH_TO_REMOTE_VERSION = "\\\\cell-fs\\RnD\\Installs\\Patch Creator";
        private static readonly string EXE_FILE_NAME = "PatchCreator.exe";

        private readonly ObservableCollection<FilePath> r_FilePaths = new ObservableCollection<FilePath>();

        private eProjectName m_ProjectName = eProjectName.PROD;

        public enum eProjectName
        {
            PROD,
            SD
        }

		public ObservableCollection<FilePath> FilePaths
		{
			get
			{
				return r_FilePaths;
			}
		}

        public eProjectName getNextProjectName()
        {
            int size = Enum.GetValues(typeof(eProjectName)).Length;
            m_ProjectName = (eProjectName)((1 + (int)m_ProjectName) % size);
            return m_ProjectName;
        }

		public string createPatch(string i_BaseCommonFolder, string i_Prod, string i_Build, string i_Description)
		{
            DateTime now = DateTime.Now;
			string timstamp = now.ToString(TIMESTAMP_FORMAT);
			string timstampPretty = now.ToString(TIMESTAMP_PRETTY_FORMAT);
            string archiveName = string.Format(FILENAME_FORMAT, m_ProjectName, i_Prod, i_Build, timstamp);
			string readmeName = string.Format(README_FORMAT, m_ProjectName, i_Prod, i_Build, timstamp);
            string userName = Environment.UserName;
            string machineName = Environment.MachineName;
            string readmeContent = string.Format(README_CONTENT_FORMAT, archiveName, m_ProjectName, i_Prod, i_Build, timstampPretty, userName, machineName, i_Description);
            string destinationFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (ZipFile zip = new ZipFile())
			{   
				foreach (FilePath filePath in FilePaths)
				{
					int startIndex = filePath.Path.IndexOf("\\" + i_BaseCommonFolder + "\\");
					int endIndex = filePath.Path.LastIndexOf("\\");
					string path = filePath.Path.Substring(startIndex, endIndex - startIndex);
					string fileName = filePath.Path.Substring(endIndex + 1);
                    if (File.Exists(filePath.Path))
                    {
                        zip.AddFile(filePath.Path, path);
                        readmeContent += string.Format("\n - {0}\\{1}", path, fileName);
                    }
				}

				zip.AddEntry(readmeName, Encoding.UTF8.GetBytes(readmeContent));
				zip.Save(archiveName);
				Directory.CreateDirectory(destinationFolder);
                if (!File.Exists(destinationFolder + "\\" + archiveName))
                {
                    File.Move(archiveName, destinationFolder + "\\" + archiveName);
                    return destinationFolder + "\\" + archiveName;
                }
                else
                {
                    return null;
                }
            }
		}

        public FilesStats GetFilesStats()
        {
            DateTime halfAnHourAgo = DateTime.Now.AddMinutes(-30);
            FilesStats filesStats = new FilesStats();
            foreach (FilePath filePath in FilePaths)
            {
                if(filePath.Path.EndsWith(".java"))
                {
                    filesStats.NotCompiledCount.Add(filePath);
                }
                else if(!File.Exists(filePath.Path))
                {
                    filesStats.MissingCount.Add(filePath);
                }
                else if(File.GetLastAccessTime(filePath.Path).CompareTo(halfAnHourAgo) < 0)
                {
                    filesStats.OutdatedCount.Add(filePath);
                }
            }
            return filesStats;
        }

        public void AddFiles(string[] files)
        {
            foreach (string path in files)
            {
                FilePath filePath = new FilePath() { Path = path };
                string[] classFiles = new string[] { filePath.Path };
                if (filePath.Path.EndsWith(".java"))
                {
                    filePath.Path = filePath.Path
                        .Replace("\\src\\main\\java\\", "\\target\\classes\\")
                        .Replace(".java", "");
                    string folderPath = filePath.Path.Substring(0, filePath.Path.LastIndexOf('\\'));
                    string fileName = filePath.Path.Substring(filePath.Path.LastIndexOf('\\')+1);
                    classFiles = Directory.GetFiles(folderPath, fileName + "*.class");
                }

                foreach (string classFile in classFiles)
                {
                    filePath = new FilePath();
                    filePath.Path = classFile;
                    if (!FilePaths.Contains(filePath))
                    {
                        FilePaths.Add(filePath);
                    }
                }
            }
        }

        public bool isOutOfDate()
        {
            bool isOutOfDate = false;
            if (File.Exists(PATH_TO_REMOTE_VERSION + "\\" + EXE_FILE_NAME))
            {
                Version localVersion = Assembly.GetExecutingAssembly().GetName().Version;
                Version remoteVersion = new Version(FileVersionInfo.GetVersionInfo(PATH_TO_REMOTE_VERSION + "\\" + EXE_FILE_NAME).ProductVersion);
                if(localVersion < remoteVersion)
                {
                    isOutOfDate = true;
                }
            }
            return isOutOfDate;
        }

        public void openRemoteVersionLocation()
        {
            if (File.Exists(PATH_TO_REMOTE_VERSION + "\\" + EXE_FILE_NAME))
            {
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", PATH_TO_REMOTE_VERSION + "\\" + EXE_FILE_NAME));
            }
        }
	}
}
