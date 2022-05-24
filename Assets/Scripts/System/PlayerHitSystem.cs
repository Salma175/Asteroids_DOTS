using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
partial class PlayerHitSystem : SystemBase
{
    StepPhysicsWorld stepPhysicsWorld;
    EndSimulationEntityCommandBufferSystem entityCommandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();

        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        RequireSingletonForUpdate<ExplosionSpawner>();
    }
    protected override void OnUpdate()
    {
        var gameState = GetSingleton<GameState>();
        if (gameState.Value != GameStates.InGame)
            return;
        var explosions = GetSingleton<ExplosionSpawner>();

        Dependency = new CollisionEventSystemJob
        {
            explosionPrefab = explosions.Prefab,
            gameState = gameState,
            gameStateEntity = GetSingletonEntity<GameState>(),
            buffer = entityCommandBufferSystem.CreateCommandBuffer(),
            translationData = GetComponentDataFromEntity<Translation>(),
            asteroids = GetComponentDataFromEntity<Asteroid>(),
            player = GetComponentDataFromEntity<Player>()
        }.Schedule(stepPhysicsWorld.Simulation, Dependency);

        entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

    struct CollisionEventSystemJob : ITriggerEventsJob
    {
        public Entity explosionPrefab;
        [ReadOnly] public GameState gameState;
        [ReadOnly] public Entity gameStateEntity;
        public EntityCommandBuffer buffer;
        public ComponentDataFromEntity<Translation> translationData;
        [ReadOnly] public ComponentDataFromEntity<Asteroid> asteroids;
        [ReadOnly] public ComponentDataFromEntity<Player> player;

        public void Execute(TriggerEvent triggerEvent)
        {
            var (isPlayerHitbyEnemy, translation) = IsPlayerHitByEnemy(triggerEvent.EntityA, triggerEvent.EntityB);

            if (isPlayerHitbyEnemy)OnPlayerHitByEnemy(translation);
        }

        private (bool,float3) IsPlayerHitByEnemy(Entity entityA, Entity entityB) {

            float3 translation = float3.zero;

            bool isPlayerHitbyEnemy = false;

            bool isBodyAEnemy = asteroids.HasComponent(entityA);
            bool isBodyBEnemy = asteroids.HasComponent(entityB);

            // Ignoring Triggers overlapping other Triggers
            if (isBodyAEnemy && isBodyBEnemy)
                return (isPlayerHitbyEnemy,translation);

            bool isBodyAPlayer = player.HasComponent(entityA);
            bool isBodyBPlayer = player.HasComponent(entityB);

            if (isBodyAEnemy && isBodyBPlayer)
            {
                translation = translationData[entityA].Value;
                buffer.DestroyEntity(entityA);
                isPlayerHitbyEnemy = true;
            }

            if (isBodyBEnemy && isBodyAPlayer)
            {
                translation = translationData[entityB].Value;
                buffer.DestroyEntity(entityB);
                isPlayerHitbyEnemy = true;

            }
            return (isPlayerHitbyEnemy,translation);
        }

        private void OnPlayerHitByEnemy(float3 translation)
        {
            var livesLeft = gameState.Lives - 1;

            buffer.SetComponent(gameStateEntity, new GameState
            {
                Value = GameStates.InGame,
                Lives = livesLeft,
                Score = gameState.Score
            });

            var exp = buffer.Instantiate(explosionPrefab);
            buffer.SetComponent(exp, new Translation { Value = translation });

            var isPlayerDead = livesLeft == Constants.Zero;
            
            if (isPlayerDead)
            {
                buffer.SetComponent(gameStateEntity, new GameState
                {
                    Value = GameStates.Start,
                    Lives = Constants.Lives,
                    Score = Constants.Zero
                });
            }
        }
    }
}
