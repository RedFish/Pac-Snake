/// Richard GUERCI
/// CS426 - Computer Graphics
/// Assignment 2

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace Pac_Snake
{
    class SoundManager
    {
        private SoundEffect sound_begin, sound_death, sound_eatfruit, sound_siren;
        private SoundEffectInstance sound;
        public bool intro_playing;
        private bool gameover_playing;
        private bool m;
        public bool muted
        {
            get
            {
                return m;
            }
            set
            {
                m = value;
                if (sound != null)
                {
                    sound.Volume = GetVolume();
                }
            }
        }

        public SoundManager(ContentManager content)
        {
            sound_begin = content.Load<SoundEffect>("pacman_beginning");
            sound_death = content.Load<SoundEffect>("pacman_death");
            sound_eatfruit = content.Load<SoundEffect>("pacman_eatfruit");
            sound_siren = content.Load<SoundEffect>("pacman_siren");
            muted = false;
        }

        public void Init()
        {
            gameover_playing = false;
            intro_playing = true;
            if (sound != null)
            {
                sound.Stop();
                sound.Dispose();
            }
            sound = sound_begin.CreateInstance();
            sound.Volume = GetVolume();
            sound.Play();
        }

        public void Start()
        {
            if (sound.State != SoundState.Playing)
            {
                intro_playing = false;
                sound.Dispose();
                sound = sound_siren.CreateInstance();
                sound.Volume = GetVolume();
                sound.IsLooped = true;
                sound.Play();
            }
        }

        public void GameOver()
        {
            if (!gameover_playing)
            {
                sound.Stop();
                sound.Dispose();
                sound = sound_death.CreateInstance();
                sound.Volume = GetVolume();
                sound.IsLooped = false;
                sound.Play();
                gameover_playing = true;
            }
        }

        public void Fruit()
        {
            SoundEffectInstance s = sound_eatfruit.CreateInstance();
            s.Volume = GetVolume();
            s.Play();
        }

        private float GetVolume()
        {
            if (muted) return 0;
            else return 1;
        }
    }
}
