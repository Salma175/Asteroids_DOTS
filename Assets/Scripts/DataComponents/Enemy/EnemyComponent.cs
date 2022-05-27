using Unity.Entities;
using UnityEngine;

public class EnemyComponent : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent<Enemy>(entity);
    }
}
