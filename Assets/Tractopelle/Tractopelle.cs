using UnityEngine;


public class Tractopelle : BasePlayer //,IControllable
{

    // Declarations
    float InputMove_h, InputMove_v, OffsetTrackLeft, OffsetTrackRight, TrackLenght, RotateMultiplyMove;


    // public vars to connect scene elements
    public GameObject MeshTractopelle; // main tank

    //public Material MaterialTrackLeft; // shaders tracks
    //public Material MaterialTrackRight;

    public GameObject MeshPelleLevel3;
    public GameObject MeshPelleLevel2;
    public GameObject MeshPelleLevel1;
    public GameObject Gyrophare;
    public AudioSource GyrophareSound;
    public ProceduralMaterial ProcMaterial;



    public float SpeedMoveFactor = 2f; // movements speed adjustement

    private int m_PelleLevel = 3;

    //private bool _inputEnabled = false;
    //private bool _hasJustSwitched = false;

    //private Animator TankAnim;
    //private AudioSource AudioBoum, AudioTurret, AudioWheel;
    //private float Audio_Turret_snd, AudioTrack;

    // public float SpeedTurretFactor = 8f; // turret speed adjustement


    // initialisation (one time)
    void Start()
    {
        DisableInput();
        // Textures offsets init
        OffsetTrackLeft = 0f;
        OffsetTrackRight = 0f;

        // Defaults values for tracks and movement
        TrackLenght = 2.873945f;
        RotateMultiplyMove = 25f;

        //TankAnim = MeshTractopelle.GetComponent<Animator>();

        //AudioBoum = MeshPelleLevel3.GetComponent<AudioSource>();

        //AudioTurret = MeshTurret_Y.GetComponent<AudioSource>();

        //AudioWheel = MeshTractopelle.GetComponent<AudioSource>();

        //AudioTurret.volume = 0;
        /*AudioTurret.Play();
    AudioWheel.volume = 0.1f;
    AudioWheel.pitch = 1.0f;
    AudioWheel.Play();*/

    }




    void Pelle()
    {
        //Keyboards inputs values for turret 

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (m_PelleLevel - 1 == 0)
                m_PelleLevel = 3;
            else
            {
                --m_PelleLevel;
            }

        }

        switch (m_PelleLevel)
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
        //   // Audio_Turret_snd = Time.time + 0.1f;
        //}


        //if (Input.GetKey("k"))
        //{
        //    MeshPelleLevel2.transform.Rotate(0, -1, 0);
        //    //Audio_Turret_snd = Time.time + 0.1f;
        //}


        //if (Input.GetKey("l"))
        //{
        //    MeshPelleLevel3.transform.Rotate(1, 0, 0);
        //    //Audio_Turret_snd = Time.time + 0.1f;
        //}


        //if (Input.GetKey("m"))
        //{
        //    MeshPelleLevel2.transform.Rotate(0, 1, 0);
        //    //Audio_Turret_snd = Time.time + 0.1f;
        //}


        //if (Audio_Turret_snd > Time.time)

        //    AudioTurret.volume = 1;

        //else
        //    AudioTurret.volume = AudioTurret.volume - 0.05f;
    }

    void TractopelleMove() // function to move tank
    {
        // Keyboards inputs values for movements
        InputMove_v = Input.GetAxis("Vertical") * Time.deltaTime;
        InputMove_h = Input.GetAxis("Horizontal") * Time.deltaTime;


        // compute speed of movements
        float SpeedMove_v = InputMove_v * SpeedMoveFactor * SpeedMoveFactor;
        float SpeedMove_h = InputMove_h * SpeedMoveFactor;
        float SpeedTurnMove = SpeedMove_h * RotateMultiplyMove;

        // movement of tank
        MeshTractopelle.transform.Translate(0, 0, SpeedMove_v);
        MeshTractopelle.transform.Rotate(0, SpeedTurnMove, 0);

        // sliding of textures tracks
        OffsetTrackLeft = OffsetTrackLeft + (SpeedMove_v + SpeedMove_h) / TrackLenght;
        OffsetTrackRight = OffsetTrackRight + (SpeedMove_v - SpeedMove_h) / TrackLenght;

        // apply sliding on shaders
        //MaterialTrackLeft.SetTextureOffset("_MainTex", new Vector2(OffsetTrackLeft, 0));
        //MaterialTrackRight.SetTextureOffset("_MainTex", new Vector2(OffsetTrackRight, 0));

        //Sound of wheel

        //if (InputMove_v != 0 || InputMove_h != 0)
        //    AudioTrack = AudioTrack + .1f;
        //else
        //    AudioTrack = AudioTrack - .05f;

        //if (AudioTrack < 0f)
        //    AudioTrack = 0f;
        //else if (AudioTrack > 1f)
        //    AudioTrack = 1f;

        //AudioWheel.volume = 0.05f + AudioTrack;
        //AudioWheel.pitch = 1.0f + AudioTrack;
    }

    void DirtLevel()
    {
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
        Gyrophare.transform.Rotate(0, 5, 0);
    }

    // main loop (each time)
    void Update()
    {
        if (_inputEnabled)
        {
           
            TractopelleMove();
            Pelle();
            DirtLevel();
            TurnGyrophare();
            CheckChangePlayer();

            if (!GyrophareSound.isPlaying)
                GyrophareSound.Play();



            //if (Input.GetKeyDown(KeyCode.JoystickButton3) && !_hasJustSwitched)
            //{
            //    DisableInput();
            //    GameObject.Find("SpaceShip").GetComponent<SpaceShip>().EnableInput();

            //}
            //else if (_hasJustSwitched)
            //{
            //    _hasJustSwitched = false;
            //}
        }
        else
        {
            GyrophareSound.Stop();
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

