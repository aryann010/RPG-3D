using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
   

    public Transform mainSelection,tempSelection,pullObjDestination;
    public RaycastHit hit;
    public Color highlightedMat, defaultMat;
    public Renderer selectionRenderer;
    public LayerMask layerMask;
    private bool pullObjectReady, lockRayCast, throwPowerActive;
    public Rigidbody rigidbodyMain;
    public Camera cam;
    public float powerThrow, rayRange,speedOfDrag;
    public Transform[] objects;

   
    private void Update()
    {
        if (mainSelection != null)
        {
            selectionRenderer = tempSelection.GetComponent<Renderer>();
            selectionRenderer.material.color = defaultMat ;

            mainSelection = null;
        }
        if (lockRayCast == false)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, rayRange, layerMask)) ;
            {
                Debug.DrawRay(cam.transform.position, cam.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                tempSelection = hit.transform;
                selectionRenderer = tempSelection.GetComponent<Renderer>();
                rigidbodyMain = tempSelection.GetComponent<Rigidbody>();
                if (tempSelection.tag == selectableTag)
                {
                    if (selectionRenderer != null)
                    {
                        selectionRenderer.material.color = highlightedMat;
                    }
                    mainSelection = tempSelection;
                }
            }
          //  if (mainSelection == null)
          //  {
          //      tempSelection = null;
          //      selectionRenderer.material.color = defaultMat;
          //  }

        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && tempSelection.tag == selectableTag)
        {
            playerPickupObject();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && tempSelection != null && tempSelection.tag == selectableTag)
        {
            playerLetGoObject();
        }
        if (tempSelection.tag == selectableTag && lockRayCast == false && throwPowerActive == true)
        {
            objectAddForce();
        }
        if (pullObjectReady == true)
        {
            objectToDest();
        }
     
    }
    public void playerPickupObject()
    {
        pullObjectReady = true;
        lockRayCast = true;
        tempSelection.transform.SetParent(pullObjDestination);
        rigidbodyMain.useGravity = false;
        throwPowerActive = false;
    }
    public void playerLetGoObject()
    {
        rigidbodyMain.useGravity = true;
        pullObjDestination.transform.DetachChildren();
        lockRayCast = false;
        throwPowerActive = true;
        pullObjectReady = false;
    }
    public void objectAddForce()
    {
        Vector3 pushDir = tempSelection.transform.position - cam.transform.position;
        rigidbodyMain.AddForceAtPosition(pushDir.normalized * powerThrow, pushDir, ForceMode.Impulse);
        throwPowerActive = false;
    }
    public void objectToDest()
    {
        tempSelection.localPosition = Vector3.Lerp(tempSelection.localPosition, pullObjDestination.transform.localPosition, speedOfDrag);
       
    }
}