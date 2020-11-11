using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids3 : MonoBehaviour
{
    // Start is called before the first frame update

    public int boidCount;
    public GameObject boidModel; //bees
    GameObject[] boids;
    Vector3[] velocitys;

    Vector3 v1;
    Vector3 v2;
    Vector3 v3;

    Vector3 Distancing = new Vector3(0, 0, 0);
    void Start()
    {
        Time.timeScale = 0.3f;

        boids = new GameObject[boidCount];
        velocitys = new Vector3[boidCount];
        for (int i = 0; i < boidCount; i++)
        {
            boids[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            boids[i].GetComponent<Renderer>().material.color = Color.red;
            velocitys[i] = new Vector3(0, 0, 0);
        }
        for (int i = 0; i < boidCount; i++)
        {

            boids[i] = Instantiate(boidModel);
        }
        SpawnBoids(boids);
       
        }
  
     void FixedUpdate()
    {
       // MoveAllboids();
        MoveAllboidstest();
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

            if (Vector3.Magnitude( item.transform.position - bj.transform.position) < 15)
            {
                Distancing -= (item.transform.position - bj.transform.position);
            }

        }
        return Distancing;
    }
    Vector3 rule3(GameObject bj,Vector3 Boidvelocity)
    {
        Vector3 PrecievedVelocity = new Vector3(0, 0, 0);
        for (int i = 0; i < boids.Length; i++)
        {
            if( boids[i] != bj)
            {
                PrecievedVelocity = PrecievedVelocity + velocitys[i];//var velocity = (current - previous) / Time.deltaTime;
            }
        }
        return (PrecievedVelocity - Boidvelocity / 8);
    }
    void MoveAllboids()
    {
        for (int i = 0; i < boidCount; i++)
        {
            v1 = rule1(boids[i]);
            v2 = rule2(boids[i]);
            v3 = rule3(boids[i],velocitys[i]);
            boids[i].transform.position = v1 + v2 + v3;
            velocitys[i] =  v1 + v2 + v3 ;
            velocitys[i] = velocitys[i].normalized;
            velocitys[i] = velocitys[i] * Time.deltaTime * 0.1f;
        }
    }
    void MoveAllboidstest()
    {
        for (int i = 0; i < boidCount; i++)
        {
            v1 = rule1(boids[i]);
            v2 = rule2(boids[i]);
            v3 = rule3(boids[i], velocitys[i]);

            velocitys[i] = velocitys[i]+ v1 + v2 + v3;
            boids[i].transform.position = Vector3.Lerp(v1, boids[i].transform.position,0.98f)+ (v2/99);   
            velocitys[i] = velocitys[i].normalized;
            velocitys[i] = velocitys[i] * Time.deltaTime * 0.3f;
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
