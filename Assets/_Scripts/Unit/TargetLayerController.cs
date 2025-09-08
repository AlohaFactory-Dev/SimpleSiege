using UnityEngine;

public static class TargetLayerController
{
    private static readonly int EnemyLayerMask = LayerMask.NameToLayer("EnemyDetector");
    private static readonly int PlayerLayerMask = LayerMask.NameToLayer("PlayerDetector");
    private static readonly int BuildingLayerMask = LayerMask.NameToLayer("BuildingDetector");

    public static int GetLayerMaskByTargetType(TeamType teamType, TargetType targetType, TargetGroup targetGroup)
    {
        if (targetGroup == TargetGroup.Building)
        {
            return BuildingLayerMask;
        }

        if (teamType == TeamType.Player)
        {
            if (targetType == TargetType.Enemy)
            {
                return EnemyLayerMask;
            }

            if (targetType == TargetType.Ally)
            {
                return PlayerLayerMask;
            }
        }
        else
        {
            if (targetType == TargetType.Enemy)
            {
                return PlayerLayerMask;
            }

            if (targetType == TargetType.Ally)
            {
                return EnemyLayerMask;
            }
        }

        return EnemyLayerMask | PlayerLayerMask;
    }
}