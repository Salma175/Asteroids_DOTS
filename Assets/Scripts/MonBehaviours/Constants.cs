using System;

public enum GameState
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

public enum PowerUpType
{
    None,
    Shield,
    Laser,
    DoubleLaser,
}

public static class Constants
{
    public static class FireRate { 
        public const float Default = 3f;
        public const float Laser = 3f;
        public const float DoubleLaser = 3f;
    }

    public const float RotationSpeed = 2f;
    public const float MoveSpeed = 4f;
    public const float MissileSpeed = 5f;
    public const float ShieldSpan = 20f;

    public const int Lives = 3;
    public const float SpawnPosX = 10;
    public const float SpawnPosY = 5;
    public const float SpawnRate = 1f;
    public const float MinSpeed = 1f;
    public const float MaxSpeed = 3f;
    public const float PathVariation = 0.1f;


    public const float LifeAnchorX = -6;
    public const float LifeAnchorY = 4.5f;

    public const int Zero = 0;

    public const float PowerUpPathVariation = 0.05f;
    public const float SpanTime = 20f;
    public const float PowerupSpeed = 1f;
}