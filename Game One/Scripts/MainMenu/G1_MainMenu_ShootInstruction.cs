using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scripted animation to show on the G1 Menu, which demonstrate the toothpaste shooting.
/// </summary>
public class G1_MainMenu_ShootInstruction : MonoBehaviour
{
    [Title ("References")]
    [SerializeField]
    private Image hand;

    [SerializeField]
    private Animator toothpaste;

    [SerializeField]
    private Sprite handTouchSprite;

    private Sprite handIdleSprite;

    [Title ("Parameters")]
    [SerializeField]
    private float animationInterval = 1.0f;
    
    void Start()
    {
        handIdleSprite = hand.sprite;
        StartCoroutine(Tap());
    }

    private IEnumerator Tap ()
    {
        yield return new WaitForSeconds(animationInterval);
        hand.sprite = handTouchSprite;
        toothpaste.SetTrigger("Play");

        yield return new WaitForSeconds(0.1f);
        hand.sprite = handIdleSprite;

        StartCoroutine(Tap());
    }
}