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
    public void executeSideEffect (int phase, PlayerManager playerManager, int cardPlayed, HeroEnum hero)
    {
        switch (hero) {
            case HeroEnum.Timothy:
                TimothySideEffects(phase, playerManager, cardPlayed);
                break;

            case HeroEnum.Harold:
                HaroldSideEffects(phase, playerManager, cardPlayed);
                break;

            case HeroEnum.Yuri:
                YuriSideEffects(phase, playerManager, cardPlayed);
                break;
        }
    }

    public void TimothySideEffects(int phase, PlayerManager playerManager, int cardPlayed)
    {
        if (phase == 0) {
            // Deja Vu
            if (playerManager.sideList[2] == 2)
            {
                if (playerManager == HeroDecks.HD.myManager) {
                    GameOverseer.GO.enemyPredicted = true;
                } else if (playerManager == HeroDecks.HD.enemyManager) {
                    GameOverseer.GO.predicted = true;
                }
               playerManager.sideList[2]--;

            } else if (playerManager.sideList[2] == 1) {
                if (playerManager == HeroDecks.HD.myManager)  {
                    GameOverseer.GO.enemyPredicted = false;
                } else if (playerManager == HeroDecks.HD.enemyManager) {
                    GameOverseer.GO.predicted = false;
                }
                playerManager.sideList[2]--;
            }

        } else if (phase == 2) {
            // Marca do Tempo
            if (playerManager.sideList[0] > 0) {
                // A Marca do Tempo não faz nada sozinha - este espaço é apenas para formalizar o Side Effect
            }

        }
    }

    public void HaroldSideEffects(int phase, PlayerManager playerManager, int cardPlayed)
    {
        if (playerManager == HeroDecks.HD.myManager) {
            executeSideEffect(phase, playerManager, cardPlayed, HeroDecks.HD.enemyManager.hero);
        } else if (playerManager == HeroDecks.HD.enemyManager) {
            executeSideEffect(phase, playerManager, cardPlayed, HeroDecks.HD.myManager.hero);
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
                    playerManager.cardList[cardPlayed] = (Card)cc;
                }
            }

        } else if (phase == 3) {
            // Vodka
            if (playerManager.hero == HeroEnum.Yuri)
            {
                if (playerManager.cardList[cardPlayed].isNullified)
                    playerManager.HP--;
            }

            // Ponto Fraco
            if (playerManager.sideList[0] > 0) {
                if (playerManager.cardList[cardPlayed] is Damage) {
                        Damage cc = (Damage)playerManager.cardList[cardPlayed];
                        cc.isUnblockable = false;
                        playerManager.cardList[cardPlayed] = (Card)cc;
                }
                playerManager.sideList[2]--;
            }
        }
    }
}
