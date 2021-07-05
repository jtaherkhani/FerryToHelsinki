using FerryToHelsinki.Constants;
using System;

namespace FerryToHelsinki.Filing
{
    public class File
    {
        public string FileName { get; }
        public FileExtension Extension { get; }

        public File(string fileName, FileExtension fileExtension)
        {
            FileName = fileName;
            Extension = fileExtension;
        }

        public FileExecuteResult ExecuteFile() =>
            Extension switch
            {
                FileExtension.exe => Execute(),
                _ => FileExecuteResult.NewExecuteFailue("Unknown extension")
            };

        private FileExecuteResult Execute()
        {
            if (FileName == FerryConstants.FerryFileName)
            {
                return FileExecuteResult.NewExecuteSuccess(true, "Loading...");
            }

            return FileExecuteResult.NewExecuteFailue("Unknown filename");
        }

    }
}
