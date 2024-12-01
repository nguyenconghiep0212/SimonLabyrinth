using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] GameManager.PlayableCharacter character;

    public void SelectCharacter()
    {
        GameManager.Instance.ApplySelectedCharacterToPlayer(character); 
    }
}
