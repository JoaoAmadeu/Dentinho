using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Settings with data of each difficulty on G1.
/// </summary>
[CreateAssetMenu (fileName = "NewG1Settings", menuName = "G1Settings", order = 0)]
public class G1Settings : ScriptableObject
{
    public Sprite splashScreen;
    public Color fillColor;
    public Color gradienteColor;
    public int spawnCount;
}