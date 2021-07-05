using FerryToHelsinki.Enums;
using System.Threading.Tasks;

namespace FerryToHelsinki.Pages.Terminal
{
    public partial class FerryTerminal
    {
        private TerminalStates CurrentTerminalState = TerminalStates.Opened;

        private void UpdateTerminalState(TerminalStates currentTerminalState)
        {
            CurrentTerminalState = currentTerminalState;
        }
    }
}
