using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverseer : MonoBehaviour
{
    [SerializeField]
    private Player myPlayer;
    [SerializeField]
    private Player enemyPlayer;
    [SerializeField]
    private NetworkTrain networkTrain;
    [SerializeField]
    private Interface interfface;

    private bool myConfirm = false;
    private bool enemyConfirm = false;

    private GameState state = GameState.Purchase;

    private void Awake()
    {
        GameObject netObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Test"), Vector3.zero, Quaternion.identity);
        networkTrain = netObject.AddComponent<NetworkTrain>();
        networkTrain = new NetworkTrain(this);
    }

    // State Stuff
    public void UpdatedCardPlayed(Player player, bool cardPlayed)
    {
        if (state == GameState.Choice)
        {
            if (player == myPlayer) { SetMyConfirm(cardPlayed); }
            else if (player == enemyPlayer) { SetEnemyConfirm(cardPlayed); }
        }
    }

    public void StateMachine() { StateMachine(false); }

    public void StateMachine(bool overrider)
    {
        if ((myConfirm && enemyConfirm) || overrider)
        {
            switch (state)
            {
                case GameState.Purchase:
                    ToChoiceState();
                    myPlayer.PurchaseCards();
                    enemyPlayer.PurchaseCards();
                    break;

                case GameState.Choice:
                    if (myPlayer.GetCardPlayed() is Interfacer || enemyPlayer.GetCardPlayed() is Interfacer)
                    {
                        ToInterfaceState();
                    } else if (myPlayer.HasReactionCard() || enemyPlayer.HasReactionCard())
                    {
                        ToReactionState();
                    } else
                    {
                        ToEffectsState();
                    }
                    break;

                case GameState.Reaction:
                    ToEffectsState();
                    break;

                case GameState.Effects:
                    ToPurchaseState();
                    break;
            }

            myConfirm = false;
            enemyConfirm = false;
        }
    }

    private void ToChoiceState()
    {
        state = GameState.Choice;
        myPlayer.ActivateSideEffects(SEPhase.Choice, enemyPlayer);
        enemyPlayer.ActivateSideEffects(SEPhase.Choice, myPlayer);

    }

    private void ToInterfaceState()
    {
        state = GameState.Interface;
        if (myPlayer.GetCardPlayed() is Interfacer)
        {
            Interfacer inter = (Interfacer)myPlayer.GetCardPlayed();
            inter.Interfacing(myPlayer, enemyPlayer);
        }

        if (enemyPlayer.GetCardPlayed() is Interfacer)
        {
            Interfacer inter = (Interfacer)enemyPlayer.GetCardPlayed();
            inter.Interfacing(enemyPlayer, myPlayer);
        }
    }

    private void ToReactionState()
    {
        state = GameState.Reaction;
        myPlayer.ActivateSideEffects(SEPhase.Reaction, enemyPlayer);
        enemyPlayer.ActivateSideEffects(SEPhase.Reaction, myPlayer);
    }

    private void ToEffectsState()
    {
        state = GameState.Effects;
        // Activate Side Effects
        myPlayer.ActivateSideEffects(SEPhase.EffectsBefore, enemyPlayer);
        enemyPlayer.ActivateSideEffects(SEPhase.EffectsBefore, myPlayer);

        // Activate Effects
        ActivateCards();

        // Activate Side Effects
        myPlayer.ActivateSideEffects(SEPhase.EffectsAfter, enemyPlayer);
        enemyPlayer.ActivateSideEffects(SEPhase.EffectsAfter, myPlayer);
    }

    private void ActivateCards()
    {
        for (int e = Mathf.Min(myPlayer.GetCardPlayed().GetMinOrMax(true) / 100, enemyPlayer.GetCardPlayed().GetMinOrMax(true) / 100);
            e <= Mathf.Max(myPlayer.GetCardPlayed().GetMinOrMax(false) % 100, enemyPlayer.GetCardPlayed().GetMinOrMax(false) % 100);
            e++)
        {
            if (!myPlayer.GetCardPlayed().GetIsNullified())
                myPlayer.GetCardPlayed().Effect(myPlayer, enemyPlayer, e);
            if (!enemyPlayer.GetCardPlayed().GetIsNullified())
                enemyPlayer.GetCardPlayed().Effect(enemyPlayer, myPlayer, e);
        }
    }

    private void ToPurchaseState()
    {
        state = GameState.Purchase;
    }

    // Interfacing
    public void Interfacing(Card[] cardList, Card invoker)
    {
        interfface.gameObject.SetActive(true);
        interfface.Setup(cardList, invoker);
    }

    public void Interfacing(Card baseCard, string[] textList, Card invoker)
    {
        interfface.gameObject.SetActive(true);
        interfface.Setup(baseCard, textList, invoker);
    }


    // Getter
    public GameState GetState()
    {
        return state;
    }

    public Player GetMyPlayer()
    {
        return myPlayer;
    }

    public Player GetEnemyPlayer()
    {
        return enemyPlayer;
    }

    // Setter
    public void SetMyConfirm(bool myConfirm)
    {
        this.myConfirm = myConfirm;
        StateMachine();
    }

    public void SetEnemyConfirm(bool enemyConfirm)
    {
        this.enemyConfirm = enemyConfirm;
        StateMachine();
    }

    // Network Sender
    public void SendCard()
    {
        networkTrain.SendCard();
    }

    public void SendShuffle(int[] cardIndexes)
    {
        networkTrain.SendShuffle(cardIndexes);
    }

    // Network Receiver
    public void ReceiveShuffle(int[] receivedCardIndexes)
    {
        enemyPlayer.ReceiveShuffle(receivedCardIndexes);
    }

}
