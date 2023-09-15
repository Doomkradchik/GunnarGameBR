using Characters.Animations;
using Characters.Sounds;
using MapSystem.Structs;

namespace Characters.Player.States
{
    public class Bottle : BaseState
    {
        public Bottle() : base(null, null, new View(), null)
        {
        }

        public Bottle(IAnimationCommand animation, IAudioCommand audioCommand, View view, VFXTransforms vfxTransforms) : base(animation, audioCommand,
            view, vfxTransforms)
        {
        }

        public override void Tick(float tickTime)
        {
        }

    }
}