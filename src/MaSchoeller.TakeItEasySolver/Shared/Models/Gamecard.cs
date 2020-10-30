using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.TakeItEasySolver.Shared.Models
{
    public record Gamecard(int Top, int Right, int Left)
    {
        public static IEnumerable<Gamecard> GetAllCards()
        {
            //Nines
            yield return new Gamecard(9, 3, 2);
            yield return new Gamecard(9, 4, 2);
            yield return new Gamecard(9, 8, 2);
            yield return new Gamecard(9, 3, 6);
            yield return new Gamecard(9, 3, 7);
            yield return new Gamecard(9, 4, 6);
            yield return new Gamecard(9, 4, 7);
            yield return new Gamecard(9, 8, 6);
            yield return new Gamecard(9, 8, 7);

            //Ones
            yield return new Gamecard(1,3,2);
            yield return new Gamecard(1,4,2);
            yield return new Gamecard(1,8,2);
            yield return new Gamecard(1,3,6);
            yield return new Gamecard(1,3,7);
            yield return new Gamecard(1,4,6);
            yield return new Gamecard(1,4,7);
            yield return new Gamecard(1, 8, 6);
            yield return new Gamecard(1, 8, 7);

            //Fives
            yield return new Gamecard(5, 3, 2);
            yield return new Gamecard(5, 4, 2);
            yield return new Gamecard(5, 8, 2);
            yield return new Gamecard(5, 3, 6);
            yield return new Gamecard(5, 3, 7);
            yield return new Gamecard(5, 4, 6);
            yield return new Gamecard(5, 4, 7);
            yield return new Gamecard(5, 8, 6);
            yield return new Gamecard(5, 8, 7);

        }
    }
}
