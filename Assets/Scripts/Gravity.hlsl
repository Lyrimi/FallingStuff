// Each #kernel tells which function to compile; you can have many kernels
#include "Common.hlsl"

void Gravity(int2 cellPos)
{
    TryMove(cellPos, (cellPos.x, cellPos.y - 1));
}