using FerryToHelsinki.Filing;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FerryToHelsinki.Commands
{
    public class CommandFileSystemExecuteFile : CommandFileSystem, IChangeTerminalStatesCommand
    {
        private bool _terminalStateUpdated;

        public CommandFileSystemExecuteFile(FileSystem fileSystem, IJSRuntime jsRuntime)
            : base(fileSystem, jsRuntime) { }

        protected override string ExpectedMessageValue => "exe";

        public override async Task ExecuteAsync(string messageValue)
        {
            var result = _fileSystem.ExecuteFile(messageValue);

            if (string.IsNullOrEmpty(result.MessageToDisplay))
            {
                await _jsRuntime.InvokeVoidAsync("terminalFunctions.animateResponse", result.MessageToDisplay, MessagePrompt);
            }

            _terminalStateUpdated = result.ShouldUpdateTerminalState;
        }

        public bool HasUpdatedTerminalState() => _terminalStateUpdated;
    }
        
}
