using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial class PlayerShieldSystem : SystemBase
{
    private float m_ElapsedTime;

    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<GameParameters>();
    }

    protected override void OnUpdate()
    {
        var gameState = GetSingleton<GameParameters>();

        if (gameState.State != GameState.InGame)
            return;

        if (gameState.PowerUp == PowerUpType.None)
            return;
       
        var player = GetSingleton<Player>();

        if (gameState.PowerUp == PowerUpType.Shield)
            EnableShieldEnity(player);

        m_ElapsedTime += Time.DeltaTime;
        if (m_ElapsedTime > player.PowerUpSpan)
        {
            if (gameState.PowerUp == PowerUpType.Shield)
                DisbaleShieldEntity(player);
            DisablePowerUp();
        }
    }

    private void EnableShieldEnity(Player player)
    {
        bool isDisabled = EntityManager.HasComponent<Disabled>(player.Shield);
        if (isDisabled) { 
            EntityManager.RemoveComponent<Disabled>(player.Shield);
        }
    }

    private void DisablePowerUp()
    {
        var gameParamsEntity = GetSingletonEntity<GameParameters>();
        var gameParams = GetSingleton<GameParameters>();
        gameParams.PowerUp = PowerUpType.None;
        EntityManager.SetComponentData(gameParamsEntity, gameParams);

        EventManager.PlayAudioEvent?.Invoke(AudioClipType.ShieldDown);
    }

    private void DisbaleShieldEntity(Player player)
    {
        EntityManager.AddComponent<Disabled>(player.Shield);
        m_ElapsedTime = 0;
    }
}
