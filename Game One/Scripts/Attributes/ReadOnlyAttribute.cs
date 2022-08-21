using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make the related property be visible on the Editor but not interactive
/// </summary>
public class ReadOnlyAttribute : PropertyAttribute
{
    public string tooltip;
    public ReadOnlyAttribute (string tooltip = "")
    {
        this.tooltip = tooltip;
    }
}