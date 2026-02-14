// Each #kernel tells which function to compile; you can have many kernels
#include "Common.hlsl"

void Sand(int2 cellPos)
{
    float randomNum = RandomSeed(Frame, cellPos);
    if (CheckEmpty(int2(cellPos.x, cellPos.y - 1)))
    {
        return;
    }
    if (randomNum < 0.5)
    {
        if (TryMove(cellPos, int2(cellPos.x - 1, cellPos.y - 1)))
        {
            return;
        }
        TryMove(cellPos, int2(cellPos.x + 1, cellPos.y - 1));
    }
    else
    {
        if (TryMove(cellPos, int2(cellPos.x + 1, cellPos.y - 1)))
        {
            return;
        }
        TryMove(cellPos, int2(cellPos.x - 1, cellPos.y - 1));
    }
}