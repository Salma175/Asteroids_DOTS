using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class AsteroidSpawnerComponent : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public Sprite[] Sprites;

    public GameObject Prefab = null;
    public float SpawnRate = 0.5f;
    public float MinSpeed = 0.5f;
    public float MaxSpeed = 3f;
    public float PathVariation = 0.1f;
    public float SpawnPosX = 10;
    public float SpawnPosY = 5;

    public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
    {
        entityManager.AddComponentData(entity, new AsteroidSpawner
        {
            Prefab = conversionSystem.GetPrimaryEntity(Prefab),
            Rate = SpawnRate,
            MinSpeed = MinSpeed,
            MaxSpeed = MaxSpeed,
            PathVariation = PathVariation,
            SpawnPosX = SpawnPosX,
            SpawnPosY = SpawnPosY
        });


        var buffer = entityManager.AddBuffer<AsteroidSprite>(entity);

        if (Sprites == null)
            return;

        foreach (var s in Sprites)
        {
            buffer.Add(new AsteroidSprite
            {
                Sprite = conversionSystem.GetPrimaryEntity(s)
            });
        }
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        if (Prefab != null)
            referencedPrefabs.Add(Prefab);
    }
}

[UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
class DeclareAsteroidSpriteReference : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((AsteroidSpawnerComponent mgr) =>
        {
            if (mgr.Sprites == null)
                return;

            foreach (var s in mgr.Sprites)
            {
                DeclareReferencedAsset(s);
            }
        });
    }
}