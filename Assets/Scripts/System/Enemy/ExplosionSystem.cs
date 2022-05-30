using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(MissileHitSystem))]
partial class ExplosionSystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<ExplosionSpawner>();
    }
    protected override void OnUpdate()
    {
        var explosions = GetSingleton<ExplosionSpawner>();
        var explosionsEntity = GetSingletonEntity<ExplosionSpawner>();

        var ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer().AsParallelWriter();

        var deltaTime = Time.DeltaTime;
        Dependency = Entities.ForEach((ref Explosion explosion) =>
        {
            explosion.Timer += deltaTime;
        }).Schedule(Dependency);

        Entities
            .WithoutBurst()
            .ForEach((
                Entity e,
                int entityInQueryIndex,
                in Explosion explosion) =>
            {
                if (explosion.Timer >= explosions.ExplotionSpan)
                {
                    ecb.DestroyEntity(entityInQueryIndex, e);
                }
            }).Run();

        ecbSystem.AddJobHandleForProducer(Dependency);
    }
}
