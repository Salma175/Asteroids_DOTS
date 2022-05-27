using Unity.Entities;
using UnityEngine;

public class PowerUpComponent : MonoBehaviour, IConvertGameObjectToEntity
{
    public PowerUpType type;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new PowerUp { Type = type });
    }
}