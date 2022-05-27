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

        if (gameState.IsSheildOn)
        {
            var player = GetSingleton<Player>();

            EnableShieldEnity(player);

            m_ElapsedTime += Time.DeltaTime;
            if (m_ElapsedTime > player.ShieldSpan)
            {
                DisbaleShieldEntity(player);
            }
        }
    }

    private void EnableShieldEnity(Player player)
    {
        bool isDisabled = EntityManager.HasComponent<Disabled>(player.Shield);
        if (isDisabled) EntityManager.RemoveComponent<Disabled>(player.Shield);
    }

    private void DisbaleShieldEntity(Player player)
    {
        var gameParamsEntity = GetSingletonEntity<GameParameters>();
        var gameParams = GetSingleton<GameParameters>();
        gameParams.IsSheildOn = false;
        EntityManager.SetComponentData(gameParamsEntity, gameParams);

        EntityManager.AddComponent<Disabled>(player.Shield);
        m_ElapsedTime = 0;
    }
}
