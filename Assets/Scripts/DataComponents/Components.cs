using Unity.Entities;


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