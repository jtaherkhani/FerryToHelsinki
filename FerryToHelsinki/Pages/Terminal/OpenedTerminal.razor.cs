using FerryToHelsinki.Filing;
using FerryToHelsinki.Services;
using FerryToHelsinkiWebsite.Data.Constants;
using FerryToHelsinkiWebsite.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages.Terminal
{
    public partial class OpenedTerminal
    {
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

        private void ReadNewMessage(Message newMessage)
        {
            Console.WriteLine(newMessage.MessageContents);
        }

        private string[][] TitleContents = new string[][]
        {
            new string[] { AsciiArt.TerminalInitializing, "terminal-initializing" },
            new string[] { AsciiArt.JoshTerminal, "terminal-subtitle"},
            new string[] { AsciiArt.TerminalInitialized, "terminal-initialized"},
            new string[] { "Welcome to Josh Terminal! Type 'help' for a list of commands", "terminal-welcome" }
        };
    }
}
