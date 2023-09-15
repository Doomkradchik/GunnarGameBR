using System.Threading.Tasks;
using Characters.Animations;
using MapSystem.Structs;
using Characters.Sounds;

namespace Characters.AbilitiesSystem.States
{
    public class AttackStun : AbilityBase
    {
        public AttackStun(){}
        public AttackStun(IAnimationCommand animation, IAudioCommand audio, View view, VFXTransforms vfxTransforms) : base(animation, audio, view, vfxTransforms)
        {
            _parameterName = "attackStun";
        }
        public override void Enter()
        {
            CanSkip = false;
            base.Enter();
            _animation.SetAnimation(_parameterName);
            Wait();
        }

        private async void Wait()
        {
            int milliseconds = SecondToMilliseconds(_animation.LengthAnimation(_parameterName));
            await Task.Delay(milliseconds);
            CanSkip = true;
        }

        public override void Tick(float tickTime)
        {
            
        }
    }
}