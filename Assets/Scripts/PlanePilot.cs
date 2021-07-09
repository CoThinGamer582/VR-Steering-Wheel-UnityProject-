using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePilot : MonoBehaviour
{
    [SerializeField]
    float eulerAngX;
    [SerializeField]
    float eulerAngY;
    [SerializeField]
    float eulerAngZ;

    public GameObject barrel;
    public GameObject ammo;

    private bool turnLeft = false;
    private bool turnRight = false;
    private bool dive = false;
    private bool climb = false;
    private bool fire = false;

    public float speed = 40f;
    public float turnSpeed = 0.4f;

    private float nextFire = 0f;
    private float fireDelay = 0.1f;

    public float shootPower = 1000f;

    void Update()
    {
        GetShoot();
        GetAngles();
        GetTurn();
        Turn();
        Stabilize();
        Shoot();
        ForwardMovement();
        CheckCollisionWithTerrain();
    }

    private void CheckCollisionWithTerrain()
    {
        float terrainHeightWhereWeAre = Terrain.activeTerrain.SampleHeight(transform.position);

        if(terrainHeightWhereWeAre >= transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, terrainHeightWhereWeAre, transform.position.z);
        }
    }

    private void ForwardMovement()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void GetAngles()
    {
        eulerAngX = transform.localEulerAngles.x;
        eulerAngY = transform.localEulerAngles.y;
        eulerAngZ = transform.localEulerAngles.z;
    }

    private void Shoot()
    {
        if(fire && nextFire < Time.time)
        {
            var fired = Instantiate(ammo, barrel.transform);
            fired.GetComponent<Rigidbody>().AddForce(transform.forward * shootPower);
            Destroy(fired, 5f);
            nextFire = Time.time + fireDelay;
        }
    }

    private void Stabilize()
    {
        if(eulerAngZ < 270f && eulerAngZ > 268f)
        {
            eulerAngX = eulerAngX + 0.2f;
            eulerAngZ = 270f;
            transform.eulerAngles = new Vector3(eulerAngX, eulerAngY, eulerAngZ);
        }

        if(eulerAngZ > 90 && eulerAngZ < 92f)
        {
            eulerAngX = eulerAngX + 0.2f;
            eulerAngZ = 90f;
            transform.eulerAngles = new Vector3(eulerAngX, eulerAngY, eulerAngZ);
        }
    }

    private void Turn()
    {
        if(turnLeft)
        {
            eulerAngZ = eulerAngZ + turnSpeed;
            transform.eulerAngles = new Vector3(eulerAngX, eulerAngY, eulerAngZ);
        }

        if(turnRight)
        {
            eulerAngZ = eulerAngZ - turnSpeed;
            transform.eulerAngles = new Vector3(eulerAngX, eulerAngY, eulerAngZ);
        }

        if(dive)
        {
            eulerAngX = eulerAngX + turnSpeed;
            transform.eulerAngles = new Vector3(eulerAngX, eulerAngY, eulerAngZ);
        }

        if(climb)
        {
            bool sharpTurnNoLiftLeft = eulerAngZ < 92 && eulerAngZ > 69;
            bool sharpTurnLittleLiftLeft = eulerAngZ < 69 && eulerAngZ > 49;
            bool mildTurnAndLiftLeft = eulerAngZ < 49 && eulerAngZ > 29;
            bool sharpTurnNoLiftRight = eulerAngZ > 269 && eulerAngZ < 292;
            bool sharpTurnLittleLiftRight = eulerAngZ > 292 && eulerAngZ < 312;
            bool mildTurnAndLiftRight = eulerAngZ > 312 && eulerAngZ < 332;

            if(sharpTurnNoLiftLeft)
            {
                eulerAngY -= turnSpeed * Time.deltaTime * 100f;
            }
            else if (sharpTurnLittleLiftLeft)
            {
                eulerAngY -= turnSpeed * Time.deltaTime * 85f;
                eulerAngX -= turnSpeed * Time.deltaTime * 20f;
            }
            else if (mildTurnAndLiftLeft)
            {
                eulerAngY -= turnSpeed * Time.deltaTime * 40f;
                eulerAngX -= turnSpeed * Time.deltaTime * 60f;
            }
            else if (sharpTurnLittleLiftRight)
            {
                eulerAngY += Time.deltaTime * 100f;
            }
            else if (sharpTurnLittleLiftRight)
            {
                eulerAngY += turnSpeed * Time.deltaTime * 85f;
                eulerAngX -= turnSpeed * Time.deltaTime * 20f;
            }
            else if (mildTurnAndLiftRight)
            {
                eulerAngY += turnSpeed * Time.deltaTime * 40f;
                eulerAngX -= turnSpeed * Time.deltaTime * 60f; 
            }

            transform.eulerAngles = new Vector3(eulerAngX, eulerAngY, eulerAngZ);
        }
    }

    private void GetShoot()
    {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            fire = true;
        }

        if(OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            fire = false;
        }
    }

    private void GetTurn()
    {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.RTouch))
        {
            turnLeft = true;
        }

        if(OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.RTouch))
        {
            turnLeft = false;
        }

        if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.RTouch))
        {
            turnRight = true;
        }

        if(OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.RTouch))
        {
            turnRight = false;
        }

        if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp, OVRInput.Controller.RTouch))
        {
            dive = true;
        }

        if(OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickUp, OVRInput.Controller.RTouch))
        {
            dive = false;
        }

        if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown, OVRInput.Controller.RTouch))
        {
            climb = true;
        }

        if(OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickDown, OVRInput.Controller.RTouch))
        {
            climb = false;
        }
    }
}
