using FerryToHelsinki.Constants;
using FerryToHelsinki.Enums;
using FerryToHelsinki.Filing;
using Microsoft.JSInterop;

namespace FerryToHelsinki.Commands
{
    public abstract class CommandFileSystem : Command
    {
        protected override string MessagePrompt { get { return _fileSystem.GetCurrentDirectoryPath() + MessageConstants.MessagePrompt; } }
        protected FileSystem _fileSystem;

        protected CommandFileSystem(FileSystem fileSystem, IJSRuntime jsRuntime) : base(jsRuntime, TerminalStates.Opened, fileSystem.GetCurrentDirectoryPath() + MessageConstants.MessagePrompt)
        {
            _fileSystem = fileSystem;
        }
    }
}
