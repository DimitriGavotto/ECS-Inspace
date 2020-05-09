using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class AddComponentToEntity : MonoBehaviour
{
    private EntityManager _entityManager;
    [SerializeField] private Transform centrePoint;

    private void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void AddMovementData()
    {
        //get query of entity matching the right criteria to add MovementData on
        EntityQuery queryForAddingMovementData = _entityManager.CreateEntityQuery(
            ComponentType.ReadWrite<RenderMesh>(),
            ComponentType.ReadOnly<Translation>());

        NativeArray<Entity> entitiesToAddMovementData = queryForAddingMovementData.ToEntityArray(Allocator.TempJob);

        //add  MovementDate
        foreach (var t in entitiesToAddMovementData)
            _entityManager.AddComponent<MovementData>(t);

        // query for assigning the values in MovementData
        EntityQuery query = _entityManager.CreateEntityQuery(
            ComponentType.ReadWrite<MovementData>(),
            ComponentType.ReadOnly<Translation>());

        NativeArray<Entity> entities = query.ToEntityArray(Allocator.TempJob);

        //create job in charge of set MovementDataValue
        AssignMovementData job = new AssignMovementData()
        {
            CenterPoint = centrePoint.position,
            MovementDatas = query.ToComponentDataArray<MovementData>(Allocator.TempJob),
            Translations = query.ToComponentDataArray<Translation>(Allocator.TempJob),
            Entities = entities,
            EntityManager = _entityManager
        };
        job.Execute();
        query.Dispose();
        entities.Dispose();
        queryForAddingMovementData.Dispose();
        entitiesToAddMovementData.Dispose();
    }
}