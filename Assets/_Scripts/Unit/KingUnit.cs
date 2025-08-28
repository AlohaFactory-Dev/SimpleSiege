using UnityEngine;

public class KingUnit : UnitController
{
    [SerializeField] private string id;

    public void SetKingUnit()
    {
        var kingData = TableListContainer.Get<UnitTableList>().GetUnitTable(id);
        Spawn(transform.position, kingData, false);
        ChangeState(UnitState.Siege);
    }
}