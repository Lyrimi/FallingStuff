using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InputHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    InputAction pointAction;
    InputAction clickAction;

    SharedVariables variables;
    Vector2 ScaledPoint;
    Vector2Int arrayPoint;
    Vector2Int lastArrayPoint = new(1, 1);
    void Start()
    {
        pointAction = InputSystem.actions.FindAction("Point");
        variables = GetComponent<SharedVariables>();
        clickAction = InputSystem.actions.FindAction("Click");
    }

    // Update is called once per frame
    int e0 = 0;
    void Update()
    {
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
        if (ScaleGamepoint.x >= 0 && ScaleGamepoint.x <= 1 && ScaleGamepoint.y >= 0 && ScaleGamepoint.y <= 1)
        {
            variables.SelectedArray = SetValueAtPostion(variables.SelectedArray, lastArrayPoint.x, lastArrayPoint.y, 0);
            variables.SelectedArray = SetValueAtPostion(variables.SelectedArray, arrayPoint.x, arrayPoint.y, 1);
            lastArrayPoint = arrayPoint;
        }

        if (clickAction.IsPressed())
        {
            Click();
        }

        //print($"{ScaleGamepoint} | ${e0}");

    }

    bool IsInGameWindow(Vector2 scaledPoint)
    {
        Vector2 gamesize = variables.ViewPortSize;
        Vector2 ScaleGameSize = new(gamesize.x / 1920, gamesize.y / 1080); //It scales with the resoultion the canvas uses
        print(ScaleGameSize);
        if (0 <= scaledPoint.x && scaledPoint.x <= ScaleGameSize.x && 1 - ScaleGameSize.y <= scaledPoint.y && scaledPoint.y <= 1)
        {
            return true;
        }
        return false;
    }

    int[] SetValueAtPostion(int[] array, int x, int y, int value)
    {
        array[variables.Width * y + x] = value;
        return array;

    }

    void Click()
    {
        variables.Array = SetValueAtPostion(variables.Array, arrayPoint.x, arrayPoint.y, 1);
    }
}
