using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct AssignMovementData : IJob
{
    public float3 CenterPoint;
    public NativeArray<MovementData> MovementDatas;
    public NativeArray<Entity> Entities;
    public EntityManager EntityManager;
    public NativeArray<Translation> Translations;


    public void Execute()
    {
        for (int i = 0; i < MovementDatas.Length; i++)
        {
            //getting copy of struct
            MovementData movdata = MovementDatas[i];

            //assigning values in struct
            movdata.StartPosition = Translations[i].Value;
            movdata.Direction = math.normalize(Translations[i].Value - CenterPoint);

            //assigning original struct to new one
            MovementDatas[i] = movdata;
        }

        for (int i = 0; i < MovementDatas.Length; i++)
        {
            EntityManager.SetComponentData(Entities[i], MovementDatas[i]);
        }

        MovementDatas.Dispose();
        Translations.Dispose();
    }
}