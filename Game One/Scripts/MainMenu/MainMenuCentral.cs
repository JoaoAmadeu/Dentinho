using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central for the Main Menu. Has methods to be used in the buttons click events.
/// </summary>
public class MainMenuCentral : MonoBehaviour
{
    [Title ("Animation")]
    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private Animator introAnimator;

    [SerializeField]
    private float introMusicDelay = 3.5f;

    [SerializeField]
    private float returnMusicDelay = 0.1f;

    [SerializeField]
    private float musicFadeInTime = 2.0f;

    [Title ("Menus")]
    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject introMenu;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///                                             Methods
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // If the game just started, the music is faded based on the intro animation
    // Otherwise, the music is faded with no intro animation
    private void Start ()
    {
        introMenu.SetActive (true);
        mainMenu.SetActive (false);

        if (Time.realtimeSinceStartup < 20)
        {
            introAnimator.SetTrigger ("Company");
            StartCoroutine(FadeMusic (introMusicDelay));
        }
        else {
            introAnimator.SetTrigger ("Game");
            StartCoroutine (FadeMusic (returnMusicDelay));
        }
    }

    private IEnumerator FadeMusic (float delay)
    {
        yield return new WaitForSeconds (delay);
        musicSource.Play ();
        mainMenu.SetActive (true);

        float volume = 0.0f;
        while (volume < 1.0f)
        {
            volume += Time.deltaTime * (1 / musicFadeInTime);
            musicSource.volume = volume;
            yield return null;
        }
    }

    public void StartG1Easy ()
    {
        MainCentral.Instance.G1Difficulty = Difficulty.Easy;
        MainCentral.Instance.LoadLevel (SceneName.G1);
    }

    public void StartG1Medium ()
    {
        MainCentral.Instance.G1Difficulty = Difficulty.Medium;
        MainCentral.Instance.LoadLevel (SceneName.G1);
    }

    public void StartG1Hard ()
    {
        MainCentral.Instance.G1Difficulty = Difficulty.Hard;
        MainCentral.Instance.LoadLevel (SceneName.G1);
    }
}