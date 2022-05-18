using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

partial class MissileSpawnSystem : SystemBase
{
    private float m_ElapsedTime;

    protected override void OnUpdate()
    {

        var playerEntity = GetSingletonEntity<Player>();
        var player = GetSingleton<Player>();
        var playerRot = EntityManager.GetComponentData<Rotation>(playerEntity);
        var playerTr = EntityManager.GetComponentData<Translation>(playerEntity);

        m_ElapsedTime += Time.DeltaTime;
        var timeLimit = 1.0f / player.FireRate;
        if (m_ElapsedTime > timeLimit)
        {
            var missile = EntityManager.Instantiate(player.MissilePrefab);

            EntityManager.SetComponentData(missile, playerRot);
            EntityManager.SetComponentData(missile, playerTr);

            EntityManager.SetComponentData(missile, new PhysicsVelocity
            {
                Linear = math.mul(playerRot.Value, new float3(0f, player.FireSpeed, 0f)).xyz
            });

            m_ElapsedTime = 0;
        }
    }
}
