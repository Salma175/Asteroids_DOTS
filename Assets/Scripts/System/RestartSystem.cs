using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class RestartSystem : SystemBase
{
    private EntityQuery m_AsteroidsQuery;
    private EntityQuery m_MissileQuery;

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

        m_AsteroidsQuery = GetEntityQuery(new ComponentType[]
        {
                ComponentType.ReadOnly<Asteroid>(), ComponentType.ReadOnly<SpriteRenderer>()
        });

        m_MissileQuery = GetEntityQuery(new ComponentType[]
        {
                ComponentType.ReadOnly<Missile>(), ComponentType.ReadOnly<SpriteRenderer>()
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
            EntityManager.DestroyEntity(m_AsteroidsQuery);
            EntityManager.DestroyEntity(m_MissileQuery);

            // put the ship back in the center
            EntityManager.SetComponentData(playerEntity, new Translation
            {
                Value = float3.zero
            });
            EntityManager.SetComponentData(playerEntity, new Rotation
            {
                Value = quaternion.identity
            });
        }
    }

}
