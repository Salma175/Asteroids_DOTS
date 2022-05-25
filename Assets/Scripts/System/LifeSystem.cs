using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
public partial class LifeSystem : SystemBase
{
    private GameStates currentGameState = GameStates.None;
    private int noOfLives = -1;
    private EntityCommandBuffer cmdBuffer;
    private int score = -1;

    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<LifeManager>();
        RequireSingletonForUpdate<GameState>();
    }

    protected override void OnUpdate()
    {
        var gameState = GetSingleton<GameState>();

        var ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        cmdBuffer = ecbSystem.CreateCommandBuffer();

        if (gameState.Value != currentGameState)
            GameStateChangeUpdate(gameState.Value);
        else if (gameState.Value == GameStates.InGame && gameState.Lives != noOfLives)
            LifeReduction(gameState.Lives);

        if (gameState.Score != score) {
            GameScoreChange(gameState.Score);
        }
    }

    private void GameStateChangeUpdate(GameStates m_currentGameState)
    {
        if (m_currentGameState == GameStates.Start)
        {
            DestroyLife();
            EventManager.HandleGameUIEvent?.Invoke(false);
        }
        else if (m_currentGameState == GameStates.InGame)
        {
            SpawnLifes();
            EventManager.HandleGameUIEvent?.Invoke(true);
        }

        currentGameState = m_currentGameState;
    }

    private void GameScoreChange(int m_score) {
        EventManager.ScoreUpdateEvent?.Invoke(m_score);
        score = m_score;
        EventManager.PlayAudioEvent?.Invoke(AudioClipType.AsteroidExplosion);
    }
    private void DestroyLife()
    {
        var lifeManagerEntity = GetSingletonEntity<LifeManager>();
        var lifeBuffers = GetBufferFromEntity<Life>();
        if (!lifeBuffers.HasComponent(lifeManagerEntity))
            return;

        var lives = lifeBuffers[lifeManagerEntity];

        for (var i = 0; i < lives.Length; i++)
            cmdBuffer.DestroyEntity(lives[i].Value);
        lives.Clear();
    }

    private void SpawnLifes()
    {
        var gameState = GetSingleton<GameState>();
        var lifeManager = GetSingleton<LifeManager>();
        var lifeManagerEntity = GetSingletonEntity<LifeManager>();

        var lifeAnchor = new float3(Constants.LifeAnchorX,Constants.LifeAnchorY,0);

        var lifeBuffer = cmdBuffer.AddBuffer<Life>(lifeManagerEntity);

        for (var i = 0; i < gameState.Lives; i++)
        {
            var translation = new Translation
            {
                Value = lifeAnchor
            };

            var life = cmdBuffer.Instantiate(lifeManager.LifePrefab);

            cmdBuffer.SetComponent(life, translation);

            lifeBuffer.Add(new Life()
            {
                Value = life
            });

            lifeAnchor.x--;
        }

        noOfLives = gameState.Lives;
    }

    private void LifeReduction(int m_lives)
    {
        var lifeManagerEntity = GetSingletonEntity<LifeManager>();
        var lifeBuffers = GetBufferFromEntity<Life>();

        var lives = lifeBuffers[lifeManagerEntity];
        cmdBuffer.AddComponent<Disabled>(lives[m_lives].Value);

        noOfLives = m_lives;
        EventManager.PlayAudioEvent?.Invoke(AudioClipType.PlayerExplosion);
    }
}
