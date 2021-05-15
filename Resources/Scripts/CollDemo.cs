using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;



public class CollDemo : MonoBehaviour
{
    // multiplier so things dont move so slow.
    float TimeMultiplier = 20;

    // variable to start and stop simulation
    public bool Simulating = true;

    // List with all active objects
    public List<GravityObject> CreatedObjs = new List<GravityObject>();

    // variable for main camera object
    public Camera cam;

    // when script destroyed, destroy all objects
    public void OnDestroy()
    {
        DestroyObjects();
    }

    // destroy objects
    public void DestroyObjects()
    {
        foreach (var a in CreatedObjs)
        {
            Destroy(a.Object);
        }
    }

    // create demo objects
    public void CreateDemo()
    {
        // create objs
        CreatedObjs.Add(new GravityObject(new Vector3(0, 10, 0), Vector3.zero, GravityObject.SunMass, 5, 0));
        CreatedObjs.Add(new GravityObject(new Vector3(20, 10, 0), Vector3.zero, GravityObject.SunMass, 5, 0));
        CreatedObjs.Add(new GravityObject(new Vector3(0, 10, 20), Vector3.zero, GravityObject.SunMass, 5, 0));
        CreatedObjs.Add(new GravityObject(new Vector3(20, 10, 20), Vector3.zero, GravityObject.SunMass, 5, 0));
        CreatedObjs.Add(new GravityObject(new Vector3(30, 10, 0), Vector3.zero, GravityObject.SunMass, 5, 0));
        CreatedObjs.Add(new GravityObject(new Vector3(0, 10, 30), Vector3.zero, GravityObject.SunMass, 5, 0));
        CreatedObjs.Add(new GravityObject(new Vector3(30, 10, 30), Vector3.zero, GravityObject.SunMass, 5, 0));
        // add earth material
        CreatedObjs[0].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/earth");
        CreatedObjs[1].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/earth");
        CreatedObjs[2].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/earth");
        CreatedObjs[3].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/earth");
        CreatedObjs[4].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/earth");
        CreatedObjs[5].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/earth");
        CreatedObjs[6].Object.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/earth");
    }

    // Start is called before the first frame update
    void Start()
    {
        // set render pipeline to HDRP
        GraphicsSettings.renderPipelineAsset = Resources.Load<RenderPipelineAsset>("HDRenderPipelineAsset");

        // create demo objects
        CreateDemo();
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


            // If we press F, speed up the sim otherwise set to default
            if (Input.GetKey(KeyCode.F))
            {
                TimeMultiplier = 200;
            }
            else
            {
                TimeMultiplier = 20;
            }

            // if we press r, reset simulation
            if (Input.GetKeyDown(KeyCode.R))
            {
                DestroyObjects();
                CreatedObjs = new List<GravityObject>();
                CreateDemo();
            }
        }
    }
}
