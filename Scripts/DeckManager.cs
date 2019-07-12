using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public List<Vector2> cardLocations = new List<Vector2>();
    public Vector2 ultiLocation = new Vector2();
    public bool holdingCard = false;

    [HideInInspector]
    public int cardAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
