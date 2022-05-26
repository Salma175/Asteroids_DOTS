using System;
using Unity.Entities;
using UnityEngine;

public struct Player : IComponentData
{
    public float RotationSpeed;
    public float MoveSpeed;
    public float FireRate;
    public float MissileSpeed;
    public float ShieldSpan;
    public Entity MissilePrefab;
    public Entity Shield;
}

public struct Shield : IComponentData { }
public struct Missile : IComponentData { }
public struct Enemy : IComponentData { }

public struct ShieldSpawner : IComponentData
{
    public Entity Prefab;
    public float Speed;
    public float PathVariation;
    public float SpawnPosX;
    public float SpawnPosY;
    public float SpanTime;
}

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

public struct EnemySprite : IBufferElementData
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
    public int Score;
    public bool IsSheildOn;
}

public struct GameShowState : IComponentData
{
    public GameStates Value;
}

public struct GameStateObject : IBufferElementData
{
    public Entity Value;
}

public struct LifeManager : IComponentData
{
    public Entity LifePrefab;
}

public struct Life : IBufferElementData
{
    public Entity Value;
}

