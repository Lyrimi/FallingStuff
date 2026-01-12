using UnityEngine;

public class SharedVariables : MonoBehaviour
{
    public int Width;
    public int Height;
    public Vector2 ViewPortSize;
    public int[] SelectedArray;
    public int[] Array;

    void Start()
    {
        //init variables
        SelectedArray = new int[Width * Height];
        Array = new int[Width * Height];
    }

}