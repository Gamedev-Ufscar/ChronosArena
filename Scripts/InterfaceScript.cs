using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceScript : MonoBehaviour
{
    public int cardAmount = 0;
    public Sprite[] interfaceList;
    public int interfaceSignal = 200;
    public GameObject optionPrefab;
    public List<Vector2> cardLocations;
    public Card invoker;

    private List<GameObject> destructionList = new List<GameObject>();
    private bool setup = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!setup)
        {
            setup = true;
            Debug.Log("IS enabled, card amount: " + cardAmount);
            cardLocations = setupCardLocations(cardAmount);
            for (int i = 0; i < cardAmount; i++) {
                GameObject optionCreated = Instantiate(optionPrefab, gameObject.transform);
                optionCreated.transform.parent = gameObject.transform;
                optionCreated.GetComponent<InterfaceCard>().index = i;
                optionCreated.GetComponent<Image>().sprite = interfaceList[i];
                destructionList.Add(optionCreated);
                Debug.Log(i);
            }
        }

        if (interfaceSignal != 200 && invoker is Interfacer)
        {
            Interfacer cc = (Interfacer)invoker;
            cc.interfaceSignal = interfaceSignal;
            invoker = (Card)cc;

            foreach (GameObject option in destructionList) {
                Destroy(option);
            }

            gameObject.SetActive(false);
        }
       
    }

    List<Vector2> setupCardLocations(int cardAmount) {
        List<Vector2> ccardLocations = new List<Vector2>();
        float baseDistance, actualDistance, initialPosition;
        baseDistance = 80 /Mathf.Pow(2, cardAmount-2);
        actualDistance = baseDistance + 83;

        if (cardAmount != 1) { 
            if (cardAmount % 2 == 0) {
                initialPosition = (cardAmount * actualDistance) / 4;
            } else {
                initialPosition = ((cardAmount - 1)/2) * actualDistance;
            }

            for (int i = 0; i < cardAmount; i++) {
                ccardLocations.Add(new Vector2(-initialPosition+(i*actualDistance), 0f));
            }
        } else {
            ccardLocations.Add(new Vector2(0f, 0f));
        }

        return ccardLocations;

               
    }
}
