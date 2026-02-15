#include "Common.hlsl"

void Water(int2 cellPos)
{
    if (CheckEmpty(int2(cellPos.x, cellPos.y - 1)))
    {
        return;
    }
    float randomNum = RandomSeed(Frame, cellPos);
    int negate = -1;
    // Random direction
    if (randomNum < 0.5)
    {
        negate = 1;
    }

    // move logic
    if (TryMove(cellPos, int2(cellPos.x - 1 * negate, cellPos.y - 1)))
    {
        return;
    }
    if (TryMove(cellPos, int2(cellPos.x + 1 * negate, cellPos.y - 1)))
    {
        return;
    }
    if (TryMove(cellPos, int2(cellPos.x - 1 * negate, cellPos.y)))
    {
        return;
    }
    TryMove(cellPos, int2(cellPos.x + 1 * negate, cellPos.y));
}
