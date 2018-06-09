using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerKeyboard : MonoBehaviour {
    public float speed = 1f;
    private SteamVR_TrackedObject trackObject;
    private SteamVR_Controller.Device device;
    public GameObject CameraRig;
	// Use this for initialization
	void Start () {
        
	}

    private void FixedUpdate()
    {
        if (trackObject == null)
            trackObject = GetComponent<SteamVR_TrackedObject>();
        if (trackObject == null)
            return;
        if (device == null)
            device = SteamVR_Controller.Input((int)trackObject.index);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
            CameraRig.transform.Translate(Camera.main.transform.forward * speed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.S))
            CameraRig.transform.Translate(-Camera.main.transform.forward * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.Q))
            CameraRig.transform.Translate(-Camera.main.transform.right * speed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.D))
            CameraRig.transform.Translate(Camera.main.transform.right * speed * Time.deltaTime);
        if (device == null)
            return;
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = (device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

            if (Mathf.Abs(touchpad.y) > 0.7f)
            {
                CameraRig.transform.Translate(Camera.main.transform.forward * speed * Time.deltaTime * touchpad.y);
            }

            if (Mathf.Abs(touchpad.x) > 0.7f)
            {
                CameraRig.transform.Translate(Camera.main.transform.right * speed * Time.deltaTime * touchpad.x);
            }
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.LogError("Activate");
            GPUFlock.activate = !GPUFlock.activate;
        }
    }
}
