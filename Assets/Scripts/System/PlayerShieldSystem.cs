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
        RequireSingletonForUpdate<GameState>();
    }

    protected override void OnUpdate()
    {
        var gameState = GetSingleton<GameState>();

        if (gameState.Value != GameStates.InGame)
            return;

        if (gameState.IsSheildOn)
        {
            var player = GetSingleton<Player>();

            EnableShieldEnity(player);

            m_ElapsedTime += Time.DeltaTime;
            if (m_ElapsedTime > player.ShieldSpan)
            {
                DisbaleShieldEntity(gameState, player);
            }
        }
    }

    private void EnableShieldEnity(Player player)
    {
        bool isDisabled = EntityManager.HasComponent<Disabled>(player.Shield);
        if (isDisabled) EntityManager.RemoveComponent<Disabled>(player.Shield);
    }

    private void DisbaleShieldEntity(GameState gameState, Player player)
    {
        var gameStateEntity = GetSingletonEntity<GameState>();

        EntityManager.SetComponentData(gameStateEntity, new GameState
        {
            Value = gameState.Value,
            Lives = gameState.Lives,
            Score = gameState.Score,
            IsSheildOn = false
        });
        EntityManager.AddComponent<Disabled>(player.Shield);
        m_ElapsedTime = 0;
    }
}
