// Each #kernel tells which function to compile; you can have many kernels
#include "Common.hlsl"

bool CheckDisplacement(int2 pos)
{
    Cell cell = GetValue(pos);
    int id = cell.Element_id;

    if (id == 1 && GetValue(int2(pos.x, pos.y - 1)).Element_id == 3) // Check if it's sand and theres water bellow
    {
        return true;
    }
    if (id == 3 && GetValue(int2(pos.x, pos.y + 1)).Element_id == 1)
    {
        return true;
    }
    return false;
}

void Swap(int2 FirstPos, int2 SecondPos)
{
    SetValue(FirstPos, GetValue(SecondPos));
    SetValue(SecondPos, GetValue(FirstPos));
}
void DoDisplacment(int2 pos)
{
    float randomNum = RandomSeed(Frame, pos);
    Cell cell = GetValue(pos);
    int id = cell.Element_id;
    if (id == 3)
    {
        return;
    }

    int negate = -1;
    // Random direction
    if (randomNum < 0.5)
    {
        negate = 1;
    }

    int2 WaterPos = int2(pos.x, pos.y - 1);
    if (TryMove(WaterPos, int2(WaterPos.x, WaterPos.y - 1)))
    {
        SetValue(WaterPos, GetValue(pos));
        SetValue(pos, (Cell)0);
    }
    else if (TryMove(WaterPos, int2(WaterPos.x - 1 * negate, WaterPos.y - 1)))
    {
        SetValue(WaterPos, GetValue(pos));
        SetValue(pos, (Cell)0);
    }
    else if (TryMove(WaterPos, int2(WaterPos.x + 1 * negate, WaterPos.y - 1)))
    {
        SetValue(WaterPos, GetValue(pos));
        SetValue(pos, (Cell)0);
    }
    else if (TryMove(WaterPos, int2(WaterPos.x - 1 * negate, WaterPos.y)))
    {
        SetValue(WaterPos, GetValue(pos));
        SetValue(pos, (Cell)0);
    }
    else if (TryMove(WaterPos, int2(WaterPos.x + 1 * negate, WaterPos.y)))
    {
        SetValue(WaterPos, GetValue(pos));
        SetValue(pos, (Cell)0);
    }
    else
    {
        Swap(pos, WaterPos);
    }
}
