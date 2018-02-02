using MegaSena.Common;
using MegaSena.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaSena.Stores
{
    public class GameStore : IGameStore<Game>
    {
        private int _currId = 1;
        static object _lockObj = new Object();
        public IDictionary<int, Game> Store { get; private set; }

        public GameStore()
        {
            this.Store = new Dictionary<int, Game>();
        }

        public Game RegisterGame(Game game)
        {
            lock (_lockObj)
            {
                game.Id = _currId;
                Store.Add(_currId, game);
                _currId++;
            }

            return game;
        }
    }
}
