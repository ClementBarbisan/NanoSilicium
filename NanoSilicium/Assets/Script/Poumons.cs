using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poumons : MonoBehaviour {
    Vector3 arrival = Vector3.zero;
    public Material shader;
    Rigidbody rb;
    Renderer render;
    public List<GameObject> left;
    public List<GameObject> right;
    bool onPlane = false;
    bool cross = false;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shader = GetComponentInChildren<Renderer>().material;
        render = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!shader)
            return;
        //left[0].transform.position += new Vector3(Mathf.Sin(Time.fixedTime * 10) * transform.forward.z / 80, Mathf.Sin(Time.fixedTime * 10) * transform.forward.x / 80, Mathf.Sin(Time.fixedTime * 10) * transform.forward.y / 80);
        //right[0].transform.position += new Vector3(Mathf.Sin(Time.fixedTime * 10) * transform.forward.z / 80, Mathf.Sin(Time.fixedTime * 10) * transform.forward.x / 80, Mathf.Sin(Time.fixedTime * 10) * transform.forward.y / 80);
        //left[1].transform.position += new Vector3(Mathf.Sin(Time.fixedTime * 10) * transform.forward.z / 60, Mathf.Sin(Time.fixedTime * 10) * transform.forward.x / 60, Mathf.Sin(Time.fixedTime * 10) * transform.forward.y / 60);
        //right[1].transform.position += new Vector3(Mathf.Sin(Time.fixedTime * 10) * transform.forward.z / 60, Mathf.Sin(Time.fixedTime * 10) * transform.forward.x / 60, Mathf.Sin(Time.fixedTime * 10) * transform.forward.y / 60);
        //left[2].transform.position += new Vector3(Mathf.Sin(Time.fixedTime * 10) * transform.forward.z / 60, Mathf.Sin(Time.fixedTime * 10) * transform.forward.x / 60, Mathf.Sin(Time.fixedTime * 10) * transform.forward.y / 60);
        //right[2].transform.position += new Vector3(Mathf.Sin(Time.fixedTime * 10) * transform.forward.z / 60, Mathf.Sin(Time.fixedTime * 10) * transform.forward.x / 60, Mathf.Sin(Time.fixedTime * 10) * transform.forward.y / 60);
        if (arrival == Vector3.zero || Vector3.Distance(arrival, this.transform.position) < 4f)
            arrival = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), Random.Range(-20f, 20f));
        rb.velocity = Vector3.Normalize(arrival - this.transform.position) * 5;
        Debug.Log(Vector3.Distance(arrival, this.transform.position));
        cross = false;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(arrival - transform.position, Vector3.up) * Quaternion.Euler(0, 180, 0), 0.1f);

        //for (int i = 0; i < IntestinsManager.Instance.intestins.Count; i++)
        //{
        //    if (IntestinsManager.Instance.intestins[i] != this.gameObject && Vector3.Distance(IntestinsManager.Instance.intestins[i].transform.position, this.transform.position) < 6f)
        //    {
        //        if (cross == false)
        //            rb.velocity = Vector3.Normalize(this.transform.position - IntestinsManager.Instance.intestins[i].transform.position) / 2;
        //        else
        //            rb.velocity += Vector3.Normalize(this.transform.position - IntestinsManager.Instance.intestins[i].transform.position) / 2;
        //        cross = true;
        //    }
        //}
        //if (cross == true)
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(arrival - transform.position, Vector3.up) * Quaternion.Euler(90, 0, 0), 0.5f);
    }

}
