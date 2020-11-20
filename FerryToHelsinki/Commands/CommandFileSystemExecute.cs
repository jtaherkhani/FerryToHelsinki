using FerryToHelsinki.Filing;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FerryToHelsinki.Commands
{
    public class CommandFileSystemExecute : CommandFileSystem, IChangeTerminalStatesCommand
    {
        CommandFileSystemExecute(FileSystem fileSystem, IJSRuntime jsRuntime)
            : base(fileSystem, jsRuntime) { }

        protected override string ExpectedMessageValue => "exe";

        public override Task ExecuteAsync(string messageValue)
        {
            throw new System.NotImplementedException();
        }

        public bool HasUpdatedTerminalState()
        {
            throw new System.NotImplementedException();
        }

        private string Message = "\nLoading the Ferry...\n";
    }
        
}
