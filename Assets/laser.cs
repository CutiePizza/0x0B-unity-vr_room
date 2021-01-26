using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{

     public LineRenderer Line;
     // boolean to determine if the line render is enabled or disabled
     public bool toggled = false;
     
     private float HandLeft = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
    // Start is called before the first frame update
    void Start()
    {
        Line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandLeft = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        Line.SetPosition(0, transform.position);
        if (HandLeft > 0.9)
        {
            toggled = true;
            Line.enabled = true;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                Debug.Log("I hit it!!!");
                if (hit.collider)
                {
                    Line.SetPosition(1, hit.point);
                    if (hit.collider.tag == "Interactable")
                        Debug.Log("HAHAHAHHAHA");
                }
            }

        } else
        {
            toggled = false;
            Line.enabled = false;
        }

        
    }
}
