using FerryToHelsinki.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryToHelsinki.Models.AppConfig
{
    public class FerryStartStateConfiguration
    {
        public TerminalStates StartingTerminalState { get; } = TerminalStates.Opened;

        public FerryStartStateConfiguration(IConfiguration configuration)
        {
            if (Enum.TryParse<TerminalStates>(configuration.GetValue<string>("StartingTerminalState"), true, out var outTerminalState))
            {
                StartingTerminalState = outTerminalState;
            }
        }
    }
}
