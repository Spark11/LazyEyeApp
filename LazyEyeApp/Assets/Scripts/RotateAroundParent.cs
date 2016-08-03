using UnityEngine;
using System.Collections;

public class RotateAroundParent : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(transform.parent.position, Vector3.up, 40 * Time.deltaTime);
    }
}
