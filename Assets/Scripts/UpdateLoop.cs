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
    ComputeBuffer databuffer;
    ComputeBuffer checkbuffer;
    ComputeBuffer selectedbuffer;

    int[] array;
    int[] checkArray;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        sharedVariables = GetComponent<SharedVariables>();
        width = sharedVariables.Width;
        height = sharedVariables.Height;

        array = sharedVariables.ColorArray;
        checkArray = new int[array.Length];

        Temp = new(width, height, 0, RenderTextureFormat.ARGB32);

        Temp.enableRandomWrite = true;
        Temp.filterMode = FilterMode.Point;
        kernel = CompShader.FindKernel("CSMain");
        CompShader.SetTexture(kernel, "Result", Temp);
        CompShader.SetInt("Width", width);
        CompShader.SetInt("Height", height);

        databuffer = new(array.Length, sizeof(int));
        checkbuffer = new(array.Length, sizeof(int));
        selectedbuffer = new(array.Length, sizeof(int));


    }

    /*
    GraphicsFence fence = Graphics.CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, SynchronisationStageFlags.PixelProcessing);
    Graphics.WaitOnAsyncGraphicsFence(fence, SynchronisationStage.PixelProcessing);
    */

    // Update is called once per frame
    void Update()
    {
        checkbuffer.SetData(checkArray);
        databuffer.SetData(array);
        selectedbuffer.SetData(sharedVariables.SelectedArray);
        pixeldisplay = GetComponent<RawImage>();

        RectInt area = new(0, 0, width, height);
        //Selected
        CompShader.SetBuffer(kernel, "Data", databuffer);
        CompShader.SetBuffer(kernel, "Selected", selectedbuffer);
        CompShader.SetBuffer(kernel, "Claims", checkbuffer);
        CompShader.Dispatch(kernel, (int)math.ceil(area.width / 8f), (int)math.ceil(area.height / 8f), 1);

        //GraphicsFence fence = Graphics.CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, SynchronisationStageFlags.PixelProcessing);
        //Graphics.WaitOnAsyncGraphicsFence(fence, SynchronisationStage.PixelProcessing);

        pixeldisplay.texture = Temp;
        databuffer.GetData(array);
        sharedVariables.ColorArray = array;

    }
    int[] SetValueAtPostion(int[] array, int x, int y, int value)
    {
        array[width * y + x] = value;
        return array;

    }


    void OnDisable()
    {
        databuffer.Release();
        checkbuffer.Release();
        selectedbuffer.Release();
    }
}
