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

    // Phases: 0 Choice, 1 Revelation, 2 EffectsBefore, 3 EffectsAfter
    public void executeSideEffect (int phase, PlayerManager playerManager, int cardPlayed)
    {
        switch (playerManager.hero) {
            case 1:
                TimothySideEffects(phase, playerManager, cardPlayed);
                break;

            case 21:
                YuriSideEffects(phase, playerManager, cardPlayed);
                break;
        }
    }

    public void TimothySideEffects(int phase, PlayerManager playerManager, int cardPlayed)
    {
        if (phase == 0) {
            // Deja Vu
            if (playerManager.sideList[1] == 2)
            {
                if (playerManager == HeroDecks.HD.myManager) {
                    GameOverseer.GO.enemyPredicted = true;
                } else if (playerManager == HeroDecks.HD.enemyManager) {
                    GameOverseer.GO.predicted = true;
                }
               playerManager.sideList[1]--;

            } else if (playerManager.sideList[1] == 1) {
                if (playerManager == HeroDecks.HD.myManager)  {
                    GameOverseer.GO.enemyPredicted = false;
                } else if (playerManager == HeroDecks.HD.enemyManager) {
                    GameOverseer.GO.predicted = false;
                }
                playerManager.sideList[1]--;
            }

        } else if (phase == 2) {
            // Marca do Tempo
            if (playerManager.sideList[0] > 0) {
                // A Marca do Tempo não faz nada sozinha - este espaço é apenas para formalizar o Side Effect
            }

        }
    }

    public void YuriSideEffects(int phase, PlayerManager playerManager, int cardPlayed)
    {
        if (phase == 2) {
            // Ponto Fraco
            if (playerManager.sideList[0] > 0) {
                if (playerManager.cardList[cardPlayed] is Damage) {
                    Damage cc = (Damage)playerManager.cardList[cardPlayed];
                    cc.isUnblockable = true;
                    Debug.Log("Ponto Fraco ativado");
                    playerManager.cardList[cardPlayed] = (Card)cc;
                }
            }

        } else if (phase == 3) {
            // Vodka
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
    }
}
