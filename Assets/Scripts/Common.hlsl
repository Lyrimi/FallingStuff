#pragma message("Including Common.compute")
#ifndef COMMON_COMPUTE_INCLUDED
#define COMMON_COMPUTE_INCLUDED

// Create a RenderTexture with enableRandomWrite flag and set it
// with cs.SetTexture

#ifndef CELL_TYPE_DEFINED
#define CELL_TYPE_DEFINED 1

struct Cell
{
    int Element_id;
    uint HasGravity;
    float3 Color;
};

#else
#error "Duplicate definition: 'Cell' was already defined before including Common.compute"
#endif

// Remember to asign each of these in the unity script
uint Width;
uint Height;
uint Frame;
RWStructuredBuffer<int> Claims;
RWStructuredBuffer<int> Selected;
RWStructuredBuffer<Cell> GameArray;
RWStructuredBuffer<Cell> BufferGameArray;

float RandomSeed(uint seed, int2 pos)
{
    uint h = seed;

    h ^= (uint)pos.x * 374761393u;
    h ^= (uint)pos.y * 668265263u;

    h = (h ^ (h >> 13)) * 1274126177u;
    h ^= (h >> 16);

    return (h & 0x00FFFFFFu) / 16777216.0; // [0,1)
}

Cell GetValue(int2 pos)
{
    return GameArray[pos.y * Width + pos.x];
}

void SetValue(int2 pos, Cell value)
{
    BufferGameArray[pos.y * Width + pos.x] = value;
}

bool IsClaimed(int2 pos)
{
    int index = Width * pos.y + pos.x;
    int outvalue;
    InterlockedCompareExchange(Claims[index], 0, 1, outvalue);
    if (outvalue == 0)
    {
        return false;
    }
    return true;
}

bool CheckEmpty(int2 pos)
{
    if (pos.y < 0 || pos.x < 0 || pos.y > Height - 1 || pos.x > Width - 1)
    {
        return false;
    }
    if (GetValue(pos).Element_id == 0)
    {
        return true;
    }
    return false;
}

bool TryMove(int2 currentPos, int2 Destiation)
{
    if (CheckEmpty(Destiation) && IsClaimed(Destiation) == false)
    {
        SetValue(Destiation, GetValue(currentPos));
        SetValue(currentPos, (Cell)0);
        return true;
    }
    return false;
}

#endif