using System;
using System.Threading;
using Mono.Cecil;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UpdateLoop : MonoBehaviour
{
    public ComputeShader CompShader;
    RawImage pixeldisplay;
    RenderTexture Temp;
    public int Width;
    public int Height;
    int[] array;

    struct cell
    {
        public bool selected;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setDisplayPortSize();

        array = new int[Width * Height];
        ComputeBuffer databuffer = new(array.Length, sizeof(int));
        array = SetValueAtPostion(array, 2, 1, 1);
        array = SetValueAtPostion(array, 4, 2, 1);
        array = SetValueAtPostion(array, 1, 1, 1);
        array = SetValueAtPostion(array, 6, 3, 2);
        databuffer.SetData(array);

        pixeldisplay = GetComponent<RawImage>();

        RectInt area = new(0, 0, Width, Height);

        Temp = new(Width, Height, 0, RenderTextureFormat.ARGB32);

        Temp.enableRandomWrite = true;
        Temp.filterMode = FilterMode.Point;

        int kernel = CompShader.FindKernel("CSMain");
        CompShader.SetTexture(kernel, "Result", Temp);
        CompShader.SetInt("Width", Width);
        CompShader.SetInt("Height", Height);
        CompShader.SetBuffer(kernel, "Data", databuffer);
        CompShader.Dispatch(kernel, (int)math.ceil(area.width / 8f), (int)math.ceil(area.height / 8f), 1);

        GraphicsFence fence = Graphics.CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, SynchronisationStageFlags.PixelProcessing);
        Graphics.WaitOnAsyncGraphicsFence(fence, SynchronisationStage.PixelProcessing);

        pixeldisplay.texture = Temp;
        databuffer.Release();


    }

    /*
    GraphicsFence fence = Graphics.CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, SynchronisationStageFlags.PixelProcessing);
    Graphics.WaitOnAsyncGraphicsFence(fence, SynchronisationStage.PixelProcessing);
    */

    // Update is called once per frame
    void Update()
    {


    }
    int[] SetValueAtPostion(int[] array, int x, int y, int value)
    {
        array[Width * y + x] = value;
        return array;

    }

    void setDisplayPortSize()
    {
        float WToHRatio = (float)Width / (float)Height; // X/Y
        RectTransform rectTransform = GetComponent<RectTransform>();
        float x = rectTransform.rect.width;
        float y = rectTransform.rect.height;
        if (Height >= Width)
        {
            rectTransform.sizeDelta = new Vector2(y * WToHRatio, y);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(x, x / WToHRatio);
            print(WToHRatio);
        }
    }
    void OnPoint(InputAction.CallbackContext context)
    {
        print(context.ReadValue<Vector2>());
    }
}
