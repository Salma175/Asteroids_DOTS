using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ExplosionSpawnerComponent : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject Prefab = null;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new ExplosionSpawner
        {
            Prefab = conversionSystem.GetPrimaryEntity(Prefab),
            ExplotionSpan = Constants.ExplotionSpan,
        });
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        if (Prefab != null)
            referencedPrefabs.Add(Prefab);
    }
}