using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

partial class PowerUpSpawnerSystem: SystemBase
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
        var gameState = GetSingleton<GameParameters>();
        if (gameState.State != GameState.InGame)
            return;

        if (gameState.PowerUp != PowerUpType.None)
            return;

        var powerUpSpawner = GetSingleton<PowerUpSpawner>();

        m_ElapsedTime += Time.DeltaTime;
        if (m_ElapsedTime > powerUpSpawner.SpanTime)
        {

            var (pos, velocity) = getTranslationAndVelocity(powerUpSpawner);
            var asteroid = EntityManager.Instantiate(GetPrefab());

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

    private Entity GetPrefab() {
        var powerUpSpawner = GetSingleton<PowerUpSpawner>();
        int randInt = Random.Range(0,3);
        switch (randInt)
        {
            case 1:
                return powerUpSpawner.LaserPrefab;
            case 2:
                return powerUpSpawner.DoubleLaserPrefab;
            default:
                return powerUpSpawner.ShieldPrefab;
        }
    }

    private (float3, float3) getTranslationAndVelocity(PowerUpSpawner asteroidSpawner)
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
