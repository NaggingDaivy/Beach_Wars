using UnityEngine;
using System.Collections;

public class SpaceShip : BasePlayer //, IControllable
{


    public GameObject Cockpit;
    //public Vector3 DefaultSpeed;
    //public Vector3 MaxSpeed;
    public float DefaultSpeed = 30f;
    public float MinSpeed = 10f;
    public float MaxSpeed = 100f;
    public float _AutoPositionBrake = 0.2f;
    public float _AccelerationScale = 1.0f;

    //public float DiveAcceleration = 1.0f;
    //public float DiveSpring = 0.4f;
    //public float MaxDiveSpeed = 2.0f;
    //public float BankSpring = 0.4f;

    //public float RotAcceleration = 1.0f;
    //public float RotAutoBrake = 0.2f;
    //public float BankAmplification = 10.0f;
    //public float MaxRotSpeed = 2.0f;


    public GameObject LaserMesh;
    public float LaserShootDistance = 10f;
    public float LaserShootSpeed = 10f;
    public Transform[] LasersPositions;
    public AudioSource LaserSound;
    public SpaceShipMode _SpaceshipMode = SpaceShipMode.Levitate;


    //private Vector3 _Speed;
    private float _Speed = 0.0f;
   
    
    //private float _RotSpeed;
    //private float _DiveOrientation;
    //private float accumulatedRotation = 0.0f;
    //private float accumulatedDive = 0.0f;


    private bool isCockpitOpened = false;
    private int _LaserShootCounter = 0;
    private float _Delta;
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
            _Delta = Time.smoothDeltaTime;
            

            SpaceShipMove();

            //SpaceShipMoveTest();

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





            //if (Input+.GetKey(KeyCode.JoystickButton4)) // left bumper
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


    private void SpaceShipMove()
    {
        if (Input.GetAxis("RT") != 0)
        {
            float acceleration = Input.GetAxis("RT");
            _Speed = Mathf.Min(_Speed + _Speed * acceleration * _AccelerationScale * _Delta, MaxSpeed);
        }
        else if (Input.GetAxis("LT") != 0)
        {
            float deceleration = Input.GetAxis("LT");
            _Speed = Mathf.Max(_Speed - _Speed * deceleration * _AccelerationScale * _Delta, MinSpeed);
        }
        else
        {
            _Speed = Mathf.Max(_Speed - _Speed * _AutoPositionBrake * _Delta, DefaultSpeed);
        }


        this.transform.position += (this.transform.rotation * new Vector3(0,0,_Speed) * Time.deltaTime);



        //Quaternion RotationX =  transform.rotation * Quaternion.Euler(Input.GetAxis("Vertical") * 40, 0, 0);

        //Quaternion RotationY = transform.rotation * Quaternion.Euler(0, Input.GetAxis("Horizontal") * 40, 0);


        transform.Rotate(2 * Input.GetAxis("Vertical"), 0, 0, Space.Self);

        transform.Rotate(0, 2 * Input.GetAxis("Horizontal"), 0, Space.World);
        
    }

    //private void SpaceShipMoveTest()
    //{
    //    _Delta = Time.smoothDeltaTime;

    //    if (Input.GetAxis("RT") != 0)
    //    {
    //        float acceleration = Input.GetAxis("RT");
    //        _Speed = Mathf.Min(_Speed + _Speed * acceleration * _AccelerationScale * _Delta, (MaxSpeed));
    //    }
    //    else if (Input.GetAxis("LT") != 0)
    //    {
    //        float deceleration = Input.GetAxis("LT");
    //        _Speed = Mathf.Max(_Speed - _Speed * deceleration * _AccelerationScale * _Delta, DefaultSpeed);
    //    }
    //    else
    //    {
    //        _Speed = Mathf.Max(_Speed - _Speed * _AutoPositionBrake * _Delta, DefaultSpeed);
    //    }

    //    if (Input.GetAxis("Horizontal") < 0.0f)
    //        _RotSpeed = Mathf.Max(-MaxRotSpeed, _RotSpeed - RotAcceleration * _AccelerationScale * _Delta);
    //    else if (Input.GetAxis("Horizontal") > 0.0f)
    //        _RotSpeed = Mathf.Min(MaxRotSpeed, _RotSpeed + RotAcceleration * _AccelerationScale  * _Delta);
    //    else
    //        _RotSpeed = Mathf.Lerp(0.0f, _RotSpeed, 1.0f - BankSpring * _Delta);

    //    if (Input.GetAxis("Vertical") < 0.0f)
    //        _DiveOrientation = Mathf.Max(-MaxDiveSpeed, _DiveOrientation - DiveAcceleration * _AccelerationScale * _Delta);
    //    else
    //    {
    //        if (Input.GetAxis("Vertical") > 0.0f)
    //            _DiveOrientation = Mathf.Min(MaxDiveSpeed, _DiveOrientation + DiveAcceleration * _AccelerationScale * _Delta);
    //        else
    //            _DiveOrientation = Mathf.Lerp(0.0f, _DiveOrientation, 1.0f - DiveSpring * _Delta);

    //        float distanceToGround = Mathf.Clamp01((transform.position.y * 0.25f) - 1.0f);
    //        _DiveOrientation = Mathf.Lerp(0.0f, _DiveOrientation, distanceToGround);
    //    }



    //    accumulatedRotation += _RotSpeed * _Delta;
    //    accumulatedDive += _DiveOrientation * _Delta;

    //    transform.position += transform.forward * _Speed * _Delta;
    //    transform.rotation = Quaternion.Euler(_DiveOrientation, accumulatedRotation,
    //        -_RotSpeed * BankAmplification);



    //}
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
        rayCastProjection.position = LasersPositions[_LaserShootCounter].position;
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
                //print(hit.collider.name);
                hasHitTarget = true;
            }

        }
        else
        {
            // laserProjection.transform.position = this.transform.position + this.transform.rotation * new Vector3(0, 0, LaserShootDistance);
            dist = LaserShootDistance;
        }


        GameObject laserMesh = Instantiate(LaserMesh);


        laserMesh.transform.position = LasersPositions[_LaserShootCounter].position;


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
            

            if (hit.collider.gameObject != null)
            {
                hit.collider.gameObject.GetComponent<Target>().UpdateScore();
                hit.collider.gameObject.GetComponent<Target>().Explose(transform.position);
            }
            
           
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
