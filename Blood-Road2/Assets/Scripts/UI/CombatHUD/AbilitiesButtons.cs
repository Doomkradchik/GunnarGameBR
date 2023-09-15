using System.Collections.Generic;
using Characters.Player;
using MapSystem;
using UnityEngine;

namespace UI.CombatHUD
{
    public class AbilitiesButtons : MonoBehaviour
    {
        [SerializeField] private List<ActionButtonCombat> button;
        [SerializeField] private List<Item> abilities;
        [SerializeField] private PlayerController playerController;
        public void AddAbility(Item ability)
        {
            if (abilities.Contains(ability)) return;
                abilities.Add(ability);
            RecheckAbilities();
        }

        public List<Item> GetCopy() => new(abilities);
        

        public void RemoveAbility(Item ability)
        {
            abilities.Remove(ability);
        }
        
        private void Awake()
        {
            RecheckAbilities();
        }

        private void RecheckAbilities()
        {
            for (var i = 0; i < button.Count; i++)
            {
                if (abilities.Count == 0|| abilities.Count-1<i) continue;
                var info = abilities[i];
                button[i].Initialize(() => playerController.UseAbility(info.Ability.AbilityCommand, info.Ability.Cost), info.UIInfo);
            }
        }
    }
}