using System;
using UnityEngine;

public class SharedVariables : MonoBehaviour
{
    public struct Cell
    {
        public bool Selected;
        public int Element_id;
    }

    public int Width;
    public int Height;
    public Vector2 ViewPortSize;
    [NonSerialized] public int[] SelectedArray;
    [NonSerialized] public int[] ColorArray;
    [NonSerialized] public Cell[] GameArray;

    void Start()
    {
        //init variables
        GameArray = new Cell[Width * Height];
        SelectedArray = new int[Width * Height];
        ColorArray = new int[Width * Height];
    }

}