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
    InputAction scrollAction;
    float scrollValue;

    PausePlay pausePlay;
    SharedVariables variables;
    Vector2 ScaledPoint;
    Vector2Int arrayPoint;
    Vector2Int lastArrayPoint = new(1, 1);
    int lastDrawRadius = 0;
    void Start()
    {
        pointAction = InputSystem.actions.FindAction("Point");
        spaceAction = InputSystem.actions.FindAction("PressSpace");
        clickAction = InputSystem.actions.FindAction("Click");
        rClickAction = InputSystem.actions.FindAction("RightClick");
        scrollAction = InputSystem.actions.FindAction("ScrollWheel");
        variables = GetComponent<SharedVariables>();
        pausePlay = FindFirstObjectByType<PausePlay>();
    }

    // Update is called once per frame
    void Update()
    {
        if (variables.CanPlayerInput == false)
        {
            return;
        }

        scrollValue = scrollAction.ReadValue<Vector2>().y;
        if (scrollValue != 0)
        {
            Scroll();
        }

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
            drawAreaCircle(true, lastDrawRadius + 1);
            drawAreaCircle(false, variables.DrawRadius + 1);
            lastArrayPoint = arrayPoint;
            lastDrawRadius = variables.DrawRadius;
        }

        if (clickAction.IsPressed() && IsInGameWindow(ScaleGamepoint))
        {
            Click(variables.SelectedElement);
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
                if (offset.magnitude - 0.5f <= radius)
                {
                    variables.GameArray = SetValueAtPostion(variables.GameArray, CurrentGamePoint, cell);
                }
            }
        }

        // variables.GameArray = SetValueAtPostion(variables.GameArray, arrayPoint.x, arrayPoint.y, Elements.sand);
    }
    void Scroll()
    {
        if (scrollValue > 0.01)
        {
            variables.DrawRadius += 1;
        }
        if (scrollValue < -0.01)
        {
            variables.DrawRadius -= 1;
            if (variables.DrawRadius < 0)
            {
                variables.DrawRadius = 0;
            }
        }
    }

    void drawAreaCircle(bool clear, int radius)
    {
        int x = 0; int y = radius;

        while (x < y)
        {
            float ymid = y + 0.5f;
            if ((ymid * ymid + x * x) > radius * radius)
            {
                y -= 1;
            }

            setSeleced8Way(new(x, y));
            x += 1;
        }
        setSelected(new(0, 0));

        void setSelected(Vector2Int RelativePos)
        {

            if (clear)
            {
                Vector2Int CurrentGamePoint = RelativePos + lastArrayPoint;
                if (CurrentGamePoint.y < 0 || CurrentGamePoint.x < 0 || CurrentGamePoint.y > variables.Height - 1 || CurrentGamePoint.x > variables.Width - 1)
                {
                    return;
                }
                variables.SelectedArray = SetValueAtPostion(variables.SelectedArray, lastArrayPoint + RelativePos, 0);
            }
            else
            {
                Vector2Int CurrentGamePoint = RelativePos + arrayPoint;
                if (CurrentGamePoint.y < 0 || CurrentGamePoint.x < 0 || CurrentGamePoint.y > variables.Height - 1 || CurrentGamePoint.x > variables.Width - 1)
                {
                    return;
                }
                variables.SelectedArray = SetValueAtPostion(variables.SelectedArray, arrayPoint + RelativePos, 1);
            }
        }

        void setSeleced8Way(Vector2Int pos)
        {
            setSelected(new(+pos.x, +pos.y));
            setSelected(new(+pos.x, -pos.y));
            setSelected(new(-pos.x, +pos.y));
            setSelected(new(-pos.x, -pos.y));

            setSelected(new(+pos.y, +pos.x));
            setSelected(new(+pos.y, -pos.x));
            setSelected(new(-pos.y, +pos.x));
            setSelected(new(-pos.y, -pos.x));
        }

    }
}
