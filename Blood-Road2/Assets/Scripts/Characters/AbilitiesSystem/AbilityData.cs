using Better.UnityPatterns.Runtime.StateMachine;
using Characters.Animations;
using Characters.Player;
using MapSystem;
using Characters.Sounds;

namespace Characters.AbilitiesSystem
{
    public struct AbilityData
    {
        public int ID { get; }
        public VFXTransforms VFXTransforms { get; }
        public Impenetrable ImpenetrableDelegate { get; }

        public StateMachine<BaseState> StateMachine { get; private set; }
        public IAnimationCommand AnimationCommand { get; private set; }
        public IAudioCommand AudioCommand { get; private set; }
        public BaseState IdleState { get; private set; }
        public Placeholder Placeholder { get; }

        public AbilityData(VFXTransforms vfxTransforms, Impenetrable impenetrable, Placeholder placeholder, int id)
        {
            VFXTransforms = vfxTransforms;
            StateMachine = null;
            AnimationCommand = null;
            IdleState = null;
            ImpenetrableDelegate = impenetrable;
            Placeholder = placeholder;
            ID = id;
            AudioCommand = null;
        }

        public void SetStateMachine(StateMachine<BaseState> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public void SetAnimationCommand(IAnimationCommand animationCommand) => AnimationCommand = animationCommand;
        public void SetAudioCommand(IAudioCommand audioCommand) => AudioCommand = audioCommand;
        public void SetIdleState(BaseState state) => IdleState = state;
    }
}