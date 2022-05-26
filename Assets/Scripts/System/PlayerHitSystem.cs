using System;
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
            asteroids = GetComponentDataFromEntity<Enemy>(),
            player = GetComponentDataFromEntity<Player>(),
            shields = GetComponentDataFromEntity<Shield>()
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
        [ReadOnly] public ComponentDataFromEntity<Enemy> asteroids;
        [ReadOnly] public ComponentDataFromEntity<Player> player;
        [ReadOnly] public ComponentDataFromEntity<Shield> shields;

        public void Execute(TriggerEvent triggerEvent)
        {
            #region CAUGHT SHIELD POWER UP
            bool isShieldPowerUp = DisPlayerCatchShieldPowerUp(triggerEvent.EntityA, triggerEvent.EntityB);

            if (isShieldPowerUp)
            {
                UpdateGameShieldState();
            }
            #endregion

            if (gameState.IsSheildOn)
                return;

            #region HIT BY ENEMY
            var (isPlayerHitbyEnemy, translation) = IsPlayerHitByEnemy(triggerEvent.EntityA, triggerEvent.EntityB);

            if (isPlayerHitbyEnemy)
            {
                OnPlayerHitByEnemy(translation);
            }
            #endregion
        }

        private (bool, float3) IsPlayerHitByEnemy(Entity entityA, Entity entityB)
        {

            float3 translation = float3.zero;

            bool isPlayerHitbyEnemy = false;

            bool isBodyAEnemy = asteroids.HasComponent(entityA);
            bool isBodyBEnemy = asteroids.HasComponent(entityB);

            // Ignoring Triggers overlapping other Triggers
            if (isBodyAEnemy && isBodyBEnemy)
                return (isPlayerHitbyEnemy, translation);

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
            return (isPlayerHitbyEnemy, translation);
        }

        private void OnPlayerHitByEnemy(float3 translation)
        {
            var livesLeft = gameState.Lives - 1;

            buffer.SetComponent(gameStateEntity, new GameState
            {
                Value = gameState.Value,
                Lives = livesLeft,
                Score = gameState.Score,
                IsSheildOn = gameState.IsSheildOn
            });

            var exp = buffer.Instantiate(explosionPrefab);
            buffer.SetComponent(exp, new Translation { Value = translation });

            if (livesLeft == Constants.Zero)
            {
                UpdateGameStateOnPlayerDead();
            }
        }

        private void UpdateGameStateOnPlayerDead() {

            buffer.SetComponent(gameStateEntity, new GameState
            {
                Value = GameStates.Start,
                Lives = Constants.Lives,
                Score = Constants.Zero,
                IsSheildOn = false
            });
        }

        private bool DisPlayerCatchShieldPowerUp(Entity entityA, Entity entityB)
        {
            bool isPlayerHitShield = false;

            bool isBodyAShield = shields.HasComponent(entityA);
            bool isBodyBShield = shields.HasComponent(entityB);

            // Ignoring Triggers overlapping other Triggers
            if (isBodyAShield && isBodyBShield)
                return (isPlayerHitShield);

            bool isBodyAPlayer = player.HasComponent(entityA);
            bool isBodyBPlayer = player.HasComponent(entityB);

            if (isBodyAShield && isBodyBPlayer)
            {
                buffer.DestroyEntity(entityA);
                isPlayerHitShield = true;
            }

            if (isBodyBShield && isBodyAPlayer)
            {
                buffer.DestroyEntity(entityB);
                isPlayerHitShield = true;

            }
            return isPlayerHitShield;
        }

        private void UpdateGameShieldState()
        {
            buffer.SetComponent(gameStateEntity, new GameState
            {
                Value = gameState.Value,
                Lives = gameState.Lives,
                Score = gameState.Score,
                IsSheildOn = true
            });
        }
    }
}
