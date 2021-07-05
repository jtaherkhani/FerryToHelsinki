using FerryToHelsinki.Enums;
using FerryToHelsinki.Filing;
using FerryToHelsinki.Services;
using FerryToHelsinkiWebsite.Data.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages.Terminal
{
    public partial class OpenedTerminal
    {
        [Parameter]
        public EventCallback<TerminalStates> OnTerminalStateChanged { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        protected MessageService MessageService { get; set; }

        private bool IsRendered;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                MessageService = new MessageService(new FileSystem(), JsRuntime);
                IsRendered = await JsRuntime.InvokeAsync<bool>("terminalFunctions.animateTerminalOpened", TitleContents);
                await InvokeAsync(StateHasChanged);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task UpdateTerminalState(TerminalStates currentTerminalState)
        {
             await OnTerminalStateChanged.InvokeAsync(currentTerminalState);
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
