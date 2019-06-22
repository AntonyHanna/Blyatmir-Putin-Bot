using System;
using System.Collections.Generic;
using System.Text;

namespace Blyatmir_Putin_Bot
{
    public class PointManager
    {
        public static int points { get; set; }
        public static int highestPoints { get; set; }
        public static int lowestPoints { get; set; }


        public static void PointCalculations()
        {
            if (points > highestPoints)
                highestPoints = points;

            if (points < lowestPoints)
                lowestPoints = points;
        }
    }
}
