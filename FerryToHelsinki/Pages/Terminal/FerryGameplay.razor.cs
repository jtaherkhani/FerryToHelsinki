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
        private string _helsinki = AsciiArt.Helsinki;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (ShouldRenderForTerminalState)
            {
                await JsRuntime.InvokeVoidAsync("ferryGameplayFunctions.animateTimeline", _startingFerryTerminalFrame);
                GameStateSingleton.GameStarted = true;
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private string FirstMessage =>
            "You find yourself sitting at a desk in a decently sized office. \n There is a small pamphlet on the desk in front of you saying \" Wecome to the Ferry...\". \n A door greets you to the right, along with more desks in front and behind you. What do you do?";
    }
}
