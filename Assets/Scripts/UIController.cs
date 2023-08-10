using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject[] MainMenuUI;
    public List<GameObject> GamePlayUI;
    public List<Button> LevelSelectUI;

    public Button m_ButtonPrefab;

    public Text m_LevelNumText;
    public Font m_Font; //The main font to use

    public LevelController m_LC;

    public Vector3 ButtonLocalScale = new Vector3(2,2,2);
    public int GridRowCol = 5;
    public int OffsetX = 150;
    public int OffsetY = 100;
    public int GridSpacing = 50;

    enum ActiveMenu : byte
    {
        None,
        Main,
        GameChoosing,
        GameRunning,
        SelectingLevel,
    }
    ActiveMenu m_ActiveMenu;

    // Start is called before the first frame update
    void Start()
    {
        if (m_LC == null)
        {
            m_LC = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
        }

        int levelLen = m_LC.m_Levels.Length / 2;
        for (int i = 0; i < m_LC.m_Levels.Length; i++)
        {
            Button button = Instantiate<Button>(m_ButtonPrefab) as Button;
            LevelSelectScript lss = button.GetComponent<LevelSelectScript>();
            RectTransform rect = button.GetComponent<RectTransform>();

            lss.ThisButtonsLevel = i;
            lss.m_LC = m_LC;
            lss.m_UI = this;

            button.transform.SetParent(this.gameObject.transform);

            rect.anchoredPosition3D = new Vector3( 
                ((i % GridRowCol) *  GridSpacing) - OffsetX , 
                ((i / GridRowCol) * -GridSpacing) + OffsetY ,
                0);

            rect.localScale = ButtonLocalScale;
            rect.localRotation = Quaternion.Euler(0, 0, 0);

            button.GetComponentInChildren<Text>().text = (i + 1).ToString();
            button.onClick.AddListener(lss.SelectLevel);

            LevelSelectUI.Add(button);
            LevelSelectUI[i].gameObject.SetActive(false);
        }

        Text[] texts = GetComponentsInChildren<Text>(true);

        foreach (Text t in texts)
        {
            t.font = m_Font;
        }

        m_ActiveMenu = ActiveMenu.None;
        ActivateMainMenu();

    }

    public void LevelSelecting()
    {
        if (m_ActiveMenu == ActiveMenu.SelectingLevel) return;

        m_ActiveMenu = ActiveMenu.SelectingLevel;

        foreach (GameObject m in MainMenuUI)
        {
            m.SetActive(false);
        }

        foreach(GameObject homeButton in GamePlayUI)
        {
            homeButton.SetActive(true);
        }

        //Loop through the max levels saved
        for (int i = 0; i < m_LC.m_Data.SavedLevel ; i++)
        {
            LevelSelectUI[i].gameObject.SetActive(true);
        }
        //Make the most recent level available 
        if (m_LC.m_Data.SavedLevel < m_LC.m_Levels.Length)
            LevelSelectUI[m_LC.m_Data.SavedLevel].gameObject.SetActive(true);
    }

    public void GameActivateGamePlay()
    {
        if (m_ActiveMenu == ActiveMenu.GameChoosing) return;

        m_ActiveMenu = ActiveMenu.GameChoosing;

        foreach (GameObject g in GamePlayUI)
        {
            g.SetActive(true);
        }
    }

    public void GameDeactivateGamePlay()
    {
        if (m_ActiveMenu == ActiveMenu.GameRunning) return;

        m_ActiveMenu = ActiveMenu.GameRunning;

        foreach (GameObject g in GamePlayUI)
        {
            g.SetActive(false);
        }
    }

    public void ClearLevelSelect()
    {
        foreach (Button b in LevelSelectUI)
        {
            b.gameObject.SetActive(false);
        }
    }

    public void ActivateGamePlay()
    {
        if (m_ActiveMenu == ActiveMenu.GameChoosing) return;

        m_ActiveMenu = ActiveMenu.GameChoosing;

        foreach (GameObject m in MainMenuUI)
        {
            m.SetActive(false);
        }

        foreach (GameObject g in GamePlayUI)
        {
            g.SetActive(true);
        }

        foreach (Button b in LevelSelectUI)
        {
            b.gameObject.SetActive(false);
        }
    }

    public void ActivateMainMenu()
    {
        if (m_ActiveMenu == ActiveMenu.Main) return; //Return if main is active

        m_ActiveMenu = ActiveMenu.Main;

        m_LevelNumText.text = "";

        foreach (GameObject m in MainMenuUI)
        {
            m.SetActive(true);
        }

        foreach (GameObject g in GamePlayUI)
        {
            g.SetActive(false);
        }

        foreach (Button b in LevelSelectUI)
        {
            b.gameObject.SetActive(false);
        }
    }

    public void ReturnToMainMenu()
    {
        ActivateMainMenu();
        m_LC.ClearOldLevel();
        m_LC.player.gameObject.SetActive(false);
        m_LC.m_MainWater.SetActive(false);
        m_LC.m_Island.SetActive(false);
    }

}
