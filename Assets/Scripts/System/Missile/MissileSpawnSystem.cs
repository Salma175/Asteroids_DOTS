using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial class MissileSpawnSystem : SystemBase
{
    private float m_ElapsedTime;

    protected override void OnUpdate()
    {
        var gameState = GetSingleton<GameParameters>();
        if (gameState.State != GameState.InGame)
            return;

        var playerEntity = GetSingletonEntity<Player>();
        var player = GetSingleton<Player>();
        var playerRot = EntityManager.GetComponentData<Rotation>(playerEntity);
        var playerTr = EntityManager.GetComponentData<Translation>(playerEntity);

        m_ElapsedTime += Time.DeltaTime;
        var timeLimit = 1.0f / player.FireRate;
        if (m_ElapsedTime > timeLimit)
        {
            var missile = EntityManager.Instantiate(GetMissiblePrefab(gameState,player));

            EntityManager.SetComponentData(missile, playerRot);
            EntityManager.SetComponentData(missile, playerTr);

            EntityManager.SetComponentData(missile, new PhysicsVelocity
            {
                Linear = math.mul(playerRot.Value, new float3(0f, player.MissileSpeed, 0f))
            });

            m_ElapsedTime = 0;
            EventManager.PlayAudioEvent?.Invoke(AudioClipType.PlayerFire);
        }
    }

    private Entity GetMissiblePrefab(GameParameters gameState, Player player) {
        switch (gameState.PowerUp)
        {
            case PowerUpType.Laser:
                return player.LaserMissile;
            case PowerUpType.DoubleLaser:
                return player.DoubleLaserMissile;
            default:
                return player.DefaultMissile;
        }
    }
}
