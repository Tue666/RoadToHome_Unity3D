using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverSound : MonoBehaviour
{
    public AudioSource myFx;
    public AudioClip hoverFx;
    public AudioClip clickFx;
    public void Hover()
    {
        myFx.PlayOneShot(hoverFx);
    }

    public void ClickSound()
    {
        myFx.PlayOneShot(clickFx);
    }

    public void OutGame()
    {
        Application.Quit();
    }
}
