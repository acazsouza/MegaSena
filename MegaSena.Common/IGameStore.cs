using MegaSena.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaSena.Common
{
    public interface IGameStore<TGame> where TGame : Game
    {
        IDictionary<int, TGame> Store { get; }
        Game RegisterGame(Game game);
    }
}
