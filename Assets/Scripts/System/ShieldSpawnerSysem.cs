using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

partial class ShieldSpawnerSystem: SystemBase
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
        var gameState = GetSingleton<GameState>();
        if (gameState.Value != GameStates.InGame)
            return;

        if (gameState.IsSheildOn)
            return;

        var shieldSpawner = GetSingleton<ShieldSpawner>();

        m_ElapsedTime += Time.DeltaTime;

        if (m_ElapsedTime > shieldSpawner.SpanTime)
        {

            var (pos, velocity) = getTranslationAndVelocity(shieldSpawner);
            var asteroid = EntityManager.Instantiate(shieldSpawner.Prefab);

            EntityManager.SetComponentData(asteroid, new Translation
            {
                Value = pos
            });

            EntityManager.SetComponentData(asteroid, new PhysicsVelocity
            {
                Linear = velocity
            });
            m_ElapsedTime = 0;
        }
    }

    private (float3, float3) getTranslationAndVelocity(ShieldSpawner asteroidSpawner)
    {

        // find a point somewhere outside of view,
        var rot = quaternion.RotateZ(random.NextFloat(2 * math.PI));
        var pos = new float3(asteroidSpawner.SpawnPosX, asteroidSpawner.SpawnPosX, 0);

        pos = math.mul(rot, pos);

        var dir = math.normalize(float3.zero - pos) * asteroidSpawner.Speed;

        // vary the aim by a a little to miss the center
        rot = quaternion.RotateZ(random.NextFloat(asteroidSpawner.PathVariation * 2 * math.PI)); // 10% variation

        var velocity = math.mul(rot, dir);

        return (pos, velocity);
    }
}
