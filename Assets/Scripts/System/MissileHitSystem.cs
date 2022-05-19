using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

partial class MissileHitSystem : SystemBase
{
    protected override void OnCreate()
     {
         base.OnCreate();
         RequireSingletonForUpdate<ExplosionSpawner>();
     }

     protected override void OnUpdate()
     {
        /*
         var physicsWorldSystem = World.GetExistingSystem<PhysicsWorldSystem>();
         var physicsWorld = physicsWorldSystem.PhysicsWorld;

         var explosions = GetSingleton<ExplosionSpawner>();

         var cmdBuffer = new EntityCommandBuffer(Allocator.TempJob);

         Entities.WithAll<Missile>()
            .ForEach((
                Entity missileEntity,
                in PhysicsColliderBlob collider,
                in Translation tr,
                in Rotation rot) =>
            {
                     // check with missile
                     if (physicsWorld.OverlapCollider(
                    new OverlapColliderInput
                    {
                        Collider = collider.Collider,
                        Transform = new PhysicsTransform(tr.Value, rot.Value),
                        Filter = collider.Collider.Value.Filter
                    },
                    out OverlapColliderHit hit))
                {
                    var asteroidEntity = physicsWorld.AllBodies[hit.PhysicsBodyIndex].Entity;

                    var exp = cmdBuffer.Instantiate(explosions.Prefab);
                    cmdBuffer.SetComponent(exp, new Translation { Value = tr.Value });

                    cmdBuffer.DestroyEntity(asteroidEntity);
                    cmdBuffer.DestroyEntity(missileEntity);
                }
            }).Run();

         cmdBuffer.Playback(EntityManager);
         cmdBuffer.Dispose();
        */
     }
}
