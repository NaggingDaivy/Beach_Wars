using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{

    public float Health_Amount = 20f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        print("On Trigger : " + other.gameObject.name);
        
        Projectile pro = other.GetComponent<Projectile>();

        if(pro)
        {
            
        }
    }
}
