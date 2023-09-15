using System.Collections.Generic;
using MapSystem.Structs;
using UnityEngine;

namespace MapSystem
{
    [CreateAssetMenu(fileName = "OneToManyItems", menuName = "Mapper/OneToManyItems")]
    public class OneToManyItems : MapperItem
    {
        [SerializeField] private Item one;
        [SerializeField] private List<Item> many;

        public override void GetLocalizationLines(List<string> lines)
        {
            TryGetLine(lines, one.UIInfo.Name);
            TryGetLine(lines, one.UIInfo.Description);

            foreach (var entity in many)
            {
                TryGetLine(lines, entity.UIInfo.Name);
                TryGetLine(lines, entity.UIInfo.Description);
            }
        }

        public override void Map(MappersMaped mappers)
        {
            var stateCharacterKey = new StateCharacterKey(one.View.ID, one.View.State?.GetType(),
                one.Ability.AbilityCommand?.GetType());
            mappers.MappingList.AddValue(stateCharacterKey, many);
            mappers.MappingView.AddValue(stateCharacterKey, one.View);
            mappers.MappingAbilityByStateKey.AddValue(stateCharacterKey, one.Ability);
            mappers.MappingUI.AddValue(stateCharacterKey, one.UIInfo);
            foreach (var item in many)
            {
                mappers.MappingView.AddValue(
                    new StateCharacterKey(item.View.ID, item.View.State?.GetType(),
                        item.Ability.AbilityCommand?.GetType()), item.View);
            }
        }

        public override void SetLocalizationLines(string[] lines, ref int lastIndex, ref int skipTimes)
        {
            var copy = one;
            copy.UIInfo = new UIInfo(ReadLine(lines, ref lastIndex, ref skipTimes),
                ReadLine(lines, ref lastIndex, ref skipTimes), one.UIInfo.Sprite, one.UIInfo.Cooldown);
            one = copy;

            for (int i = 0; i < many.Count; i++)
            {
                copy = many[i];
                copy.UIInfo = new UIInfo(ReadLine(lines, ref lastIndex, ref skipTimes),
                    ReadLine(lines, ref lastIndex, ref skipTimes), many[i].UIInfo.Sprite, many[i].UIInfo.Cooldown);
                many[i] = copy;
            }
        }
    }
}