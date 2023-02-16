using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate3D : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float rotateSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var euler = transform.eulerAngles;
        euler.x += rotateSpeed * Time.deltaTime;
       
    }
}
