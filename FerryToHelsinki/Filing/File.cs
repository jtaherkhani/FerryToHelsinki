
namespace FerryToHelsinki.Filing
{
    public class File
    {
        public string FileName { get; }
        public string FileExtension { get; }

        public File(string fileName, string fileExtension)
        {
            FileName = fileName;
            FileExtension = fileExtension;
        }
    }
}
