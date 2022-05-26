using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EnemySpawnerComponent : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public Sprite[] Sprites;
    public GameObject Prefab;

    public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
    {
        entityManager.AddComponentData(entity, new EnemySpawner
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
            Score = Constants.Zero,
            IsSheildOn = false
        });

        var buffer = entityManager.AddBuffer<EnemySprite>(entity);

        if (Sprites == null)
            return;

        foreach (Sprite s in Sprites)
        {
            Entity spriteEntity = conversionSystem.GetPrimaryEntity(s);
            buffer.Add(new EnemySprite
            {
                Sprite = spriteEntity
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
class DeclareEnemySpriteReference : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((EnemySpawnerComponent mgr) =>
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