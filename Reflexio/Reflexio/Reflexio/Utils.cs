using Microsoft.Xna.Framework;

using System;

namespace Reflexio
{
    public class Utils
    {
        public const float EPSILON = 0.001f;

        private static Random random;
        public static Random Random
        {
            get
            {
                if (random == null)
                    random = new Random();
                return random;
            }
        }
    }
}