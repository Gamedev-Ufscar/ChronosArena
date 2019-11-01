using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NyarScript : MonoBehaviour
{
    public static NyarScript NS;
    [HideInInspector]
    private bool anyButtoned = false;
    private float? masterVolume = null;

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
    public float? GetMasterVolume()
    {
        return masterVolume;
    }

    public bool GetAnyButtoned()
    {
        return anyButtoned;
    }

    public void SetMasterVolume(float masterVolume)
    {
        this.masterVolume = masterVolume;
    }

    public void SetAnyButtoned(bool anyButtoned)
    {
        this.anyButtoned = anyButtoned;
    }
}
