using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class TransformObjectToEntity : MonoBehaviour
{
    #region Visible Variables

    [SerializeField, Tooltip("Material that will be assign to the new entity")]
    private Material material;

    #endregion

    #region InvisibleVariable

    private EntityManager _entityManager;

    public List<Color> Colors { get; private set; }
    public Dictionary<Color, Material> Materials { get; private set; }

    public Material Material => material;

    private bool _alreadyConvertedEntities;

    #endregion

    void Start()
    {
        Materials = new Dictionary<Color, Material>();
        Colors = new List<Color>();
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    #region ConvertToEntity

    public void AddConvertToEntity(GameObject objectToTransform)
    {
        foreach (var meshRenderer in objectToTransform.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.gameObject.AddComponent<ConvertToEntity>();
        }

        StartCoroutine(DestroyGameObjectLeft(objectToTransform));
    }

    public void TransformGameObjectIntoEntity(GameObject objectToTransform)
    {
        if (_alreadyConvertedEntities)
            return;


        //create archetype
        EntityArchetype archetype = _entityManager.CreateArchetype(
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(WorldRenderBounds),
            typeof(ChunkWorldRenderBounds),
            typeof(LocalToWorld),
            typeof(ObjectConvertedToEntity));

        //convert all meshRenders into entities
        foreach (var meshRenderer in objectToTransform.GetComponentsInChildren<MeshRenderer>())
        {
            //create entity
            Entity entity = _entityManager.CreateEntity(archetype);

            //get info of the mesh renderer and assign it to new struct
            RenderMesh renderMesh = new RenderMesh();
            Color color = meshRenderer.material.color;

            //make a list of all the colors present is this gameObject
            Colors.Add(color);

            renderMesh.material = material;
            renderMesh.mesh = meshRenderer.GetComponent<MeshFilter>().mesh;

            LocalToWorld localToWorld = new LocalToWorld {Value = float4x4.Scale(1000, 1000, 1000)};

            //assign values to entity
            _entityManager.SetSharedComponentData(entity, renderMesh);
            _entityManager.SetComponentData(entity, localToWorld);
        }

        StartCoroutine(DestroyGameObjectLeft(objectToTransform));

        _alreadyConvertedEntities = true;
    }

    #endregion


    #region Tools

    private IEnumerator DestroyGameObjectLeft(GameObject gameObjectToDestroy)
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObjectToDestroy);
    }

    #endregion
}