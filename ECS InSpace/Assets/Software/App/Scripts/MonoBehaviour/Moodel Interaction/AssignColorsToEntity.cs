using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

public class AssignColorsToEntity : MonoBehaviour
{
    EntityManager _entityManager;
    [SerializeField] private TransformObjectToEntity transformObjectToEntity;
    
    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

    }
    /// <summary>
    /// create materials based on the original object however make sure not to create two material with the same color
    /// to set up GPU instancing and to optimise performance 
    /// </summary>
    public void AssignColors()
    {
        List<Color> colors = transformObjectToEntity.Colors;
        Dictionary<Color,Material> materials = transformObjectToEntity.Materials;
        Material material = transformObjectToEntity.Material;
        
        EntityQuery queryForChangingRenderMesh = _entityManager.CreateEntityQuery(
            ComponentType.ReadWrite<RenderMesh>(),
            ComponentType.ReadOnly<ObjectConvertedToEntity>());

        NativeArray<Entity> entities = queryForChangingRenderMesh.ToEntityArray(Allocator.TempJob);


        for (int i = 0; i < entities.Length; i++)
        {
            RenderMesh renderMesh = _entityManager.GetSharedComponentData<RenderMesh>(entities[i]);

            RenderMesh newRenderMesh = new RenderMesh();

            newRenderMesh.mesh = renderMesh.mesh;

            //check if a material with this color already exist is so assign this material
            if (materials.ContainsKey(colors[i]))
            {
                if (CheckIfMaterialExist(colors[i],materials ,out var materialToAssign))
                {
                    newRenderMesh.material = materialToAssign;
                }
                else
                {
                    Debug.LogError($"this key: {colors[i]}, does not exist");
                }
            }

            //create a new material and add it to the dictionary of materials
            else
            {
                Material newMaterial = new Material(material);
                newMaterial.name = "material" + materials.Count;

                newMaterial.SetColor("_BaseColor", colors[i]);
                newRenderMesh.material = newMaterial;
                materials.Add(colors[i], newMaterial);
            }

            _entityManager.SetSharedComponentData(entities[i], newRenderMesh);
        }

        entities.Dispose();
        queryForChangingRenderMesh.Dispose();
    }

    #region Tools
 private bool CheckIfMaterialExist(Color colorValue, Dictionary<Color, Material> materials,
     out Material materialToAssign)
    {
        var exist = materials.TryGetValue(colorValue, out materialToAssign);

        if (exist) return true;

        Debug.LogError($"this key: {colorValue}, does not exist");

        return false;
    }
    

    #endregion

}
