using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MaSchoeller.TakeItEasySolver.Shared.Models
{
    public class Gamefield : IEnumerable<((int x, int y) Position, Gamecard? Card)>
    {
        private readonly Gamecard?[,] _field;
        private readonly Stack<(int x, int y)> _roundChanges;

        //      Y/X          ---
        //             ___ / 4/4 \ ___ 
        //           / 4/3 \     / 3/4 \     
        //      ___  \       ---         ---
        //    / 4/2    ___ / 3/3 \ ___ / 2/4 \
        //    \ ___  / 3/2 \     / 2/3 \     /
        //    / 3/1  \       ---         ---
        //    \        ___ / 2/2 \ ___ / 1/3 \
        //      ___  / 2/1 \     / 1/2 \     /
        //    / 2/0  \       ---         ---
        //    \ Y/X    ___ / 1/1 \ ___ / 0/2 \
        //      ___  / 1/0 \     / 0/1 \ Y/X /
        //           \ Y/X   ---   Y/X   ---
        //             ___ / 0/0 \ ___ / 
        //                 \ Y/X /    
        //                   ---

        public Gamefield()
        {
            _field = new Gamecard[5,5];

            _roundChanges = new Stack<(int x, int y)>();
        }

        public IEnumerator<((int x, int y) Position, Gamecard? Card)> GetEnumerator()
        {
            for (int i = 0; i < 3; i++)
                yield return ((i, 0), _field[i,0]);
            for (int i = 0; i < 4; i++)
                yield return ((i, 1), _field[i, 1]);
            for (int i = 0; i < 5; i++)
                yield return ((i, 2), _field[i, 2]);
            for (int i = 1; i < 5; i++)
                yield return ((i, 3), _field[i, 3]);
            for (int i = 2; i < 5; i++)
                yield return ((i, 4), _field[i, 4]);
        }
        public Gamecard? this[(int x, int y) index]
        {
            get => _field[index.x,index.y];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void SaveRound() => _roundChanges.Clear();

        public void ResetRound()
        {
            while (TryUndoLastMove()) ;
        }

        public bool TrySetCard((int x, int y) position, Gamecard gamecard)
        {
            //TODO: Check out of range for the cards..
            if (_field[position.x,position.y] is not null)
            {
                return false;
            }
            _field[position.x,position.y] = gamecard;
            _roundChanges.Push(position);
            return true;
        }

        public bool TryUndoLastMove()
        {
            var success = _roundChanges.TryPop(out var pos);
            if (!success) return false;
            _field[pos.x,pos.y] = null;
            return true;
        }

        public IEnumerable<Gamecard> GetUnusedGamecards()
            => Gamecard.GetAllCards().Except(this.Select(g => g.Card!).Distinct());

        public IEnumerable<(int x, int y)> GetFreePositions() 
            => this.Where(i => i.Card is null).Select(i => i.Position);

        public PositionInfo GetPositioInfo(int x, int y, Gamecard gamecard, bool removeAfterCalculate = true)
        {
            TrySetCard((x, y), gamecard);

            var rightRowInfo = CalulateRightRow(x);
            var leftRowInfo = CalulateRightRow(y);
            var topRowInfo = CalulateTopRow((x, y) switch
            {
                (2, 0) or (3, 1) or (4, 2) => 1,
                (1, 0) or (2, 1) or (3, 2) or (4, 3) => 2,
                (1, 1) or (2, 2) or (3, 3) or (4, 4) or (5, 5) => 3,
                (0, 1) or (1, 2) or (2, 3) or (3, 4) => 4,
                (0, 2) or (1, 3) or (2, 4) => 5,
                _ => throw new ArgumentException()
            });
            if (removeAfterCalculate) TryUndoLastMove();
            return new PositionInfo(leftRowInfo, rightRowInfo, topRowInfo);
        }

        private RowInfo CalulateTopRow(int row)
        {
            (int x, int y) cord = row switch
            {
                1 => (0, 2),
                2 => (0, 1),
                3 => (0, 0),
                4 => (1, 0),
                5 => (2, 0),
                _ => throw new ArgumentException()
            };
            int maxCount = 5 - (cord.x + cord.y);
            int number = 0;
            bool isFinished = true;
            bool isCorruped = false;
            for (int i = 0; i < maxCount; i++)
            {
                if (_field[cord.y + 1,cord.x + i] is Gamecard card)
                {
                    if (number == 0)
                    {
                        number = card.Top;
                    }
                    else
                    {
                        isCorruped = isCorruped || card.Top != number;
                    }

                }
                else
                {
                    isFinished = false;
                }
            }
            return new RowInfo(isFinished, isCorruped, (isCorruped ? maxCount * number : 0));
        }

        private RowInfo CalulateRightRow(int row)
        {
            int number = 0;
            bool isFinished = true;
            bool isCorruped = false;
            for (int i = 0; i < (2 + row % 3); i++)
            {
                if (_field[i,row] is Gamecard card)
                {
                    if (number == 0)
                    {
                        number = card.Right;
                    }
                    else
                    {
                        isCorruped = isCorruped || card.Right != number;
                    }

                }
                else
                {
                    isFinished = false;
                }
            }
            return new RowInfo(isFinished, isCorruped, (isCorruped ? (2 + row % 3) * number : 0));
        }

        private RowInfo CalulateLeftRow(int row)
        {
            int number = 0;
            bool isFinished = true;
            bool isCorruped = false;
            for (int i = 0; i < (2 + row % 3); i++)
            {
                if (_field[row,i] is Gamecard card)
                {
                    if (number == 0)
                    {
                        number = card.Left;
                    }
                    else
                    {
                        isCorruped = isCorruped || card.Left != number;
                    }

                }
                else
                {
                    isFinished = false;
                }
            }
            return new RowInfo(isFinished, isCorruped, (isCorruped ? (2 + row % 3) * number : 0));
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine(@$"       Y/X         ---                     ");
            builder.AppendLine(@$"                 /     \                   ");
            builder.AppendLine(@$"             ---         ---               ");
            builder.AppendLine(@$"           /     \     /     \             ");
            builder.AppendLine(@$"       ---         ---         ---         ");
            builder.AppendLine(@$"     /     \     /     \     /     \       ");
            builder.AppendLine(@$"             ---         ---               ");
            builder.AppendLine(@$"     \     /     \     /     \     /       ");
            builder.AppendLine(@$"       ---         ---         ---         ");
            builder.AppendLine(@$"     /     \     /     \     /     \       ");
            builder.AppendLine(@$"             ---         ---               ");
            builder.AppendLine(@$"     \     /     \     /     \     /       ");
            builder.AppendLine(@$"       ---         ---         ---         ");
            builder.AppendLine(@$"     /     \     /     \     /     \       ");
            builder.AppendLine(@$"             ---         ---               ");
            builder.AppendLine(@$"     \     /     \     /     \     /       ");
            builder.AppendLine(@$"       ---         ---         ---         ");
            builder.AppendLine(@$"           \     /     \     /             ");
            builder.AppendLine(@$"             ---         ---               ");
            builder.AppendLine(@$"                 \     /                   ");
            builder.AppendLine(@$"                   ---                     ");


           

            return base.ToString();
        }
    }
}
