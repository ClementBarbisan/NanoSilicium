using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    public float xRotation = 2;
    public float yRotation = 2;
    public float speed = 1;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Q))
            this.transform.Rotate(new Vector3(0, -yRotation, 0));
        else if (Input.GetKey(KeyCode.D))
            this.transform.Rotate(new Vector3(0, yRotation, 0));
        if (Input.GetKey(KeyCode.A))
            this.transform.Rotate(new Vector3(-xRotation, 0, 0));
        else if (Input.GetKey(KeyCode.E))
            this.transform.Rotate(new Vector3(xRotation, 0, 0));
        if (Input.GetKey(KeyCode.Z))
            this.transform.Translate(transform.forward * speed);
        else if (Input.GetKey(KeyCode.S))
            this.transform.Translate(-transform.forward * speed);
    }
}
