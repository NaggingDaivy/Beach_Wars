using UnityEngine;
using System.Collections;

public class SpaceShip : MonoBehaviour, IControllable
{



    public Vector3 DefaultSpeed;
    public Vector3 MaxSpeed;
    public GameObject LaserMesh;
    public float LaserShootDistance = 10f;
    public float LaserShootSpeed = 10f;

    private Vector3 m_Speed;

    //private Transform _laserProjection;

    private bool _inputEnabled = false;
    private bool _hasJustSwitched = false;



    // Use this for initialization
    void Start()
    {

        m_Speed = DefaultSpeed;
        //_laserProjection = new GameObject().transform;

    }

    // Update is called once per frame
    void Update()
    {



        if (_inputEnabled)
        {
            float value = Time.deltaTime;
            this.transform.position += (this.transform.rotation * m_Speed * Time.deltaTime);

            Quaternion RotationX = transform.rotation * Quaternion.Euler(Input.GetAxis("Vertical") * 40, 0, 0);

            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, RotationX, 0.05f);

            Quaternion RotationY = transform.rotation * Quaternion.Euler(0, Input.GetAxis("Horizontal") * 40, 0);

            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, RotationY, 0.05f);

            if (Input.GetKey(KeyCode.JoystickButton4)) // left bumper
            {
                Quaternion RotationZ = transform.rotation * Quaternion.Euler(0, 0, 40);

                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, RotationZ, 0.05f);

            }

            if (Input.GetKey(KeyCode.JoystickButton5)) // right bumper 
            {
                Quaternion RotationZ = transform.rotation * Quaternion.Euler(0, 0, -40);

                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, RotationZ, 0.05f);

            }

            if (Input.GetKeyDown(KeyCode.JoystickButton3) && !_hasJustSwitched)
            {
                DisableInput();

                GameObject.Find("Tractopelle").GetComponent<Tractopelle>().EnableInput();
            }
            else if (_hasJustSwitched)
                _hasJustSwitched = false;

            if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                StartCoroutine("Fire");
                //_fireLaser = true;
                //Vector3 laserProjection = this.transform.position + transform.rotation * new Vector3(0, 0, 30);

            }




        }
    }

    public void EnableInput()
    {
        _inputEnabled = true;
        _hasJustSwitched = true;

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollowPlayer>().Player = this.transform;
    }

    public void DisableInput()
    {
        _inputEnabled = false;
    }


    private IEnumerator Fire()
    {
        //GameObject LaserMesh = Instantiate(Resources.Load("LaserMesh",typeof(GameObject))) as GameObject;
        //UpdateLaser();
        Transform laserProjection = new GameObject().transform;

        laserProjection.transform.position = this.transform.position + this.transform.rotation * new Vector3(0, 0, LaserShootDistance);
        

        GameObject laserMesh = Instantiate(LaserMesh);

        laserMesh.transform.position = this.transform.position + transform.rotation * new Vector3(-7, 0, 0);
       

        laserMesh.transform.LookAt(laserProjection);

        laserMesh.transform.Rotate(-90, 0, 0);

        while (Vector3.Distance(laserMesh.transform.position, laserProjection.transform.position) > 0.005f)
        {
            laserMesh.transform.position = Vector3.Lerp(laserMesh.transform.position, laserProjection.transform.position, Time.deltaTime * LaserShootSpeed);
            
            yield return null;
        }

        Destroy(laserMesh.gameObject);
        Destroy(laserProjection.gameObject);

        //GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube2.transform.position = this.transform.position + transform.rotation * new Vector3(0, 0, 10);
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = this.transform.position + transform.rotation * new Vector3(-7, 0, 0);

        ////cube.transform.LookAt(laserProjection);
        //cube2.transform.LookAt(this.transform.position);
        //cube.transform.LookAt(cube2.transform.position);

        ////Vector3.Lerp(cube.transform.position, laserProjection, Time.deltaTime * 2.0f);

        //while (Vector3.Distance(cube.transform.position, cube2.transform.position) > 0.05f)
        //{
        //    cube.transform.position = Vector3.Lerp(cube.transform.position, cube2.transform.position, 0.05f);
        //    yield return null;
        //}



    }
}
