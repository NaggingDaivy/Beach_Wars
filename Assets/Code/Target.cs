using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public int ScoreValue = 10;
    public ParticleSystem ParticleFire;
    public ParticleSystem ParticleSmoke;

	// Use this for initialization
	void Start ()
	{

        //ParticleFire.enableEmission = false;
        //ParticleSmoke.enableEmission = false;


	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 2, 0);
	
	}

    public void UpdateScore()
    {
        GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().UpdateScore(ScoreValue);
        Instantiate(ParticleFire,transform.position,transform.rotation);
        Instantiate(ParticleSmoke,transform.position,transform.rotation);
        
    }

    //void OnDestroy()
    //{
        
    //    //Instantiate(ParticleFire);
    //    //Instantiate(ParticleSmoke);
    //    //ParticleFire.transform.position = transform.position;
    //    //ParticleSmoke.transform.position = transform.position;
    //}
}
