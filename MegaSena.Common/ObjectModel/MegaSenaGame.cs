using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegaSena.Results;

namespace MegaSena.ObjectModel
{
    public class MegaSenaGame : Game
    {
        public MegaSenaGame(int[] numbers) : base(numbers)
        {

        }

        public static MegaSenaGame Random()
        {
            var currTime = 0;
            var currNumbers = new List<int>();
            var random = new Random();
            do
            {
                currNumbers.Add(random.Next(1, 60));
                currTime++;
            } while (currTime < 6);

            return new MegaSenaGame(currNumbers.ToArray());
        }

        public override ValidateGameResult Validate()
        {
            var result = new ValidateGameResult();
            result.Success = true;

            if (this.Numbers.Length != 6)
            {
                result.Error = "Só é possível escolher 6 números. Tente novamente.";
                result.Success = false;
            }

            return result;
        }
    }
}
