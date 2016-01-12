using UnityEngine;
using System.Collections;
//using UnityEditor.SceneManagement;

public class CameraFollowPlayer : MonoBehaviour
{

    public BasePlayer _Player;
    //public Vector3 DistanceCameraFromPlayer;

    public CameraMode _CameraMode; // = CameraMode.Normal;
    public Vector3 FrontCameraPosition;
    public Quaternion FrontCameraRotation;

    private Transform CameraInitial;

    //public bool CanRotate = true;

    //public float CameraOffset;

    // Use this for initialization
    void Start()
    {
        //Player.GetComponent<IControllable>().EnableInput();
        CameraInitial = new GameObject().transform;
        CameraInitial.localPosition = transform.localPosition;
        CameraInitial.localRotation = transform.localRotation;



    }

    public void ResetCameraPosition()
    {
        transform.localPosition = CameraInitial.localPosition;
        transform.localRotation = CameraInitial.localRotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        //Vector3 destinationPosition = Player.gameObject.transform.position + (Player.gameObject.transform.rotation*DistanceCameraFromPlayer);


        //this.transform.position = Vector3.Lerp(this.transform.position, destinationPosition, CameraOffset*Time.deltaTime);


        //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Player.gameObject.transform.rotation, CameraOffset*Time.deltaTime);

        //transform.RotateAround(_Player.transform.position,Vector3.up,2);


        if (_Player.isInputEnabled())
        {
            switch (_CameraMode)
            {
                case CameraMode.Normal:
                    {
                        if (Input.GetKey(KeyCode.JoystickButton9)) // right click right joystick
                        {
                            transform.localPosition = FrontCameraPosition;
                            transform.localRotation = FrontCameraRotation;
                        }
                        else if (Input.GetKeyUp(KeyCode.JoystickButton9))
                        {
                            transform.localPosition = CameraInitial.localPosition;
                            transform.localRotation = CameraInitial.localRotation;

                        }
                        else if (Input.GetAxis("RightH") != 0 || Input.GetAxis("RightV") != 0)
                        {

                            transform.RotateAround(_Player.transform.position, _Player.transform.up, Input.GetAxis("RightH") * 2);
                            transform.RotateAround(_Player.transform.position, transform.right, Input.GetAxis("RightV") * 2);
                        }
                        else
                        {
                            Transform from = transform;


                            transform.localPosition = Vector3.Lerp(from.localPosition, CameraInitial.localPosition, Time.deltaTime * 10.0f);

                            transform.localRotation = Quaternion.Lerp(from.localRotation,
                                CameraInitial.localRotation, Time.deltaTime * 10.0f);
                        }






                        break;
                    }
                case CameraMode.Fixed:
                    {
                        break;
                    }

                case CameraMode.Free:
                    {
                        transform.localPosition = transform.localPosition + transform.localRotation * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                        transform.Rotate(2 * Input.GetAxis("RightV"), 0, 0, Space.Self);

                        transform.Rotate(0, 2 * Input.GetAxis("RightH"), 0, Space.World);


                        break;
                    }

            }


        }




    }
}
