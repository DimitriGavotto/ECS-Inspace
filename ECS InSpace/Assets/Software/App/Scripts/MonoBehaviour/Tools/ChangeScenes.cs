using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
  
  public void LoadNextScene()
  {
    //delete all entities before changing scene
    var entityManager=World.DefaultGameObjectInjectionWorld.EntityManager;
    entityManager.DestroyEntity(entityManager.GetAllEntities());
    SceneManager.LoadScene("DOTUniqueMeshes");
  }
}
