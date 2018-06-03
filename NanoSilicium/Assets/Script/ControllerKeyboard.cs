using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerKeyboard : MonoBehaviour {
    public float speed = 0.05f;
    private SteamVR_TrackedObject trackObject;
    private SteamVR_Controller.Device device;
	// Use this for initialization
	void Start () {
        trackObject = Camera.main.GetComponentInParent<SteamVR_TrackedObject>();
	}

    private void FixedUpdate()
    {
        device = SteamVR_Controller.Input((int)trackObject.index);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
            transform.Translate(Camera.main.transform.forward * speed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.S))
            transform.Translate(-Camera.main.transform.forward * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.Q))
            transform.Translate(-Camera.main.transform.right * speed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.D))
            transform.Translate(Camera.main.transform.right * speed * Time.deltaTime);
        Vector2 touchpad = (device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

        if (touchpad.y > 0.7f)
        {
            print("Moving Up");
            transform.Translate(Camera.main.transform.forward * speed * Time.deltaTime);
        }

        else if (touchpad.y < -0.7f)
        {
            print("Moving Down");
            transform.Translate(-Camera.main.transform.forward * speed * Time.deltaTime);
        }

        if (touchpad.x > 0.7f)
        {
            print("Moving Right");
            transform.Translate(Camera.main.transform.right * speed * Time.deltaTime);
        }

        else if (touchpad.x < -0.7f)
        {
            print("Moving left");
            transform.Translate(-Camera.main.transform.right * speed * Time.deltaTime);
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            GPUFlock.activate = !GPUFlock.activate;
        }
    }
}
