using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioThemeController : MonoBehaviour
{

    //A Basic class for now to implement the main theme

    public AudioSource m_AS;
    public Text m_ButtonText;

    public AudioSource m_PAudio;

    public void ButtonClick()
    {
        if (m_AS.isPlaying)
        {
            MuteButton();
        }
        else
        {
            PlayButton();
        }
    }

    public void MuteButton()
    {
        m_AS.Stop();
        m_PAudio.volume = 0.0f;
        m_ButtonText.text = "Unmute";
    }

    void PlayButton()
    {
        m_AS.Play();
        m_PAudio.volume = 0.7f;
        m_ButtonText.text = "Mute";
    }

}
