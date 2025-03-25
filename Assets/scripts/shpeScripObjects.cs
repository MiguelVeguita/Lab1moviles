using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shape", menuName = "Game/Shape Data")]
public class ShapeDate : ScriptableObject 
{
     public string Nombre;
     public Sprite Sprite;
    [SerializeField] private int index;

    // Agrega propiedades públicas si necesitas acceder a los valores
    public Sprite SpriteForma => Sprite;
    public int Index => index;
}
