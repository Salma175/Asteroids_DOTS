using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class LifeManagerComponent : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject lifePrefab;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new LifeManager()
        {
            LifePrefab = conversionSystem.GetPrimaryEntity(lifePrefab),
            NoOfLives = Constamts.Lives
        });
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(lifePrefab);
    }
}
