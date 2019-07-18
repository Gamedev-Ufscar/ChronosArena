using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugaScream : Card
{
    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority) {
            case 16:
                if (1 - enemy.protection >= 0) {
                    enemy.HP -= (1 - enemy.protection);
                }
                Debug.Log(user.gameObject.name + "'s Buga Scream");
                break;

            case 17:
                if (user.cardList[0] != null) {
                    Attack cc = (Attack)user.cardList[0];
                    cc.damage++;
                    user.cardList[0] = (Card)cc;
                }
                cost++;

                break;
        }
    }
}
