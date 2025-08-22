using UnityEngine;

namespace _Scripts.Unit
{
    public class UnitStatusSystem : MonoBehaviour
    {
        public UnitMoveSystem MoveSystem { get; private set; }
        public UnitActionSystem ActionSystem { get; private set; }
        public UnitAnimationSystem AnimationSystem { get; private set; }

        public void Init(UnitController unitController)
        {
            MoveSystem = GetComponentInChildren<UnitMoveSystem>();
            ActionSystem = GetComponentInChildren<UnitActionSystem>();
            AnimationSystem = GetComponentInChildren<UnitAnimationSystem>();

            MoveSystem.Init(unitController);
            ActionSystem.Init(unitController);
            AnimationSystem.Init();
        }

        public void ApplyState(Transform target, UnitState state)
        {
            switch (state)
            {
                case UnitState.Move:
                    MoveSystem.StartMove(target);
                    ActionSystem.StopAction();
                    AnimationSystem.PlayMove();
                    break;
                case UnitState.Action:
                    ActionSystem.StartAction(target);
                    MoveSystem.StopMove();
                    AnimationSystem.PlayAttack();
                    break;
                default:
                    MoveSystem.StopMove();
                    ActionSystem.StopAction();
                    AnimationSystem.PlayIdle();
                    break;
            }
        }
    }
}