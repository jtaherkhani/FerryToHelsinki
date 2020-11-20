using FerryToHelsinki.Filing;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FerryToHelsinki.Commands
{
    public class CommandFileSystemPresentWorkingDirectory : CommandFileSystem
    {
        public CommandFileSystemPresentWorkingDirectory(FileSystem fileSystem, IJSRuntime jsRuntime)
            : base(fileSystem, jsRuntime) { }

        protected override string ExpectedMessageValue => "pwd";

        public async override Task ExecuteAsync(string messageValue)
        {
            await _jsRuntime.InvokeVoidAsync("terminalFunctions.animateResponse", CreateMessage(messageValue), MessagePrompt);
        }

        private string CreateMessage(string messageValue) =>
            string.Format("\n{0}\n", _fileSystem.GetCurrentDirectoryPath());
    }
}
