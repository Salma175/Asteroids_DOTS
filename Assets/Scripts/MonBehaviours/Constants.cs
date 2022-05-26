using System;

public enum GameStates
{
    None,
    Start,
    InGame
}

public enum AudioClipType
{
    None,
    AsteroidExplosion,
    PlayerExplosion,
    PlayerFire,
}


public static class Constants
{
    public const float RotationSpeed = 2f;
    public const float MoveSpeed = 4f;
    public const float FireRate = 3f;
    public const float MissileSpeed = 5f;
    public const float ShieldSpan = 5f;

    public const int Lives = 3;
    public const float SpawnPosX = 10;
    public const float SpawnPosY = 5;
    public const float SpawnRate = 1.5f;
    public const float MinSpeed = 1f;
    public const float MaxSpeed = 4f;
    public const float PathVariation = 0.1f;


    public const float LifeAnchorX = -6;
    public const float LifeAnchorY = 4.5f;

    public const int Zero = 0;

    public const float PowerUpPathVariation = 0.05f;
    public const float SpanTime = 20f;
    public const float PowerupSpeed = 1f;
}