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

        RequireSingletonForUpdate<GameParameters>();
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
                ComponentType.ReadOnly<PowerUp>(), ComponentType.ReadOnly<SpriteRenderer>()
      });
    }

    private void Start_performed(InputAction.CallbackContext obj)
    {
        var gameState = GetSingleton<GameParameters>();

        if (gameState.State == GameState.Start)
        {
            gameState.State = GameState.InGame;
            SetSingleton(gameState);
        }
    }

    protected override void OnUpdate()
    {
        var gameState = GetSingleton<GameParameters>();
        var playerEntity = GetSingletonEntity<Player>();

        if (gameState.State == GameState.Start)
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
            var gameParamsEntity = GetSingletonEntity<GameParameters>();
            EntityManager.SetComponentData(gameParamsEntity, new GameParameters
            {
                State = GameState.Start,
                Lives = Constants.Lives,
                Score = Constants.Zero,
                IsSheildOn = false,
                Shot = ShotType.Default
            });
        }
    }

}
