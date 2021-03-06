﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids2 : MonoBehaviour
{
    // Start is called before the first frame update

    public int boidCount;
    GameObject[] boids;

    Vector3 v1;
    Vector3 v2;
    Vector3 v3;


    Vector3 Distancing = new Vector3(0, 0, 0);
    void Start()
    {
        Time.timeScale = 0.3f;
        boids = new GameObject[boidCount];
        for (int i = 0; i < boidCount; i++)
        {
            boids[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
        SpawnBoids(boids);
    }
  
     void FixedUpdate()
    {
        MoveAllboids();
    }

    Vector3 rule1(GameObject bj) //boids fly towards other boids
    {
        Vector3 savedlocation = new Vector3(0, 0, 0);
        foreach (GameObject item in boids) //safe the collective location of the boids
        {
            if (item != bj)
            {
                savedlocation += item.transform.position;
            }
        }
        savedlocation = savedlocation / (boidCount-1);
        return (savedlocation - bj.transform.position) / 1.15f;
    }
    Vector3 rule2(GameObject bj)
    {  
        foreach (GameObject item in boids)
        {
            if (Vector3.Magnitude( item.transform.position - bj.transform.position) < 8)
            {
                Distancing -= (item.transform.position - bj.transform.position);
            }
        }
        return Distancing;
    }
    void MoveAllboids()
    {
        for (int i = 0; i < boidCount; i++)
        {
            v1 = rule1(boids[i]);
            v2 = rule2(boids[i]); 
            boids[i].transform.position = Vector3.Lerp(v1, boids[i].transform.position,0.95f)+ (v2 /50);
        }
    }
    void SpawnBoids(GameObject[] array)
    {
        foreach (GameObject item in array)
        {
            int x = Random.Range(1, 10);
            int y = Random.Range(1, 10);
            int z = Random.Range(1, 10);
            item.transform.position = new Vector3(x, y, z);
        }
    }
}
