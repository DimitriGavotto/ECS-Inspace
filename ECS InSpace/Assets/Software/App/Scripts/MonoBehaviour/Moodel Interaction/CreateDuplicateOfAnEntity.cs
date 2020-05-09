using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CreateDuplicateOfAnEntity : MonoBehaviour
{
    private EntityManager _entityManager;
    private float3 _whereToPlaceEntity ;
    [SerializeField] private float3  separationAmount;
     
    private void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

    }

    public void CreateDuplicateFunc()
    {
        //set the position of the entities
        _whereToPlaceEntity += separationAmount;
        
        EntityQuery query = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<ObjectConvertedToEntity>());
        
        //create job responsible for creating duplicate
        CreateDuplicate job=new CreateDuplicate()
        {
            Entities = query.ToEntityArray(Allocator.TempJob),
            EntityPlacementPosition = _whereToPlaceEntity 
            
        };
        job.Execute();
        query.Dispose();
    }
}
