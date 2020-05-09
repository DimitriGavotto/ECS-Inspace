using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

public class MoveMeshRenderer : JobComponentSystem
{
    private float _maxDistance = 500;

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float maxD = _maxDistance;
        float sliderValue = ReferencesToObjects.MoveSliderValue;
        
        JobHandle jobHandle = Entities.ForEach((ref Translation translation, in MovementData movementData) =>
        {
            translation.Value = math.lerp(movementData.StartPosition, movementData.Direction * maxD, sliderValue);
        }).Schedule(inputDeps);
        jobHandle.Complete();
        return jobHandle;
    }
}