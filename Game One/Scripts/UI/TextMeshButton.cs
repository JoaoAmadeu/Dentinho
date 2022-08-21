using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Facilitates the creation of stylized texts on the canvas.
/// </summary>
public class TextMeshButton : MonoBehaviour
{
    [SerializeField]
    private int textSize = 36;

    [SerializeField]
    private string text;

    [SerializeField]
    private TMP_Text[] texts;


    /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    // Methods
    /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    private void OnValidate() {
        foreach (var item in texts)
        {
            item.fontSize = textSize;
            item.text = text;
        }

        RectTransform trans = transform.Find("Shadow") as RectTransform;
        float lerpValue = Mathf.Lerp(0, -7, textSize / 50f);
        trans.anchoredPosition = new Vector2(0, lerpValue);
    }
}