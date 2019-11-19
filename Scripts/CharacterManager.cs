using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    public Character[] characterList;

    public Character GetCharacter(int index)
    {
        return characterList[index];
    }
}
