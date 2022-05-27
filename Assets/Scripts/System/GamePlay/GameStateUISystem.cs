using Unity.Entities;

partial class GameStateUISystem : SystemBase
{
    private GameState currentGameState = GameState.None;

    protected override void OnCreate()
    {
        base.OnCreate();

        RequireSingletonForUpdate<GameParameters>();
    }

    protected override void OnUpdate()
    {
        var gameState = GetSingleton<GameParameters>();
        if (gameState.State == currentGameState)
            return;

        currentGameState = gameState.State;

        var ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        var cmdBuffer = ecbSystem.CreateCommandBuffer().AsParallelWriter();

        Dependency = Entities.ForEach((Entity entity, int entityInQueryIndex, in GameShowState showState, in DynamicBuffer<GameStateObject> gameStateObjects) =>
        {
            for (var i = 0; i < gameStateObjects.Length; i++)
            {
                var isVisible = gameState.State == showState.Value;

                if (isVisible)
                {
                    cmdBuffer.RemoveComponent<Disabled>(entityInQueryIndex, gameStateObjects[i].Value);
                }
                else
                {
                    cmdBuffer.AddComponent<Disabled>(entityInQueryIndex, gameStateObjects[i].Value);
                }
            }
        }).Schedule(Dependency);

        ecbSystem.AddJobHandleForProducer(Dependency);
    }
}