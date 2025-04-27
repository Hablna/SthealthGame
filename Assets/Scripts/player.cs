using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public float speed;
    Rigidbody myRigidbody;

    void Start(){
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 input = new Vector3 (Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (input != Vector3.zero){
            Quaternion targetRotation = Quaternion.LookRotation(input);
            myRigidbody.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }
        myRigidbody.position += input* Time.deltaTime* speed;
    }
}    
