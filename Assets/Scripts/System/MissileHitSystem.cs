using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
partial class MissileHitSystem : SystemBase
{

    BuildPhysicsWorld buildPhysicsWorldSystem;
    StepPhysicsWorld stepPhysicsWorld;
    EndSimulationEntityCommandBufferSystem entityCommandBufferSystem;

    protected override void OnCreate()
     {
         base.OnCreate();

        buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        RequireSingletonForUpdate<ExplosionSpawner>();
    }

    protected override void OnUpdate()
     {
        //var explosions = GetSingleton<ExplosionSpawner>();

        var job = new CollisionEventSystemJob {
            //explosionPrefab = explosions.Prefab,
            buffer = entityCommandBufferSystem.CreateCommandBuffer()
        }.Schedule(stepPhysicsWorld.Simulation, Dependency);

        entityCommandBufferSystem.AddJobHandleForProducer(job);
     }


   [BurstCompile]
    struct CollisionEventSystemJob : ITriggerEventsJob
    {
        // public Entity explosionPrefab;
        public EntityCommandBuffer buffer;
        public void Execute(TriggerEvent triggerEvent)
        {
            Debug.Log($"collision event: {triggerEvent}. Entities: {triggerEvent.EntityA}, {triggerEvent.EntityB}");

            //Translation tr= manager.GetComponentData<Translation>(triggerEvent.EntityA);
            //var exp = cmdBuffer.Instantiate(explosionPrefab);
            //cmdBuffer.SetComponent(exp, new Translation { Value = tr.Value });

            buffer.DestroyEntity(triggerEvent.EntityA);
            buffer.DestroyEntity(triggerEvent.EntityB);
        }
    }
}
