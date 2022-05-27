using Unity.Entities;
using UnityEngine;

public class ExplosionComponent : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent<Explosion>(entity);
    }
}
