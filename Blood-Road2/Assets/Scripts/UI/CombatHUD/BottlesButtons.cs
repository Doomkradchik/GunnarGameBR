using System.Collections.Generic;
using Banks;
using Characters.Player;
using Characters.Player.States;
using MapSystem;
using MapSystem.Structs;
using UnityEngine;

namespace UI.CombatHUD
{
    public class BottlesButtons : MonoBehaviour
    {
        [SerializeField] private ActionButtonCombat[] buttons;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Placeholder placeholder;
        private List<BaseBank> _banks;

        private void Start()
        {
            _banks = new List<BaseBank>();
            if (placeholder.TryGetList(new StateCharacterKey(0, typeof(Bottle), null), out List<Item> bottles))
                for (var i = 0; i < buttons.Length; i++)
                {
                    var bank = new Bank.Bottle();     
                    var info = bottles[i].UIInfo;
                    bank.Initialize(info.Name);
                    var effectData = bottles[i].Ability.EffectData;
                    var delegates = bank.Delegates;
                    buttons[i].Initialize((() => playerController.UseBottle(effectData)), info,
                        delegates.Remove);

                    delegates.InitGetValue.Subscribe(buttons[i].SetValue);

                    _banks.Add(bank);
                }
        }

        private void OnDestroy()
        {
            for (var i = 0; i < _banks.Count; i++)
            {
                var delegates = _banks[i].Delegates;
                delegates.InitGetValue.Unsubscribe(buttons[i].SetValue);
            }
        }
    }
}