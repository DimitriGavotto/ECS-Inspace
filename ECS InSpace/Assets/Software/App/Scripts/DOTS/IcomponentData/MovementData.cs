using Unity.Entities;
using Unity.Mathematics;

public struct MovementData : IComponentData
{
    public float3 StartPosition;
    public float3 Direction;
}