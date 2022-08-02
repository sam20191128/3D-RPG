using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    public GameObject testObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = new Vector3(Random.Range(-20, 20), Random.Range(-15, 15), Random.Range(0, 30));
            GameObject g = Instantiate(testObject, pos, Quaternion.identity) as GameObject;
            Destroy(g, 5);
        }
        //if (Input.touchCount>0)
        //{
        //    Vector3 pos = new Vector3(Random.Range(-10, 10), Random.Range(-15, 15), Random.Range(0, 10));
        //    GameObject g = Instantiate(testObject, pos, Quaternion.identity) as GameObject;
        //    Destroy(g, 5);
        //}
    }
}
