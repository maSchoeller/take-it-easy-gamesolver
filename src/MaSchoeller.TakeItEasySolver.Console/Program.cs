using MaSchoeller.TakeItEasySolver.Shared.Analysers;
using MaSchoeller.TakeItEasySolver.Shared.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MaSchoeller.TakeItEasySolver.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var gameBoard = new Gamefield();
            IGameAnalyser analyser = new BasicGameAnalyser();

            var count = gameBoard.GetFreePositions().Count();
            for (int i = 0; i < count; i++)
            {
                var rnd = GenerateRandomNumber(0, 19 - i);
                var card = gameBoard.GetUnusedGamecards().Skip(rnd).First();
                var result = await analyser.PlayRoundAsync(gameBoard, card);
                gameBoard.TrySetCard(result, card);
                //gameBoard.SaveRound();
                System.Console.WriteLine(gameBoard.ToString());
                System.Console.WriteLine("Boardpoints: " + gameBoard.GetBoardPoints());
                System.Console.ReadKey();
            }
            //var result = gameBoard.GetPositioInfo(4, 2, new Gamecard(3, 4, 5));
            //System.Console.WriteLine(result);
        }

        static int GenerateRandomNumber(int min, int max)
        {
            RNGCryptoServiceProvider c = new RNGCryptoServiceProvider();
            byte[] randomNumber = new byte[4];
            c.GetBytes(randomNumber);
            int result = Math.Abs(BitConverter.ToInt32(randomNumber, 0));
            return result % max + min;
        }
    }
}
