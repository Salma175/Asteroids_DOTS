using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

partial class PlayerMoveSystem : SystemBase
{
    PlayerControls pc;
    InputAction movement;

    protected override void OnCreate() {
        base.OnCreate();

        pc = new PlayerControls();
        pc.Player.Enable();
        movement = pc.Player.Move;

    }

    protected override void OnUpdate()
    {
        var gameState = GetSingleton<GameState>();
        if (gameState.Value != GameStates.InGame)
            return;

        float hAxis = movement.ReadValue<Vector2>().x;
        float vAxis = movement.ReadValue<Vector2>().y;


        Entities.WithoutBurst().ForEach(
            (ref Translation trans,
             ref Rotation rot,
             in Player player) => {

                 rot.Value = math.mul(rot.Value, quaternion.RotateZ(hAxis * player.RotationSpeed * Time.DeltaTime));

                 float3 pos = float3.zero;
                 pos.y += vAxis * player.MoveSpeed *Time.DeltaTime;

                 trans.Value += math.mul(rot.Value, pos);

             }).Run();

    }
}
