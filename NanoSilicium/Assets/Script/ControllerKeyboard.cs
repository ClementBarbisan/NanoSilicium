using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerKeyboard : MonoBehaviour {
    public float speed = 0.05f;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
            transform.Translate(Camera.main.transform.forward * speed);
        else if (Input.GetKey(KeyCode.S))
            transform.Translate(-Camera.main.transform.forward * speed);
        if (Input.GetKey(KeyCode.Q))
            transform.Translate(-Camera.main.transform.right * speed);
        else if (Input.GetKey(KeyCode.D))
            transform.Translate(Camera.main.transform.right * speed);
    }
}
