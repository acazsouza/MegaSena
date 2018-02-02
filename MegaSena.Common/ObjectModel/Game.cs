using MegaSena.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaSena.ObjectModel
{
    public abstract class Game
    {
        public Game(int[] numbers)
        {
            this.Numbers = numbers;
            this.CreatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; private set; }
        public int LotteryNumber { get; set; }
        public int[] Numbers { get; private set; }

        public abstract ValidateGameResult Validate();
    }
}
