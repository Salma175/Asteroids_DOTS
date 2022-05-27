using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

partial class EnemySpawnerSystem : SystemBase
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

        var enemySpawner = GetSingleton<EnemySpawner>();

        m_ElapsedTime += Time.DeltaTime;
        var timeLimit = 1.0f / enemySpawner.Rate;

        if (m_ElapsedTime > timeLimit)
        {
            InstantiateEnemy(enemySpawner);

            m_ElapsedTime = 0;
        }
    }

    private void InstantiateEnemy(EnemySpawner enemySpawner)
    {
        var (pos, velocity) = getTranslationAndVelocity(enemySpawner);
        var asteroid = EntityManager.Instantiate(enemySpawner.Prefab);

        EntityManager.SetComponentData(asteroid, new Translation
        {
            Value = pos
        });

        EntityManager.SetComponentData(asteroid, new PhysicsVelocity
        {
            Linear = velocity
        });

        //Replace Sprite
        var enemySpawnerEntity = GetSingletonEntity<EnemySpawner>();
        var enemySprites = EntityManager.GetBuffer<EnemySpritePrefab>(enemySpawnerEntity);
        var index = UnityEngine.Random.Range(0, enemySprites.Length);
        var spriteRenderer = EntityManager.GetComponentObject<SpriteRenderer>(asteroid);
        var spriteEntity = enemySprites[index].SpritePrefab;
        spriteRenderer.sprite = EntityManager.GetComponentObject<SpriteRenderer>(spriteEntity).sprite;
    }

    private (float3, float3) getTranslationAndVelocity(EnemySpawner asteroidSpawner)
    {

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
