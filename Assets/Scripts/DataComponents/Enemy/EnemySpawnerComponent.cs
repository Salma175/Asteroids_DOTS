using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EnemySpawnerComponent : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject Prefab;
    public GameObject[] EnemyPrefabs;

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


        entityManager.AddComponentData(entity, new GameParameters
        {
            State = GameState.Start,
            Lives = Constants.Lives,
            Score = Constants.Zero,
            PowerUp = PowerUpType.None
        });

        var buffer = entityManager.AddBuffer<EnemySpritePrefab>(entity);

        if (EnemyPrefabs == null)
            return;

        foreach (var prefab in EnemyPrefabs)
        {
            buffer.Add(new EnemySpritePrefab
            {
                SpritePrefab = conversionSystem.GetPrimaryEntity(prefab)
            });
        }
    }
    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        if (Prefab != null)
            referencedPrefabs.Add(Prefab);

        if (EnemyPrefabs == null)
            return;

        foreach (var prefab in EnemyPrefabs)
        {
            referencedPrefabs.Add(prefab);
        }
    }
}