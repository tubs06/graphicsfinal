using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// class for gravity object


public class SolarSystem : MonoBehaviour
{
    // multiplier so things dont move so slow.
    float TimeMultiplier = 40;

    // variable to start and stop simulation
    public bool Simulating = true;

    // List with all active objects
    public List<GravityObject> CreatedObjs = new List<GravityObject>();

    // variable for main camera object
    public Camera cam;

    // function to calculate perfect orbit speed for a circular orbit v = sqrt(GM/D)
    public float OrbitSpeed(float Mass, float Distance) => (float)Math.Sqrt(Mass * GravityObject.BigG / Distance);


    // function that sets up all planets and their respective starting velocities and distances from the sun.
    void CreateSolarSystem()
    {
        // create planet objects
        CreatedObjs.Add(new GravityObject(new Vector3(0, 10, 0), Vector3.zero, GravityObject.SunMass, 25, 0)); // Create Sun
        CreatedObjs.Add(new GravityObject(new Vector3(41.633f, 10, 0), new Vector3(0, 0, OrbitSpeed(GravityObject.SunMass + GravityObject.Masses[0], 41.633f)), GravityObject.Masses[0], .85f, 1)); // Create Mercury
        CreatedObjs.Add(new GravityObject(new Vector3(77.67f, 10, 0), new Vector3(0, 0, OrbitSpeed(GravityObject.SunMass + GravityObject.Masses[1], 77.67f)), GravityObject.Masses[1], 2.15f, 1.2f)); // Create Venus
        CreatedObjs.Add(new GravityObject(new Vector3(107.457f, 10, 0), new Vector3(0, 0, OrbitSpeed(GravityObject.SunMass + GravityObject.Masses[2], 107.457f)), GravityObject.Masses[2], 2.45f, 1.4f)); // Create Earth
        CreatedObjs.Add(new GravityObject(new Vector3(163.689f, 10, 0), new Vector3(0, 0, OrbitSpeed(GravityObject.SunMass + GravityObject.Masses[3], 163.689f)), GravityObject.Masses[3], 1.2f, 1.6f)); //  Create Mars
        CreatedObjs.Add(new GravityObject(new Vector3(559.048f, 10, 0), new Vector3(0, 0, OrbitSpeed(GravityObject.SunMass + GravityObject.Masses[4], 559.048f)), GravityObject.Masses[4], 10f, 1.8f)); // Create Jupiter
        CreatedObjs.Add(new GravityObject(new Vector3(1025.217f, 10, 0), new Vector3(0, 0, OrbitSpeed(GravityObject.SunMass + GravityObject.Masses[5], 1025.217f)), GravityObject.Masses[5], 9f, 2)); // Create Saturn
        CreatedObjs.Add(new GravityObject(new Vector3(2062.145f, 10, 0), new Vector3(0, 0, OrbitSpeed(GravityObject.SunMass + GravityObject.Masses[6], 2062.145f)), GravityObject.Masses[6], 5f, 2.2f)); // Create Uranus
        CreatedObjs.Add(new GravityObject(new Vector3(3232.919f, 10, 0), new Vector3(0, 0, OrbitSpeed(GravityObject.SunMass + GravityObject.Masses[7], 3232.919f)), GravityObject.Masses[7], 5f, 2.4f)); // Create Neptune
        CreatedObjs.Add(new GravityObject(new Vector3(4248.15f, 10, 0), new Vector3(0, 0, OrbitSpeed(GravityObject.SunMass + GravityObject.Masses[8], 4248.15f)), GravityObject.Masses[8], .65f, 2.6f)); // Create Pluto

        // load planet materials
        CreatedObjs[0].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/sun");
        CreatedObjs[1].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/mercury");
        CreatedObjs[2].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/venus");
        CreatedObjs[3].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/earth");
        CreatedObjs[4].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/mars");
        CreatedObjs[5].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/jupiter");
        CreatedObjs[6].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/saturn");
        CreatedObjs[7].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/uranus");
        CreatedObjs[8].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/neptune");
        CreatedObjs[9].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/pluto");

        // function which updates the materials on the objects
        DynamicGI.UpdateEnvironment();
    }

    // destroy objects when script is destroyed
    public void OnDestroy()
    {
        foreach(var a in CreatedObjs)
        {
            Destroy(a.Object);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // set render pipeline to HDRP
        GraphicsSettings.renderPipelineAsset = Resources.Load<RenderPipelineAsset>("HDRenderPipelineAsset");
    
        // create solar system and lock mouse
        CreateSolarSystem();
    }

    // Update is called once per frame
    void Update()
    {
        // if we are simulating
        if (Simulating)
        {
            // get good scaled delta time so things dont move too slow
            float GoodDeltaTime = TimeMultiplier * Time.deltaTime;
            // update objects
            GravityObject.CalculateForces(CreatedObjs);
            GravityObject.UpdateObjects(CreatedObjs, GoodDeltaTime);
            GravityObject.HandleCollisions(CreatedObjs, GoodDeltaTime);
            GravityObject.HandleRots(CreatedObjs, GoodDeltaTime);


            // If we press F, speed up the sim otherwise set to default
            if (Input.GetKey(KeyCode.F))
            {
                TimeMultiplier = 4000;
            }
            else
            {
                TimeMultiplier = 40;
            }
        }
    }
}
