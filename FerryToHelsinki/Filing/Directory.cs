using System.Collections.Generic;
using System.Linq;

namespace FerryToHelsinki.Filing
{
    public class Directory
    {
        private const string DirectoryExtension = "DIR";
        private List<Directory> _childDirectories = new List<Directory>();
        private List<File> _childFiles = new List<File>();

        public string DirectoryName { get; }
        public bool RootNode { get; }
        public Directory ParentDirectory { get; }

        public Directory(string directoryName)
        {
            RootNode = true;
            DirectoryName = directoryName;

        }

        public Directory(Directory parentDirectory, string directoryName)
        {
            ParentDirectory = parentDirectory;

            DirectoryName = directoryName;
            parentDirectory.AddChildDirectory(this);
        }

        public Directory(Directory parentDirectory, string directoryName, List<File> childFields)
        {
            ParentDirectory = parentDirectory;
            _childFiles = childFields;

            DirectoryName = directoryName;

            parentDirectory.AddChildDirectory(this);
        }

        protected void AddChildDirectory(Directory directory)
        {
            _childDirectories.Add(directory);
        }


        public string GetFullFilePath()
        {
            if (RootNode)
            {
                return DirectoryName + FileSystem.RootIdentifier + FileSystem.DirectorySeparator;
            }

            if (ParentDirectory.RootNode)
            {
                return ParentDirectory.GetFullFilePath() + DirectoryName;
            }

            return ParentDirectory.GetFullFilePath() + FileSystem.DirectorySeparator + DirectoryName;
        }

        public Dictionary<string, string> GetDirectoryContents()
        {
            var directoryContents = new Dictionary<string, string>();

            foreach (var directory in _childDirectories)
            {
                directoryContents.Add(DirectoryExtension, directory.DirectoryName);
            }

            foreach (var file in _childFiles)
            {
                directoryContents.Add(file.FileExtension, file.FileName);
            }

            return directoryContents;
        }

        public Directory FindChildDirectory(string directoryName) =>
            _childDirectories.FirstOrDefault(x => x.DirectoryName == directoryName);
    }
}
