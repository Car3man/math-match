using System.Collections.Generic;
using MathMatch.Game.Behaviours;

namespace MathMatch.Game.Models
{
    public class Segment
    {
        public Cube Start;
        public List<Cylinder> Cylinders;
        public Cube End;
    }
}