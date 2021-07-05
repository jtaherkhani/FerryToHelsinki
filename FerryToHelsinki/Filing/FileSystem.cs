using FerryToHelsinki.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FerryToHelsinki.Filing
{
    public class FileSystem
    {
        public const string RootIdentifier = @":";
        public const string DirectorySeparator = "\\";
        private const string BackDirectoryShortcut = @"..";
        private Directory _currentDirectory;

        public FileSystem()
        {
            CreateFileSystem();
            _currentDirectory = _gameDirectory;
        }

        private void CreateFileSystem()
        {
            _rootDirectory = new Directory("Josh");
            _brainDirectory = new Directory(_rootDirectory, "Brain");
            _coronaDirectory = new Directory(_brainDirectory, "Corona");
            _gameDirectory = new Directory(_coronaDirectory, "Games", new List<File>
            { 
                new File(FerryConstants.FerryFileName, FileExtension.exe)
            });
        }

        public string GetCurrentDirectoryPath()
        {
            return _currentDirectory.GetFullFilePath();
        }

        public Dictionary<string, string> GetDirectoryContents()
        {
            return _currentDirectory.GetDirectoryContents();
        }

        public bool TryNavigateDirectories(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                _currentDirectory = _rootDirectory;
                return true;
            }
            else if (directoryPath == BackDirectoryShortcut)
            {
                return TryNavigateToParentDirectory();
            }
            else if (!directoryPath.Contains(DirectorySeparator))
            {
                return TryNavigateToChildDirectory(directoryPath);
            }
            else
            {
                return TryParsePathAndNavigateToDirectory(directoryPath);
            }
        }

        public FileExecuteResult ExecuteFile(string fileName)
        {
            var fileNameParsed = Path.GetFileNameWithoutExtension(fileName);

            var childFile = _currentDirectory.FindChildFile(fileNameParsed);

            if (childFile == null)
            {
                return FileExecuteResult.NewExecuteFailue("Unable to locate file with this name");
            }

            return childFile.ExecuteFile();
        }

        private bool TryNavigateToParentDirectory()
        {
            if (!_currentDirectory.RootNode)
            {
                _currentDirectory = _currentDirectory.ParentDirectory;
            }

            return true;
        }

        private bool TryNavigateToChildDirectory(string directoryPath)
        {
            var childDirectory = _currentDirectory.FindChildDirectory(directoryPath);

            if (childDirectory == null)
            {
                return false;
            }

            _currentDirectory = childDirectory;
            return true;
        }

        private bool TryParsePathAndNavigateToDirectory(string directoryPath)
        {
            var directoryValues = directoryPath.Split(DirectorySeparator, System.StringSplitOptions.RemoveEmptyEntries).ToList();

            if (!directoryValues.Any())
            {
                return false;
            }

            // first value must be the roote entry
            var rootDirectoryName = directoryValues.First().Trim(RootIdentifier.ToCharArray());

            if (rootDirectoryName != _rootDirectory.DirectoryName)
            {
                return false;
            }
            else
            {
                directoryValues.RemoveAt(0); // skip root value
            }

            var currentSearchDirectory = _rootDirectory;
            
            foreach (var directory in directoryValues)
            {
                var nextSearchDirectory = currentSearchDirectory.FindChildDirectory(directory);

                if (nextSearchDirectory == null)
                {
                    return false;
                }

                currentSearchDirectory = nextSearchDirectory;
            }

            _currentDirectory = currentSearchDirectory;
            return true;
        }

        #region FileDirectory

        private Directory _rootDirectory; 
        private Directory _brainDirectory;
        private Directory _coronaDirectory;
        private Directory _gameDirectory;

        #endregion FileDirectory

    }
}
