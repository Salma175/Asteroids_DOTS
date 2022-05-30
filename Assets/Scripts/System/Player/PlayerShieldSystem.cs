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
        var gameParams = GetSingleton<GameParameters>();

        if (gameParams.State != GameState.InGame)
            return;

        if (gameParams.PowerUp == PowerUpType.None)
            return;
       
        var player = GetSingleton<Player>();

        if (gameParams.PowerUp == PowerUpType.Shield)
            EnableShieldEnity(player);

        m_ElapsedTime += Time.DeltaTime;
        if (m_ElapsedTime > player.PowerUpSpan)
        {
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

        if (gameParams.PowerUp == PowerUpType.Shield)
            DisbaleShieldEntity();

        var newParams = gameParams;
        newParams.PowerUp = PowerUpType.None;
        EntityManager.SetComponentData(gameParamsEntity, newParams);

        EventManager.PlayAudioEvent?.Invoke(AudioClipType.ShieldDown);
        m_ElapsedTime = 0;
    }

    private void DisbaleShieldEntity()
    {
        var player = GetSingleton<Player>();
        EntityManager.AddComponent<Disabled>(player.Shield);
    }
}
