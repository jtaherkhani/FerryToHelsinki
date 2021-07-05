using FerryToHelsinki.Constants;
using FerryToHelsinki.Enums;
using FerryToHelsinki.Filing;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryToHelsinki.Commands
{
    public abstract class Command
    {
        protected IJSRuntime _jsRuntime;
        protected TerminalStates _currentTerminalState;
        private string _messagePrompt;

        protected virtual string MessagePrompt { get { return _messagePrompt; } }

        protected abstract string ExpectedMessageValue { get; }

        protected Command(IJSRuntime jsRuntime, TerminalStates currentTerminalState, string messagePrompt)
        {
            _jsRuntime = jsRuntime;
            _currentTerminalState = currentTerminalState;
            _messagePrompt = messagePrompt;
        }

        public abstract Task ExecuteAsync(string messageValue);

        public static Dictionary<string, Command> CreateCommandsDictionary(IJSRuntime jSRuntime, TerminalStates currentTerminalState) =>
            currentTerminalState switch
            {
                //TerminalStates.Opened => CreateTerminalOpenCommands(jSRuntime, messagePrefix).ToDictionary(key => key.ExpectedMessageValue),
                _ => throw new ArgumentOutOfRangeException(nameof(currentTerminalState))
            };

        public static Dictionary<string, Command> CreateCommandsDictionaryTerminalOpened(FileSystem fileSystem, IJSRuntime jSRuntime) =>
            CreateTerminalOpenFileSystemCommands(fileSystem, jSRuntime).ToDictionary(key => key.ExpectedMessageValue);


        private static List<Command> CreateTerminalOpenFileSystemCommands(FileSystem fileSystem, IJSRuntime jSRuntime) =>
            new List<Command>()
            {
                new CommandFileSystemList(fileSystem, jSRuntime),
                new CommandFileSystemHelp(fileSystem, jSRuntime),
                new CommandFileSystemChangeDirectory(fileSystem, jSRuntime),
                new CommandFileSystemPresentWorkingDirectory(fileSystem, jSRuntime),
                new CommandFileSystemExecuteFile(fileSystem, jSRuntime)
            };
    }
}
