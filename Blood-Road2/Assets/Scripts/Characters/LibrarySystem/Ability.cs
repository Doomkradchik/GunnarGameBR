using Characters.AbilitiesSystem;

namespace Characters.LibrarySystem
{
    public class Ability : Library<IAbilityCommand>
    {
        public static void Initialize()
        {
            _entityLibraryInstance = new Ability();
        }
    }
}