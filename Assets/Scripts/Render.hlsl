// Each #kernel tells which function to compile; you can have many kernels
#include "Common.hlsl"
RWTexture2D<float4> Result;

bool IsSelected(int2 pos)
{
    return Selected[pos.y * Width + pos.x] == 1;
}

void Render(int2 cellPos)
{
    if (IsSelected(cellPos))
    {
        Result[cellPos] = float4(1.0, 1.0, 1.0, 1.0);
        return;
    }
    float3 color = GetValue(cellPos).Color;
    Result[cellPos] = float4(color.x, color.y, color.z, 1.0);
}
