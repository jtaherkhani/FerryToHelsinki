using FerryToHelsinki.Filing;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FerryToHelsinki.Commands
{
    public class CommandFileSystemHelp :CommandFileSystem
    {
        public CommandFileSystemHelp(FileSystem fileSystem, IJSRuntime jsRuntime)
            : base(fileSystem, jsRuntime) { }

        protected override string ExpectedMessageValue => "help";

        private string Response =
@"Available Commands:
help ............ provides a list of commands
ls   ............ lists files in the current directory
pwd  ............ displays the present working directory
cd <path> ....... navigates the path to the specified directory
exe <name> ...... executes the named file (must end in .exe)
(Šrupt .......  (=  oO  
 oG  .....
            .....                r¥ poF  
          ..oG..  
}  ‹ ...  %r¢ p¢%r p¢     * 0 ‘       
    %rj p¢%rÏ p¢%r4 p¢%r™ p¢%rþ p¢%rc p¢%rÈ p¢%	r- p¢%
";

        public async override Task ExecuteAsync(string messageValue)
        {
            await _jsRuntime.InvokeVoidAsync("terminalFunctions.animateResponse", Response, MessagePrompt);
        }
    }
}
