using UnityEngine;
using System.Collections;

public class BasePlayer : MonoBehaviour
{

    protected bool _inputEnabled = false;
    public Camera _camera;

    public bool isInputEnabled()
    {
        return _inputEnabled;
    }


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

            //GameObject.FindGameObjectWithTag("HUDSpaceShip").SetActive(true);
            GameObject.FindGameObjectWithTag("Spaceship").GetComponent<SpaceShip>().EnableInput();
            // Spaceship
        }
        else if (Input.GetAxis("D-Pad Y Axis") < 0 && Input.GetAxis("D-Pad Y Axis") >= -1) // Down
        {
            DisableInput();

            //GameObject.FindGameObjectWithTag("HUDTractopelle").SetActive(true);
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
        else if (Input.GetKey(KeyCode.JoystickButton6)) // Back
        {
            //DisableInput();
            _camera.GetComponent<CameraFollowPlayer>()._CameraMode = CameraMode.Free;

        }

    // _camera.GetComponent<CameraFollowPlayer>();

    }




}
