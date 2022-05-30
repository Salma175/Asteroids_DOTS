using Unity.Entities;
using UnityEngine;

// Add Component to Enemy Prefab
public class EnemyComponent : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent<Enemy>(entity);
    }
}
