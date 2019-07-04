using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StandaloneInputModuleV2 : StandaloneInputModule
{
    public GameObject GameObjectUnderPointer(int pointerId)
    {
        var lastPointer = GetLastPointerEventData(pointerId);
        if (lastPointer != null)
            return lastPointer.pointerCurrentRaycast.gameObject;
        return null;
    }

    public GameObject GameObjectUnderPointer()
    {
        return GameObjectUnderPointer(PointerInputModule.kMouseLeftId);
    }

    private static StandaloneInputModuleV2 currentInput;
    private StandaloneInputModuleV2 CurrentInput
    {
        get
        {
            if (currentInput == null)
            {
                currentInput = EventSystem.current.currentInputModule as StandaloneInputModuleV2;
                if (currentInput == null)
                {
                    Debug.LogError("Missing StandaloneInputModuleV2.");
                    // some error handling
                }
            }

            return currentInput;
        }
    }
}