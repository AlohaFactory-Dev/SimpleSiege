using UnityEngine;

public class SpellController
{
    public void CastSpell(Vector3 position, string id)
    {
        var table = TableListContainer.Get<SpellTableList>().GetSpellTable(id);
        // 실제 마법 효과 적용 로직 (예: 범위 내 적에게 피해, 회복 등)
    }
}