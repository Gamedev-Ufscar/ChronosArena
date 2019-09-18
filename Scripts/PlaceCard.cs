using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCard : MonoBehaviour
{
    public float speed = 13f;

    public Transform point;
    private GameObject go;
    private bool fu = false;
    private Quaternion targetRot;
    private Queue<GameObject> queue;
    private Queue<bool> boolQueue;

    void Start() {
        queue = new Queue<GameObject>();
        boolQueue = new Queue<bool>();
    }

    // Place Card
    public void PlaceOnSlot(GameObject g, bool faceUp)
    {
        if (go == null) {
            go = g; // go = card
            fu = faceUp;
            go.GetComponent<Rigidbody>().useGravity = false;
            if (faceUp) { targetRot = Quaternion.LookRotation(Vector3.back, Vector3.down); }
            else { targetRot = Quaternion.LookRotation(Vector3.back, Vector3.up); }

            // Slight rotation
            //float f1 = Random.Range(-5f, 5f);
            //float f2 = Random.Range(-1f, 1f);
            //targetRot = Quaternion.LookRotation(Vector3.back, Vector3.up);
        } else {
            queue.Enqueue(g);
            boolQueue.Enqueue(g);
        }
    }

    void Update()
    {
        if (go == null)
        {
            return;
        }

        //go.transform.position = Vector3.Lerp(point.position, go.transform.position, Time.deltaTime * 0.1f);

        // Gravity pull
        go.transform.position = Vector3.MoveTowards(go.transform.position, point.transform.position, Time.deltaTime * speed);
        go.transform.rotation = Quaternion.Lerp(go.transform.rotation, 
                        targetRot, Time.deltaTime * 6f);

        // When close, activate gravity
        if ((Vector3.Distance(point.position, go.transform.position) <= 0.05f && fu == false) ||
            Mathf.Approximately(Vector3.Distance(point.position, go.transform.position), 0f))
        {
            if (fu == false) {
                go.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                go.GetComponent<Rigidbody>().velocity = Vector3.zero;
                go.GetComponent<Rigidbody>().useGravity = true;
            } else {
                go.GetComponent<BoardCard>().RaiseAnimState();
                go.GetComponent<BoardCard>().SetWaiting(false);
            }

            //go.name = go.name + "1";

            /*if (tag.StartsWith("Slot/"))
            {
                go.name = "AOSJOAIUHSI";
            }
            else
                Destroy(go);*/

            if (queue.Count == 0) { 
                go = null;
                fu = false;
            } else {
                go = queue.Dequeue();
                go.GetComponent<Rigidbody>().useGravity = false;
                fu = boolQueue.Dequeue();
                //float f1 = Random.Range(-15f, 15f);
                //float f2 = Random.Range(-15f, 15f);
                //targetRot = Quaternion.LookRotation(Vector3.back, Vector3.up);
            }
        }
    }
}
