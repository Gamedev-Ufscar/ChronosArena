using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NyarScript : MonoBehaviour
{
    public static NyarScript NS;
    [HideInInspector]
    private bool anyButtoned = false;
    private float? masterVolume = 1;
    private float? musicVolume = 1;
    private float? soundVolume = 1;

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

    public float? GetMasterVolume()
    {
        return masterVolume;
    }

    public float? GetMusicVolume()
    {
        return musicVolume;
    }

    public float? GetSoundVolume()
    {
        return soundVolume;
    }

    public bool GetAnyButtoned()
    {
        return anyButtoned;
    }

    public void SetMasterVolume(float masterVolume)
    {
        this.masterVolume = masterVolume;
    }

    public void SetMusicVolume(float musicVolume)
    {
        this.musicVolume = musicVolume;
    }

    public void SetSoundVolume(float soundVolume)
    {
        this.soundVolume = soundVolume;
    }

    public void SetAnyButtoned(bool anyButtoned)
    {
        this.anyButtoned = anyButtoned;
    }
}
