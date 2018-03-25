using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poumons : MonoBehaviour {
    Vector3 arrival = Vector3.zero;
    public Material shader;
    Rigidbody rb;
    bool onPlane = false;
    bool cross = false;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shader = GetComponentInChildren<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (!shader)
            return;
        shader.SetFloat("_forwardX", transform.forward.x);
        shader.SetFloat("_forwardY", transform.forward.y);
        shader.SetFloat("_forwardZ", transform.forward.z);
        shader.SetFloat("_posX", transform.position.x);
        shader.SetFloat("_posY", transform.position.y);
        shader.SetFloat("_posZ", transform.position.z);
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
