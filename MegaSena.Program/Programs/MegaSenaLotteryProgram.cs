using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegaSena.ObjectModel;
using MegaSena.Results;
using MegaSena.Lotteries;

namespace MegaSena
{
    public class MegaSenaLotteryProgram : LotteryProgram<MegaSenaLottery, MegaSenaGame>
    {
        public override RegisterGameResult RegisterAutomaticGame()
        {
            var newGame = MegaSenaGame.Random();

            var result = base.RegisterAutomaticGameInternal(newGame);

            return result;
        }
    }
}
