using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using UnityEngine.SceneManagement;

public class SelectionOverseer : MonoBehaviour
{
    //[SerializeField]
    //private NetworkTrain networkTrain;
    [SerializeField]
    private CharacterManager characterManager;
    [SerializeField]
    private CharacterButton[] characterButtons;
    [SerializeField]
    private ConfirmButton confirmButton;
    [SerializeField]
    private Image enemyConfirmButton;
    [SerializeField]
    private HermesScript hermes;

    [SerializeField]
    private Sprite confirm;
    [SerializeField]
    private Sprite confirmed;

    [SerializeField]
    private GameObject[] buttons;
    [SerializeField]
    private GameObject myPortrait;
    [SerializeField]
    private GameObject heroTitle;
    [SerializeField]
    private GameObject enemyPortrait;
    [SerializeField]
    private GameObject enemyTitle;
    [SerializeField]
    private Text gameStartsIn;

    private int? myHeroIndex = null;
    private int? enemyHeroIndex = null;
    private bool myConfirm = false;
    private bool enemyConfirm = false;
    private float? timeTillAdvance = null;

    [SerializeField]
    private Sprite noHero;


    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("A new Train");
        if (NetworkTrain.networkTrain != null)
        {
            Destroy(NetworkTrain.networkTrain.gameObject);
        } 
        GameObject netObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Test"), Vector3.zero, Quaternion.identity);
        netObject.GetComponent<NetworkTrain>().GiveSelectionOverseer(this);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("myHeroIndex: " + myHeroIndex);

        if (timeTillAdvance != null)
        {
            timeTillAdvance -= Time.deltaTime;
            if (timeTillAdvance > 2f)
            {
                gameStartsIn.text = "Game starts in 3 seconds...";
            }
            else if (timeTillAdvance > 1f)
            {
                gameStartsIn.text = "Game starts in 2 seconds...";
            }
            else if (timeTillAdvance > 0f)
            {
                gameStartsIn.text = "Game starts in 1 second...";
            }
            else
            {
                StartGame();
            }
        } else
        {
            gameStartsIn.text = "";
        }
    }

    // Getter
    public bool GetMyConfirm()
    {
        return myConfirm;
    }

    /// Setter ----------------------------------------------------------------------------------
    // Hero
    public void SetMyHeroIndex(int? myHeroIndex)
    {
        this.myHeroIndex = myHeroIndex;

        // Change Profile
        if (myHeroIndex != null)
        {
            Character character = characterManager.GetCharacter((int)myHeroIndex);
            SetProfile(character.GetProfile(), character.GetHeroString());
        } else
        {
            SetProfile(noHero, "");
        }

        // Change Scale and Tone
        if (myHeroIndex != null)   // Selecting a character
        {
            DeselectAllButtons(0.55f);
            // Search for corresponding button, then it gets selected
            foreach (CharacterButton charButton in characterButtons)
            {
                if (myHeroIndex == charButton.GetCharIndex())
                {
                    charButton.ChangeScale(1.1f);
                    charButton.ChangeTone(1f);
                }
            }
            
        }
        else // Deselecting a character
        {
            // All buttons get deselected
            DeselectAllButtons(0.55f);
        }

        // Send info to other player
        if (myHeroIndex == null)
        {
            NetworkTrain.networkTrain.SendChosenHero(200);
        }
        else { NetworkTrain.networkTrain.SendChosenHero((int)myHeroIndex); }
    }

    public void SetMyHeroIndex(CharacterButton charButton)
    {
        myHeroIndex = charButton.GetCharIndex();

        // Change Hero Profile
        Character character = characterManager.GetCharacter(charButton.GetCharIndex());
        SetProfile(character.GetProfile(), character.GetHeroString());

        // Change Scale and Tone
        DeselectAllButtons(0.55f);
        charButton.ChangeScale(1.1f);
        charButton.ChangeTone(1f);

        // Send info to other player
        NetworkTrain.networkTrain.SendChosenHero((int)myHeroIndex);
    }

    public void SetMyHeroIndexToNull(CharacterButton charButton)
    {
        myHeroIndex = null;

        // Update Hero Profile
        SetProfile(noHero, "");

        // Change Scale and Tone
        charButton.ChangeScale(1f);
        charButton.ChangeTone(0.8f);

        // Send info to other player
        NetworkTrain.networkTrain.SendChosenHero(200);
    }


    // Profile
    public void SetProfile(Sprite image, string text)
    {
        myPortrait.GetComponent<Image>().sprite = image;
        heroTitle.GetComponent<Text>().text = text;
    }

    public void SetEnemyProfile(Sprite image, string text)
    {
        if (enemyPortrait == null)
        {
            enemyPortrait = GameObject.Find("Enemy Portrait");
        }
        enemyPortrait.GetComponent<Image>().sprite = image;

        if (enemyTitle == null)
        {
            enemyTitle = GameObject.Find("Enemy Title");
        }
        enemyTitle.GetComponent<Text>().text = text;
    }


    // Confirm
    public void ConfirmButton()
    {
        if (myHeroIndex != null)
        {
            SetMyConfirm(!myConfirm);

            if (myConfirm == false)
            {
                int? a = null;
                SetMyHeroIndex(a);
            }
        }
    }

    public void SetMyConfirm(bool confirm)
    {
        myConfirm = confirm;
        CheckConfirmStart();
        NetworkTrain.networkTrain.SendConfirm(myConfirm);
    }


    // Receive enemy Confirm
    public void SetEnemyConfirm(bool enemyConfirm)
    {
        this.enemyConfirm = enemyConfirm;
        CheckConfirmStart();
        if (this.enemyConfirm) {
            enemyConfirmButton.sprite = confirmed;
        } else {
            enemyConfirmButton.sprite = confirm;
        }
    }

    /// ----------------------------------------------------------------------------------

    public void JuicyButton(CharacterButton charButton)
    {
        if (!myConfirm && enemyHeroIndex != charButton.GetCharIndex())
        {
            charButton.ChangeScale(0.9f);
        }
    }

    // Advance and Hermes
    public void CheckConfirmStart()
    {
        if (myConfirm && enemyConfirm)
        {
            timeTillAdvance = 3f;
        } else
        {
            timeTillAdvance = null;
        }
    }

    public void StartGame()
    {
        // Load Player
        Character chr = characterManager.GetCharacter((int)myHeroIndex);
        hermes.LoadHermes(chr.GetHero(), chr.GetSideListSize(), chr.GetCardCount(), chr.GetUltiCount(), chr.GetPassiveCount(), 
            chr.GetAttackDisableList(), chr.GetProfile());

        // Load Enemy
        chr = characterManager.GetCharacter((int)enemyHeroIndex);
        hermes.LoadEnemyHermes(chr.GetHero(), chr.GetSideListSize(), chr.GetCardCount(), chr.GetUltiCount(), chr.GetPassiveCount(),
            chr.GetAttackDisableList(), chr.GetProfile());

        SetMyConfirm(false);
        SetEnemyConfirm(false);
        foreach (GameObject nt in GameObject.FindGameObjectsWithTag("Avatar"))
        {
            Destroy(nt);
        }

        SceneManager.LoadScene(3);
    }

    // Click and Hover
    public void HeroClicked(CharacterButton charButton)
    {
        if (!myConfirm && enemyHeroIndex != charButton.GetCharIndex())
        {

            if (this.myHeroIndex != charButton.GetCharIndex()) // Choose a hero
            {
                SetMyHeroIndex(charButton);
            }
            else // Deselect this hero
            {
                SetMyHeroIndexToNull(charButton);
                Debug.Log("null");
            }

            // Unconfirm
            SetMyConfirm(false);
        }

    }

    public void HeroHover(CharacterButton charButton)
    {
        Character character = characterManager.GetCharacter(charButton.GetCharIndex());

        // Only change anything if not confirmed
        if (!myConfirm)
        {
            // Only change portrait if hasn't chosen a hero yet
            if (myHeroIndex == null)
            {
                SetProfile(character.GetProfile(), character.GetHeroString());
            }

            if (NotSelected(charButton.GetCharIndex()))
            {
                // Update scale
                charButton.ChangeScale(1.1f);

                // Update tone
                charButton.ChangeTone(0.8f);
            }

            // Send info to other player
            NetworkTrain.networkTrain.SendHoverHero(charButton.GetCharIndex());
        }

    }

    public void HeroStopHover(CharacterButton charButton)
    {
        // Only change anything if not confirmed
        if (!myConfirm)
        {
            // Only change portrait if haven't chosen a hero yet
            if (myHeroIndex == null)
            {
                SetProfile(noHero, "");
            }

            if (NotSelected(charButton.GetCharIndex()))
            {
                // Update scale
                charButton.ChangeScale(1f);

                // Update tone 
                charButton.ChangeTone(0.55f);
            }

            // Send info to other player
            NetworkTrain.networkTrain.SendHoverHeroStop(charButton.GetCharIndex());
        }

    }

    // Receive enemy Click and Hover
    public void SetEnemyHero(int enemyCharIndex)
    {
        // Update enemyHeroIndex
        if (enemyCharIndex == 200)
            enemyHeroIndex = null;
        else 
            this.enemyHeroIndex = enemyCharIndex;


        // Only change portrait if hasn't chosen a hero yet
        if (enemyHeroIndex == null)
        {
            SetEnemyProfile(noHero, "");
        }
        else
        {
            Character character = characterManager.GetCharacter(enemyCharIndex);
            SetEnemyProfile(character.GetProfile(), character.GetHeroString());
        }

        // Send to the char buttons
        foreach (CharacterButton charButton in characterButtons)
        {
            if (enemyCharIndex == charButton.GetCharIndex()) {
                charButton.ChangeScale(0.9f);
                charButton.ChangeTone(0.3f, 0.3f);
            } else if (NotSelected(charButton.GetCharIndex())) {
                charButton.ChangeScale(1f);
                charButton.ChangeTone(0.55f, 0f);
            }
        }
    }

    public void EnemyHeroHover(int receivedCharIndex)
    {
        // Change profile
        Debug.Log("Received: " + receivedCharIndex);

        if (enemyHeroIndex == null)
        {
            Character character = characterManager.GetCharacter(receivedCharIndex);
            SetEnemyProfile(character.GetProfile(), character.GetHeroString());
        }

        // Send to the char buttons
        foreach (CharacterButton charButton in characterButtons)
        {
            if (charButton != null && charButton.GetCharIndex() == receivedCharIndex && NotSelected(receivedCharIndex))
            {
                // Update scale
                charButton.ChangeScale(1.1f);

                // Update tone
                charButton.ChangeTone(0.55f, 0.3f);
            }
        }
    }

    public void EnemyHeroHoverStop(int receivedCharIndex)
    {
        if (enemyHeroIndex == null)
        {
            SetEnemyProfile(noHero, "");
        }

        // Send to the char buttons
        foreach (CharacterButton charButton in characterButtons)
        {
            if (charButton.GetCharIndex() == receivedCharIndex && NotSelected(receivedCharIndex))
            {
                // Update scale
                charButton.ChangeScale(1f);

                // Update tone
                charButton.ChangeTone(0.8f, 0f);
            }
        }
    }


    // Helper Scripts
    public bool NotSelected(int indexInQuestion)
    {
        if (myHeroIndex == indexInQuestion || enemyHeroIndex == indexInQuestion)
        {
            return false;
        }

        return true;
    }

    public void DeselectAllButtons(float tone)
    {
        foreach (CharacterButton charButton in characterButtons)
        {
            if (enemyHeroIndex != charButton.GetCharIndex())
            {
                charButton.ChangeScale(1f);
                charButton.ChangeTone(tone);
            }
        }
    }
}
