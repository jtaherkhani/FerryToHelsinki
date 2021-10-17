using FerryToHelsinki.Enums;
using FerryToHelsinki.Models.AppConfig;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages.Terminal
{
    public partial class FerryTerminal
    {
        [Inject]
        private FerryStartStateConfiguration StartStateConfiguration { get; set; }

        private TerminalStates CurrentTerminalState;

        protected override void OnInitialized()
        {
            CurrentTerminalState = StartStateConfiguration.StartingTerminalState;

            base.OnInitialized();
        }

        private void UpdateTerminalState(TerminalStates currentTerminalState)
        {
            CurrentTerminalState = currentTerminalState;
        }
    }
}
