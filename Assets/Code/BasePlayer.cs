using UnityEngine;
using System.Collections;

public class BasePlayer : MonoBehaviour
{

    protected bool _inputEnabled = false;
    protected bool _hasJustSwitched = false;
    public Camera _camera;


    protected void EnableInput()
    {
        _inputEnabled = true;
        _camera.enabled = true;


    }

    protected void DisableInput()
    {
        _inputEnabled = false;
        _camera.enabled = false;
    }

    protected virtual void CheckChangePlayer()
    {
        

        if (Input.GetAxis("D-Pad Y Axis") > 0 && Input.GetAxis("D-Pad Y Axis") <= 1) //Up
        {
            DisableInput();
            GameObject.FindGameObjectWithTag("Spaceship").GetComponent<SpaceShip>().EnableInput();
            // Spaceship
        }
        else if (Input.GetAxis("D-Pad Y Axis") < 0 && Input.GetAxis("D-Pad Y Axis") >= -1) // Down
        {
            DisableInput();
            GameObject.FindGameObjectWithTag("Tractopelle").GetComponent<Tractopelle>().EnableInput();
            //tractopelle
        }

        else if (Input.GetAxis("D-Pad X Axis") < 0 && Input.GetAxis("D-Pad X Axis") >= -1) // left
        {
            DisableInput();
            GameObject.FindGameObjectWithTag("Meca").GetComponent<Meca>().EnableInput();
            //meca
        }

        else if (Input.GetAxis("D-Pad X Axis") > 0 && Input.GetAxis("D-Pad X Axis") <= 1) // right
        {
            DisableInput();
            GameObject.FindGameObjectWithTag("Creature").GetComponent<Creature>().EnableInput();
            //Creature

        }

    // _camera.GetComponent<CameraFollowPlayer>();

    }




}
