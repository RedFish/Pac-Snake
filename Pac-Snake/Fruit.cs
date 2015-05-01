/// Richard GUERCI
/// CS426 - Computer Graphics
/// Assignment 2

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
#endregion

namespace Pac_Snake
{
    /// <summary>
    /// This class manage fruits (position, kind of fruit, score, ...)
    /// </summary>
    class Fruit
    {
        public bool eaten;
        private int number_eaten;
        public int sprite_index;
        public Vector2 position;
        private int[] value;

        /// <summary>
        /// Constructor
        /// Initialize variable (number of fruit eaten, value associated for each fruit).
        /// Create the first fruit.
        /// </summary>
        public Fruit()
        {
            eaten = true;
            number_eaten = -1;
            value = new int[8];
            value[0] = 100;
            value[1] = 300;
            value[2] = 500;
            value[3] = 700;
            value[4] = 1000;
            value[5] = 2000;
            value[6] = 3000;
            value[7] = 5000;
            Refresh();
        }

        /// <summary>
        /// A new fruit is generated (kind, position) if a fruit has been eaten.
        /// The kind of the fruit (sprite_index) depends on the number of fruit eaten.
        /// </summary>
        public void Refresh()
        {
            if (eaten)
            {
                eaten = false;
                number_eaten++;
                if (number_eaten < 5) sprite_index = 0;
                else if (number_eaten < 10) sprite_index = 1;
                else if (number_eaten < 15) sprite_index = 2;
                else if (number_eaten < 20) sprite_index = 3;
                else if (number_eaten < 25) sprite_index = 4;
                else if (number_eaten < 35) sprite_index = 5;
                else if (number_eaten < 45) sprite_index = 6;
                else sprite_index = 7;

                Random rnd = new Random();
                int x = rnd.Next(50, 508); // creates a number between 50 and 508
                int y = rnd.Next(25, 354); // creates a number between 25 and 354
                position = new Vector2(x,y);
            }
        }

        /// <summary>
        /// Get the score of the fruit eaten.
        /// </summary>
        /// <returns>Integer: value of the eaten fruit.</returns>
        public int GetScore()
        {
            return value[sprite_index];
        }
    }
}
