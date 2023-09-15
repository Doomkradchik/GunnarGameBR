namespace Characters.AbilitiesSystem
{
    public interface IInteractableAbility
    {
        public void UseAbility(IAbilityCommand abilityCommand);
    }
}