using UnityEngine;
using System.Collections;

public class FairyMeteorAddForceDemo : MonoBehaviour {

    public int strength;
    public Vector3 direction;

    protected Rigidbody rgbd;

    public void Awake()
    {
        rgbd = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        rgbd.AddForce(direction * strength);
    }
}
