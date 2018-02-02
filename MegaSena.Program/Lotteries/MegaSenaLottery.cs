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
    public class MegaSenaLottery : Lottery, ILottery<Game>
    {
        public override RealizeLotteryResult RealizeLottery(IGameStore<Game> store)
        {
            var quadraGames = new List<Game>();
            var quinaGames = new List<Game>();
            var megaGames = new List<Game>();

            var result = new RealizeLotteryResult();

            if (store.Store.Count == 0)
            {
                result.Success = false;
                result.Error = "Não existem jogos pra realizar um sorteio.";
                return result;
            }

            var currentLotteryNumbers = this.GetLotteryNumbers();

            foreach (var storeItem in store.Store)
            {
                var currGame = storeItem.Value;
                var matches = 0;
                foreach (var number in currGame.Numbers)
                {
                    if (currentLotteryNumbers.Contains(number))
                    {
                        matches++;
                    }
                }

                if (matches == 4)
                {
                    quadraGames.Add(currGame);
                }
                else if (matches == 5)
                {
                    quinaGames.Add(currGame);
                }
                else if (matches == 6)
                {
                    megaGames.Add(currGame);
                }
            }

            result.LotteryNumbers = currentLotteryNumbers;

            result.QuadraGames = quadraGames.ToArray();
            result.QuinaGames = quinaGames.ToArray();
            result.MegaGames = megaGames.ToArray();

            result.Success = true;

            return result;
        }

        private int[] GetLotteryNumbers()
        {
            var numbers = new List<int>();

            var i = 0;
            var rnd = new Random();
            do
            {
                numbers.Add(rnd.Next(1, 60));
                i++;
            } while (i < 6);

            return numbers.OrderBy(x => x).ToArray();
        }
    }
}
