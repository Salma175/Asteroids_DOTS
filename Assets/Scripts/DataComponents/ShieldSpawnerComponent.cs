using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ShieldSpawnerComponent : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject Prefab;

    public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
    {
        entityManager.AddComponentData(entity, new ShieldSpawner
        {
            Prefab = conversionSystem.GetPrimaryEntity(Prefab),
            Speed = Constants.PowerupSpeed,
            PathVariation = Constants.PowerUpPathVariation,
            SpawnPosX = Constants.SpawnPosX,
            SpawnPosY = Constants.SpawnPosY,
            SpanTime = Constants.SpanTime
        });

    }
    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        if (Prefab != null)
            referencedPrefabs.Add(Prefab);
    }
}