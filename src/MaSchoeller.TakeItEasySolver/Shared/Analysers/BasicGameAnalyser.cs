using MaSchoeller.TakeItEasySolver.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.TakeItEasySolver.Shared.Analysers
{
    public class BasicGameAnalyser : IGameAnalysers
    {
        public Task<(int x, int y)> PlayRoundAsync(Gamefield gamefield, Gamecard gamecard)
        {
            (int x, int y) actualPosition = (-1,-1);
            int maxPoints = 0;
            foreach (var item in gamefield.GetFreePositions())
            {
                var info = gamefield.GetPositioInfo(item.x, item.y,gamecard);
                if (maxPoints < info.Points)
                    actualPosition = item;
            }

            return Task.FromResult(actualPosition);
        }
    }
}
