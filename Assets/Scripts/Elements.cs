using System.Collections;
using Unity.Mathematics;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public static class Elements
{
    public struct Cell
    {
        public int Element_id;
        public uint HasGravity;
        public float3 Color;
    }

    public static readonly Cell empty = new Cell
    {
        Element_id = 0,
        HasGravity = 0,
        Color = new float3(0, 0, 0)
    };
    public static readonly Cell sand = new Cell
    {
        Element_id = 1,
        HasGravity = 1,
        Color = new float3(255 / 255f, 202 / 255f, 66 / 255f)
    };

    public static readonly Cell wood = new Cell
    {
        Element_id = 2,
        HasGravity = 0,
        Color = new float3(79 / 255f, 45 / 255f, 13 / 255f)
    };

    public static readonly Cell water = new Cell
    {
        Element_id = 3,
        HasGravity = 1,
        Color = new float3(52 / 255f, 91 / 255f, 235 / 255f)
    };
}
