using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSideEffects : MonoBehaviour
{
    public static HeroSideEffects HSD;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void executeSideEffect (int hero, bool isBefore, PlayerManager playerManager)
    {
        switch (hero) {
            case 21:
                if (isBefore) {
                    // Ponto Fraco
                    if (playerManager.sideList[0] > 0) {
                        playerManager.sideList[0]--;
                    }
                } else { 
                    // Vodka
                    if (playerManager.cardList[GameOverseer.GO.myCardPlayed].isNullified && playerManager.gameObject.name == "Player Manager") {
                        playerManager.HP--;
                    } else if (playerManager.cardList[GameOverseer.GO.enemyCardPlayed].isNullified && playerManager.gameObject.name == "Enemy Manager") {
                        playerManager.HP--;
                    }
                }
                break;
        }
    }
}
