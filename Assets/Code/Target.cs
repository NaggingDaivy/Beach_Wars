using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Target : MonoBehaviour
{

    public float Health_Amount = 20f;
    public int ScoreValue = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {


        Projectile projectile = other.GetComponent<Projectile>();

        if(projectile)
        {
            Health_Amount -= projectile.Damage;



            if (Health_Amount <= 0)
            {

                GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().UpdateScore(ScoreValue);
                
                DestroyObject(this.gameObject);
            }
                
        }
   }
}
