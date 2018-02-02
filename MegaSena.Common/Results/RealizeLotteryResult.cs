using MegaSena.ObjectModel;

namespace MegaSena.Results
{
    public class RealizeLotteryResult : Result
    {
        public int Id { get; set; }
        public Game[] QuadraGames { get; set; }
        public Game[] QuinaGames { get; set; }
        public Game[] MegaGames { get; set; }
        public int[] LotteryNumbers { get; set; }
    }
}