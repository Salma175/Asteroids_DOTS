using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class RestartSystem : SystemBase
{
    private EntityQuery asteroidsQuery;
    private EntityQuery missileQuery;
    private EntityQuery shieldQuery;

    GameControls gc;
    InputAction start;

    protected override void OnCreate()
    {
        base.OnCreate();

        gc = new GameControls();
        gc.Game.Enable();
        start = gc.Game.Start;

        start.performed += Start_performed;

        RequireSingletonForUpdate<GameState>();
        RequireSingletonForUpdate<GameShowState>();
        RequireSingletonForUpdate<Player>();

        asteroidsQuery = GetEntityQuery(new ComponentType[]
        {
                ComponentType.ReadOnly<Enemy>(), ComponentType.ReadOnly<SpriteRenderer>()
        });

        missileQuery = GetEntityQuery(new ComponentType[]
        {
                ComponentType.ReadOnly<Missile>(), ComponentType.ReadOnly<SpriteRenderer>()
        });

        shieldQuery = GetEntityQuery(new ComponentType[]
      {
                ComponentType.ReadOnly<Shield>(), ComponentType.ReadOnly<SpriteRenderer>()
      });
    }

    private void Start_performed(InputAction.CallbackContext obj)
    {
        var gameState = GetSingleton<GameState>();

        if (gameState.Value == GameStates.Start)
        {
            gameState.Value = GameStates.InGame;
            SetSingleton(gameState);
        }
    }

    protected override void OnUpdate()
    {
        var gameState = GetSingleton<GameState>();
        var playerEntity = GetSingletonEntity<Player>();

        if (gameState.Value == GameStates.Start)
        {
            // remove all stuff
            EntityManager.DestroyEntity(asteroidsQuery);
            EntityManager.DestroyEntity(missileQuery);
            EntityManager.DestroyEntity(shieldQuery);

            // put the ship back in the center
            EntityManager.SetComponentData(playerEntity, new Translation
            {
                Value = float3.zero
            });
            EntityManager.SetComponentData(playerEntity, new Rotation
            {
                Value = quaternion.identity
            });
            EventManager.ResetAudioEvent?.Invoke();

        }
    }

}
