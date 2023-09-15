using Characters.Animations;
using Characters.Sounds;
using MapSystem.Structs;

namespace Characters.Player.States
{
    public class Damaged : BaseState
    {
        public Damaged() : base(null, null, new View(), null)
        {
        }

        public Damaged(IAnimationCommand animation, IAudioCommand audioCommand, View view, VFXTransforms vfxTransforms) : base(animation, audioCommand,
            view, vfxTransforms)
        {
        }

        public override void Tick(float tickTime)
        {
        }
    }
}