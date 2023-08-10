using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LevelController m_LC;

    public LevelData m_ActiveLevel;

    public UIController m_UIControl;

    AudioSource m_AudioSource;
    public AudioClip m_Sinking;

    //public Camera mCam;
    public GameObject m_Ship;
    public GameObject m_Waves;

    public Vector3Int m_MoveToPos;
    public Vector3Int LevelMax = new Vector3Int(0,0,0);

    public int CurrentDirection = 4;
    public Vector3[] m_Rotations = new Vector3[5]
    {
        new Vector3(10,90,0), //Left
        new Vector3(10,-90,0), //Right
        new Vector3(10,180,0), //Up
        new Vector3(10,0,0), //Down
         
        new Vector3(90,0,0), //Died
    };

    public Vector2 m_StartTouch;
    private Vector2 m_EndTouch;

    public float m_DeathTimerMax; //Speed to let the player move from space to space
    float m_DeathTimer; //Current time that has elapsed since the player last moved

    public float m_PlayerSpeed;
    public float m_TurnSpeed;

    public float m_SlideDistance;

    bool bCanChooseDirection;
    bool bDidDie = false;
    bool bIsMoving = false;
    bool bIsDying = false;

    // Start is called before the first frame update
    void Start()
    {
        bCanChooseDirection = true;
        bDidDie = false;
        bIsMoving = false;
        bIsDying = false;
        //mCam = Camera.main;

        if (m_UIControl == null) m_UIControl = GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>();

        if (m_AudioSource == null) m_AudioSource = GetComponent<AudioSource>();

        m_StartTouch = m_EndTouch = new Vector3(0, 0, 0);

        m_Waves.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (!m_LC.bGameActive)
        {
            return;
        }

        if (bCanChooseDirection)
        {
            //Player can choose the direction
            m_UIControl.GameActivateGamePlay();
            m_Waves.SetActive(false);
            ChooseDirection();
        }
        else if (bDidDie)
        {
            m_LC.CreateLevel();
        }
        else if (!bIsMoving)
        {
            m_Waves.SetActive(true);
            //Check the new space we are at
            CheckSpace();
            //bCanChooseDirection = true;
        }

    }

    private void LateUpdate()
    {
        bIsMoving = false;
        //Move the player between the 2 positions
        if (this.transform.position != m_MoveToPos)
        {
            Vector3 nPos = Vector3.MoveTowards(this.transform.position, m_MoveToPos, m_PlayerSpeed * Time.deltaTime);
            this.transform.position = nPos;
            bIsMoving = true;
        }

        if (m_ActiveLevel != null)
        {
            //mCam.transform.position = transform.position + m_ActiveLevel.NewCameraPos;
        }

        if (m_Ship.transform.rotation != Quaternion.Euler(m_Rotations[CurrentDirection]))
        {

            if (!bIsDying) //Just turn the player
            {
                m_Ship.transform.rotation = Quaternion.Euler(m_Rotations[CurrentDirection]);
            }
            else //We died and let's play that rotation animation
            {
                m_Waves.SetActive(false);
                Vector3 nRot = Vector3.MoveTowards
                    (m_Ship.transform.rotation.eulerAngles,
                    m_Rotations[CurrentDirection],
                    m_TurnSpeed * Time.deltaTime);

                m_Ship.transform.rotation = Quaternion.Euler(nRot);

                m_DeathTimer += Time.deltaTime;

                if (m_Ship.transform.rotation == Quaternion.Euler(m_Rotations[CurrentDirection]) || m_DeathTimer > m_DeathTimerMax)
                {
                    bIsDying = false;
                    bDidDie = true;
                    m_DeathTimer = 0;
                }
            }
        }

    }

    void ChooseDirection()
    {

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveRight(-1);
            return;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight(1);
            return;
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp(1); 
            return;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveUp(-1);
            return;
        }

        if (Input.touchCount > 0)
        {

            if (Input.GetTouch(0).phase == TouchPhase.Began )
            {
                m_StartTouch = Input.GetTouch(0).position;
            }

            if(Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                m_EndTouch = Input.GetTouch(0).position;

                if (Mathf.Abs(Vector3.Distance(m_StartTouch, m_EndTouch)) > m_SlideDistance)
                {
                    TouchMovementCheckDistance();
                }
            }

            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                m_EndTouch = Input.GetTouch(0).position;

                TouchMovementCheckDistance();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_StartTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            //Hopefully simulate the same as touch
            m_EndTouch = Input.mousePosition;

            if (Mathf.Abs(Vector3.Distance(m_StartTouch, m_EndTouch)) > m_SlideDistance)
            {
                TouchMovementCheckDistance();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_EndTouch = Input.mousePosition;

            TouchMovementCheckDistance();
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameLose();
            bCanChooseDirection = false;
        }
#endif
    }

    void TouchMovementCheckDistance()
    {
        float tx = m_EndTouch.x - m_StartTouch.x;
        float ty = m_EndTouch.y - m_StartTouch.y;

        m_StartTouch = m_EndTouch; //Hopefully will avoid an issue with the player double moving, since we have started moving

        if (Mathf.Abs(tx) > Mathf.Abs(ty))
        {
            if (tx > m_SlideDistance)
            {
                MoveRight(1);
                return;
            }
            else if (tx < -m_SlideDistance)
            {
                MoveRight(-1);
                return;
            }
        }

        else if (Mathf.Abs(tx) < Mathf.Abs(ty))
        {
            if (ty > m_SlideDistance)
            {
                MoveUp(1);
                return;
            }
            else if (ty < -m_SlideDistance)
            {
                MoveUp(-1);
                return;
            }
        }
    }

    void CheckSpace()
    {
        BlockType type = GameplayStatics.GetTypeFromLevel(m_ActiveLevel, m_MoveToPos);

        switch (type)
        {
            case BlockType.Start:
            case BlockType.Idle:
                bCanChooseDirection = true;
                break;

            case BlockType.Left:
                MoveRight(-1);
                break;

            case BlockType.Right:
                MoveRight(1);
                break;

            case BlockType.Up:
                MoveUp(1);
                break;

            case BlockType.Down:
                MoveUp(-1);
                break;

            case BlockType.Win:
                GameWin();
                break;

            case BlockType.Lose:
                GameLose();
                break;
        }
    }

    public void MoveRight(int val)
    {
        m_UIControl.GameDeactivateGamePlay();
        bIsMoving = true;

        m_MoveToPos.x += val;

        m_Waves.SetActive(true);

        //Converts the value passed in into a direction we can use
        CurrentDirection = (int) ( (  (float)val * 0.5f ) + 0.5f ) ;

        m_MoveToPos.x = Mathf.Clamp(m_MoveToPos.x, 0, LevelMax.x);
        bCanChooseDirection = false;
    }

    public void MoveUp(int val)
    {
        m_UIControl.GameDeactivateGamePlay();
        bIsMoving = true;

        m_MoveToPos.z += val;

        m_Waves.SetActive(true);

        //Converts the value passed in into a direction we can use
        CurrentDirection = (int)(((float)val * -0.5f) + 2.5f);

        m_MoveToPos.z = Mathf.Clamp(m_MoveToPos.z , 0, LevelMax.z);
        bCanChooseDirection = false;
    }

    void GameWin()
    {
        m_LC.OnWin();
    }

    void GameLose()
    {
        float curY = m_Rotations[CurrentDirection].y;
        CurrentDirection = 4;
        m_Rotations[CurrentDirection].y = curY;
        bIsDying = true;
        m_AudioSource.PlayOneShot(m_Sinking);
    }

    public void LevelStart()
    {
        CurrentDirection = 3; //Start Facing down
        this.transform.rotation = Quaternion.Euler(m_Rotations[CurrentDirection]);

        LevelMax.x = m_ActiveLevel.Height - 1;
        LevelMax.z = m_ActiveLevel.Width - 1;

        m_DeathTimer = 0;

        bDidDie = false;
        bCanChooseDirection = true;
        bIsMoving = false;
    }

}
