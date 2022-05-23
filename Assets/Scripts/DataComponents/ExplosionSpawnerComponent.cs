using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ExplosionSpawnerComponent : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public Sprite[] sprites;

    public GameObject Prefab = null;
    public float TimePerSprite = 0.05f;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new ExplosionSpawner
        {
            Prefab = conversionSystem.GetPrimaryEntity(Prefab),
            TimePerSprite = TimePerSprite,
        });

        var buffer = dstManager.AddBuffer<ExplosionSprite>(entity);

        if (sprites == null)
            return;

        foreach (var s in sprites)
        {
            buffer.Add(new ExplosionSprite
            {
                Sprite =  conversionSystem.GetPrimaryEntity(s)
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
class DeclareExplosionSpriteReference : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ExplosionSpawnerComponent mgr) =>
        {
            if (mgr.sprites == null)
                return;

            foreach (var s in mgr.sprites)
            {
                DeclareReferencedAsset(s);
            }
        });
    }
}
