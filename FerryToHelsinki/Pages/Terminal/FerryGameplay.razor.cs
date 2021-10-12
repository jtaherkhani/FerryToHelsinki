using FerryToHelsinki.Singleton;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages.Terminal
{
    public partial class FerryGameplay
    {
        [Inject]
        private GameStateSingleton GameStateSingleton { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (ShouldRenderForTerminalState)
            {
                GameStateSingleton.GameStarted = true;
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
