using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scripted animation to show on the G1 Menu, which demonstrate the toothbrush movement.
/// </summary>
public class G1_MainMenu_BrushInstruction : MonoBehaviour
{
    [Title ("References")]
    [SerializeField]
    private RectTransform hand;

    [SerializeField]
    private RectTransform toothbrush;

    [SerializeField]
    private RectTransform arrow;

    [Title ("Parameters")]
    [SerializeField]
    private float handSpeed = 1.0f;

    [SerializeField]
    private float arrowSpeed = 1.0f;

    [SerializeField]
    private float arrowRadius = 20f;

    [SerializeField]
    private float handAngleOffset;

    [SerializeField]
    private RectTransform frothPrefab;

    [SerializeField]
    private int frothCount = 6;

    [SerializeField]
    private float frothSpeed = 1.0f;

    [SerializeField][ReadOnly]
    private float angle;

    private Image[] frothes = new Image[6];

    [SerializeField]
    private float[] frothAngles = new float[6];

    /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    // Methods
    /*////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    private void Start ()
    {
        for (int i = 0; i < frothCount; i++)
        {
            RectTransform froth = Instantiate(frothPrefab) as RectTransform;
            frothes[i] = froth.GetComponent<Image>();
            frothes[i].gameObject.SetActive(true);
            float _angle = (i * (360 / frothCount));
            frothAngles[i] = (90 / frothCount) * (frothCount-i);

            froth.SetParent (toothbrush.parent);
            froth.anchoredPosition = arrow.anchoredPosition + new Vector2(Mathf.Sin(_angle / arrowRadius) * arrowRadius,
                                                                        Mathf.Cos(_angle / arrowRadius) * arrowRadius);
        }
        toothbrush.SetAsLastSibling();
        hand.SetAsLastSibling();
    }

    // Update is called once per frame
    private void Update()
    {
        angle += Time.deltaTime;
        angle = angle > 360 ? angle - 360 : angle;
        float handAngle = (angle + handAngleOffset) * handSpeed;
        Vector2 position = arrow.anchoredPosition + new Vector2(Mathf.Cos(handAngle) * arrowRadius, Mathf.Sin(handAngle) * arrowRadius);

        arrow.rotation = Quaternion.Euler (0, 0, angle * arrowRadius * arrowSpeed);
        hand.anchoredPosition = position;
        toothbrush.anchoredPosition = position;

        for (int i = 0; i < frothes.Length; i++)
        {
            frothAngles [i] -= Time.deltaTime * frothSpeed;
            frothAngles[i] = frothAngles[i] < 0 ? 90 : frothAngles[i];
            float rad = frothAngles[i];
            rad *= Mathf.Deg2Rad;


            float cos = Mathf.Abs(Mathf.Cos(rad));
            frothes[i].color = new Color(1, 1, 1, cos);
        }
    }
}