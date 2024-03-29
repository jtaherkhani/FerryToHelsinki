﻿using FerryToHelsinki.Commands;
using FerryToHelsinki.Constants;
using FerryToHelsinki.Enums;
using FerryToHelsinki.Filing;
using FerryToHelsinkiWebsite.Data.Models;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FerryToHelsinki.Services
{
    public class MessageService
    {
        private Dictionary<string, Command> _eligibleCommands;
        private Command _undefinedCommand;
        public TerminalStates CurrentTerminalState {get; set;}
        public string MessagePrompt { get; }

        public MessageService(FileSystem fileSystem, IJSRuntime jSRuntime)
        {
            MessagePrompt = fileSystem.GetCurrentDirectoryPath() + MessageConstants.MessagePrompt;

            CurrentTerminalState = TerminalStates.Opened;
            _eligibleCommands = Command.CreateCommandsDictionaryTerminalOpened(fileSystem, jSRuntime);
            _undefinedCommand = new CommandUndefined(jSRuntime, TerminalStates.Opened, MessagePrompt);
        }

        public async Task HandleMessageAsync(Message message)
        {
            (var messageKey, var messageValue) = ParseMessageContents(message);

            if (_eligibleCommands.TryGetValue(messageKey, out var commandToExecute))
            {
                await commandToExecute.ExecuteAsync(messageValue);

                if (commandToExecute is IChangeTerminalStatesCommand terminalStatesCommand
                    && terminalStatesCommand.HasUpdatedTerminalState())
                {
                    CurrentTerminalState = ChangeTerminalState();
                }
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

        private TerminalStates ChangeTerminalState() =>
            CurrentTerminalState switch
            {
                TerminalStates.Opened => TerminalStates.FerryToHelsinkiStart,
                TerminalStates.FerryToHelsinkiStart => TerminalStates.FerryToHelsinkiGameplay,
                _ => throw new ArgumentOutOfRangeException("Unexpected enum value")
            };

    }
}
