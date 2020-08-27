using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Spinner : MonoBehaviour
{
    public float spinSpeed = 3600;
    public bool doSpin = false;
    private Rigidbody rb;
    public GameObject graphicsObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (doSpin)
        {
            graphicsObject.transform.Rotate(new Vector3(0, spinSpeed * Time.deltaTime, 0));
        }
    }


}
