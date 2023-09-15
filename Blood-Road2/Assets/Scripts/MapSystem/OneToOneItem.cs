using MapSystem.Structs;
using System.Collections.Generic;
using UnityEngine;

namespace MapSystem
{
    [CreateAssetMenu(fileName = "OneToOneItems", menuName = "Mapper/OneToOneItems")]
    public class OneToOneItem : MapperItem
    {
        [SerializeField] private Item item;

        public override void GetLocalizationLines(List<string> lines)
        {
            TryGetLine(lines, item.UIInfo.Name);
            TryGetLine(lines, item.UIInfo.Description);
        }

        public override void Map(MappersMaped mappers)
        {
            var stateCharacterKey = new StateCharacterKey(item.View.ID, item.View.State?.GetType(), item.Ability.AbilityCommand?.GetType());
            mappers.MappingUI.AddValue(stateCharacterKey, item.UIInfo);
            mappers.MappingView.AddValue(stateCharacterKey, item.View);
            mappers.MappingAbilityByStateKey.AddValue(stateCharacterKey,
                item.Ability);
        }

        public override void SetLocalizationLines(string[] lines, ref int lastIndex, ref int skipTime)
        {
            var copy = item;
            copy.UIInfo = new UIInfo(ReadLine(lines, ref lastIndex, ref skipTime), ReadLine(lines, ref lastIndex, ref skipTime), item.UIInfo.Sprite, item.UIInfo.Cooldown);
            item = copy;
        }
    }
}