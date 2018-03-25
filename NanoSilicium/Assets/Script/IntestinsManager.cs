using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntestinsManager : MonoBehaviour {
    public static IntestinsManager Instance;
    public List<GameObject> intestins;
    public GameObject prefab;

    private void Awake()
    {
        Instance = this;
        intestins = new List<GameObject>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Space))
        {
            GameObject go = Instantiate<GameObject>(prefab, new Vector3(0, 0, 0), Quaternion.Euler(90, 0, 0));
            intestins.Add(go);
        }
	}
}
