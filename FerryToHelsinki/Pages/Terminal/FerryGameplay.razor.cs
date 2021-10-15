using FerryToHelsinki.Singleton;
using FerryToHelsinkiWebsite.Data.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages.Terminal
{
    public partial class FerryGameplay
    {
        [Inject]
        private GameStateSingleton GameStateSingleton { get; set; }

        private string _startingFerryTerminalFrame = AsciiArt.FerryTimeLineFrame1;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (ShouldRenderForTerminalState)
            {
                await JsRuntime.InvokeVoidAsync("ferryGameplayFunctions.animateTimeline", _startingFerryTerminalFrame);
                GameStateSingleton.GameStarted = true;
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
