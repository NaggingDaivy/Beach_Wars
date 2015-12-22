using UnityEngine;
using System.Collections;

public class testLookat : MonoBehaviour {

    public Transform obj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        

        transform.LookAt(obj.position); ;

        transform.Rotate(-90, 0, 0, Space.Self);
	
	}
}
