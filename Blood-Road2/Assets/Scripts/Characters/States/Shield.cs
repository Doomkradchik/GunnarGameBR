using System.Threading.Tasks;
using Characters.Animations;
using Characters.Sounds;
using MapSystem.Structs;

namespace Characters.Player.States
{
    public class Shield : BaseState
    {
        private int _currentMilliseconds;
        public int Milliseconds => _currentMilliseconds;

        public Shield(){}
        public Shield(IAnimationCommand animation,  IAudioCommand audio, View view, VFXTransforms vfxTransforms): base( animation, audio, view,vfxTransforms)
        {
            _parameterName = "shield";
        }
        public override void Enter()
        {
            base.Enter();
            _animation.SetAnimation(_parameterName);
            Wait();
        }

        private async void Wait()
        {
            int milliseconds = SecondToMilliseconds(_animation.LengthAnimation(_parameterName) / 2);
            _currentMilliseconds = milliseconds*2;
            await Task.Delay(milliseconds);
            _currentMilliseconds = milliseconds;
            await Task.Delay(milliseconds);
        }

        public override void Tick(float tickTime)
        {
        }
    }
}