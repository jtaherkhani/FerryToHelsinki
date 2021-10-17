using FerryToHelsinki.Filing;
using FerryToHelsinki.Services;
using FerryToHelsinki.Singleton;
using FerryToHelsinkiWebsite.Data.Constants;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages.Terminal
{
    public partial class OpenedTerminal
    {
        protected MessageService MessageService { get; set; }

        [Inject]
        private GameStateSingleton GameStateSingleton { get; set; }

        private bool IsRendered;
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (ShouldRenderForTerminalState && firstRender)
            {
                MessageService = new MessageService(new FileSystem(), JsRuntime);
                IsRendered = await JsRuntime.InvokeAsync<bool>("terminalFunctions.animateTerminalOpened", TitleContents);
                await InvokeAsync(StateHasChanged);
                GameStateSingleton.GameStarted = false;
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private readonly string[][] TitleContents = new string[][]
        {
            new string[] { AsciiArt.TerminalInitializing, "terminal-initializing" },
            new string[] { AsciiArt.JoshTerminal, "terminal-subtitle"},
            new string[] { AsciiArt.TerminalInitialized, "terminal-initialized"},
            new string[] { "Welcome to Josh Terminal! Type 'help' for a list of commands", "terminal-welcome" }
        };
    }
}
