using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// State that plays an animation.
/// </summary>
public class PlayAnimationState : GameState
{
    [SerializeField]
    [Tooltip ("The animator it is controlling.")]
    private Animator animator;

    [SerializeField]
    [Tooltip ("The animator parameter name.")]
    private string parameterName;

    [SerializeField]
    [Tooltip("The animator parameter type.")]
    private AnimatorControllerParameterType parameterType;

    public override void StateStart (G1Settings settings, UnityAction endCallback = null)
    {
        base.StateStart (settings, endCallback);
        StartCoroutine (StartRoutine ());
    }
    private IEnumerator StartRoutine ()
    {
        float animLenght = 0.0f;

        switch (parameterType)
        {
            case AnimatorControllerParameterType.Float:
                break;
            case AnimatorControllerParameterType.Int:
                break;
            case AnimatorControllerParameterType.Bool:
                break;

            case AnimatorControllerParameterType.Trigger:
                animator.SetTrigger (parameterName);
                break;

            default:
                break;
        }

        yield return null;

        if (animator.IsInTransition (0)) {
            animLenght = animator.GetNextAnimatorStateInfo (0).length;
        }
        else {
            animLenght = animator.GetCurrentAnimatorStateInfo (0).length;
        }

        yield return new WaitForSeconds (animLenght);

        StateEnd ();
    }
}
