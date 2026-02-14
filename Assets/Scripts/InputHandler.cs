using System;
using NUnit.Framework.Constraints;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InputHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    InputAction pointAction;
    InputAction clickAction;
    InputAction rClickAction;
    InputAction spaceAction;

    PausePlay pausePlay;
    SharedVariables variables;
    Vector2 ScaledPoint;
    Vector2Int arrayPoint;
    Vector2Int lastArrayPoint = new(1, 1);
    void Start()
    {
        pointAction = InputSystem.actions.FindAction("Point");
        spaceAction = InputSystem.actions.FindAction("PressSpace");
        clickAction = InputSystem.actions.FindAction("Click");
        rClickAction = InputSystem.actions.FindAction("RightClick");
        variables = GetComponent<SharedVariables>();
        pausePlay = FindFirstObjectByType<PausePlay>();
    }

    // Update is called once per frame
    int e0 = 0;
    void Update()
    {
        if (variables.CanPlayerInput == false)
        {
            return;
        }
        e0++;
        Vector2 point = pointAction.ReadValue<Vector2>();
        ScaledPoint = new(point.x / Screen.width, point.y / Screen.height);

        //something something scaledsize
        Vector2 gamesize = variables.ViewPortSize;
        Vector2 ScaleGameSize = new(gamesize.x / 1920, gamesize.y / 1080); //It scales with the resoultion the canvas uses


        Vector2 ScaleGamepoint = ScaledPoint / ScaleGameSize;
        ScaleGamepoint.y = 1 - ((1 / ScaleGameSize.y) - ScaleGamepoint.y);
        Vector2 arraySize = new(variables.Width, variables.Height);
        arrayPoint = new((int)math.floor(ScaleGamepoint.x * arraySize.x), (int)math.floor(ScaleGamepoint.y * arraySize.y)); // minues 1 for y because
        if (IsInGameWindow(ScaleGamepoint))
        {
            variables.SelectedArray = SetValueAtPostion(variables.SelectedArray, lastArrayPoint, 0);
            variables.SelectedArray = SetValueAtPostion(variables.SelectedArray, arrayPoint, 1);
            lastArrayPoint = arrayPoint;
        }

        if (clickAction.IsPressed() && IsInGameWindow(ScaleGamepoint))
        {
            Click(Elements.sand);
        }

        if (rClickAction.IsPressed() && IsInGameWindow(ScaleGamepoint))
        {
            Click(Elements.empty);
        }

        if (spaceAction.WasPressedThisFrame())
        {
            pausePlay.Click();
        }

        //print($"{ScaleGamepoint} | ${e0}");

    }

    bool IsInGameWindow(Vector2 ScaleGamepoint)
    {
        if (ScaleGamepoint.x >= 0 && ScaleGamepoint.x <= 1 && ScaleGamepoint.y >= 0 && ScaleGamepoint.y <= 1)
        {
            return true;
        }
        return false;
    }

    Elements.Cell[] SetValueAtPostion(Elements.Cell[] array, Vector2Int pos, Elements.Cell value)
    {
        array[variables.Width * pos.y + pos.x] = value;
        return array;

    }
    int[] SetValueAtPostion(int[] array, Vector2Int pos, int value)
    {
        array[variables.Width * pos.y + pos.x] = value;
        return array;

    }
    void Click(Elements.Cell cell)
    {
        int radius = variables.DrawRadius;
        int SquareLength = radius * 2 + 1;
        for (int x = 0; x < SquareLength; x++)
        {
            for (int y = 0; y < SquareLength; y++)
            {
                Vector2Int offset = new(x - radius, y - radius);
                Vector2Int CurrentGamePoint = arrayPoint + offset;
                if (CurrentGamePoint.y < 0 || CurrentGamePoint.x < 0 || CurrentGamePoint.y > variables.Height - 1 || CurrentGamePoint.x > variables.Width - 1)
                {
                    continue;
                }

                //Check if point is within circle
                if (offset.sqrMagnitude <= radius * radius)
                {
                    variables.GameArray = SetValueAtPostion(variables.GameArray, CurrentGamePoint, cell);
                }
            }
        }

        // variables.GameArray = SetValueAtPostion(variables.GameArray, arrayPoint.x, arrayPoint.y, Elements.sand);
    }
}
