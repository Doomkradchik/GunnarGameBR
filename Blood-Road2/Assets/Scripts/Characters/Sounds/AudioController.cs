using MapSystem;
using UnityEngine;


namespace Characters.Sounds
{
    public class AudioController : MapperBase<string, AudioClip>,  IAudioCommand
    {
        private readonly AudioSource audioSource;
        public AudioController(AudioSource audioSource, bool playOnAwake = false)
        {
            this.audioSource = audioSource;
            audioSource.playOnAwake = playOnAwake;
            audioSource.spatialBlend = 0f;
        }

        public float GetAudioLength(string audioname)
        {
            return _dictionary[audioname].length;
        }

        public void Play(AudioParameters audioParameters)
        {
          if(audioSource==null) return;
              audioSource.loop = audioParameters.loop;
            //audioSource.volume = audioParameters.volume > 0.12f ? audioParameters.volume : 1f;
            audioSource.volume = 0; // Currently Modified

            audioSource.priority = audioParameters.priority > 50 ? audioParameters.priority : 128;
            audioSource.Play();
        }

        public void SetAudio(string audioname)
        {
            audioSource.clip = _dictionary[audioname];
        }

        public void Stop()
        {
            if(audioSource.clip != null || audioSource.isPlaying) 
                audioSource.Stop();
        }
    }
}

