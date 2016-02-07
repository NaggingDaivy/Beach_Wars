using UnityEngine;
using UnityEngine.UI;


public class Tractopelle : BasePlayer //,IControllable
{

    // Declarations
    float _inputMoveH,_inputMoveV,_offsetTrackLeft,_offsetTrackRight,_trackLenght,_rotateMultiplyMove;


    // public vars to connect scene elements
    public GameObject MeshTractopelle; // main tank

    //public Material MaterialTrackLeft; // shaders tracks
    //public Material MaterialTrackRight;

    public GameObject MeshPelleLevel3;
    public GameObject MeshPelleLevel2;
    public GameObject MeshPelleLevel1;
    public GameObject Gyrophare;
    public GameObject GyrophareSpotlight;
    public AudioSource GyrophareSound;
    public AudioSource AudioEngine;
    public ProceduralMaterial ProcMaterial;
    public Slider DirtLevelSlider;



    public float SpeedMoveFactor = 2f; // movements speed adjustement

    private int _pelleLevel = 3;

    //private bool _inputEnabled = false;
    //private bool _hasJustSwitched = false;

    //private Animator TankAnim;
    //private AudioSource AudioBoum, AudioTurret, AudioEngine;
    
    private float _audioTurretSnd;
    private float _audioTrack;
  

    // public float SpeedTurretFactor = 8f; // turret speed adjustement


    // initialisation (one time)
    void Start()
    {
        DisableInput();
        //GameObject.FindGameObjectWithTag("HUDTractopelle").SetActive(false);
        // Textures offsets init
        _offsetTrackLeft = 0f;
        _offsetTrackRight = 0f;

        // Defaults values for tracks and movement
        _trackLenght = 2.873945f;
        _rotateMultiplyMove = 25f;

        //TankAnim = MeshTractopelle.GetComponent<Animator>();

        //AudioBoum = MeshPelleLevel3.GetComponent<AudioSource>();

        //AudioTurret = MeshTurret_Y.GetComponent<AudioSource>();

        //AudioEngine = MeshTractopelle.GetComponent<AudioSource>();

        //AudioTurret.volume = 0;
        //AudioTurret.Play();
        //AudioEngine.volume = 0.1f;
        //AudioEngine.pitch = 1.0f;
        //AudioEngine.Play();

    }




    void Pelle()
    {
        //Keyboards inputs values for turret 

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (_pelleLevel - 1 == 0)
                _pelleLevel = 3;
            else
            {
                --_pelleLevel;
            }

        }

        switch (_pelleLevel)
        {
            case 1:
                MeshPelleLevel1.transform.Rotate(Input.GetAxis("RightV"), 0, 0);
                break;
            case 2:
                MeshPelleLevel2.transform.Rotate(Input.GetAxis("RightV"), 0, 0);
                break;
            case 3:
                MeshPelleLevel3.transform.Rotate(Input.GetAxis("RightV"), 0, 0);
                break;

        }









        //if (Input.GetKey("o"))
        //{
        //    MeshPelleLevel3.transform.Rotate(-1, 0, 0);
        //   // _audioTurretSnd = Time.time + 0.1f;
        //}


        //if (Input.GetKey("k"))
        //{
        //    MeshPelleLevel2.transform.Rotate(0, -1, 0);
        //    //_audioTurretSnd = Time.time + 0.1f;
        //}


        //if (Input.GetKey("l"))
        //{
        //    MeshPelleLevel3.transform.Rotate(1, 0, 0);
        //    //_audioTurretSnd = Time.time + 0.1f;
        //}


        //if (Input.GetKey("m"))
        //{
        //    MeshPelleLevel2.transform.Rotate(0, 1, 0);
        //    //_audioTurretSnd = Time.time + 0.1f;
        //}


        //if (_audioTurretSnd > Time.time)

        //    AudioTurret.volume = 1;

        //else
        //    AudioTurret.volume = AudioTurret.volume - 0.05f;
    }

    void TractopelleMove() // function to move tank
    {
        // Keyboards inputs values for movements
        _inputMoveV = Input.GetAxis("Vertical") * Time.deltaTime;
        _inputMoveH = Input.GetAxis("Horizontal") * Time.deltaTime;


        // compute speed of movements
        float SpeedMove_v = _inputMoveV * SpeedMoveFactor * SpeedMoveFactor;
        float SpeedMove_h = _inputMoveH * SpeedMoveFactor;
        float SpeedTurnMove = SpeedMove_h * _rotateMultiplyMove;

        // movement of tank
        MeshTractopelle.transform.Translate(0, 0, SpeedMove_v);
        MeshTractopelle.transform.Rotate(0, SpeedTurnMove, 0);

        // sliding of textures tracks
        _offsetTrackLeft = _offsetTrackLeft + (SpeedMove_v + SpeedMove_h) / _trackLenght;
        _offsetTrackRight = _offsetTrackRight + (SpeedMove_v - SpeedMove_h) / _trackLenght;

        // apply sliding on shaders
        //MaterialTrackLeft.SetTextureOffset("_MainTex", new Vector2(OffsetTrackLeft, 0));
        //MaterialTrackRight.SetTextureOffset("_MainTex", new Vector2(OffsetTrackRight, 0));

        //Sound of wheel

        if (_inputMoveV != 0 || _inputMoveH != 0)
            _audioTrack = _audioTrack + .1f;
        else
            _audioTrack = _audioTrack - .05f;

        if (_audioTrack < 0f)
            _audioTrack = 0f;
        else if (_audioTrack > 1f)
            _audioTrack = 1f;

        AudioEngine.volume = 0.5f + _audioTrack;
        AudioEngine.pitch = 1f + _audioTrack;
    }

    void DirtLevel()
    {
        DirtLevelSlider.value = ProcMaterial.GetProceduralFloat("dirt_level");
        
        if(Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            float actualValue = ProcMaterial.GetProceduralFloat("dirt_level");
            ProcMaterial.SetProceduralFloat("dirt_level", actualValue - 0.1f);
            ProcMaterial.RebuildTextures();
        }
        else if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            float actualValue = ProcMaterial.GetProceduralFloat("dirt_level");
            ProcMaterial.SetProceduralFloat("dirt_level", actualValue + 0.1f);
            ProcMaterial.RebuildTextures();
        }
    }

    void TurnGyrophare()
    {
        Gyrophare.transform.Rotate(0,5,0);
        GyrophareSpotlight.transform.Rotate(0, 5, 0);
    }

    // main loop (each time)
    void Update()
    {
        TurnGyrophare();
        if (_inputEnabled && _camera.GetComponent<CameraFollowPlayer>().CameraMode != CameraMode.Free)
        {
           
            TractopelleMove();
            Pelle();
            DirtLevel();
            CheckChangePlayer();

            if (!GyrophareSound.isPlaying)
                GyrophareSound.Play();

            if (!AudioEngine.isPlaying)
            {
                AudioEngine.volume = 0.1f;
                AudioEngine.pitch = 1.0f;
                AudioEngine.Play();
            }
           



        }
        else if(_camera.GetComponent<CameraFollowPlayer>().CameraMode == CameraMode.Free)
        {
            GyrophareSound.Stop();
            AudioEngine.Stop();
            CheckChangeCamera();

        }
        else
        {
            GyrophareSound.Stop();
            AudioEngine.Stop();
        }



    }

    //public void EnableInput()
    //{
    //    _inputEnabled = true;
    //    _hasJustSwitched = true;
    //    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollowPlayer>().Player = this.transform;

    //}

    //public void DisableInput()
    //{
    //    _inputEnabled = false;
    //}
}

