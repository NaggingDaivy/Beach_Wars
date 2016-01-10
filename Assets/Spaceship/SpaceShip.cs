using UnityEngine;
using System.Collections;

public class SpaceShip : BasePlayer //, IControllable
{


    public GameObject Cockpit;
    public Vector3 DefaultSpeed;
    public Vector3 MaxSpeed;
    public GameObject LaserMesh;
    public float LaserShootDistance = 10f;
    public float LaserShootSpeed = 10f;
    public Transform[] LasersPostions;
    public AudioSource LaserSound;
    public SpaceShipMode _SpaceshipMode = SpaceShipMode.Levitate;


    private Vector3 _Speed;
    private bool isCockpitOpened = false;
    private int _LaserShootCounter = 0;
    //private bool isCameraInFreeMode = false;

    //private Transform _laserProjection;



    // Use this for initialization
    void Start()
    {
        EnableInput();
        //GameObject.FindGameObjectWithTag("HUDSpaceShip").SetActive(true);
        _Speed = DefaultSpeed;


        //_laserProjection = new GameObject().transform;

    }

    // Update is called once per frame
    void Update()
    {



        if (_inputEnabled && _camera.GetComponent<CameraFollowPlayer>()._CameraMode != CameraMode.Free)
        {

            this.transform.position += (this.transform.rotation * _Speed * Time.deltaTime);



            //Quaternion RotationX =  transform.rotation * Quaternion.Euler(Input.GetAxis("Vertical") * 40, 0, 0);

            //Quaternion RotationY = transform.rotation * Quaternion.Euler(0, Input.GetAxis("Horizontal") * 40, 0);


            transform.Rotate(2 * Input.GetAxis("Vertical"), 0, 0, Space.Self);

            transform.Rotate(0, 2 * Input.GetAxis("Horizontal"), 0, Space.World);

            CheckChangePlayer();

            if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                StartCoroutine("Fire");
                //_fireLaser = true;
                //Vector3 laserProjection = this.transform.position + transform.rotation * new Vector3(0, 0, 30);

            }


            if (Input.GetKeyDown(KeyCode.JoystickButton3))
            {
                StartCoroutine("OpenCloseCockpit");
            }





            //if (Input.GetKey(KeyCode.JoystickButton4)) // left bumper
            //{
            //    //Quaternion RotationZ = transform.rotation * Quaternion.Euler(0, 0, 40);

            //    //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, RotationZ, 0.05f);

            //    transform.Rotate(0, 0, 2, Space.Self);

            //}

            //if (Input.GetKey(KeyCode.JoystickButton5)) // right bumper 
            //{
            //    //Quaternion RotationZ = transform.rotation * Quaternion.Euler(0, 0, -40);

            //    //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, RotationZ, 0.05f);
            //    transform.Rotate(0, 0, -2, Space.Self);

            //}







        }
        else if (_camera.GetComponent<CameraFollowPlayer>()._CameraMode == CameraMode.Free)
        {
            CheckChangeCamera();
        }
    }



    private IEnumerator Fire()
    {
        LaserSound.Play();
        bool hasHitTarget = false;

        if (_LaserShootCounter + 1 > 3)
            _LaserShootCounter = 0;
        else
        {
            ++_LaserShootCounter;
        }

        Transform laserProjection = new GameObject().transform;

        laserProjection.transform.position = this.transform.position + this.transform.rotation * new Vector3(0, 0, LaserShootDistance); // front projection


        Transform rayCastProjection = new GameObject().transform;
        rayCastProjection.position = LasersPostions[_LaserShootCounter].position;
        rayCastProjection.transform.LookAt(laserProjection);
        rayCastProjection.transform.Rotate(-90, 0, 0);



        RaycastHit hit;

        float dist;

        if (Physics.Raycast(rayCastProjection.position, transform.forward, out hit, LaserShootDistance))
        {
            laserProjection.transform.position = this.transform.position + this.transform.rotation * new Vector3(0, 0, hit.distance); // reducing projection nto the hit
            dist = hit.distance;

            if (hit.collider.tag == "Cible")
            {
                print(hit.collider.name);
                hasHitTarget = true;
            }

        }
        else
        {
            // laserProjection.transform.position = this.transform.position + this.transform.rotation * new Vector3(0, 0, LaserShootDistance);
            dist = LaserShootDistance;
        }


        GameObject laserMesh = Instantiate(LaserMesh);


        laserMesh.transform.position = LasersPostions[_LaserShootCounter].position;


        laserMesh.transform.LookAt(laserProjection);

        laserMesh.transform.Rotate(-90, 0, 0);



        float elapsedTime = 0.0f;
        float time = dist / LaserShootSpeed; // T = D / V 

        Vector3 from = laserMesh.transform.position;

        while (elapsedTime < time)
        {
            laserMesh.transform.position = Vector3.Lerp(from, laserProjection.transform.position, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(laserMesh.gameObject);
        Destroy(laserProjection.gameObject);

        if (hasHitTarget)
        {
            Destroy(hit.collider.gameObject);
        }

        Destroy(rayCastProjection.gameObject);

    }

    private IEnumerator OpenCloseCockpit()
    {
        float time = 0.5f;


        float elapsedTime = 0.0f;
        if (!isCockpitOpened)
        {

            Quaternion from = Cockpit.transform.localRotation;
            Quaternion to = Quaternion.Euler(0, 0, 0);

            while (elapsedTime < time)
            {
                Cockpit.transform.localRotation = Quaternion.Lerp(from, to, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }


            isCockpitOpened = true;

        }
        else
        {


            Quaternion from = Cockpit.transform.localRotation;
            Quaternion to = Quaternion.Euler(91, 0, 0);

            while (elapsedTime < time)
            {
                Cockpit.transform.localRotation = Quaternion.Lerp(from, to, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;

            }



            isCockpitOpened = false;
        }
    }

}
