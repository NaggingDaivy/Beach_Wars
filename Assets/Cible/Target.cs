using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

//[RequireComponent(typeof(AudioSource))]
public class Target : MonoBehaviour
{
    public int ScoreValue = 10;
    public ParticleSystem ParticleFire;
    public ParticleSystem ParticleSmoke;
    public AudioSource Explosion;

    private ParticleSystem _ParticleFire;
    private ParticleSystem _ParticleSmoke;
    private bool _BeginToExplose = false;
   // private AudioSource _ExplosionAudioSource

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
        if(!_BeginToExplose)
            GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().UpdateScore(ScoreValue);
    }

    public void Explose(Vector3 soundLocation)
    {
        



        if (!_BeginToExplose) // ne se détruira qu'apres que tout a été détruit, donc obliger de controler
        {
            _BeginToExplose = true;
            //AudioSource.PlayClipAtPoint(Explosion.clip, soundLocation, 1000f);
            Explosion.Play();
            _ParticleFire = Instantiate(ParticleFire, transform.position, transform.rotation) as ParticleSystem;
            _ParticleSmoke = Instantiate(ParticleSmoke, transform.position, transform.rotation) as ParticleSystem;

            StartCoroutine("DestroyParticules");
        }
        
    }

    public IEnumerator DestroyParticules()
    {

        this.gameObject.GetComponent<Renderer>().enabled = false; // car si on detruit l'objet ici, on ne peut plus rien faire, les yield ne seront pas executés

        
        

        yield return new WaitForSeconds(_ParticleFire.duration);
        Destroy(_ParticleFire.gameObject);
        print("Destroy Fire");

        yield return new WaitForSeconds(_ParticleSmoke.duration);
        Destroy(_ParticleSmoke.gameObject);
        print("Destroy Smoke");

        yield return new WaitForSeconds(Explosion.clip.length);

        Destroy(this.gameObject);
        
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
