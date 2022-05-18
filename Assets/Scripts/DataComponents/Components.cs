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