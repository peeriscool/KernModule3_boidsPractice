using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids4 : MonoBehaviour
{

    public int boidCount;
    boidspawn[] boids;

    Vector3 v1;
    Vector3 v2;
    Vector3 v3;

   // Vector3 Distancing = new Vector3(0, 0, 0);

    public int Centerweight = 470;
    public int CenterStep = 500;
    Vector3 Center;
    [Range(0.5f,80f)]
    public float SocialDistance = 4;
    [Range(1f, 80f)]
    public int DistanceStep = 5;
    public float DistanceWeight = 30;
    public float AlignmentWeight = 0f;
    int time = 0;
    public float Movement = 0;
    class boidspawn
        {
        //variables
        Boids4 Owner;
        public GameObject model;
        public Vector3 Position;
        public Vector3 Velocity;
        //constructor
        public boidspawn(GameObject boid, Boids4 owner) 
        {
            model = boid;
            Owner = owner;
            Owner.SpawnBoids(model); //sets inital position
            Velocity = new Vector3(0, 0, 0);
        }
        public void UpdateBoid(Vector3 position, Vector3 velocity)
        {
            Position = position;
            Velocity = velocity;
            model.transform.position = position;
            model.transform.rotation = Quaternion.LookRotation(Vector3.Normalize(velocity));
        }
        public Vector3 Seek(Transform target, float weight) //example code
        {
            if (weight < 0.0001f) return Vector3.zero;

            var desiredVelocity = Vector3.Normalize(target.position - Position) * weight;
            return desiredVelocity - Velocity;
        }
    }
    void Start()
    {
        //Time.timeScale = 0.1f;

        boids = new boidspawn[boidCount];
        for (int i = 0; i < boidCount; i++)
        {
            boids[i] = new boidspawn(GameObject.CreatePrimitive(PrimitiveType.Cube),this);
           // boids[i].model = GameObject.CreatePrimitive(PrimitiveType.Cube);
            boids[i].model.GetComponent<Renderer>().material.color = Color.red;
        }
    }
     void FixedUpdate()
    {
        time++;
        if (time < 100)
        {
            //   time = 0;


            for (int i = 0; i < boidCount; i++)
            {
                v1 = rule1(boids[i].model);
                v2 = rule2(boids[i]);
                v3 = rule3(boids[i]);

                boids[i].Velocity = boids[i].Velocity + Vector3.Normalize(v1) + v2;
                // boids[i].Velocity = boids[i].Velocity * Time.deltaTime * Movement;
                boids[i].Position = boids[i].Velocity + boids[i].Velocity;
                boids[i].UpdateBoid(boids[i].Position, boids[i].Velocity);
            }
        }
        if (time > 100)
        {
            //   time = 0;
            for (int i = 0; i < boidCount; i++)
            {
                v1 = rule1(boids[i].model);
                v2 = rule2(boids[i]);
                v3 = rule3(boids[i]);

                boids[i].Velocity = boids[i].Velocity + v1 + v2 + Vector3.Normalize(v3);
                 boids[i].Velocity = boids[i].Velocity * Time.deltaTime * Movement;
                boids[i].Position = boids[i].Velocity + boids[i].Velocity;
                boids[i].UpdateBoid(boids[i].Position, boids[i].Velocity);
            }
        }
    }

    Vector3 rule1(GameObject bj) //boids fly towards other boids
    {
        Center = Vector3.zero;
        foreach (boidspawn item in boids)
        {
            if (item.model != bj)
            {
                Center += item.model.transform.position; 
            }
        }
        Center /= boidCount; //avarage the center position
        return (Center - bj.transform.position)/ CenterStep * Centerweight; //weight can not be bigger then the step!
    }
    Vector3 rule2(boidspawn bj)
    {
        Vector3 Distancing = Vector3.zero;
        foreach (boidspawn item in boids)
        {             
                Distancing += Vector3.Normalize(bj.Position - item.model.transform.position) / Mathf.Pow(SocialDistance, 2)/ DistanceStep;
        }
        return Distancing * DistanceWeight;
    }
    Vector3 rule3(boidspawn bj)
    {
        Vector3 PrecievedVelocity = Vector3.zero;
        for (int i = 0; i < boids.Length; i++)
        {
            if( boids[i].model != bj.model)
            {
                PrecievedVelocity += boids[i].Velocity;
            }
        }
        PrecievedVelocity /= boids.Length-1; //calculate average velocity
       // Debug.Log(PrecievedVelocity);
        return (PrecievedVelocity - bj.Velocity / AlignmentWeight);
    }

  public Vector3 SpawnBoids(GameObject boid)
    {
            int x = Random.Range(4, 14);
            int y = Random.Range(4, 14);
            int z = Random.Range(4, 14);
            return boid.transform.position = new Vector3(x, y, z);
       
    }
}
