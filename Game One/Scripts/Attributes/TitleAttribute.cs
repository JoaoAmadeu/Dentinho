using UnityEngine;

/// <summary>
/// A header with better graphics
/// </summary>
public class TitleAttribute : PropertyAttribute
{
    public string name;

    public TitleAttribute (string name = "")
    {
        this.name = name;
    }
}
