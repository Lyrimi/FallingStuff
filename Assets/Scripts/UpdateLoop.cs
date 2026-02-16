using System;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
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
    public ComputeShader RenderShader;
    RawImage pixeldisplay;
    RenderTexture Temp;
    int width;
    int height;
    int kernel;
    int renderKernel;
    int curentFrame = 0;
    ComputeBuffer gameData;
    ComputeBuffer bufferGameData;
    ComputeBuffer checkbuffer;
    ComputeBuffer selectedbuffer;
    int[] checkArray;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        sharedVariables = GetComponent<SharedVariables>();
        pixeldisplay = GetComponent<RawImage>();
        Init();

    }

    public void Init()
    {
        RelaseBuffers();
        width = sharedVariables.Width;
        height = sharedVariables.Height;

        checkArray = new int[sharedVariables.GameArray.Length];

        Temp = new(width, height, 0, RenderTextureFormat.ARGB32);

        Temp.enableRandomWrite = true;
        Temp.filterMode = FilterMode.Point;
        kernel = CompShader.FindKernel("CSMain");
        renderKernel = RenderShader.FindKernel("CSMain");

        //CompShader.SetTexture(kernel, "Result", Temp);
        RenderShader.SetTexture(kernel, "Result", Temp);
        RenderShader.SetInt("Width", width);
        RenderShader.SetInt("Height", height);

        CompShader.SetInt("Width", width);
        CompShader.SetInt("Height", height);






        gameData = new(sharedVariables.GameArray.Length, UnsafeUtility.SizeOf<Elements.Cell>());
        bufferGameData = new(sharedVariables.GameArray.Length, UnsafeUtility.SizeOf<Elements.Cell>());
        checkbuffer = new(sharedVariables.GameArray.Length, sizeof(int));
        selectedbuffer = new(sharedVariables.GameArray.Length, sizeof(int));
    }

    /*
    GraphicsFence fence = Graphics.CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, SynchronisationStageFlags.PixelProcessing);
    Graphics.WaitOnAsyncGraphicsFence(fence, SynchronisationStage.PixelProcessing);
    */

    // Update is called once per frame
    void Update()
    {
        curentFrame += 1;
        checkbuffer.SetData(checkArray);
        gameData.SetData(sharedVariables.GameArray);
        bufferGameData.SetData(sharedVariables.GameArray);
        selectedbuffer.SetData(sharedVariables.SelectedArray);

        RectInt area = new(0, 0, width, height);

        CompShader.SetInt("Frame", curentFrame);
        CompShader.SetBuffer(kernel, "GameArray", gameData);
        CompShader.SetBuffer(kernel, "BufferGameArray", bufferGameData);
        CompShader.SetBuffer(kernel, "Claims", checkbuffer);
        CompShader.SetInt("renderMode", sharedVariables.renderMode);

        //Pause check
        bool FrezeGame = false;
        if (sharedVariables.Paused)
        {
            if (sharedVariables.StepFrames <= 0)
            {
                FrezeGame = true;
            }
            else
            {
                sharedVariables.StepFrames--;
            }
        }


        if (FrezeGame == false)
        {
            CompShader.Dispatch(kernel, (int)math.ceil(area.width / 8f), (int)math.ceil(area.height / 8f), 1);
        }




        //Set RenderShaders variables
        RenderShader.SetBuffer(renderKernel, "GameArray", bufferGameData);
        RenderShader.SetBuffer(renderKernel, "Selected", selectedbuffer);
        RenderShader.SetInt("renderMode", sharedVariables.renderMode);

        RenderShader.Dispatch(renderKernel, (int)math.ceil(area.width / 8f), (int)math.ceil(area.height / 8f), 1);

        pixeldisplay.texture = Temp;

        //Retrive data from Physics shader
        bufferGameData.GetData(sharedVariables.GameArray);

    }
    int[] SetValueAtPostion(int[] array, int x, int y, int value)
    {
        array[width * y + x] = value;
        return array;

    }


    void OnDisable()
    {
        RelaseBuffers();
    }

    void RelaseBuffers()
    {
        if (gameData == null)
        {
            return;
        }
        gameData.Release();
        bufferGameData.Release();
        checkbuffer.Release();
        selectedbuffer.Release();
    }
}
