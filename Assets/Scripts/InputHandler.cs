using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    InputAction pointAction;
    RectTransform rectTransform;

    ViewPort viewPort;
    Vector2 ScaledPoint;
    void Start()
    {
        pointAction = InputSystem.actions.FindAction("Point");
        rectTransform = GetComponent<RectTransform>();
        viewPort = GetComponent<ViewPort>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = pointAction.ReadValue<Vector2>();
        ScaledPoint = new(point.x / Screen.width, point.y / Screen.height);
        print($"{ScaledPoint} || {IsInGameWindow(ScaledPoint)}");
    }

    bool IsInGameWindow(Vector2 scaledPoint)
    {
        Vector2 gamesize = viewPort.ViewPortSize;
        Vector2 ScaleGameSize = new(gamesize.x / 1920, gamesize.y / 1080); //It scales with the resoultion the canvas uses
        print(ScaleGameSize);
        if (0 <= scaledPoint.x && scaledPoint.x <= ScaleGameSize.x && 1-ScaleGameSize.y <= scaledPoint.y && scaledPoint.y <= 1)
        {
            return true;
        }
        return false;
    }
}
