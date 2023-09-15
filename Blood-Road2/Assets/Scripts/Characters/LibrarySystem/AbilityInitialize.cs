using Characters.AbilitiesSystem;
using Characters.AbilitiesSystem.States;
using MapSystem.Structs;
using UnityEngine;

namespace Characters.LibrarySystem
{
    public class AbilityInitialize : MonoBehaviour
    {
        private void Awake()
        {
            Ability.Initialize();
            InitializeLibrary();
        }

        private void InitializeLibrary()
        {
           AddToLibrary(new DroneHammer(null,null, new View(), null), new AbilitiesSystem.Declaration.DroneHammer());
           AddToLibrary(new AttackStun(null, null, new View(), null), new AbilitiesSystem.Declaration.StunAttack());
           AddToLibrary(new SwordRain(null, null,new View(), null), new AbilitiesSystem.Declaration.SwordRain());
           AddToLibrary(new InductionCoil(null, null, new View(), null), new AbilitiesSystem.Declaration.InductionCoil());
        }
        
        private void AddToLibrary(AbilityBase abilityBase, IAbilityCommand abilityCommand)
        {
            LibrarySystem.Ability.StaticAddEntity(abilityBase.GetType(), abilityCommand);
            LibrarySystem.Ability.StaticAddState(abilityCommand.GetType(), abilityBase.GetType());
        }
    }
}