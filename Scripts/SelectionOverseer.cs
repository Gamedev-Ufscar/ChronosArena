using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;

public class SelectionOverseer : MonoBehaviour
{
    [SerializeField]
    private NetworkTrain networkTrain;
    [SerializeField]
    private CharacterManager characterManager;
    [SerializeField]
    private CharacterButton[] characterButtons;
    [SerializeField]
    private ConfirmButton confirmButton;
    [SerializeField]
    private Image enemyConfirmButton;

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

    private int myHeroIndex = 200;
    private int enemyHeroIndex = 200;
    private bool myConfirm = false;
    private bool enemyConfirm = false;

    [SerializeField]
    private Sprite noHero;

    private void Awake()
    {
        GameObject netObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Test"), Vector3.zero, Quaternion.identity);
        networkTrain = netObject.AddComponent<NetworkTrain>();
        networkTrain = new NetworkTrain(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Getter
    public bool GetMyConfirm()
    {
        return myConfirm;
    }

    // Confirm
    public void InvertMyConfirm()
    {
        myConfirm = !myConfirm;
        networkTrain.SendConfirm(myConfirm);
    }

    // Receive enemy Confirm
    public void SetEnemyConfirm(bool enemyConfirm)
    {
        this.enemyConfirm = enemyConfirm;
        if (this.enemyConfirm) {
            enemyConfirmButton.sprite = confirmed;
        } else {
            enemyConfirmButton.sprite = confirm;
        }
    }

    // Click and Hover
    public void CharacterClicked(CharacterButton charButton)
    {
        if (myHeroIndex != charButton.GetCharIndex()) // Choose a hero
        {
            Character character = characterManager.GetCharacter(charButton.GetCharIndex());

            // Update Hero Profile
            myPortrait.GetComponent<Image>().sprite = character.GetProfile();
            heroTitle.GetComponent<Text>().text = character.GetHeroString();
            // Update myHeroIndex
            myHeroIndex = charButton.GetCharIndex();
            // Change Button Tone
            charButton.ChangeTone(1f);

        } else // Deselect a hero
        {
            // Update Hero Profile
            myPortrait.GetComponent<Image>().sprite = noHero;
            heroTitle.GetComponent<Text>().text = "";
            // Update myHeroIndex
            myHeroIndex = 200;
            // Change Button Tone
            charButton.ChangeTone(0.55f);
        }

        // Send info to other player
        networkTrain.SendChosenHero(myHeroIndex);

    }

    public void CharacterHover(CharacterButton charButton)
    {
        Character character = characterManager.GetCharacter(charButton.GetCharIndex());

        // Only show hover if haven't chosen a hero yet
        if (myHeroIndex == 200)
        {
            // Change profile
            myPortrait.GetComponent<Image>().sprite = character.GetProfile();
            heroTitle.GetComponent<Text>().text = character.GetHeroString();
            // 
            networkTrain.SendHoverHero(charButton.GetCharIndex());
        }

        // Update tone 
        if (charButton.GetCharIndex() == charButton.GetCharIndex()) {
            if (CanChangeTone(charButton.GetCharIndex()))
                    charButton.ChangeTone(0.55f);
        } else {
            if (CanChangeTone(charButton.GetCharIndex()))
                charButton.ChangeTone(0.8f);
        }

    }

    public void CharacterStopHover()
    {
        if (myHeroIndex == 200)
        {
            myPortrait.GetComponent<Image>().sprite = noHero;
            heroTitle.GetComponent<Text>().text = "";
            networkTrain.StopHoverHero();
        }
    }

    // Receive enemy Click and Hover
    public void SetEnemyHero(int enemyCharIndex)
    {
        Character character = characterManager.GetCharacter(enemyCharIndex);
        enemyPortrait.GetComponent<Image>().sprite = character.GetProfile();
        heroTitle.GetComponent<Text>().text = character.GetHeroString();
        foreach (CharacterButton charButton in characterButtons)
        {
            if (enemyCharIndex == charButton.GetCharIndex()) {
                // CHECK LATER
                charButton.ChangeTone(0.3f);
            } else {
                charButton.ChangeTone(0.6f);
            }
        }
    }

    public void EnemyHoverHero(int receivedCharIndex)
    {
        // Only works if the enemy hasn't 
        if (receivedCharIndex == 200)
        {
            enemyPortrait.GetComponent<Image>().sprite = noHero;
            enemyTitle.GetComponent<Text>().text = "";
        } else {
            Character character = characterManager.GetCharacter(receivedCharIndex);
            enemyPortrait.GetComponent<Image>().sprite = character.GetProfile();
            enemyTitle.GetComponent<Text>().text = character.GetHeroString();
        }

        // Send to the char buttons
        foreach (CharacterButton charButton in characterButtons)
        {
            if (receivedCharIndex == charButton.GetCharIndex()) {
                // CHECK LATER
                if (CanChangeTone(receivedCharIndex))
                    charButton.ChangeTone(0.55f);
            } else {
                if (CanChangeTone(receivedCharIndex))
                    charButton.ChangeTone(0.8f);
            }
        }
    }


    // Helper Script
    public bool CanChangeTone(int indexInQuestion)
    {
        if (myHeroIndex == indexInQuestion || enemyHeroIndex == indexInQuestion)
        {
            return false;
        }

        return true;
    }
}
