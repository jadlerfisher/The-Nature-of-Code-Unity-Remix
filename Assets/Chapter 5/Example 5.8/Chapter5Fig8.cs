using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Drag code from: Chirag Donga
// https://vasundharavision.com
public class Chapter5Fig8 : MonoBehaviour
{
    public GameObject ropeGrip;
    bool isMouseDragging;
    Vector3 screenPosition;
    Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            ropeGrip = ReturnClickedObject(out hitInfo);
            if (ropeGrip != null)
            {
                isMouseDragging = true;
                Debug.Log("our target position :" + ropeGrip.transform.position);
                //Here we Convert world position to screen position.
                screenPosition = Camera.main.WorldToScreenPoint(ropeGrip.transform.position);
                offset = ropeGrip.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDragging = false;
        }

        if (isMouseDragging)
        {
            //tracking mouse position.
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);

            //convert screen position to world position with offset changes.
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;

            //It will update target gameobject's current postion.
            ropeGrip.transform.position = currentPosition;
        }

    }


    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject targetObject = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            targetObject = hit.collider.gameObject;
        }
        return targetObject;
    }
}
