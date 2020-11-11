
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{

    int Scalar = 10;
    GameObject[] Boid;
    GameObject Center;
    int time = 0;
    // Start is called before the first frame update
    void Start()
    {
        Boid = new GameObject[Scalar];
        for (int i = 0; i < Scalar; i++)
        {
            Boid[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Boid[i].GetComponent<Renderer>().material.color = Color.green;
        }
        SpawnBoids(Boid);
        CalculateCenter();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W)) //respawn the bois
        {
            SpawnBoids(Boid);

        }
        if (Input.GetKeyDown(KeyCode.E)) //respawn the bois
        {

        }
        time++;
        if (time == 1)
        {
            time = 0;
            Debug.Log("second");
            for (int i = 0; i < Boid.Length; i++)
            {
                MoveTowardsCenter(Boid[i]);
                SocialDistancing(Boid[i], 3);

            }
        }
    }
   
    Vector3 CalculateCenter()
    {
        // Rule 1: Boids try to fly towards the centre of mass of neighbouring boids
        Vector3 collection = new Vector3(0, 0, 0);
        foreach (GameObject a in Boid)
        {
            collection += a.transform.position;
        }
        collection = collection / 10; //combination of all vectors
        return collection;
    }

    void MoveTowardsCenter(GameObject moveThis)
    {

        Vector3 Pcj = CalculateCenter(); //precieved center
        foreach (GameObject item in Boid)
        {
            if (item.transform.position != moveThis.transform.position)//Exclude self from center callculation
            {
                Pcj = Pcj + item.transform.position;
            }
        }
        Pcj = Pcj / (Scalar-1); //callculates center for each boid
        moveThis.transform.position = (Pcj - moveThis.transform.position) / 1.1f;  //moveobject towards center
    }

    void SocialDistancing(GameObject MoveThis, int distance)
    {
        //Rule 2: Boids try to keep a small distance away from other objects (including other boids).
        Vector3 C = CalculateCenter();
        foreach (GameObject boi in Boid)
        {
            if (boi.transform.position != MoveThis.transform.position)
            {
                if (Vector3.Magnitude(boi.transform.position - MoveThis.transform.position) < distance)
                {
                    C = C - (boi.transform.position - MoveThis.transform.position) /5f;
                }
            }
        }
        MoveThis.transform.position = C;
    }
    void SpawnBoids(GameObject[] array)
    {
        foreach (GameObject item in array)
        {
            int x = Random.Range(1, 15);
            int y = Random.Range(1, 15);
            int z = Random.Range(1, 15);
            item.transform.position = new Vector3(x, y, z);

        }
    }
}
/*
*        //   Vector3 position = new Vector3(Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3));

       //  Vector3 velocity = ((boi.transform.position + position) - boi.transform.position) / Time.deltaTime;

       // boi.transform.position = velocity;

    */
