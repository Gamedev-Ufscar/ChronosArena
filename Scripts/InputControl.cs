using UnityEngine;

static class InputControl
{
    public static bool TouchRaycast()
    {
        RaycastHit hit = new RaycastHit();
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
            {
                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                if (Physics.Raycast(ray, out hit))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool IsDoubleTap()
    {
        float MaxTimeWait = 1;
        float VariancePosition = 15;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            float DeltaTime = Input.GetTouch(0).deltaTime;
            float DeltaPositionLenght = Input.GetTouch(0).deltaPosition.magnitude;

            if (DeltaTime > 0f && DeltaTime < MaxTimeWait && DeltaPositionLenght < VariancePosition)
                return true;
        }
        return false;
    }
}
