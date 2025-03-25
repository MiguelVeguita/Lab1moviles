using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "Game/Color Palette")]

public class ColorData : ScriptableObject
{
    [Header("Available Colors")]
    public Color colors;
    public int index;

}