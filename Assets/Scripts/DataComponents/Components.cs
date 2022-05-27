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
    public Entity DefaultMissile;
    public Entity LaserMissile;
    public Entity DoubleLaserMissile;
    public Entity Shield;
}

public struct PowerUp : IComponentData {
    public PowerUpType Type;
}
public struct Missile : IComponentData {
   // public ShotType Type;
}
public struct Enemy : IComponentData { }

public struct PowerUpSpawner : IComponentData
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

public struct EnemySpritePrefab : IBufferElementData
{
    public Entity SpritePrefab;
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

public struct GameParameters : IComponentData
{
    public GameState State;
    public ShotType Shot;
    public int Lives;
    public int Score;
    public bool IsSheildOn;
}

public struct GameShowState : IComponentData
{
    public GameState Value;
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

