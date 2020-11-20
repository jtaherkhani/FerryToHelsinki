using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryToHelsinki.Commands
{
    public interface IChangeTerminalStatesCommand
    {
        public bool HasUpdatedTerminalState();
    }
}
