using FerryToHelsinki.Filing;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FerryToHelsinki.Commands
{
    public class CommandFileSystemChangeDirectory : CommandFileSystem
    {
        public CommandFileSystemChangeDirectory(FileSystem fileSystem, IJSRuntime jsRuntime) 
            : base(fileSystem, jsRuntime) { }

        protected override string ExpectedMessageValue => "cd";

        public override async Task ExecuteAsync(string messageValue)
        {
            if (!_fileSystem.TryNavigateDirectories(messageValue))
            {
                await _jsRuntime.InvokeVoidAsync("terminalFunctions.animateResponse", FailedNavigation, MessagePrompt);
                return;
            }

            await _jsRuntime.InvokeVoidAsync("terminalFunctions.animateResponse", " ", MessagePrompt);
        }

        private string FailedNavigation = "\nThe system cannot find the path specified.\n";
    }
}
