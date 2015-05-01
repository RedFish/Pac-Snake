/// Richard GUERCI
/// CS426 - Computer Graphics
/// Assignment 2
/// This project is based on https://www.youtube.com/user/KnnthRA monogame tutorial

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace Pac_Snake
{
    /// <summary>
    /// This is the board of a game.
    /// Inherit of AnimatedSprites class.
    /// All the logic of the game.
    /// </summary>
    class Board : AnimatedSprites
    {
        /// <summary>
        /// Key pressed are too sensible.
        /// Sensibility variable is incremented each time the key is pressed, 
        /// and makes an action a if the number is over 3 (button M - mute case).
        /// </summary>
        private int sensibility;

        public Board(){}

        /// <summary>
        /// Loads all of the content : background, sprites, sounds, animations
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>("background");
            sprites_texture = content.Load<Texture2D>("sprites");
            sound_manager = new SoundManager(content);
            Init();
            AddAnimation();
        }

        /// <summary>
        /// Allows to get input from the keyboard and to update the world (base.Update(gameTime);)
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            HandleInput(Keyboard.GetState());

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows to get input from the keyboard
        /// - "spacebar" to reset the game.
        /// - "m" to switch on/off the sound.
        /// - arrows to move the pacman
        /// </summary>
        /// <param name="keyboardState">state of the keyboard</param>
        private void HandleInput(KeyboardState keyboardState)
        {
            //New game
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                Init();
            }

            //mute
            if (keyboardState.IsKeyDown(Keys.M))
            {
                sensibility++;
                if (sensibility > 3)
                {
                    sound_manager.muted = !sound_manager.muted;
                    sensibility = 0;
                }
            }

            if (!gameover)
            {
                //Left arrow pressed
                if (keyboardState.IsKeyDown(Keys.Left) && (!current_animation.Equals(Keys.Right.ToString()) || tail_position.Count == 0))
                {
                    pacman_direction = new Vector2(-1, 0) * speed;
                    current_animation = Keys.Left.ToString();
                }

                //Right arrow pressed
                if (keyboardState.IsKeyDown(Keys.Right) && (!current_animation.Equals(Keys.Left.ToString()) || tail_position.Count == 0))
                {
                    pacman_direction = new Vector2(1, 0) * speed;
                    current_animation = Keys.Right.ToString();
                }

                //Down arrow pressed
                if (keyboardState.IsKeyDown(Keys.Up) && (!current_animation.Equals(Keys.Down.ToString()) || tail_position.Count == 0))
                {
                    pacman_direction = new Vector2(0, -1) * speed;
                    current_animation = Keys.Up.ToString();
                }

                //Up arrow pressed
                if (keyboardState.IsKeyDown(Keys.Down) && (!current_animation.Equals(Keys.Up.ToString()) || tail_position.Count == 0))
                {
                    pacman_direction = new Vector2(0, 1) * speed;
                    current_animation = Keys.Down.ToString();
                }
            }
        }
    }
}
