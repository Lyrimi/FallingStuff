using System;
using Unity.Mathematics;
using UnityEngine;


public class SharedVariables : MonoBehaviour
{


    public int Width;
    public int Height;
    public Vector2 ViewPortSize;
    [NonSerialized] public int[] SelectedArray;
    [NonSerialized] public int[] ColorArray;
    [NonSerialized] public Elements.Cell[] GameArray;

    void Start()
    {
        //init variables
        GameArray = new Elements.Cell[Width * Height];
        SelectedArray = new int[Width * Height];
        ColorArray = new int[Width * Height];
    }

}