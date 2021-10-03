using FerryToHelsinki.Filing;
using FerryToHelsinki.Services;
using FerryToHelsinkiWebsite.Data.Constants;
using Figgle;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages.Terminal
{
    public partial class FerryStarted : IDisposable
    {
        protected MessageService MessageService { get; set; }

        private DotNetObjectReference<FerryStarted> ferryStartedReference;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                MessageService = new MessageService(new FileSystem(), JsRuntime);
            }

            if (ShouldRenderForTerminalState)
            {
                await JsRuntime.InvokeVoidAsync("terminalFunctions.animateFerries", FerryFrames);

                ferryStartedReference = DotNetObjectReference.Create(this);
                await JsRuntime.InvokeVoidAsync("ferryMainMenuFunctions.animateCanvas", ferryStartedReference);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        [JSInvokable]
        public async Task NewGameStartAsync(string options) // take options in the future and pass back to the gameplay.
        {
            await UpdateTerminalState(Enums.TerminalStates.FerryToHelsinkiLoading);
        }

        public void Dispose()
        {
            if (ferryStartedReference != null)
            {
                ferryStartedReference.Dispose();
            }
        }

        private string Title =>
            FiggleFonts.Slant.Render("Ferry 2 Helsinki 3");

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
