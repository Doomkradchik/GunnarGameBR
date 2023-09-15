using Characters.Animations;
using Dreamteck.Splines;
using MapSystem.Structs;
using Characters.Sounds;
using UnityEngine;
using System.Threading.Tasks;

namespace Characters.Player.States
{
    public class FolowSpline : BaseState
    {
        private SplineFollower _splineFollower;
        private SplineProjector _splineProjector;
        public FolowSpline(){}

        public const float MIN_FROM_SPLINE_DISTANCE = 0.2f;
        public FolowSpline(IAnimationCommand animation, IAudioCommand audio,  View view,
            VFXTransforms vfxTransforms, SplineFollower splineFollower, SplineProjector projector) : base(
            animation, audio,view, vfxTransforms)
        {
            _splineFollower = splineFollower;
            _splineProjector = projector;
            _parameterName = "run";
        }

        public override async void Enter()
        {
            base.Enter();    
            _splineProjector.Project(_splineProjector.projectTarget.position, _splineFollower.result);
            if(Vector3.Angle(_splineProjector.projectTarget.forward, _splineFollower.result.forward) > 10f)
                 await LinearRotateTo(_splineFollower.result.forward, 0.2f);
            _animation.SetAnimation(_parameterName);
            _splineFollower.follow = true;
        }

        private async Task LinearRotateTo(Vector3 direction ,float duration)
        {
            var progress = 0f;
            var expiredTime = 0f;
            var startRotation = _splineProjector.projectTarget.rotation;
            var target = Quaternion.LookRotation(direction, Vector3.up);
            while(progress < 1f)
            {
                expiredTime += Time.deltaTime;
                progress = expiredTime / duration;
                _splineProjector.projectTarget.rotation = Quaternion.Lerp(startRotation, target, progress);
                await System.Threading.Tasks.Task.Yield();
            }
        }

        public Vector3 Project()
        {
            _splineProjector.Project(_splineProjector.projectTarget.position, _splineFollower.result);
            return _splineFollower.result.position;
        }

        public override void Tick(float tickTime)
        {

        }

        protected override void OnExit()
        {
            _splineFollower.follow = false;
            base.OnExit();
        }
    }
}