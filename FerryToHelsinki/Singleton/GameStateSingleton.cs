using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryToHelsinki.Singleton
{
    public class GameStateSingleton
    {
        private volatile bool gameStarted;

        public bool GameStarted { get => gameStarted; set => gameStarted = value; }
    }
}
