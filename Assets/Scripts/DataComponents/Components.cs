using Unity.Entities;
using UnityEngine;

public struct Player : IComponentData
{
    public float RotationSpeed;
    public float MoveSpeed;
    public float FireRate;
    public float FireSpeed;
    public Entity MissilePrefab;
}

public struct Missile : IComponentData { }

public struct Asteroid : IComponentData { }

public struct AsteroidSpawner : IComponentData
{
    public Entity Prefab;
    public float Rate;
    public float MinSpeed;
    public float MaxSpeed;
    public float PathVariation;
    public float SpawnPosX;
    public float SpawnPosY;
}

public struct AsteroidSprite : IBufferElementData
{
    public Entity Sprite;
}

public struct Explosion : IComponentData
{
    public float Timer;
}

public struct ExplosionSpawner : IComponentData
{
    public Entity Prefab;
    public float TimePerSprite;
}

public struct ExplosionSprite : IBufferElementData
{
    public Entity Sprite;
}

public struct GameState : IComponentData
{
    public GameStates Value;
    public int Lives;
}

public struct GameShowState : IComponentData
{
    public GameStates Value;
}

public struct GameStateObject : IBufferElementData
{
    public Entity Value;
}