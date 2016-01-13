using UnityEngine;
using System.Collections;

public class Meca : BasePlayer
{

    private Animator _Animator;
    public AudioClip _CrackSound;
    public AudioClip WilhemScream;
    public AudioClip Breathing;

	// Use this for initialization
	void Start ()
	{
        DisableInput();
	    _Animator = GetComponent<Animator>();
        

	}
	
	// Update is called once per frame
	void Update () {

        if (_inputEnabled && _camera.GetComponent<CameraFollowPlayer>()._CameraMode != CameraMode.Free)
	    {
            if (Input.GetKeyUp(KeyCode.JoystickButton0)
            && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("jump_state"))
            {
                _Animator.SetTrigger("jump_trigger");
            }

            CheckChangePlayer();
	    }
        else if (_camera.GetComponent<CameraFollowPlayer>()._CameraMode == CameraMode.Free)
        {
            CheckChangeCamera();
        }
	    
	}

    public void CrackKnucklesAudio()
    {
       GetComponent<AudioSource>().clip = _CrackSound;
       GetComponent<AudioSource>().Play();
    }

    public void WilhemScreamAudio()
    {
       GetComponent<AudioSource>().clip = WilhemScream;
       GetComponent<AudioSource>().Play();
    }

    public void BreathingSound()
    {
        GetComponent<AudioSource>().clip = Breathing;
        GetComponent<AudioSource>().Play();
    }

}
