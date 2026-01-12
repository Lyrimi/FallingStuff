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
    SharedVariables sharedVariables;
    public ComputeShader CompShader;
    RawImage pixeldisplay;
    RenderTexture Temp;
    int width;
    int height;
    int kernel;

    int[] array;

    struct cell
    {
        public bool selected;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        sharedVariables = GetComponent<SharedVariables>();
        width = sharedVariables.Width;
        height = sharedVariables.Height;

        array = sharedVariables.Array;

        Temp = new(width, height, 0, RenderTextureFormat.ARGB32);

        Temp.enableRandomWrite = true;
        Temp.filterMode = FilterMode.Point;
        kernel = CompShader.FindKernel("CSMain");
        CompShader.SetTexture(kernel, "Result", Temp);
        CompShader.SetInt("Width", width);
        CompShader.SetInt("Height", height);


    }

    /*
    GraphicsFence fence = Graphics.CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, SynchronisationStageFlags.PixelProcessing);
    Graphics.WaitOnAsyncGraphicsFence(fence, SynchronisationStage.PixelProcessing);
    */

    // Update is called once per frame
    void Update()
    {
        ComputeBuffer databuffer = new(array.Length, sizeof(int));
        ComputeBuffer selectedbuffer = new(array.Length, sizeof(int));

        databuffer.SetData(array);
        selectedbuffer.SetData(sharedVariables.SelectedArray);
        pixeldisplay = GetComponent<RawImage>();

        RectInt area = new(0, 0, width, height);
        //Selected
        CompShader.SetBuffer(kernel, "Data", databuffer);
        CompShader.SetBuffer(kernel, "Selected", selectedbuffer);
        CompShader.Dispatch(kernel, (int)math.ceil(area.width / 8f), (int)math.ceil(area.height / 8f), 1);

        GraphicsFence fence = Graphics.CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, SynchronisationStageFlags.PixelProcessing);
        Graphics.WaitOnAsyncGraphicsFence(fence, SynchronisationStage.PixelProcessing);

        pixeldisplay.texture = Temp;
        databuffer.Release();
        selectedbuffer.Release();

    }
    int[] SetValueAtPostion(int[] array, int x, int y, int value)
    {
        array[width * y + x] = value;
        return array;

    }


    void OnPoint()
    {

    }
}
