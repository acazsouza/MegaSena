using MegaSena.Common;
using MegaSena.ObjectModel;
using MegaSena.Results;
using MegaSena.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaSena.Lotteries
{
    public abstract class Lottery
    {
        public abstract RealizeLotteryResult RealizeLottery(IGameStore<Game> store);
    }
}
