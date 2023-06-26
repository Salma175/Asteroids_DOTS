using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerComponent : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    // Missile Objects Which Player Ship will be shooting
    // Type of missile to instantiate depends on power up consumed
    public GameObject DefaultMissilePrefab;
    public GameObject LaserMissilePrefab;
    public GameObject DoubleLaserMissilePrefab;

    //Protection Sheild of Player Ship - Enables When Power Up is Consumed
    public GameObject Sheild;

    public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
    {
        // All the player ship data
        entityManager.AddComponentData(entity, new Player
        {
            RotationSpeed = Constants.RotationSpeed,
            MoveSpeed = Constants.MoveSpeed,
            FireRate = Constants.FireRate.Default,
            MissileSpeed = Constants.MissileSpeed,
            PowerUpSpan = Constants.ShieldSpan,
            DefaultMissile = conversionSystem.GetPrimaryEntity(DefaultMissilePrefab),
            LaserMissile = conversionSystem.GetPrimaryEntity(LaserMissilePrefab),
            DoubleLaserMissile = conversionSystem.GetPrimaryEntity(DoubleLaserMissilePrefab),
            Shield = conversionSystem.GetPrimaryEntity(Sheild)
        });
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        if (DefaultMissilePrefab != null)
            referencedPrefabs.Add(DefaultMissilePrefab);
        if (LaserMissilePrefab != null)
            referencedPrefabs.Add(LaserMissilePrefab);
        if (DoubleLaserMissilePrefab != null)
            referencedPrefabs.Add(DoubleLaserMissilePrefab);
    }
}
