using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody))]
public class FairyDustRandomPosition : MonoBehaviour {
    public float intervalInSec;
    public Vector3 minCoord;
    public Vector3 maxCoord;
    public float speed;
    public float maxVelocityMagnitude;
    protected Vector3 initPos;
    protected Rigidbody rgbd;
    protected Vector3 nextPos;

    public void Awake()
    {
        rgbd = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        rgbd.drag = 1;
        initPos = transform.position;
        InvokeRepeating("GetNextPosition", 0, intervalInSec);
    }

    public void FixedUpdate()
    {
        if (rgbd.velocity.sqrMagnitude > maxVelocityMagnitude * maxVelocityMagnitude)
            return;
        Vector3 directionToNextPos;
        directionToNextPos = (nextPos - transform.position).normalized;
        rgbd.AddForce(directionToNextPos * speed);

        //Debug.DrawLine(transform.position, nextPos, Color.red);
    }

    public void GetNextPosition()
    {
        Vector3 tmp;
        float x, y, z;
        x = Random.Range(minCoord.x, maxCoord.x);
        y = Random.Range(minCoord.y, maxCoord.y);
        z = Random.Range(minCoord.z, maxCoord.z);
        tmp = new Vector3(x, y, z);
        nextPos = tmp + initPos;
    }
}
