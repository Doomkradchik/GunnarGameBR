using System.Collections.Generic;
using Characters.Player;
using MapSystem;
using MapSystem.Structs;
using UnityEngine;

namespace UI.CombatHUD
{
    public class AttackButtons : MonoBehaviour
    {
        [SerializeField] private ActionButtonCombat[] buttons;
        [SerializeField] private Placeholder placeholder;
        [SerializeField] private AttackVariants attackVariants;

        private void Start()
        {
            var key = new StateCharacterKey(0, typeof(Characters.Player.States.Attack), null);
            if (!placeholder.TryGetList(key, out List<Item> items)) return;
            if (!placeholder.TryGetUIInfo(key, out UIInfo info)) return;
            if (!placeholder.TryGetAbility(key, out Ability ability)) return;
            if (!placeholder.TryGetView(key, out View view)) return;
            items.Add(new Item(ability, info, view));
            for (int i = 0; i < buttons.Length; i++)
            {
                var a = i;
                buttons[i].Initialize(() => attackVariants.Attack(a), items[i].UIInfo, attackVariants.CanAttack);
            }
        }
    }
}