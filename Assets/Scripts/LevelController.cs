using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public LevelData[] m_Levels;
    public int CurrentLevel;

    public PlayerController player;
    public GameObject m_Island;

    public Camera m_Camera;

    public GameObject[] BlockPrefabs;

    public List<GameObject> m_ActiveBlocks;

    public SaveData m_Data;

    //Instead of each of the objects having a block under them to render
    //I will make just one and scale it
    public GameObject m_MainWater; 
    public Material WaterMaterial;

    public UIController m_UIControl;

    public bool bGameActive;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        CurrentLevel = m_Levels.Length - 1;
#else
        CurrentLevel = 0;
#endif

        m_Data = SaveLoadStatics.Load();

        if (m_Data.SavedLevel != m_Levels.Length)
            CurrentLevel = m_Data.SavedLevel;

        if (!m_Data.AudioOn)
        {
            AudioThemeController atc = GetComponent<AudioThemeController>();

            if (atc != null)
            {
                atc.MuteButton();
            }
        }

        player.m_LC = this;
        m_Camera = Camera.main;

        bGameActive = false;

        m_MainWater = GameObject.CreatePrimitive(PrimitiveType.Cube);
        m_MainWater.GetComponent<Renderer>().material = WaterMaterial;
        Destroy(m_MainWater.GetComponent<Collider>());
        SetWaterBlockDimenstions(new Vector3(1, 1, 1), new Vector3(0, 0, 0));

        //Deactivate them to start
        player.gameObject.SetActive(false);
        m_MainWater.SetActive(false);
        m_Island.SetActive(false);
    }

    public void OnWin()
    {
        CurrentLevel++;

        if (CurrentLevel > m_Data.SavedLevel)
        {
            m_Data.SavedLevel = CurrentLevel;
            SaveLoadStatics.Save(m_Data);
        }

        if (CurrentLevel >= m_Levels.Length)
        {
            CurrentLevel = 0;
            ClearOldLevel();
            bGameActive = false;

            //Hard coded reset
            player.m_MoveToPos = new Vector3Int(0, 0, 0);
            player.transform.position = player.m_MoveToPos;
            player.CurrentDirection = 3;

            m_Camera.transform.position = new Vector3(0, 6, -6);
            m_Camera.transform.rotation = Quaternion.Euler(35, 0, 0);

            SetWaterBlockDimenstions(new Vector3(1, 1, 1), new Vector3(0, 0, 0));

            player.gameObject.SetActive(false);
            m_MainWater.SetActive(false);
            m_Island.SetActive(false);

            m_UIControl.ActivateMainMenu();
        }
        else
        {
            CreateLevel();
        }
    }

    public void CreateLevel()
    {
        ClearOldLevel();

        player.gameObject.SetActive(true);
        player.m_StartTouch = Input.mousePosition;
        m_MainWater.SetActive(true);
        m_Island.SetActive(true);

        player.m_ActiveLevel = m_Levels[CurrentLevel];

        int rows = m_Levels[CurrentLevel].Width;
        int cols = m_Levels[CurrentLevel].Height;

        m_UIControl.m_LevelNumText.text = "Level: " + (CurrentLevel + 1).ToString();

        //Set the scale to be about the width and height
        //Set the position of this cube to about the center of the width and height
        SetWaterBlockDimenstions(
            new Vector3(cols, 1, rows),
            new Vector3((cols * 0.5f) - 0.5f, 0, (rows * 0.5f) - 0.5f));

        for (int r = rows - 1; r >= 0; r--)
        {
            for (int c = 0; c < cols; c++)
            {
                //Vector3Int Loc = new Vector3Int(r, 0, c);
                Vector3Int Loc = new Vector3Int(c, 0, r); 
                BlockType type = GameplayStatics.GetTypeFromLevel(m_Levels[CurrentLevel], Loc);

                if (type == BlockType.Error || type == BlockType.Idle)//Don't need to create an idle block, it will be an open space
                {
                    continue;
                }

                int blockIndex = -1; //An arbitrary default value that I should check for later
                Vector3 rot = new Vector3(0, 0, 0);

                if (type == BlockType.Start)
                {
                    player.m_MoveToPos = Loc;
                    player.gameObject.transform.position = Loc;
                    continue;
                }

                else if (type == BlockType.Win)
                {
                    m_Island.transform.position = Loc;
                    continue;
                }

                else if (type == BlockType.Left || type == BlockType.Down || type == BlockType.Right || type == BlockType.Up)
                {
                    blockIndex = 0;

                    //Setup the rotations
                    if (type == BlockType.Left) rot.y = 0;
                    if (type == BlockType.Right) rot.y = 180;
                    if (type == BlockType.Up) rot.y = 90;
                    if (type == BlockType.Down) rot.y = -90;
                }

                else if (type == BlockType.Lose)
                {
                    blockIndex = 1;
                }

                GameObject go = Instantiate(BlockPrefabs[blockIndex], Loc, Quaternion.Euler(rot));
                m_ActiveBlocks.Add(go);

            }
        }

        m_Camera.transform.position = new Vector3((cols * 0.5f) - 0.5f, m_Levels[CurrentLevel].NewCameraPos.y, m_Levels[CurrentLevel].NewCameraPos.z) ;
        m_Camera.transform.rotation.SetLookRotation(new Vector3((cols * 0.5f) - 0.5f, 0, (rows * 0.5f) - 0.5f), Vector3.up);

        player.LevelStart();
    }

    public void ClearOldLevel()
    {
        foreach (GameObject g in m_ActiveBlocks)
        {
            Destroy(g);
        }
        m_ActiveBlocks.Clear();
    }


    void SetWaterBlockDimenstions(Vector3 size, Vector3 pos)
    {
        m_MainWater.transform.localScale = size;
        m_MainWater.transform.position = pos;
    }

}
