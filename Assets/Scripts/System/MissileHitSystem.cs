using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
partial class MissileHitSystem : SystemBase
{

    StepPhysicsWorld stepPhysicsWorld;
    EndSimulationEntityCommandBufferSystem entityCommandBufferSystem;
    Entity scoreTextEntity;

    protected override void OnCreate()
     {
         base.OnCreate();

        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        RequireSingletonForUpdate<ExplosionSpawner>();
    }

    protected override void OnUpdate()
     {
        var explosions = GetSingleton<ExplosionSpawner>();

        Dependency = new CollisionEventSystemJob {
            explosionPrefab = explosions.Prefab,
            buffer = entityCommandBufferSystem.CreateCommandBuffer(),
            translationData = GetComponentDataFromEntity<Translation>(),
            asteroids = GetComponentDataFromEntity<Asteroid>(),
            missiles = GetComponentDataFromEntity<Missile>(),
        }.Schedule(stepPhysicsWorld.Simulation, Dependency);

        entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
     }

    [BurstCompile]
    struct CollisionEventSystemJob : ITriggerEventsJob
    {
        public Entity explosionPrefab;
        public EntityCommandBuffer buffer;
        public ComponentDataFromEntity<Translation> translationData;
        [ReadOnly] public ComponentDataFromEntity<Asteroid> asteroids;
        [ReadOnly] public ComponentDataFromEntity<Missile> missiles;

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

                //GameUIManager.instance.UpdateScore();
            }
        }
    }

  
}
