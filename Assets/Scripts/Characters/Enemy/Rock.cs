using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody rb;
    [Header("Basic Settings")] public float force;
    public GameObject target;
    private Vector3 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FlyToTarget()
    {
        Vector3 direction = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
}