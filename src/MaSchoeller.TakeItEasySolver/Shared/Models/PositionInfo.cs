using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.TakeItEasySolver.Shared.Models
{
    public record PositionInfo(RowInfo Left, RowInfo Right, RowInfo Top)
    {
        public int Points => Left.Points + Right.Points + Top.Points;
    }
}
