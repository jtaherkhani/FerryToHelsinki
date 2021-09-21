using FerryToHelsinki.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FerryToHelsinki.Shared.BaseComponents
{
    public abstract class TerminalStateComponent : ComponentBase
    {
        [Parameter]
        public EventCallback<TerminalStates> OnTerminalStateChanged { get; set; }

        [Parameter]
        public TerminalStates CurrentTerminalState { get; set; }
        
        [Parameter]
        public TerminalStates ExpectedTerminalState { get; set; }

        [Inject]
        protected IJSRuntime JsRuntime { get; set; }

        /// <summary>
        /// Returns true if the current terminal state matches the expected state; otherwise false.
        /// </summary>
        protected bool ShouldRenderForTerminalState =>
            ExpectedTerminalState == CurrentTerminalState;

        /// <summary>
        /// Calls the event callback when the terminal state has been changed.
        /// </summary>
        /// <param name="newTerminalState">The new value of <see cref="TerminalStates"/>.</param>
        /// <returns>A <see cref="Task"/> that can be awaited.</returns>
        protected async Task UpdateTerminalState(TerminalStates newTerminalState) =>
            await OnTerminalStateChanged.InvokeAsync(newTerminalState);
    }
}
