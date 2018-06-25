//
//  Created by jiadong chen
//  http://www.chenjd.me
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUFlock : MonoBehaviour {

    public enum TypeTransform
    {
        Cos = 0,
        Sin = 1,
        SinCos = 2
    };

    public ComputeShader cshader;
    public GameObject boidPrefab;
    public GameObject boidAnimPrefab;
    public int boidsCount;
    public int boidsAnimCount;
    public float spawnRadius;
    public GameObject[] boidsGo;
    public GPUBoid[] boidsData;
    public float flockSpeed;
    public float nearbyDis;
    //public Vector3[] positionsTarget;
    private Vector3 targetPos = Vector3.zero;
    private int kernelHandle;
    private ComputeBuffer buffer;
    public static bool activate = true;
    private bool ping = false;
    void Start()
    {
        this.boidsGo = new GameObject[this.boidsCount];
        this.boidsData = new GPUBoid[this.boidsCount];
        this.kernelHandle = cshader.FindKernel("CSMain");

        //positionsTarget = new Vector3[3];
        for (int i = 0; i < this.boidsCount; i++)
        {
            this.boidsData[i] = this.CreateBoidData();
            if (i >= boidsCount - boidsAnimCount)
                this.boidsGo[i] = Instantiate(boidAnimPrefab, this.boidsData[i].pos, Quaternion.Euler(this.boidsData[i].rot)) as GameObject;
            else
                this.boidsGo[i] = Instantiate(boidPrefab, this.boidsData[i].pos, Quaternion.Euler(this.boidsData[i].rot)) as GameObject;
            this.boidsData[i].rot = this.boidsGo[i].transform.forward;
            this.boidsGo[i].GetComponent<PathTransform>().type = (TypeTransform)Random.Range(0, 3);
        }
        buffer = new ComputeBuffer(boidsCount, 40);
        cshader.SetBuffer(this.kernelHandle, "boidBuffer", buffer);
        cshader.SetFloat("boidsCount", boidsCount);
        cshader.SetFloat("nearbyDis", nearbyDis);
    }

    GPUBoid CreateBoidData()
    {
        GPUBoid boidData = new GPUBoid();
        Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
        Quaternion rot = Quaternion.Slerp(transform.rotation, Random.rotation, 0.3f);
        boidData.pos = pos;
        boidData.flockPos = transform.position;
        boidData.speed = this.flockSpeed + Random.Range(-3f, 3f);

        return boidData;
    }

    private void OnApplicationQuit()
    {
        buffer.Release();        
    }

    void Update()
    {
        if (!activate)
            return;

        if (targetPos.x > Mathf.PI)
            ping = true;
        else if (targetPos.x <= 0)
            ping = false;
        if (ping)
            this.targetPos += new Vector3(0.0015f, -0.003f, 0.015f);
        else
            this.targetPos += new Vector3(0.0015f, 0.003f, 0.015f);

        //positionsTarget[0] += new Vector3(
        //    (Mathf.Sin(Mathf.Deg2Rad * this.targetPos.x) * -0.05f),
        //    (Mathf.Sin(Mathf.Deg2Rad * this.targetPos.y) * 0.05f),
        //    (Mathf.Sin(Mathf.Deg2Rad * this.targetPos.z) * 0.05f)
        //);
        //positionsTarget[1] += new Vector3(
        //    (Mathf.Cos(Mathf.Deg2Rad * this.targetPos.x) * -0.05f),
        //    (Mathf.Cos(Mathf.Deg2Rad * this.targetPos.y) * 0.05f),
        //    (Mathf.Cos(Mathf.Deg2Rad * this.targetPos.z) * 0.05f)
        //);
        transform.position = new Vector3(
            (Mathf.Cos(this.targetPos.x) * Mathf.Sin(this.targetPos.y) * 100),
            (Mathf.Sin(this.targetPos.x) * Mathf.Sin(this.targetPos.y) * 100),
            (Mathf.Cos(this.targetPos.x) * 100)
        );



        for (int i = 0; i < boidsCount - boidsAnimCount; i++)
        {
            this.boidsData[i].flockPos = transform.position;

            // if (boidsGo[i].GetComponent<PathTransform>().type == TypeTransform.Cos)
            //this.boidsData[i].flockPos = positionsTarget[0];
            // else if (boidsGo[i].GetComponent<PathTransform>().type == TypeTransform.Sin)
            // this.boidsData[i].flockPos = positionsTarget[1];
            // else if (boidsGo[i].GetComponent<PathTransform>().type == TypeTransform.SinCos)
            // this.boidsData[i].flockPos = positionsTarget[2];
        }
        for (int i = boidsCount - boidsAnimCount; i < boidsCount; i++)
        {
            this.boidsData[i].flockPos = -transform.position / 2;
        }

        buffer.SetData(this.boidsData);


        cshader.SetFloat("deltaTime", Time.deltaTime);

        cshader.Dispatch(this.kernelHandle, this.boidsCount, 1, 1);

        buffer.GetData(this.boidsData);


        for (int i = 0; i < this.boidsData.Length; i++)
        {

            this.boidsGo[i].transform.localPosition = this.boidsData[i].pos;

            if(!this.boidsData[i].rot.Equals(Vector3.zero))
            {
                this.boidsGo[i].transform.rotation = Quaternion.LookRotation(this.boidsData[i].rot) * Quaternion.Euler(new Vector3(0, 270, 0));
            }

        }
    }

}
