using Unity.Entities;
using UnityEngine;

public class GameStateComponent : MonoBehaviour, IConvertGameObjectToEntity
{
    public GameState ShowingState = GameState.None;
    public GameObject[] GameStateObjects = null;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        if (GameStateObjects == null)
            return;

        dstManager.AddComponentData(entity, new GameStateData
        {
            State = ShowingState,
        });

        var gameStateObjects = dstManager.AddBuffer<GameStateObject>(entity);
        for (var i = 0; i < GameStateObjects.Length; i++)
        {
            gameStateObjects.Add(new GameStateObject()
            {
                Value = GameStateObjects[i] != null ? conversionSystem.GetPrimaryEntity(GameStateObjects[i]) : Entity.Null
            });
        }
    }
}
