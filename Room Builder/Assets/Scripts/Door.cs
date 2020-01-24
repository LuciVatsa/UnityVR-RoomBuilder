using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform Handposition;
    Rigidbody rigidbody; 
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    
    void MoveDoor()
    {
        rigidbody.MovePosition(Handposition.transform.position);
    }
}
