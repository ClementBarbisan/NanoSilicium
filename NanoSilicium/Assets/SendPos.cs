using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendPos : MonoBehaviour {
    public Material shader;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!shader)
            return;
        float[] floatArray = { gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z, 1.0f };
        shader.SetFloatArray("_position", floatArray);
	}
}
