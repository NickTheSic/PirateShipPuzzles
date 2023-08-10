using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectScript : MonoBehaviour
{
    public int ThisButtonsLevel;
    public LevelController m_LC;
    public UIController m_UI;

    public void SelectLevel()
    {
        if (m_LC != null)
        {
            m_LC.CurrentLevel = ThisButtonsLevel;
            m_LC.bGameActive = true;
            m_LC.CreateLevel();
        }
        if (m_UI != null)
        {
            m_UI.ClearLevelSelect();
            m_UI.ActivateGamePlay();
        }
    }
}
