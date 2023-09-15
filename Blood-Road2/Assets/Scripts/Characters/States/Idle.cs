using Characters.Animations;
using MapSystem.Structs;
using Characters.Sounds;

namespace Characters.Player.States
{
    public class Idle : BaseState
    {
        public Idle(){}
        public Idle(IAnimationCommand animation, IAudioCommand audio, View view, VFXTransforms vfxTransforms) : base(animation, audio, view,vfxTransforms)
        {
            _parameterName = "idle";
        }

        public override void Enter()
        {
            base.Enter();
            _animation.SetAnimation(_parameterName);
           
        }

        public override void Tick(float tickTime)
        {
        }
    }
}