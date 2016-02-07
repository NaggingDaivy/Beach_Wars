using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BasePlayer : MonoBehaviour
{

    protected bool _inputEnabled = false;
    public Camera _camera;
    private CameraMode _previousCameraMode;
    public GameObject HUDPlayer;



    public bool isInputEnabled()
    {
        return _inputEnabled;
    }


    protected void EnableInput()
    {
        _inputEnabled = true;
        _camera.enabled = true;
        _camera.GetComponent<AudioListener>().enabled = true;
        if(HUDPlayer != null)
            HUDPlayer.SetActive(true);


    }

    protected void DisableInput()
    {
        _inputEnabled = false;
        _camera.enabled = false;
        _camera.GetComponent<AudioListener>().enabled = false;
        if (HUDPlayer != null)
            HUDPlayer.SetActive(false);


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
        else if (Input.GetKeyDown(KeyCode.JoystickButton6)) // Back
        {
            //DisableInput();
            _previousCameraMode = _camera.GetComponent<CameraFollowPlayer>().CameraMode;
            _camera.GetComponent<CameraFollowPlayer>().CameraMode = CameraMode.Free;




        }

        // _camera.GetComponent<CameraFollowPlayer>();

    }

    protected void CheckChangeCamera()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            _camera.transform.parent = this.transform;
            _camera.GetComponent<CameraFollowPlayer>().ResetCameraPosition();
            _camera.GetComponent<CameraFollowPlayer>().CameraMode = _previousCameraMode;
        }
            


    }




}
