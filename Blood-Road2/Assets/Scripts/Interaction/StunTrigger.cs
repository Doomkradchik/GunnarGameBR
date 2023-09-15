using Characters;
using Characters.AbilitiesSystem.Declaration;
using UnityEngine;

public class StunTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent(out BaseCharacter character))return;
        if(character.IsPlayer()) return;
        character.TakeAbility(new StunAttack());
    }
}
