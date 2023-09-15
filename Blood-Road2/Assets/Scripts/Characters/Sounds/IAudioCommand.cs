using MapSystem;
using UnityEngine;


namespace Characters.Sounds
{
    public interface IAudioCommand : IMapper<string, AudioClip>
    {
        public float GetAudioLength(string audioname);
        public void SetAudio(string audioname);
        public void Play(AudioParameters audioParameters);
        public void Stop();
    }
}
