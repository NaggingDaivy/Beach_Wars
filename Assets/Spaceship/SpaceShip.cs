using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SpaceShip : BasePlayer //, IControllable
{


    public GameObject Cockpit;
    public float DefaultSpeed = 30f;
    public float MinSpeed = 10f;
    public float MaxSpeed = 100f;
    public float _AutoPositionBrake = 0.2f;
    public float _AccelerationScale = 1.0f;
    public Image SpeedJaugeUI;
    public Text SpeedTextUI;


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

    public float ParticleEmissionMin = 5f;
    public float ParticleEmissionMax = 200f;
    public float ParticleEmissionDefault = 20f;
    public ParticleSystem[] ArrayParticleSystems;

    public Transform[] LasersPositions;
    public AudioSource LaserSound;
    public AudioSource EngineSound;
    public SpaceShipMode _SpaceshipMode = SpaceShipMode.Levitate;

    public Transform PointBeachRotate;


    //private Vector3 _Speed;
    private float _Speed = 0.0f;
    private float _ParticleEmissionValue = 0.0f;


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
        _Speed = DefaultSpeed;
        _ParticleEmissionValue = ParticleEmissionDefault;

    }

    // Update is called once per frame
    void Update()
    {



        if (_inputEnabled && _camera.GetComponent<CameraFollowPlayer>()._CameraMode != CameraMode.Free)
        {
            if (!EngineSound.isPlaying)
                EngineSound.Play();

            _Delta = Time.smoothDeltaTime;

            SpaceShipMove();

            CheckChangePlayer();

            if (Input.GetKeyDown(KeyCode.JoystickButton0))
                StartCoroutine("Fire");

            if (Input.GetKeyDown(KeyCode.JoystickButton3))
                StartCoroutine("OpenCloseCockpit");

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
            _camera.transform.parent = null;
            EngineSound.Stop();
            CheckChangeCamera();
            RotateAroundBeach(); // make spaceship tourn around the beach
        }
        else
        {
            EngineSound.Stop();
        }
    }

    private void RotateAroundBeach()
    {
        if (ReturnToBeach())
        {
            this.transform.LookAt(PointBeachRotate);
            this.transform.Rotate(0, -90, 0, Space.World);

            AutoPilot();
            this.transform.RotateAround(PointBeachRotate.position, PointBeachRotate.up, 0.1f);
        }

    }

    //return true if returned to beach, false if not the case
    private bool ReturnToBeach() 
    {
        this.transform.LookAt(PointBeachRotate);

        if (Vector3.Distance(transform.position, PointBeachRotate.position) > 200)
        {
            AutoPilot();
            return false;
        }

        return true;
    }

    private void Accelerate()
    {
        float acceleration = Input.GetAxis("RT");
        _Speed = Mathf.Min(_Speed + _Speed * acceleration * _AccelerationScale * _Delta, MaxSpeed);
        EngineSound.pitch = Mathf.Min(EngineSound.pitch + EngineSound.pitch * acceleration * _AccelerationScale * _Delta, 3.0f);

        foreach (ParticleSystem particleSystem in ArrayParticleSystems)
        {
            _ParticleEmissionValue = Mathf.Min(_ParticleEmissionValue + _ParticleEmissionValue * acceleration * _AccelerationScale * _Delta, ParticleEmissionMax);

            particleSystem.emissionRate = _ParticleEmissionValue;
        }

        this.transform.position += (this.transform.rotation * new Vector3(0, 0, _Speed) * Time.deltaTime);

        PrintHUDInfo();
    }

    private void Break()
    {
        float deceleration = Input.GetAxis("LT");
        _Speed = Mathf.Max(_Speed - _Speed * deceleration * _AccelerationScale * _Delta, MinSpeed);
        EngineSound.pitch = Mathf.Max(EngineSound.pitch - EngineSound.pitch * deceleration * _AccelerationScale * _Delta, 0.3f);

        foreach (ParticleSystem particleSystem in ArrayParticleSystems)
        {
            _ParticleEmissionValue = Mathf.Max(_ParticleEmissionValue - _ParticleEmissionValue * deceleration * _AccelerationScale * _Delta, ParticleEmissionMin);
            particleSystem.emissionRate = _ParticleEmissionValue;
        }
        this.transform.position += (this.transform.rotation * new Vector3(0, 0, _Speed) * Time.deltaTime);
        PrintHUDInfo();
    }

    private void AutoPilot()
    {

        _Speed = Mathf.Max(_Speed - _Speed * _AutoPositionBrake * _Delta, DefaultSpeed);
        EngineSound.pitch = Mathf.Max(EngineSound.pitch - EngineSound.pitch * _AutoPositionBrake * _Delta, 1.0f);

        foreach (ParticleSystem particleSystem in ArrayParticleSystems)
        {
            _ParticleEmissionValue = Mathf.Max(_ParticleEmissionValue - _ParticleEmissionValue * _AutoPositionBrake * _AccelerationScale * _Delta, ParticleEmissionDefault);
            particleSystem.emissionRate = _ParticleEmissionValue;
        }
        this.transform.position += (this.transform.rotation * new Vector3(0, 0, _Speed) * Time.deltaTime);

        PrintHUDInfo();

    }

    private void SpaceShipRotate()
    {
        transform.Rotate(2 * Input.GetAxis("Vertical"), 0, 0, Space.Self);

        transform.Rotate(0, 2 * Input.GetAxis("Horizontal"), 0, Space.World);
    }

    private void SpaceShipMove()
    {
        if (Input.GetAxis("RT") != 0)
        {
            Accelerate();
        }
        else if (Input.GetAxis("LT") != 0)
        {
            Break();
        }
        else
        {
            AutoPilot();
        }

        SpaceShipRotate();
        
    }

    private void PrintHUDInfo()
    {
        SpeedJaugeUI.fillAmount = _Speed / MaxSpeed;
        SpeedTextUI.text = String.Format("{0:0.00}", _Speed);
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
        rayCastProjection.position = LasersPositions[_LaserShootCounter].position;
        rayCastProjection.transform.LookAt(laserProjection);
        rayCastProjection.transform.Rotate(-90, 0, 0);



        RaycastHit hit;

        float dist;

        if (Physics.Raycast(rayCastProjection.position, transform.forward, out hit, LaserShootDistance))
        {
            laserProjection.transform.position = this.transform.position + this.transform.rotation * new Vector3(0, 0, hit.distance); // reducing projection nto the hit
            dist = hit.distance;

            if (hit.collider.tag == "Cible" && hit.collider != null)
            {
               
                hasHitTarget = true;
            }

        }
        else
        {
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
