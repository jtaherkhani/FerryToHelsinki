using FerryToHelsinki.Enums;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace FerryToHelsinki.Commands
{
    public class CommandUndefined : Command
    {
        public CommandUndefined(IJSRuntime jsRuntime, TerminalStates currentTerminalState, string messagePrefix) : base(jsRuntime, currentTerminalState, messagePrefix) { }

        protected override string ExpectedMessageValue => throw new NotImplementedException(); // Undefined is a catch all when nothing can be found.

        public override async Task ExecuteAsync(string messageValue)
        {
            await _jsRuntime.InvokeVoidAsync("terminalFunctions.animateResponse", Message, MessagePrompt);
        }

        private readonly string Message =
            "\n The value enter is not recognized as a Josh command, operable Josh program or a Josh idea. Type help for assistance\n";
    }
}
