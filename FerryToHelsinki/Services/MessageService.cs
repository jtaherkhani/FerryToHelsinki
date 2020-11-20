using FerryToHelsinki.Commands;
using FerryToHelsinki.Constants;
using FerryToHelsinki.Enums;
using FerryToHelsinki.Filing;
using FerryToHelsinkiWebsite.Data.Models;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FerryToHelsinki.Services
{
    public class MessageService
    {
        private Dictionary<string, Command> _eligibleCommands;
        private Command _undefinedCommand;
        public string MessagePrompt { get; }

        /*
        public MessageService(IJSRuntime jSRuntime, TerminalStates terminalState, string messagePrefix)
        {
            _eligibleCommands = Command.CreateCommandsDictionary(jSRuntime, terminalState, messagePrefix);
            _undefinedCommand = new CommandUndefined(jSRuntime, terminalState, messagePrefix);
            MessagePrefix = MessagePromptCharacter;
        }
        */

        public MessageService(FileSystem fileSystem, IJSRuntime jSRuntime)
        {
            MessagePrompt = fileSystem.GetCurrentDirectoryPath() + MessageConstants.MessagePrompt;
            _eligibleCommands = Command.CreateCommandsDictionaryTerminalOpened(fileSystem, jSRuntime);
            _undefinedCommand = new CommandUndefined(jSRuntime, TerminalStates.Opened, MessagePrompt);
        }

        public async Task HandleMessageAsync(Message message)
        {
            (var messageKey, var messageValue) = ParseMessageContents(message);

            if (_eligibleCommands.TryGetValue(messageKey, out var commandToExecute))
            {
                await commandToExecute.ExecuteAsync(messageValue);
            }
            else
            {
                await _undefinedCommand.ExecuteAsync(messageValue);
            }
        }

        private (string messageKey, string messageValue) ParseMessageContents(Message message)
        {
            var wordsInMessage = message.MessageContents.Split(' ', 2, System.StringSplitOptions.RemoveEmptyEntries); // split once to ensure we have the right values

            return (messageKey: wordsInMessage.First(), messageValue: wordsInMessage.Skip(1).FirstOrDefault());

        }
    }
}
