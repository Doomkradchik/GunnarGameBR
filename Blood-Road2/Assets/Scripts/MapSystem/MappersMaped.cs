using MapSystem.Mappers;

namespace MapSystem
{
    public struct MappersMaped
    {
        public MappingView MappingView { get; }
        public MappingUI MappingUI { get; }
        public MappingList MappingList { get; }
        public MappingAbilityByUIInfo MappingAbilityByUIInfo { get; }
        public MappingAbilityByStateKey MappingAbilityByStateKey { get; }

        public MappersMaped(MappingUI mappingUI, MappingView mappingView,
            MappingAbilityByUIInfo mappingAbilityByUIInfo,
            MappingAbilityByStateKey mappingAbilityByStateKey, MappingList mappingList)
        {
            MappingView = mappingView;
            MappingUI = mappingUI;
            MappingAbilityByUIInfo = mappingAbilityByUIInfo;
            MappingAbilityByStateKey = mappingAbilityByStateKey;
            MappingList = mappingList;
        }
    }
}