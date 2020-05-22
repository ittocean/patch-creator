using System;
using System.Collections.Generic;

namespace PatchCreator2
{
    public class FilesStats
	{
        public List<FilePath> MissingCount { get; }
        public List<FilePath> OutdatedCount { get; }
        public List<FilePath> NotCompiledCount { get; }

        public FilesStats()
        {
            MissingCount = new List<FilePath>();
            OutdatedCount = new List<FilePath>();
            NotCompiledCount = new List<FilePath>();
        }

        public Boolean AllClear
        {
            get
            {
                return (MissingCount.Count + OutdatedCount.Count + NotCompiledCount.Count) == 0;
            }
        }
    }
}
