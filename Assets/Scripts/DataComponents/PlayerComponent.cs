using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerComponent : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject MissilePrefab;
    public GameObject Sheild;

    public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
    {
        entityManager.AddComponentData(entity, new Player
        {
            RotationSpeed = Constants.RotationSpeed,
            MoveSpeed = Constants.MoveSpeed,
            FireRate = Constants.FireRate,
            MissileSpeed = Constants.MissileSpeed,
            ShieldSpan = Constants.ShieldSpan,
            MissilePrefab = conversionSystem.GetPrimaryEntity(MissilePrefab),
            Shield = conversionSystem.GetPrimaryEntity(Sheild)
        });
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        if (MissilePrefab != null)
            referencedPrefabs.Add(MissilePrefab);
    }
}
