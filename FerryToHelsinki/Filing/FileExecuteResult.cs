using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryToHelsinki.Filing
{
    public class FileExecuteResult
    {
        public bool Executed { get; }
        public bool ShouldUpdateTerminalState { get; }
        public string MessageToDisplay { get; }

        private FileExecuteResult(bool executed, bool shouldUpdateTerminalState, string messageToDisplay)
        {
            Executed = executed;
            ShouldUpdateTerminalState = shouldUpdateTerminalState;
            MessageToDisplay = messageToDisplay;
        }


        public static FileExecuteResult NewExecuteSuccess(bool shouldUpdateTerminalState, string messageToDisplay)
        {
            return new FileExecuteResult(true, shouldUpdateTerminalState, messageToDisplay);
        }

        public static FileExecuteResult NewExecuteFailue(string messageToDisplay)
        {
            return new FileExecuteResult(false, false, messageToDisplay);
        }
    }
}
