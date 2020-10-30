using MaSchoeller.TakeItEasySolver.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.TakeItEasySolver.Shared.Analysers
{
    public interface IGameAnalyser
    {
        Task<(int x, int y)> PlayRoundAsync(Gamefield gamefield, Gamecard gamecard);
    }
}
