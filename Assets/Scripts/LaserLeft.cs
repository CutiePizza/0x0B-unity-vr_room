using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLeft : MonoBehaviour
{
    //render our hand pointer raycast
    public LineRenderer telekinesisLine;

    // information about line renderer
    public float lineWidth = 0.1f;
    public float lineMaxLength = 0.1f;

    // boolean to determine if the line render is enabled or disabled
    public bool toggled = false;
    private bool doorOpen = false;
    private bool particleOn = false;

    //store input from our left hand trigger
    private float HandLeft = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);

    // boolean to determine if we hit an enemy with the raycast
    public bool enemyHit = false;

    //game object to store the enemy
    private GameObject enemy;

    public Animator animDoor;
    public ParticleSystem dust;
    public LayerMask PlayerLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        //Set up our line render
        Vector3[] startLinePositions = new Vector3[2] { Vector3.zero, Vector3.zero} ;
        telekinesisLine.SetPositions(startLinePositions);
        telekinesisLine.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        // update the value HandLeft every frame with new value from trigger
        HandLeft = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);

        //turn on/off the line render if trigger is pulled in
        if (HandLeft > 0.9)
        {
            toggled = true;
            telekinesisLine.enabled = true;
        } else
        {
            telekinesisLine.enabled = false;
            toggled = false;
            // make sure that we can't register a hit on an enemy when the line render is tunred off
            enemyHit = false;
        }
        
        if (toggled)
        {
            //starts the raycasts if we have pulled the trigger
            telekinesis(transform.position, transform.forward, lineMaxLength);
        }
    }

    // raycast funtion - draws the line renderer, detects if an emeny is hit, updates the telekinesisExplode script
    private void telekinesis(Vector3 targetPosition, Vector3 direction, float length)
    {
        // set up raycast hit
        RaycastHit hit;

        // set up raycast
        Ray telekinesisOut = new Ray(targetPosition, direction);

        //declares an end position variable for the line renderer
        Vector3 endPosition = targetPosition +  (direction * length);

        //run the raycast
        if (Physics.Raycast(telekinesisOut, out hit, PlayerLayerMask))
        {
            // update the line render with the new end position
               endPosition = hit.point;

            // set the enemy game object to the gameObject that the raycast hit
            enemy = hit.collider.gameObject;
            
            if (enemy.tag == "Interactable")
            {
                enemyHit = true;
                if (OVRInput.Get(OVRInput.Button.Four) || OVRInput.Get(OVRInput.Button.Two))
                {
                    if (doorOpen == false)
                    {
                        animDoor.SetBool("character_nearby", true);
                        doorOpen = true;
                    }
                    else
                    {
                        animDoor.SetBool("character_nearby", false);
                        doorOpen = false;
                    }
                }
            }
            if (enemy.tag == "Particle")
            {
                if (OVRInput.Get(OVRInput.Button.Four) || OVRInput.Get(OVRInput.Button.Two))
                {
                    if (particleOn == false)
                    {
                        particleOn = true;
                        dust.gameObject.SetActive(true);
                    }
                    else
                    {
                        dust.gameObject.SetActive(false);
                        particleOn = false;
                    }
                }
            }
        }
        else if (enemyHit)
        {
            enemyHit = false;
        }
        // update our line renderer declared at top of file
        telekinesisLine.SetPosition(0, targetPosition);
        telekinesisLine.SetPosition(1, endPosition);
    }
}
