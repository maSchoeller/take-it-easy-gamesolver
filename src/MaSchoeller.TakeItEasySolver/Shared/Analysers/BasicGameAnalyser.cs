using MaSchoeller.TakeItEasySolver.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.TakeItEasySolver.Shared.Analysers
{
    public class BasicGameAnalyser : IGameAnalyser
    {
        public Task<(int x, int y)> PlayRoundAsync(Gamefield gamefield, Gamecard gamecard)
        {
            (int x, int y) actualPosition = (-1,-1);
            int maxPoints = 0;
            var free = gamefield.GetFreePositions().ToArray();
            foreach (var item in free)
            {
                var info = gamefield.GetPositioInfo(item.x, item.y,gamecard,false);
                if (maxPoints < info.Points || (info.Points == 0 && maxPoints == 0))
                {
                    actualPosition = item;
                    maxPoints = info.Points;
                }
                //Console.WriteLine(gamefield.ToString());
                //Console.WriteLine("Gesamt:"+ info.Points);
                //Console.WriteLine("Top:"+ info.Top.Points);
                //Console.WriteLine("Rechts:"+ info.Right.Points);
                //Console.WriteLine("Links:"+ info.Left.Points);
                //Console.WriteLine("Position:"+item);
                gamefield.TryUndoLastMove();
            }

            return Task.FromResult(actualPosition);
        }
    }
}
