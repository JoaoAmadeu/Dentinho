using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Scripted animation to show on the G1 Menu, which demonstrate the toothpaste moving.
/// </summary>
public class G1_MainMenu_MovementInstruction : MonoBehaviour
{
    [Title ("References")]
    [SerializeField]
    private Image hand;

    [SerializeField]
    private Image toothpaste;

    [SerializeField]
    private Image arrow;

    [SerializeField]
    private Sprite handTouchSprite;

    private Sprite handIdleSprite;

    [Title ("Parameters")]
    [SerializeField]
    private float animationDistance = 100f;

    [SerializeField]
    private float handSpeed = 0.1f;

    [SerializeField]
    private float animationInterval = 1.0f;


    /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    // Methods
    /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    private void Start ()
    {
        handIdleSprite = hand.sprite;
        arrow.enabled = false;
        StartCoroutine(Slide(false));
    }

    private IEnumerator Slide (bool down)
    {
        yield return new WaitForSeconds (animationInterval);
        hand.sprite = handTouchSprite;

        float travelledDistance = 0;
        Vector2 direction = down == true ? Vector2.up : Vector2.down;
        RectTransform arrowTrans = arrow.rectTransform;

        arrow.enabled = true;
        arrowTrans.rotation = Quaternion.Euler(0, 0, (down ? 0f : 180f));
        arrowTrans.anchoredPosition = toothpaste.rectTransform.anchoredPosition - new Vector2(0, down? -50 : -110);
        arrowTrans.pivot = new Vector2(0.5f, 0);
        arrowTrans.sizeDelta *= (Vector2.right);

        while (travelledDistance < animationDistance)
        {
            travelledDistance += handSpeed;
            hand.rectTransform.anchoredPosition += direction * handSpeed;
            toothpaste.rectTransform.anchoredPosition += direction * handSpeed;
            arrowTrans.sizeDelta += Vector2.up * handSpeed;

            yield return null;
        }

        arrowTrans.pivot = new Vector2(0.5f, 1);
        arrowTrans.anchoredPosition += new Vector2 (0, down? 1 : -1) * animationDistance;
        hand.sprite = handIdleSprite;

        while (arrowTrans.sizeDelta.y > 0) {
            arrowTrans.sizeDelta += Vector2.down * (handSpeed * 3);
            yield return null;
        }

        StartCoroutine(Slide(!down));
    }
}