using FerryToHelsinki.Enums;
using FerryToHelsinkiWebsite.Data.Constants;
using FerryToHelsinkiWebsite.Data.Models;
using Figgle;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages.Terminal
{
    public partial class FerryTerminal
    {
        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        private Message CurrentMessage { get; set; }

        private TerminalStates CurrentTerminalState = TerminalStates.Opened;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await JsRuntime.InvokeVoidAsync("terminalFunctions.animateFerries", FerryFrames);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private string Title =>
            FiggleFonts.Slant.Render("Ferry To Helsinki 3");

        private string SubTitle =>
            FiggleFonts.Epic.Render("This time it's personal");

        private string[] FerryFrames = new string[14]
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
