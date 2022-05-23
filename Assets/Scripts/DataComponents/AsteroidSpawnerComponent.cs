using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class AsteroidSpawnerComponent : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public Sprite[] Sprites;

    public GameObject Prefab = null;

    public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
    {
        entityManager.AddComponentData(entity, new AsteroidSpawner
        {
            Prefab = conversionSystem.GetPrimaryEntity(Prefab),
            Rate = Constants.SpawnRate,
            MinSpeed = Constants.MinSpeed,
            MaxSpeed = Constants.MaxSpeed,
            PathVariation = Constants.PathVariation,
            SpawnPosX = Constants.SpawnPosX,
            SpawnPosY = Constants.SpawnPosY
        });


        entityManager.AddComponentData(entity, new GameState
        {
            Value = GameStates.Start,
            Lives = Constants.Lives,
            Score = Constants.Zero
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