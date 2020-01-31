using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour
{
    [Header("Gravitation Ignore Axis")]
    public bool X;
    public bool Y, Z;
    public bool DisableSpin;
    private List<Object> CaughtObjects;
    // Start is called before the first frame update
    void Start()
    {
        CaughtObjects = new List<Object>();
    }

    private void HandleSpin(GameObject other, bool entered)
    {
        if (!DisableSpin) return;
        ObjectSpin x = null;
        other.TryGetComponent<ObjectSpin>(out x);
        if (x != null)
        {
            if (entered)
                x.DisableSpin();
            else
                x.EnableSpin();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleSpin(other.gameObject, true);
        ObjectDrag x = null;
        other.TryGetComponent<ObjectDrag>(out x);
        if (x != null)
            CaughtObjects.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        HandleSpin(other.gameObject, false);
        if (CaughtObjects.Contains(other.gameObject))
            CaughtObjects.Remove(other.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject n in CaughtObjects)
        {
            Vector3 target = transform.position;

            if (X) target.x = n.transform.position.x;
            if (Y) target.y = n.transform.position.y;
            if (Z) target.z = n.transform.position.z;

            n.transform.position = Vector3.MoveTowards(n.transform.position, target, 10f*Time.deltaTime);
        }
    }
}
