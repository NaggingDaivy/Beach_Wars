using UnityEngine;
using System.Collections;

public class Meca : MonoBehaviour
{

    private Animator _Animator;

	// Use this for initialization
	void Start ()
	{

	    _Animator = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKeyUp(KeyCode.Space)
            && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("jump_state"))
	    {
	        _Animator.SetTrigger("jump_trigger");
	    }
	
	}
}
