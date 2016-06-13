using UnityEngine;
using System.Collections;

public class RotateOnTouchScript : MonoBehaviour {

    public int speed;

    // Update is called once per frame
    void Update()
    {    
        if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            foreach (Transform child in transform)
                child.RotateAround(Vector3.zero, Vector3.up, Input.GetTouch(0).deltaPosition.x * speed * Time.deltaTime);
    }
}
