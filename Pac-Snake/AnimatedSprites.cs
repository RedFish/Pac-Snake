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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace Pac_Snake
{
    abstract class AnimatedSprites
    {
        //sprite position
        protected Vector2 pacman_position;
        protected Vector2 pacman_direction;
        protected Vector2 score_position;
        protected List<Vector2> tail_position;
        protected List<Vector2> temp_last_position;
        protected Texture2D sprites_texture;
        protected Texture2D background;
        private Fruit fruit;

        private Dictionary<string, Rectangle[]> sprite_rectangle;

        private int pacman_frame_index;
        private int player_frame_index;
        private int score_frame_index;

        private double time_elapsed_pacman_animation, time_to_update_pacman_animation;
        private double time_elapsed_blink_animation, time_to_update_blink_animation;
        private double time_elapsed_score;
        public int frames_per_second_pacman
        {
            set { time_to_update_pacman_animation = (1f / value); }
        }
        public int frames_per_second_blink
        {
            set { time_to_update_blink_animation = (1f / value); }
        }

        private int sprite_size;

        protected float speed;

        protected string current_animation;
        private int score;

        private bool ready;
        protected bool gameover;
        private bool score_popup;

        protected SoundManager sound_manager;

        public AnimatedSprites() {}

        protected void Init()
        {
            //set speed
            speed = 100;
            //set score
            score = 0;
            //set sizes
            sprite_size = 20;
            //set position
            pacman_position = new Vector2(background.Width / 2 + 5, background.Height / 2 - 10);
            //set direction
            pacman_direction = new Vector2(-1, 0) * speed;
            //init fruit
            fruit = new Fruit();
            //init tail
            tail_position = new List<Vector2>();
            temp_last_position = new List<Vector2>();
            //set fps
            frames_per_second_pacman = 15;
            frames_per_second_blink = 2;
            //Init sound
            sound_manager.Init();
            //set pacman sprit
            pacman_frame_index = 3;
            //set boolean
            ready = false;
            gameover = false;
            score_popup = false;
            //set direction
            current_animation = Keys.Left.ToString();
        }

        public void AddAnimation()
        {
            sprite_rectangle = new Dictionary<string,Rectangle[]>();

            //Left animation
            Rectangle[] rectangle = new Rectangle[4];
            rectangle[0] = new Rectangle(2, 2, sprite_size, sprite_size);
            rectangle[1] = new Rectangle(2 + sprite_size, 2, sprite_size, sprite_size);
            rectangle[2] = new Rectangle(2, 2, sprite_size, sprite_size);
            rectangle[3] = new Rectangle(2 + sprite_size * 2, 2, sprite_size, sprite_size);//full pacman
            sprite_rectangle.Add(Keys.Left.ToString(), rectangle);

            //Right animation
            rectangle = new Rectangle[4];
            rectangle[0] = new Rectangle(2, 2 + sprite_size, sprite_size, sprite_size);
            rectangle[1] = new Rectangle(2 + sprite_size, 2 + sprite_size, sprite_size, sprite_size);
            rectangle[2] = new Rectangle(2, 2 + sprite_size, sprite_size, sprite_size);
            rectangle[3] = new Rectangle(2 + sprite_size * 2, 2, sprite_size, sprite_size);//full pacman
            sprite_rectangle.Add(Keys.Right.ToString(), rectangle);

            //Up animation
            rectangle = new Rectangle[4];
            rectangle[0] = new Rectangle(2, 2 + sprite_size * 2, sprite_size, sprite_size);
            rectangle[1] = new Rectangle(2 + sprite_size, 2+ sprite_size * 2, sprite_size, sprite_size);
            rectangle[2] = new Rectangle(2, 2 + sprite_size * 2, sprite_size, sprite_size);
            rectangle[3] = new Rectangle(2 + sprite_size * 2, 2, sprite_size, sprite_size);//full pacman
            sprite_rectangle.Add(Keys.Up.ToString(), rectangle);

            //Down animation
            rectangle = new Rectangle[4];
            rectangle[0] = new Rectangle(2, 2 + sprite_size * 3, sprite_size, sprite_size);
            rectangle[1] = new Rectangle(2 + sprite_size, 2+ sprite_size * 3, sprite_size, sprite_size);
            rectangle[2] = new Rectangle(2, 2 + sprite_size * 3, sprite_size, sprite_size);
            rectangle[3] = new Rectangle(2 + sprite_size * 2, 2, sprite_size, sprite_size);//full pacman
            sprite_rectangle.Add(Keys.Down.ToString(), rectangle);

            //End animation
            rectangle = new Rectangle[14];
            rectangle[0] = new Rectangle(2 + sprite_size * 2, 2, sprite_size, sprite_size);//full pacman
            rectangle[1] = new Rectangle(2 + sprite_size, 2 + sprite_size * 2, sprite_size, sprite_size);
            for (int i = 2; i < 14; i++)
            {
                rectangle[i] = new Rectangle(2 + sprite_size * i, 2 + sprite_size * 12, sprite_size, sprite_size);
            }
            sprite_rectangle.Add(Keys.End.ToString(), rectangle);

            //Fruits
            rectangle = new Rectangle[8];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    rectangle[j + i*4] = new Rectangle(8 + sprite_size * 8 + sprite_size * 2 * i, 2 + sprite_size * (8 + j), sprite_size, sprite_size);
                }
            }
            sprite_rectangle.Add("Fruit", rectangle);

            //Digits
            rectangle = new Rectangle[10];
            for (int i = 0; i < 10; i++)
            {
                rectangle[i] = new Rectangle(12 + 10 * i, 2 + sprite_size * 9, 10, 10);
            }
            sprite_rectangle.Add("Digit", rectangle);

            //Ready!
            rectangle = new Rectangle[1];
            rectangle[0] = new Rectangle(200, 0, 50, 10);
            sprite_rectangle.Add("Ready", rectangle);

            //GameOver
            rectangle = new Rectangle[2];
            rectangle[0] = new Rectangle(10, 190, 85, 10);
            rectangle[1] = new Rectangle(10, 190, 85, 10);
            sprite_rectangle.Add("GameOver", rectangle);

            //1player
            rectangle = new Rectangle[2];
            rectangle[0] = new Rectangle(210, 70, 30, 10);
            rectangle[1] = new Rectangle(240, 70, 30, 10);
            sprite_rectangle.Add("1Player", rectangle);

            //score
            rectangle = new Rectangle[8];
            for (int i = 0; i < 8; i++)
            {
                rectangle[i] = new Rectangle(165, 4 + i * 20, 25, 10);
            }
            sprite_rectangle.Add("Score", rectangle);

            //sound
            rectangle = new Rectangle[2];
            rectangle[0] = new Rectangle(210, 100, sprite_size, sprite_size);
            rectangle[1] = new Rectangle(230, 100, sprite_size, sprite_size);
            sprite_rectangle.Add("Sound", rectangle);
        }

        public virtual void Update(GameTime gameTime)
        {
            //Ready sprite at the begining of the game
            if (sound_manager.intro_playing)//pause while introduction sound
            {
                ready = true;
                sound_manager.Start();
            }
            else
            {
                ready = false;

                //Collision detection
                if (!(pacman_position.Y <= 25 //top collision
                    || pacman_position.Y + sprite_size - 5 >= 374 //bottom collision
                    || pacman_position.X <= 50 //left collision
                    || pacman_position.X + sprite_size - 5 >= 528) //right collision
                    && !TailEaten() && !gameover)
                {
                    //move sprite position
                    float delta_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    pacman_position += (pacman_direction * delta_time);
                }
                else //Collision => Game Over
                {
                    current_animation = Keys.End.ToString();//change animation
                    frames_per_second_pacman = 6;//slower animation
                    gameover = true;
                    sound_manager.GameOver();
                }
            }

            //animate pacman
            if (!ready)
            {
                if (!gameover || (gameover && pacman_frame_index < sprite_rectangle[current_animation].Length - 1))
                    time_elapsed_pacman_animation += gameTime.ElapsedGameTime.TotalSeconds;
                if (time_elapsed_pacman_animation > time_to_update_pacman_animation)
                {
                    time_elapsed_pacman_animation -= time_to_update_pacman_animation;
                    pacman_frame_index++;
                    pacman_frame_index %= sprite_rectangle[current_animation].Length;
                }
            }

            //record last position of the tail
            temp_last_position.Add(pacman_position);
            if (temp_last_position.Count > 5) temp_last_position.RemoveAt(0);

            //Pacman eat fruit detection (collision btw 2 sprite)
            Vector2 center_fruit = new Vector2(fruit.position.X + sprite_size / 2, fruit.position.Y + sprite_size / 2);
            if (center_fruit.X >= pacman_position.X && center_fruit.X <= pacman_position.X + sprite_size
                && center_fruit.Y >= pacman_position.Y && center_fruit.Y <= pacman_position.Y + sprite_size)
            {
                fruit.eaten = true;
                sound_manager.Fruit();
                score += fruit.GetScore();
                speed += 1;
                tail_position.AddRange(temp_last_position);
                time_elapsed_score = 0;
                score_frame_index = fruit.sprite_index;
                score_popup = true;
                score_position = new Vector2(fruit.position.X + sprite_size / 2 - 5, fruit.position.Y + sprite_size / 2 - 25 / 2);
            }

            //refresh tail
            tail_position.Add(temp_last_position.ElementAt(0));
            if (temp_last_position.Count!=0) tail_position.RemoveAt(0);

            //generate fruit
            fruit.Refresh();

            //player sprite animation (blink)
            time_elapsed_blink_animation += gameTime.ElapsedGameTime.TotalSeconds;
            if (time_elapsed_blink_animation > time_to_update_blink_animation)
            {
                time_elapsed_blink_animation -= time_to_update_blink_animation;
                player_frame_index++;
                player_frame_index %= 2;
            }

            //score popup 'animation'
            time_elapsed_score += gameTime.ElapsedGameTime.TotalSeconds;
            if (time_elapsed_score > 1) score_popup = false;
        }

        private bool TailEaten()
        {
            if(tail_position.Count>10)
            {
                for (int i = 0; i < tail_position.Count-10; i++)
                {
                    Vector2 tail = tail_position.ElementAt(i);
                    Vector2 center_head = new Vector2(pacman_position.X + sprite_size / 2, pacman_position.Y + sprite_size / 2);
                    int margin = 5;
                    if (center_head.X >= tail.X + margin && center_head.X <= tail.X + sprite_size - margin
                        && center_head.Y >= tail.Y + margin && center_head.Y <= tail.Y + sprite_size - margin)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //background
            spriteBatch.Draw(background, new Vector2(0, 0), new Rectangle(0, 0, background.Bounds.Width, background.Bounds.Height), Color.White);
            //ready
            if (ready) spriteBatch.Draw(sprites_texture, new Vector2(15 + background.Bounds.Width / 2 - sprite_rectangle["Ready"][0].Width / 2, background.Bounds.Height / 2 - 30), sprite_rectangle["Ready"][0], Color.White);
            //gameoverer
            if (gameover) spriteBatch.Draw(sprites_texture, new Vector2(15 + background.Bounds.Width / 2 - sprite_rectangle["GameOver"][0].Width / 2, background.Bounds.Height / 2 - 30), sprite_rectangle["GameOver"][0], Color.White);
            //player
            spriteBatch.Draw(sprites_texture, new Vector2(8, 20), sprite_rectangle["1Player"][player_frame_index], Color.White);
            //sound (on/off)
            if (sound_manager.muted) spriteBatch.Draw(sprites_texture, new Vector2(15, 190), sprite_rectangle["Sound"][1], Color.White);
            else spriteBatch.Draw(sprites_texture, new Vector2(15, 190), sprite_rectangle["Sound"][0], Color.White);
            //score
            char[] s = score.ToString().ToCharArray();
            for (int i = 0; i < s.Length; i++)
            {
                spriteBatch.Draw(sprites_texture, new Vector2(20, 50 + 10 * i), sprite_rectangle["Digit"][Convert.ToInt32(s[i].ToString())], Color.White);
            }
            //fruit collection
            for (int i = 0; i < fruit.sprite_index + 1; i++)
            {
                spriteBatch.Draw(sprites_texture, new Vector2(15, 380 - sprite_size * (i + 1)), sprite_rectangle["Fruit"][i], Color.White);
            }
            //fruit
            if (!ready && !gameover) spriteBatch.Draw(sprites_texture, fruit.position, sprite_rectangle["Fruit"][fruit.sprite_index], Color.White);
            //tail
            if (!gameover) foreach(Vector2 v in tail_position) spriteBatch.Draw(sprites_texture, v, sprite_rectangle[Keys.End.ToString()][0], Color.White);
            //pacman
            spriteBatch.Draw(sprites_texture, pacman_position, sprite_rectangle[current_animation][pacman_frame_index], Color.White);
            //score popup
            if (score_popup) spriteBatch.Draw(sprites_texture, score_position, sprite_rectangle["Score"][score_frame_index], Color.White);
        }
    }
}
