using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
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
    }
    protected override void OnUpdate()
    {
        var gameState = GetSingleton<GameState>();
        if (gameState.Value != GameStates.InGame)
            return;

        Dependency = new CollisionEventSystemJob
        {
            gameStateEntity = GetSingletonEntity<GameState>(),
            buffer = entityCommandBufferSystem.CreateCommandBuffer(),
            asteroids = GetComponentDataFromEntity<Asteroid>(),
            player = GetComponentDataFromEntity<Player>()
        }.Schedule(stepPhysicsWorld.Simulation, Dependency);

        entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }


    [BurstCompile]
    struct CollisionEventSystemJob : ITriggerEventsJob
    {
        public Entity gameStateEntity;
        public EntityCommandBuffer buffer;
        [ReadOnly] public ComponentDataFromEntity<Asteroid> asteroids;
        [ReadOnly] public ComponentDataFromEntity<Player> player;
        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            bool isBodyAEnemy = asteroids.HasComponent(entityA);
            bool isBodyBEnemy = asteroids.HasComponent(entityB);

            // Ignoring Triggers overlapping other Triggers
            if (isBodyAEnemy && isBodyBEnemy)
                return;

            bool isBodyAPlayer = player.HasComponent(entityA);
            bool isBodyBPlayer = player.HasComponent(entityB);

            // Ignoring overlapping static bodies
            if ((isBodyAEnemy && !isBodyBPlayer) ||
                (isBodyBEnemy && !isBodyAPlayer))
                return;

            var isPlayerDead = isBodyAEnemy && isBodyBPlayer || isBodyBEnemy && isBodyAPlayer;
            if (isPlayerDead)
            {
                buffer.SetComponent(gameStateEntity, new GameState
                {
                    Value = GameStates.Start
                });
            }
        }
    }
}
