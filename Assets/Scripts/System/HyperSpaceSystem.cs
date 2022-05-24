using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial class HyperSpaceSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var playerEntity = GetSingletonEntity<Player>();

        Entities.ForEach((ref Translation trans, in Player player) => {

            var position = trans.Value;
            //Check if player out of bounds
            var point = Camera.main.WorldToViewportPoint(position);
            // Out of Bounds from Left or Right
            if (point.x < 0 || point.x > 1) {
                EntityManager.SetComponentData(playerEntity, new Translation
                {
                    Value = new float3(position.x*-1, position.y, position.z)
                });
            }
            // Out of Bounds from Up or Down
            if (point.y > 1 || point.y < 0)
            {
                EntityManager.SetComponentData(playerEntity, new Translation
                {
                    Value = new float3(position.x, position.y * -1, position.z)
                });
            }
        }).WithoutBurst().Run();
    }
}
