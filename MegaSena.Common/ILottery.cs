using MegaSena.ObjectModel;
using MegaSena.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaSena.Common
{
    public interface ILottery<TGame> where TGame : Game
    {
        RealizeLotteryResult RealizeLottery(IGameStore<TGame> store);
    }
}
