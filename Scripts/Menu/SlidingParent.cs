﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingParent : MonoBehaviour
{
    [SerializeField]
    private Vector2 origin;
    [SerializeField]
    private Vector2 destination;
    bool sliding = false;
    float time = 0f;
    private float waitTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (NyarScript.NS.GetAnyButtoned() && gameObject.name == "Main Menu") {
            transform.localPosition = destination;
            sliding = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sliding)
        {
            if (waitTime > 0f) { waitTime -= 0.07f; }
            else {
                transform.localPosition = Vector2.Lerp(transform.localPosition, destination, Time.deltaTime * 2f);
                time += Time.deltaTime;
                if (time > 0.1f && AudioListener.volume < 1f && NyarScript.NS.GetMasterVolume() == null)
                {
                    time = 0f;
                    AudioListener.volume += 0.1f;
                }
            }
        } else
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, origin, Time.deltaTime * 4f);
        }
    }

    public void Slide()
    {
        sliding = true;
    }

    public void Recede()
    {
        sliding = false;
    }

    public void SetWaitTime(float waitTime)
    {
        this.waitTime = waitTime;
    }
}