using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSideEffects : MonoBehaviour
{
    public static HeroSideEffects HSE;

    // Singleton management (THERE CAN BE ONLY ONE!!!)
    private void Awake()
    {
        HeroSideEffects.HSE = this;
    }

    private void OnEnable()
    {
        if (HSE == null) {
            HSE = this;
        } else {
            if (HSE != this) {
                Destroy(HSE);
                HSE = this;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void executeSideEffect (bool isBefore, PlayerManager playerManager, int cardPlayed)
    {
        switch (playerManager.hero) {
            case 21:
                if (isBefore) {
                    // Ponto Fraco
                    if (playerManager.sideList[0] > 0) {
                        if (playerManager.cardList[cardPlayed] is Damage) {
                            Damage cc = (Damage)playerManager.cardList[cardPlayed];
                            cc.isUnblockable = true;
                            Debug.Log("Ponto Fraco ativado");
                            playerManager.cardList[cardPlayed] = (Card)cc;
                        }
                    }

                } else {
                    // Vodka
                    Debug.Log("Card Played: " + cardPlayed);

                    if (playerManager.cardList[cardPlayed].isNullified)
                        playerManager.HP--;

                    // Ponto Fraco
                    if (playerManager.sideList[0] > 0) {
                        if (playerManager.cardList[cardPlayed] is Damage) {
                            Damage cc = (Damage)playerManager.cardList[cardPlayed];
                            cc.isUnblockable = false;
                            Debug.Log("Ponto Fraco ativado");
                            playerManager.cardList[cardPlayed] = (Card)cc;
                        }
                        playerManager.sideList[0]--;
                    }
                }
                break;
        }
    }
}
