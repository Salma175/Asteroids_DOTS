using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
partial class MissileHitSystem : SystemBase
{

    StepPhysicsWorld stepPhysicsWorld;
    BuildPhysicsWorld buildPhysicsWorld;
    EndSimulationEntityCommandBufferSystem entityCommandBufferSystem;

    protected override void OnCreate()
     {
         base.OnCreate();

        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        RequireSingletonForUpdate<ExplosionSpawner>();
    }

    protected override void OnUpdate()
     {
        var explosions = GetSingleton<ExplosionSpawner>();

        var job = new CollisionEventSystemJob {
            explosionPrefab = explosions.Prefab,
            buffer = entityCommandBufferSystem.CreateCommandBuffer(),
            translationData = GetComponentDataFromEntity<Translation>(),
            asteroids = GetComponentDataFromEntity<Asteroid>(),
            missiles = GetComponentDataFromEntity<Missile>(),
        }.Schedule(stepPhysicsWorld.Simulation, Dependency);

        Dependency = job;

        entityCommandBufferSystem.AddJobHandleForProducer(job);
     }


   [BurstCompile]
    struct CollisionEventSystemJob : ITriggerEventsJob
    {
        public Entity explosionPrefab;
        public EntityCommandBuffer buffer;
        public ComponentDataFromEntity<Translation> translationData;
        public ComponentDataFromEntity<Asteroid> asteroids;
        public ComponentDataFromEntity<Missile> missiles;
        public void Execute(TriggerEvent triggerEvent)
        {
            bool isExplosion = false;
            float3 translation = float3.zero;
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            bool isBodyAEnemy = asteroids.HasComponent(entityA);
            bool isBodyBEnemy = asteroids.HasComponent(entityB);

            // Ignoring Triggers overlapping other Triggers
            if (isBodyAEnemy && isBodyBEnemy)
                return;

            bool isBodyAPlayer = missiles.HasComponent(entityA);
            bool isBodyBPlayer = missiles.HasComponent(entityB);

            // Ignoring overlapping static bodies
            if ((isBodyAEnemy && !isBodyBPlayer) ||
                (isBodyBEnemy && !isBodyAPlayer))
                return;

            if (isBodyAEnemy && isBodyBPlayer)
            {
                translation = translationData[entityA].Value;
                isExplosion = true;
            }

            if (isBodyBEnemy && isBodyAPlayer)
            {
                translation = translationData[entityB].Value;
                isExplosion = true;
            }

            if (isExplosion)
            {
                var exp = buffer.Instantiate(explosionPrefab);
                buffer.SetComponent(exp, new Translation { Value = translation });

                buffer.DestroyEntity(entityB);
                buffer.DestroyEntity(entityA);
            }
        }
    }
}
