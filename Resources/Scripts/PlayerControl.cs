using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // variable to control activation
    public bool Activated = true;
    // variables for setting mouse movement speeds
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationY = 0F;

    // game camera reference
    Camera cam;

    // movement speed
    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        // get reference to game camera (this object).
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // we are in game. active camera control and movement.
        if (Activated)
        {
            // calculate delta movement
            float deltaHorz = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float deltaVert = Input.GetAxis("Vertical") * speed * Time.deltaTime;

            // update movement and camera position with a smooth linear interpolation
            transform.Translate(deltaHorz, 0, deltaVert);

            // calcuate x rotation and y rotation
            float rotationX = cam.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            // update y rotation
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

            // clamp rotation values
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            // realize y rotation
            cam.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

            // If we press space, move player up
            if (Input.GetKey(KeyCode.Space))
            {
                transform.Translate(0, speed * Time.deltaTime, 0);

            }

            // If we press LeftControl, move player down
            if (Input.GetKey(KeyCode.LeftControl))
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);
            }

            // If we press left shift, speed up player movement otherwise set to default
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 100;
            }
            else
            {
                speed = 10;
            }
        }
    }
}
