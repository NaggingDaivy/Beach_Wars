using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//[RequireComponent(typeof(AudioSource))]
public class Target : MonoBehaviour
{
    public int ScoreValue = 10;
    public ParticleSystem ParticleFire;
    public ParticleSystem ParticleSmoke;
    public AudioClip Explosion;

    private ParticleSystem _ParticleFire;
    private ParticleSystem _ParticleSmoke;

    // Use this for initialization
    void Start()
    {
        

        //ParticleFire.enableEmission = false;
        //ParticleSmoke.enableEmission = false;


    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 2, 0);

       

    }

    public void UpdateScore()
    {
        GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().UpdateScore(ScoreValue);
    }

    public void Explose(Vector3 soundLocation)
    {
        AudioSource.PlayClipAtPoint(Explosion,soundLocation,1000f);

        //Explosion.Play();
        _ParticleFire = Instantiate(ParticleFire, transform.position, transform.rotation) as ParticleSystem;
        _ParticleSmoke = Instantiate(ParticleSmoke, transform.position, transform.rotation) as ParticleSystem;

        StartCoroutine("DestroyParticules");
    }

    public IEnumerator DestroyParticules()
    {
        
        Destroy(this.gameObject);
        yield return new WaitForSeconds(_ParticleFire.duration);
        Destroy(_ParticleFire.gameObject);
        print("Destroy Fire");

        yield return new WaitForSeconds(_ParticleSmoke.duration);
        Destroy(_ParticleSmoke.gameObject);
        print("Destroy Smoke");

        
        //yield return null;

    }

    //void OnDestroy()
    //{

    //    //Instantiate(ParticleFire);
    //    //Instantiate(ParticleSmoke);
    //    //ParticleFire.transform.position = transform.position;
    //    //ParticleSmoke.transform.position = transform.position;
    //}
}
