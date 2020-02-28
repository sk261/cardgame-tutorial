using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpin : MonoBehaviour
{
    // Start is called before the first frame update
    private bool can_spin;
    private bool spin_enabled;
    private bool spin_started;
    private int momentum;
    private Quaternion origin_orientation;
    void Start()
    {
        spin_enabled = false;
        spin_started = false;
        can_spin = true;
        momentum = 1;
    }

    public void DisableSpin()
    {
        can_spin = false;
        EndSpin();
    }

    public void EnableSpin()
    {
        can_spin = true;
    }

    public bool isSpinning()
    {
        return spin_started;
    }

    public void StartSpin()
    {
        if (!can_spin) return;
        if (!spin_enabled & !spin_started)
        {
            spin_started = true;
            origin_orientation = transform.localRotation;
        }
        spin_enabled = true;
        momentum = 1;
    }

    public void EndSpin()
    {
        spin_enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (spin_enabled)
            transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
        else if(spin_started)
        {
            float angleA = transform.localRotation.eulerAngles[1];

            transform.Rotate(new Vector3(0, 30*momentum, 0) * Time.deltaTime);
            momentum += 1;

            float angleC = transform.localRotation.eulerAngles[1];
            float angleB = origin_orientation.eulerAngles[1];

            // if A < B < C then stop and set to origin.
            angleC += 360f * ((angleC < angleA) ? 1 : 0);
            angleB += 360f * ((angleA > angleB) ? 1 : 0);
            if (angleA < angleB && angleB < angleC)
            {
                transform.localRotation = origin_orientation;
                spin_started = false;
                momentum = 1;
            }
        }
    }
}
