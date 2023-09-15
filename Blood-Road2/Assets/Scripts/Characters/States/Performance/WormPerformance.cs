using Characters.Animations;
using Characters.Sounds;
using MapSystem.Structs;
using System.Threading.Tasks;
using UnityEngine;

namespace Characters.Player.States
{
    public sealed class WormPerformance : DefaultPerformance
    {
        private readonly Vector3 _localScale;
        private float _time;

        protected override double RatioEnter => 0.8;
        protected override double RatioExit => 2.4;

        public WormPerformance()
        {
        }

        public WormPerformance(Enable onEnableCharacter, GameObject model, IAnimationCommand animation, IAudioCommand audio, View view, VFXTransforms vfxTransforms) 
            : base(onEnableCharacter, model, animation, audio, view, vfxTransforms)
        {
            _localScale = model.transform.localScale;
        }

        public void SetTimeInSecondsUnderground(float time) => _time = time;

        private async Task PureScaleTransition(Transform transform, Vector3 to, float duration)
        {
            var savedScale = transform.localScale;
            var progress = 0f;
            var expiredSeconds = 0f;

            while (progress < 1f)
            {
                expiredSeconds += Time.deltaTime;
                progress = expiredSeconds / duration;
                transform.localScale = Vector3.Lerp(savedScale, to, progress);
                await Task.Yield();
            }
        }

        protected override async Task Perform(bool enable)
        {
            if (enable) await Task.Delay((int)(_time * 1000));
            await PureScaleTransition(_model.transform, enable ? _localScale: Vector3.zero, 0.2f);
        }
    }
}
