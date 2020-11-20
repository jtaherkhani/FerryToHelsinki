using FerryToHelsinki.Enums;
using FerryToHelsinki.Filing;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FerryToHelsinki.Commands
{
    public class CommandFileSystemList : CommandFileSystem
    {
        public CommandFileSystemList(FileSystem fileSystem, IJSRuntime jsRuntime) : base(fileSystem, jsRuntime) { }

        protected override string ExpectedMessageValue => "ls";

        public override async Task ExecuteAsync(string messageValue)
        {
            await _jsRuntime.InvokeVoidAsync("terminalFunctions.animateResponse", CreateListResponse(), MessagePrompt);
        }

        private string CreateListResponse()
        {
            const string prefixSpaces = "  ";

            var response = prefixSpaces + "Directory of " + _fileSystem.GetCurrentDirectoryPath() + "\n\n";

            var currentDirectoryContents = _fileSystem.GetDirectoryContents();

            foreach (var directoryContent in currentDirectoryContents)
            {
                response += prefixSpaces + "<" + directoryContent.Key.ToUpper() + ">" + "          " + directoryContent.Value + "\n";
            }

            return response;
        }
    }
}
