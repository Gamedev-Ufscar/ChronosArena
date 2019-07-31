using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NyarScript : MonoBehaviour
{
    public static NyarScript NS;
    [HideInInspector]
    public bool anyButtoned = false;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (NS == null) {
            NS = this;
        } else {
            if (NS != this) {
                Destroy(this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
