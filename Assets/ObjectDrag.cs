using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private bool selected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (selected && !Input.GetMouseButton(0))
        {
            selected = false;
            print("Deselected");
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButton(0) && hit.collider.Equals(GetComponent<BoxCollider>()))
                selected = true;
        }
        if (selected)
        {
            float distance;
            Plane plane = new Plane(Vector3.up, new Vector3(0, 2, 0));
            if (plane.Raycast(ray, out distance))
                transform.position = ray.GetPoint(distance);
        }
    }
}
