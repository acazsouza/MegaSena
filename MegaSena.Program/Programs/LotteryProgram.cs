using MegaSena.Common;
using MegaSena.Lotteries;
using MegaSena.ObjectModel;
using MegaSena.Results;
using MegaSena.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaSena
{
    public abstract class LotteryProgram<TLottery, TGame> where TLottery : Lottery
                                                          where TGame : Game
    {
        private int _currId = 1;
        private TLottery _lottery;

        protected IGameStore<Game> GameStore { get; set; }
        protected IDictionary<int, RealizeLotteryResult> HistoryLotteries;

        public LotteryProgram()
        {
            this.GameStore = new GameStore();
            this.HistoryLotteries = new Dictionary<int, RealizeLotteryResult>();
            this._lottery = (TLottery)Activator.CreateInstance(typeof(TLottery));
        }

        public virtual RegisterGameResult TryRegisterGame(TGame newGame)
        {
            var result = new RegisterGameResult();

            var validateResult = newGame.Validate();
            if (validateResult.Success)
            {
                var lotteryValidateReslt = this.ValidateGame(newGame);
                if (lotteryValidateReslt.Success)
                {
                    var regGameResult = this.RegisterGame(newGame);

                    result.Content = regGameResult;
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.Error = lotteryValidateReslt.Error;
                }
            }
            else
            {
                result.Success = false;
                result.Error = validateResult.Error;
            }

            return result;
        }

        public abstract RegisterGameResult RegisterAutomaticGame();
        protected virtual RegisterGameResult RegisterAutomaticGameInternal(TGame newGame)
        {
            var result = new RegisterGameResult();

            var validateResult = newGame.Validate();
            if (validateResult.Success)
            {
                var lotteryValidateReslt = this.ValidateGame(newGame);
                if (lotteryValidateReslt.Success)
                {
                    var regGameResult = this.RegisterGame(newGame);

                    result.Content = regGameResult;
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.Error = lotteryValidateReslt.Error;
                }
            }
            else
            {
                result.Success = false;
                result.Error = validateResult.Error;
            }

            return result;
        }

        public virtual DoLotteryResult DoLottery()
        {
            var result = new DoLotteryResult();
            var realizeLotteryResult = this._lottery.RealizeLottery(this.GameStore);

            if (realizeLotteryResult.Success)
            {
                realizeLotteryResult.Id = _currId;
                _currId++;

                //verificar se esses numeros já foram sorteados, se sim, tentar novamente
                foreach (var lottery in this.HistoryLotteries)
                {
                    if (lottery.Value.LotteryNumbers.SequenceEqual(realizeLotteryResult.LotteryNumbers))
                    {
                        return DoLottery();
                    }
                }

                //armazenar os numeros sorteados, juntamente com os jogos que fizeram quadra, quina e mega
                this.HistoryLotteries.Add(realizeLotteryResult.Id, realizeLotteryResult);
                result.Content = this.GetLotteryOutput(realizeLotteryResult);

                //limpar o gameStoreAtual
                this.GameStore = new GameStore();

                result.Success = true;
            }
            else
            {
                result.Success = false;
                result.Error = realizeLotteryResult.Error;
            }

            return result;
        }

        public GetHistoryResult GetHistory()
        {
            var result = new GetHistoryResult();

            if (this.HistoryLotteries.Count > 0)
            {
                result.Content = GetHistoryOutput();
            }
            else
            {
                result.Content = "Nenhum sorteio foi realizado ainda.";
            }

            result.Success = true;

            return result;
        }

        private string RegisterGame(TGame game)
        {
            game.LotteryNumber = _currId;
            var currGame = this.GameStore.RegisterGame(game);

            return this.GetRegisterGameOutput(currGame);
        }


        private string GetRegisterGameOutput(Game currGame)
        {
            var builder = new StringBuilder();

            builder.AppendLine(string.Format("Registro do jogo {0} do sorteio {1}", currGame.Id, currGame.LotteryNumber));
            builder.AppendLine(string.Format("Criado em: {0}", currGame.CreatedAt.ToString("UTC dd/MM/yyyy HH:mm:ss")));
            builder.AppendLine(string.Format("Números registrados: {0}", string.Join("; ", currGame.Numbers)));

            return builder.ToString();
        }

        private string GetLotteryOutput(RealizeLotteryResult result)
        {
            var builder = new StringBuilder();

            builder.AppendLine();
            builder.AppendLine(string.Format("Sorteio de número {0}:", result.Id));
            builder.AppendLine(string.Format("Números sorteados: {0}", string.Join("; ", result.LotteryNumbers)));
            builder.AppendLine();

            if (result.QuadraGames.Length > 0)
            {
                builder.AppendLine(string.Format("{0} jogos acertaram a quadra:", result.QuadraGames.Length));

                builder.AppendLine();
                foreach (var game in result.QuadraGames)
                {
                    builder.AppendLine(string.Format("Jogo {0}:", game.Id));
                    builder.AppendLine(string.Format("Números do jogo: {0}", string.Join("; ", game.Numbers)));
                    builder.AppendLine(string.Format("Data: {0}", string.Join("; ", game.CreatedAt.ToString("dd/MM/yyyy"))));
                }
                builder.AppendLine();
            }
            else
            {
                builder.AppendLine("Nenhum jogo acertou a quadra.");
            }

            if (result.QuinaGames.Length > 0)
            {
                builder.AppendLine(string.Format("{0} jogos acertaram a quina:", result.QuadraGames.Length));

                builder.AppendLine();
                foreach (var game in result.QuinaGames)
                {
                    builder.AppendLine(string.Format("Jogo {0}:", game.Id));
                    builder.AppendLine(string.Format("Números do jogo: {0}", string.Join("; ", game.Numbers)));
                    builder.AppendLine(string.Format("Data: {0}", string.Join("; ", game.CreatedAt.ToString("dd/MM/yyyy"))));
                }
                builder.AppendLine();
            }
            else
            {
                builder.AppendLine("Nenhum jogo acertou a quina.");
            }


            if (result.MegaGames.Length > 0)
            {
                builder.AppendLine(string.Format("{0} jogos acertaram a mega:", result.QuadraGames.Length));

                builder.AppendLine();
                foreach (var game in result.MegaGames)
                {
                    builder.AppendLine(string.Format("Jogo {0}:", game.Id));
                    builder.AppendLine(string.Format("Números do jogo: {0}", string.Join("; ", game.Numbers)));
                    builder.AppendLine(string.Format("Data: {0}", string.Join("; ", game.CreatedAt.ToString("dd/MM/yyyy"))));
                }
                builder.AppendLine();
            }
            else
            {
                builder.AppendLine("Nenhum jogo acertou a mega.");
            }

            return builder.ToString();
        }

        private string GetHistoryOutput()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Histórico de sorteios:");
            foreach (var history in this.HistoryLotteries)
            {
                builder.Append(this.GetLotteryOutput(history.Value));
            }

            return builder.ToString();
        }

        protected ValidateGameResult ValidateGame(TGame game)
        {
            var result = new ValidateGameResult();

            result.Success = true;

            return result;
        }
    }
}
