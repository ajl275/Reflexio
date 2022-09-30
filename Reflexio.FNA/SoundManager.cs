using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Reflexio
{
    class SoundManager
    {
        static String current_sound = "";
        private static SoundEffectInstance se;
        static float sound_volume = 1.0f;

        static SoundEffectInstance bkg_sound;
        static String current_bkg_sound;
        static float bkg_sound_volume = 1.0f;

        public static void PlaySound(String sound)
        {
            if (se != null && se.State == SoundState.Playing)
                se.Stop();
            se = GameEngine.Instance.GetMusic(sound).CreateInstance();
            se.IsLooped = false;
            se.Volume = sound_volume;
            se.Play();
            current_sound = sound;
        }

        public static void PlaySoundIfNotPlaying(String sound)
        {
            if (se.State == SoundState.Playing && sound.Equals(current_sound))
                return;
            se = GameEngine.Instance.GetMusic(sound).CreateInstance();
            se.IsLooped = false;
            se.Volume = sound_volume;
            se.Play();
            current_sound = sound;
        }

        public static void PlaySoundLooping(String sound)
        {
            if (se != null && se.State == SoundState.Playing)
                se.Stop();
            se = GameEngine.Instance.GetMusic(sound).CreateInstance();
            se.IsLooped = true;
            se.Volume = sound_volume;
            se.Play();
            current_sound = sound;
        }

        public static void PlaySoundIfNotPlayingLooping(String sound)
        {
            if (se.State == SoundState.Playing && sound.Equals(current_sound))
                return;
            se = GameEngine.Instance.GetMusic(sound).CreateInstance();
            se.IsLooped = true;
            se.Volume = sound_volume;
            se.Play();
            current_sound = sound;
        }

        public static void PlayBkgSound(String sound)
        {
            if (bkg_sound != null && bkg_sound.State == SoundState.Playing)
                bkg_sound.Stop();
            current_bkg_sound = sound;
            bkg_sound = GameEngine.Instance.GetMusic(sound).CreateInstance();
            bkg_sound.IsLooped = true;
            bkg_sound.Volume = bkg_sound_volume;
            bkg_sound.Play();
        }

        public static void PlayBkgSoundIfNotPlaying(String sound)
        {
            if (bkg_sound != null && current_bkg_sound.Equals(sound))
                return;
            if (bkg_sound != null && bkg_sound.State == SoundState.Playing)
                bkg_sound.Stop();
            current_bkg_sound = sound;
            bkg_sound = GameEngine.Instance.GetMusic(sound).CreateInstance();
            bkg_sound.Volume = bkg_sound_volume;
            bkg_sound.IsLooped = true;
            bkg_sound.Play();
        }

        public static void PlayBkgSoundOnce(String sound)
        {
            if (bkg_sound != null && bkg_sound.State == SoundState.Playing)
                bkg_sound.Stop();
            current_bkg_sound = sound;
            bkg_sound = GameEngine.Instance.GetMusic(sound).CreateInstance();
            bkg_sound.IsLooped = false;
            bkg_sound.Volume = bkg_sound_volume;
            bkg_sound.Play();
        }

        public static void PlayBkgSoundOnceIfNotPlaying(String sound)
        {
            if (bkg_sound != null && current_bkg_sound.Equals(sound))
                return;
            if (bkg_sound != null && bkg_sound.State == SoundState.Playing)
                bkg_sound.Stop();
            current_bkg_sound = sound;
            bkg_sound = GameEngine.Instance.GetMusic(sound).CreateInstance();
            bkg_sound.Volume = bkg_sound_volume;
            bkg_sound.IsLooped = false;
            bkg_sound.Play();
        }

        public static void SetVolume(float volume)
        {
            sound_volume = volume;
        }

        public static void SetBkgVolume(float volume)
        {
            bkg_sound_volume = volume;
        }

        public static void StopBkgSounds()
        {
            if (bkg_sound != null)
                bkg_sound.Stop();
        }
    }
}
