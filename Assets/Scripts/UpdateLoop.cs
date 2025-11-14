using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UpdateLoop : MonoBehaviour
{
    public ComputeShader CompShader;
    RawImage pixeldisplay;
    RenderTexture Temp;

    struct cell
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pixeldisplay = GetComponent<RawImage>();

        RectInt area = new(0, 0, 10800, 7200);

        Temp = new(area.width, area.height, 0, RenderTextureFormat.ARGB32);

        Temp.enableRandomWrite = true;
        Temp.filterMode = FilterMode.Point;
        int kernel = CompShader.FindKernel("CSMain");
        CompShader.SetTexture(kernel, "Result", Temp);
        CompShader.SetInt("Width", area.width);
        CompShader.SetInt("Height", area.height);
        CompShader.Dispatch(kernel, (int)math.ceil(area.width / 8), (int)math.ceil(area.height / 8), 1);

        GraphicsFence fence = Graphics.CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, SynchronisationStageFlags.PixelProcessing);
        Graphics.WaitOnAsyncGraphicsFence(fence, SynchronisationStage.PixelProcessing);

        pixeldisplay.texture = Temp;




    }

    // Update is called once per frame
    void Update()
    {


    }
}
