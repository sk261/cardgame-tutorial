using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour
{
    [Header("Gravitation Ignore Axis")]
    public bool X;
    public bool Y, Z;
    public bool DisableSpin;
    public float OuterPullPower = 0f;
    public bool GravityLock = false;
    public bool DragLock = false;
    public List<Object> CaughtObjects;

    // Start is called before the first frame update
    void Start()
    {
        CaughtObjects = new List<Object>();
    }

    public bool isCaught(Object n)
    {
        if (CaughtObjects == null) return false;
        return CaughtObjects.Contains(n);
    }

    private void HandleSpin(GameObject other, bool entered)
    {
        if (this.enabled != true) return;
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
        if (this.enabled == false || (OuterPullPower == 0 && GravityLock)) return;
        HandleSpin(other.gameObject, true);
        if (other.gameObject == null) return;
        ObjectDrag x = null;
        other.TryGetComponent<ObjectDrag>(out x);
        if (x != null)
        {
            bool isCaught = false;
            foreach (GravityWell n in UnityEngine.Object.FindObjectsOfType<GravityWell>())
                if (n != this && n.CaughtObjects.Contains(x.gameObject) && n.GravityLock)
                {
                    isCaught = true;
                    break;
                }
            if (!isCaught)
                CaughtObjects.Add(other.gameObject);
        }
    }

    private void moveCloser(GameObject n, bool forced = false)
    {
        Vector3 target = transform.position;
        float power = OuterPullPower;
        if (CaughtObjects.Contains(n))
        {
            if (!forced)
            {
                if (X) target.x = n.transform.position.x;
                if (Y) target.y = n.transform.position.y;
                if (Z) target.z = n.transform.position.z;
            }
            power = 10f;
        }
        n.transform.position = Vector3.MoveTowards(n.transform.position, target, power * Time.deltaTime);
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.enabled != true) return;
        HandleSpin(other.gameObject, false);
        if (CaughtObjects.Contains(other.gameObject))
        {
            ObjectDrag x = null;
            other.gameObject.TryGetComponent<ObjectDrag>(out x);
            if (x == null)
                CaughtObjects.Remove(other.gameObject);
            else
            {
                if (DragLock)
                    x.Drop();
                else
                    CaughtObjects.Remove(other.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.enabled != true) return;
        foreach (GameObject n in UnityEngine.Object.FindObjectsOfType<GameObject>())
        {
            ObjectDrag x = null;
            n.TryGetComponent<ObjectDrag>(out x);
            if (x == null || x.isDragging())
                continue;

            // Check if object is in a gravity well, or if a gravity well 
            if (x.selectable == false && CaughtObjects.Contains(n) == true)
                moveCloser(n, true);
            else
            {
                bool isFree = true;
                foreach (GravityWell g in UnityEngine.Object.FindObjectsOfType<GravityWell>())
                {
                    if (g == this) continue;
                    else if (
                        (g.isCaught(n) && g.GravityLock)
                        || (g.OuterPullPower > 0 && this.GravityLock == false && CaughtObjects.Contains(n))
                    )
                    {
                        isFree = false;
                        break;
                    }
                }
                if (isFree == false) continue;
                moveCloser(n);
            }
        }
    }
}
