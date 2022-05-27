using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PowerUpSpawnerComponent : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject ShieldPrefab;
    public GameObject LaserPrefab;
    public GameObject DoubleLaserPrefab;

    public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
    {
        entityManager.AddComponentData(entity, new PowerUpSpawner
        {
            ShieldPrefab = conversionSystem.GetPrimaryEntity(ShieldPrefab),
            LaserPrefab = conversionSystem.GetPrimaryEntity(LaserPrefab),
            DoubleLaserPrefab = conversionSystem.GetPrimaryEntity(DoubleLaserPrefab),
            Speed = Constants.PowerupSpeed,
            PathVariation = Constants.PowerUpPathVariation,
            SpawnPosX = Constants.SpawnPosX,
            SpawnPosY = Constants.SpawnPosY,
            SpanTime = Constants.SpanTime
        });

    }
    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        if (ShieldPrefab != null)
            referencedPrefabs.Add(ShieldPrefab);
        if (LaserPrefab != null)
            referencedPrefabs.Add(LaserPrefab);
        if (DoubleLaserPrefab != null)
            referencedPrefabs.Add(DoubleLaserPrefab);
    }
}