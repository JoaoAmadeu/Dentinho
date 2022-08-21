using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents a state of the game in a specific point, which contains gameplay.
/// </summary>
public abstract class GameState :  MonoBehaviour
{
    protected UnityAction endCallback;

    protected G1Settings settings;

    public virtual void StateStart (G1Settings settings, UnityAction endCallback = null) 
    {
        this.enabled = true;
        this.settings = settings;
        this.endCallback = endCallback;
    }

    public virtual void StateUpdate () { }

    public virtual void StateEnd () 
    {
        this.enabled = false;
        endCallback?.Invoke ();
    }
}
