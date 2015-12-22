using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour
{

    public Transform Player;
    public Vector3 DistanceCameraFromPlayer;

    public float CameraOffset;

    // Use this for initialization
    void Start()
    {
        Player.GetComponent<IControllable>().EnableInput();

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 destinationPosition = Player.position + (Player.rotation*DistanceCameraFromPlayer);


        this.transform.position = Vector3.Lerp(this.transform.position, destinationPosition, CameraOffset*Time.deltaTime);


        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Player.rotation, CameraOffset*Time.deltaTime);


    }
}
