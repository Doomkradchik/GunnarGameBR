using System;
using Better.UnityPatterns.Runtime.StateMachine;
using Characters.Animations;
using Characters.Sounds;
using MapSystem;
using MapSystem.Structs;

namespace Characters.AbilitiesSystem
{
    public abstract class Abilities : IRunAbility
    {
        protected Placeholder _placeholder;
        protected StateCharacterKey _stateCharacterKey;
        protected StateMachine<BaseState> _stateMachine;
        protected Type _currentEffectType;

        public Abilities(AbilityData abilityData)
        {
            _placeholder = abilityData.Placeholder;
            _stateMachine = abilityData.StateMachine;
            _stateCharacterKey = new StateCharacterKey();
            _stateCharacterKey.SetID(0);
        }

        protected abstract void CreateStates(IAnimationCommand animationCommand, IAudioCommand audioCommand, VFXTransforms transforms);

        protected abstract void InitializeTransitions(BaseState idleState);
        protected bool TryGetView(out View view)
        {
            if (_placeholder.TryGetView(_stateCharacterKey, out view))
                return true;
            return false;
        }

        public virtual void StunAttack()
        {
        }

        public virtual void SwordRain()
        {
        }

        public virtual void InductionCoin()
        {
        }


        public virtual void DroneHammer()
        {
        }

        public virtual void ManaShield()
        {
        }

        public void RunAbility(IAbilityCommand command)
        {
            command.Apply(this);
        }

        public virtual void SetTypeAbility(Type type)
        {
            // _currentEffectType = type;
        }

        public virtual void StunTrigger()
        {
            
        }

        public virtual void UnleashingRage()
        {
        }

        public virtual void Armageddon()
        {
        }

        public virtual void Fury()
        {
        }

        public virtual void UniversalBlow()
        {
        }

        public virtual void GhostWolf()
        {
            
        }
    }
}