using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

public class CreateDuplicate : IJob
{
    public NativeArray<Entity> Entities;
    public float3 EntityPlacementPosition;

    public void Execute()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        EntityArchetype archetype = entityManager.CreateArchetype(
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(WorldRenderBounds),
            typeof(ChunkWorldRenderBounds),
            typeof(LocalToWorld),
            typeof(MovementData));
        
        foreach (var entity in Entities)
        {
            //get matrix and add new position
            LocalToWorld localToWorld= entityManager.GetComponentData<LocalToWorld>(entity);
            localToWorld.Value+= float4x4.Translate(EntityPlacementPosition);
            
            //copy render mesh information
            RenderMesh renderMesh = entityManager.GetSharedComponentData<RenderMesh>(entity);
            
            //create entity and assign values
            Entity newEntity= entityManager.CreateEntity(archetype);
            entityManager.SetSharedComponentData(newEntity,renderMesh);
            entityManager.SetComponentData(newEntity, localToWorld);
        }

        Entities.Dispose();
    }
}
