using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject
{
    // static vars
    public static double BigG = 6.674e-11; // gravitational constant
    public static float SunMass = (float)1e10; // scaled sun mass
    public static readonly float[] MassFactors = { (float)1.652e-7, (float)2.446e-6, (float)3.003e-6, (float)3.212e-7, (float)9.542e-4, (float)2.857e-4, (float)4.364e-5, (float)5.148e-5, (float)6.581e-9 }; // scaled mass factors for all planets
    public static readonly float[] Masses = { MassFactors[0] * SunMass, MassFactors[1] * SunMass, MassFactors[2] * SunMass, MassFactors[3] * SunMass, MassFactors[4] * SunMass, MassFactors[5] * SunMass, MassFactors[6] * SunMass, MassFactors[7] * SunMass, MassFactors[8] * SunMass }; // scaled masses for all planets
                  
    
    // object vars
    public float mass;
    public float radius;
    public float RotSpeed;
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    // object GameObject entry
    public GameObject Object;

    // constructor
    public GravityObject(Vector3 StartingPos, Vector3 StartingVelocity, float mass, float radius, float RotSpeed)
    {
        // create object primitive (sphere)
        Object = GameObject.CreatePrimitive(PrimitiveType.Sphere);


        // set starting vars
        this.RotSpeed = RotSpeed;
        this.mass = mass;
        this.radius = radius;
        position = StartingPos;
        velocity = StartingVelocity;
        acceleration = Vector3.zero;
        force = Vector3.zero;

        // Set the starting position
        Object.transform.position = position;

        // set obj radius
        Object.transform.localScale = new Vector3(2 * radius, 2 * radius, 2 * radius);
    }

    // calcute the forces between every sphere and every other sphere, this truly is an n-body calculator.
    public static void CalculateForces(List<GravityObject> Objects)
    {
        // for each object
        for (int i = 0; i < Objects.Count; i++)
        {
            // and all other objects
            for (int j = i + 1; j < Objects.Count; j++)
            {
                // calculate force on each and inecrement their forces
                float Coeffecient = -(float)BigG * Objects[i].mass * Objects[j].mass / (float)Math.Pow(Vector3.Distance(Objects[i].position, Objects[j].position), 2);
                Vector3 CalculatedForce = Coeffecient * Vector3.Normalize(Objects[i].position - Objects[j].position);
                Objects[i].force += CalculatedForce;
                Objects[j].force -= CalculatedForce;
            }
        }
    }
    // update object velocities and positions
    public static void UpdateObjects(List<GravityObject> Objects, float DeltaTime)
    {
        // for each objects
        foreach (GravityObject Current in Objects)
        {
            // calculate acceleration from force, update velocity and finally update position
            Current.acceleration = Current.force / Current.mass;
            Current.velocity += DeltaTime * Current.acceleration;
            Current.position += DeltaTime * Current.velocity;
            Current.Object.transform.position = Current.position;
            Current.force = Vector3.zero;

            // draw ray so objects are easily visible in debug mode
            Debug.DrawRay(Current.position, new Vector3(0, 100, 0), Color.red);
        }
    }

    // handle collisions of objects
    public static void HandleCollisions(List<GravityObject> Objects, float DeltaTime)
    {
        // for each object
        for (int i = 0; i < Objects.Count; i++)
        {
            // and all other ojects
            for (int j = i + 1; j < Objects.Count; j++)
            {
                float Distance = Vector3.Distance(Objects[i].position, Objects[j].position);
                float RadiusSum = Objects[i].radius + Objects[j].radius;
                // if we have a collision
                if (Distance <= RadiusSum)
                {
                    // calculate collision normal and impulse transfer
                    Vector3 CollisionNormal = Vector3.Normalize(Objects[i].position - Objects[j].position);
                    float ImpulseTransfer = 2 * Vector3.Dot(CollisionNormal, Objects[j].velocity - Objects[i].velocity) / ((1 / Objects[i].mass) + (1 / Objects[j].mass));
                    // calculate new velocities and positions
                    Objects[j].velocity += (-ImpulseTransfer / Objects[j].mass) * CollisionNormal;
                    Objects[i].velocity += (ImpulseTransfer / Objects[i].mass) * CollisionNormal;
                    Objects[j].position += Objects[j].velocity * DeltaTime;
                    Objects[i].position += Objects[i].velocity * DeltaTime;
                    // update the positions
                    Objects[j].Object.transform.position = Objects[j].position;
                    Objects[i].Object.transform.position = Objects[i].position;
                }
            }
        }
    }

    // handle rotation function
    public static void HandleRots(List<GravityObject> Objects, float DeltaTime)
    {
        // for each object rotate it by its rotation speed times deltatime
        foreach (var Obj in Objects)
        {
            Obj.Object.transform.Rotate(new Vector3(0, Obj.RotSpeed * DeltaTime, 0));
        }
    }
}
