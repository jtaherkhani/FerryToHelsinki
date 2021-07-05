using FerryToHelsinki.Enums;
using FerryToHelsinki.Services;
using FerryToHelsinkiWebsite.Data.Constants;
using Figgle;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages.Terminal
{
    public partial class FerryStarted
    {
        [Parameter]
        public EventCallback<TerminalStates> OnTerminalStateChanged { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        protected MessageService MessageService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsRuntime.InvokeVoidAsync("terminalFunctions.animateFerries", FerryFrames);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task UpdateTerminalState(TerminalStates currentTerminalState)
        {
            await OnTerminalStateChanged.InvokeAsync(currentTerminalState);
        }

        private string Title =>
            FiggleFonts.Slant.Render("Ferry To Helsinki 3");

        private string SubTitle =>
            FiggleFonts.Epic.Render("This time it's personal");

        private readonly string[] FerryFrames = new string[14]
            {AsciiArt.FerryFrame1,
             AsciiArt.FerryFrame2,
             AsciiArt.FerryFrame3,
             AsciiArt.FerryFrame4,
             AsciiArt.FerryFrame5,
             AsciiArt.FerryFrame6,
             AsciiArt.FerryFrame7,
             AsciiArt.FerryFrame8,
             AsciiArt.FerryFrame9,
             AsciiArt.FerryFrame10,
             AsciiArt.FerryFrame11,
             AsciiArt.FerryFrame12,
             AsciiArt.FerryFrame13,
             AsciiArt.FerryFrame14};
    }
}
