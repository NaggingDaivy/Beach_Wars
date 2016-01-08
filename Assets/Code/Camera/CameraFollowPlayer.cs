using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour
{

    public BasePlayer _Player;
    //public Vector3 DistanceCameraFromPlayer;

    public CameraMode _CameraMode = CameraMode.Normal;

    private Transform DefaultCameraPosition;

    //public bool CanRotate = true;

    //public float CameraOffset;

    // Use this for initialization
    void Start()
    {
        //Player.GetComponent<IControllable>().EnableInput();

        

    }

    // Update is called once per frame
    void Update()
    {

        //Vector3 destinationPosition = Player.gameObject.transform.position + (Player.gameObject.transform.rotation*DistanceCameraFromPlayer);


        //this.transform.position = Vector3.Lerp(this.transform.position, destinationPosition, CameraOffset*Time.deltaTime);


        //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Player.gameObject.transform.rotation, CameraOffset*Time.deltaTime);

        //transform.RotateAround(_Player.transform.position,Vector3.up,2);

        switch (_CameraMode)
        {
                case CameraMode.Normal:
            {
                transform.RotateAround(_Player.transform.position, _Player.transform.up, Input.GetAxis("RightH") * 2);
                transform.RotateAround(_Player.transform.position, transform.right, Input.GetAxis("RightV") * 2);
                
                
                
               
                
                break;
            }
            case CameraMode.Fixed:
            {
                break;
            }

                case CameraMode.Free:
            {
                break;
            }
        }




    }
}
