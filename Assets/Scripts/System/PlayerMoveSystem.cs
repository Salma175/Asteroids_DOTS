using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

partial class PlayerMoveSystem : SystemBase
{
    PlayerControls pc;
    InputAction movement;
    InputAction fire;

    protected override void OnCreate() {
        base.OnCreate();

        pc = new PlayerControls();
        pc.Player.Enable();
        movement = pc.Player.Move;
        fire = pc.Player.Fire;

        fire.performed += Fire_performed;
    }

    private void Fire_performed(InputAction.CallbackContext obj)
    {
        float spaceKey = obj.ReadValue<float>();

        Debug.Log("Fire " + spaceKey);
    }


    protected override void OnUpdate()
    {
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
