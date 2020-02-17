using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private bool selected = false;
    private Vector3 original_angles;
    public bool selectable = true;
    // Start is called before the first frame update
    void Start()
    {
        original_angles = transform.eulerAngles;
    }

    public void SelectCard()
    {
        selected = true;
    }

    public bool isDragging()
    {
        return selected;
    }

    public void Drop()
    {
        selectable = false;
        StartCoroutine(enableReselection());
    }

    private IEnumerator enableReselection()
    {
        yield return new WaitForSeconds(.1f);
        selectable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (selectable == false)
        {
            selected = false;
            return;
        }
        if (transform.eulerAngles.x != original_angles.x)
            transform.eulerAngles = new Vector3(original_angles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        if (selected && !Input.GetMouseButton(0))
        {
            selected = false;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask mask =~ LayerMask.GetMask("NoSelection");
        if (Physics.Raycast(ray, out hit, 500f, mask))
        {
            if (hit.collider.Equals(GetComponent<BoxCollider>()))
            {
                if (Input.GetMouseButton(1))
                {
                    ObjectSpin x = GetComponent<ObjectSpin>();
                    x.EndSpin();
                    if (!x.isSpinning())
                        transform.eulerAngles = new Vector3(-45f, transform.eulerAngles.y, transform.eulerAngles.z);
                }
                else
                    GetComponent<ObjectSpin>().StartSpin();

                bool can_select = true;
                foreach (ObjectDrag x in UnityEngine.Object.FindObjectsOfType<ObjectDrag>())
                {
                    if (x == this) continue;
                    if (x.selected)
                    {
                        can_select = false;
                        break;
                    }
                }
                selected = can_select && (Input.GetMouseButton(0) || Input.GetMouseButton(1));
            }
            else GetComponent<ObjectSpin>().EndSpin();
        }
        if (selected)
        {
            float distance;
            Plane plane = new Plane(Vector3.up, new Vector3(0, 1.5f, 0));
            if (plane.Raycast(ray, out distance))
                transform.position = ray.GetPoint(distance);
        }
    }
}
