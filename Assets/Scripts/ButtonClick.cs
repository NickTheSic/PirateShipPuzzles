using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    LevelController LC;

    UIController UIC;

    public PlayerController pc;

    private void Start()
    {
        LC = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();

        UIC = GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>();
    }

    public void ButtonClicked()
    {
        //Play button was clicked
        pc.gameObject.SetActive(true);

        pc.m_StartTouch = Input.mousePosition ;

        if (LC != null)
        {
            LC.bGameActive = true;
            LC.CreateLevel();
        }
        if (UIC != null)
        {
            UIC.ActivateGamePlay();
        }
    }

#if (!UNITY_ANDROID)
    private void OnMouseDown()
    {
        ButtonClicked();
    }
#endif

}
