using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// State that will show the current difficulty in an animation. Also plays the background music.
/// </summary>
public class G1IntroState : PlayAnimationState
{
    [SerializeField]
    [Tooltip("Image which have its sprite changed, accordingly to the difficulty.")]
    private Image introImage;

    [SerializeField]
    [Tooltip("Will be played on the start.")]
    private AudioSource audioSource;

    public override void StateStart (G1Settings settings, UnityAction endCallback = null)
    {
        base.StateStart (settings, endCallback);
        introImage.sprite = settings.splashScreen;
        audioSource.Play();
    }
}