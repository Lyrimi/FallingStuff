using System;
using Unity.Mathematics;
using UnityEngine;


public class SharedVariables : MonoBehaviour
{
    public int Width;
    public int Height;
    public Vector2 ViewPortSize;
    public int DrawRadius;
    [NonSerialized] public bool CanPlayerInput = true;
    [NonSerialized] public int[] SelectedArray;
    [NonSerialized] public int[] ColorArray;
    [NonSerialized] public Elements.Cell[] GameArray;
    [NonSerialized] public Elements.Cell SelectedElement;


    [Header("In GameDebug tools")]
    public bool Paused;
    public int StepFrames;
    public int renderMode;

    public bool CountElements;
    public int lastCounted;

    void Start()
    {
        SelectedElement = Elements.water;
        InitVariables();

    }

    public void InitVariables()
    {
        GameArray = new Elements.Cell[Width * Height];
        SelectedArray = new int[Width * Height];
        ColorArray = new int[Width * Height];
    }

    void Update()
    {
        if (CountElements)
        {
            CountElements = false;
            lastCounted = 0;
            foreach (Elements.Cell cell in GameArray)
            {
                if (cell.Element_id == 1)
                {
                    lastCounted++;
                }
            }
        }
    }

}