using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.U2D.Entities.Physics;
using UnityEngine;


partial class AsteroidSpawnSystem : SystemBase
{
    private float m_ElapsedTime;
    private Unity.Mathematics.Random random;
    protected override void OnCreate()
    {
        base.OnCreate();
        random = new Unity.Mathematics.Random(314159);
    }

    protected override void OnUpdate()
    {

        var asteroidSpawner = GetSingleton<AsteroidSpawner>();

        m_ElapsedTime += Time.DeltaTime;
        var timeLimit = 1.0f / asteroidSpawner.Rate;

        if (m_ElapsedTime > timeLimit)
        {

            var (pos, velocity) = getTranslationAndVelocity(asteroidSpawner);

            var asteroid = EntityManager.Instantiate(asteroidSpawner.Prefab);

            EntityManager.SetComponentData(asteroid, new Translation
            {
                Value = pos
            });
           
            EntityManager.SetComponentData(asteroid, new PhysicsVelocity
            {
                Linear = velocity.xy
            });

            m_ElapsedTime = 0;
        }
    }

    private (float3, float3) getTranslationAndVelocity(AsteroidSpawner asteroidSpawner) {
        
        // find a point somewhere outside of view,
        var rot = quaternion.RotateZ(random.NextFloat(2 * math.PI));
        var pos = new float3(asteroidSpawner.SpawnPosX, asteroidSpawner.SpawnPosX, 0);

        pos = math.mul(rot, pos);

        // aim it directly at the camera's position
        var speed = random.NextFloat(asteroidSpawner.MinSpeed, asteroidSpawner.MaxSpeed);
        var dir = math.normalize(float3.zero - pos) * speed;

        // vary the aim by a a little to miss the center
        rot = quaternion.RotateZ(random.NextFloat(asteroidSpawner.PathVariation * 2 * math.PI)); // 10% variation

        var velocity = math.mul(rot, dir);

        return (pos, velocity);
    }
}
