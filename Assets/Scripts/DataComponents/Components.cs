using Unity.Entities;

#region PLAYER
public struct Player : IComponentData
{
    public float RotationSpeed;
    public float MoveSpeed;
    public float FireRate;
    public float MissileSpeed;
    public float PowerUpSpan;
    public Entity DefaultMissile;
    public Entity LaserMissile;
    public Entity DoubleLaserMissile;
    public Entity Shield;
}

public struct Missile : IComponentData { }
#endregion

#region POWER UP
public struct PowerUp : IComponentData
{
    public PowerUpType Type;
}

public struct PowerUpSpawner : IComponentData
{
    public Entity ShieldPrefab;
    public Entity LaserPrefab;
    public Entity DoubleLaserPrefab;
    public float Speed;
    public float PathVariation;
    public float SpawnPosX;
    public float SpawnPosY;
    public float SpanTime;
}
#endregion

#region ENEMY
public struct Enemy : IComponentData { }

public struct EnemySpawner : IComponentData
{
    public Entity Prefab;
    public float Rate;
    public float MinSpeed;
    public float MaxSpeed;
    public float PathVariation;
    public float SpawnPosX;
    public float SpawnPosY;
}

public struct EnemySpritePrefab : IBufferElementData
{
    public Entity SpritePrefab;
}
#endregion

#region EXPLOSION
public struct Explosion : IComponentData {
    public float Timer;
}

public struct ExplosionSpawner : IComponentData
{
    public Entity Prefab;
    public float ExplotionSpan;
}
#endregion

#region GAME SPECIFIC
public struct GameParameters : IComponentData
{
    public GameState State;
    public int Lives;
    public int Score;
    public PowerUpType PowerUp;
}

public struct GameStateData : IComponentData
{
    public GameState State;
}

public struct GameStateObject : IBufferElementData
{
    public Entity Value;
}
#endregion

#region PLAYER HEALTH
public struct HealthManager : IComponentData
{
    public Entity HealthPrefab;
}

public struct Health : IBufferElementData
{
    public Entity Value;
}
#endregion