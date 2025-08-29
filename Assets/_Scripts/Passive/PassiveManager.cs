using System.Collections.Generic;
using System.Linq;

public class PassiveManager
{
    private List<PassiveTable> _passives;
    private readonly int _count = 3;
    private DeckSelectionManager _deckSelectionManager;
    private List<string> _deckUnitIds = new List<string>();
    private List<PassiveTable> _activePassives = new List<PassiveTable>();
    public IReadOnlyList<PassiveTable> ActivePassives => _activePassives.AsReadOnly();

    private UnitManager _unitManager;

    public PassiveManager(DeckSelectionManager deckSelectionManager, UnitManager unitManager)
    {
        _passives = new List<PassiveTable>(TableListContainer.Get<PassiveTableList>().GetAllPassiveTables());
        _deckSelectionManager = deckSelectionManager;
        _unitManager = unitManager;
    }

    public void Init()
    {
        _deckUnitIds = _deckSelectionManager.SelectedCardDatas.Select(cd => cd.id).ToList();
        _passives = _passives.Where(p => p.targetIds.Any(id => _deckUnitIds.Contains(id) || id == "All")).ToList();
    }


    public List<PassiveTable> PickRandomPassives()
    {
        var selected = new List<PassiveTable>();
        var candidates = new List<PassiveTable>(_passives);

        for (int i = 0; i < _count && candidates.Count > 0; i++)
        {
            int total = candidates.Sum(p => p.probability);
            int rand = UnityEngine.Random.Range(0, total);
            int acc = 0;
            foreach (var passive in candidates)
            {
                acc += passive.probability;
                if (rand < acc)
                {
                    selected.Add(passive);
                    candidates.Remove(passive);
                    break;
                }
            }
        }

        return selected;
    }

    public void SelectPassive(PassiveTable passiveTable)
    {
        ActivePassive(passiveTable);
        _passives.Remove(passiveTable);
    }


    private void ActivePassive(PassiveTable passiveTable)
    {
        if (_activePassives.Contains(passiveTable)) return;
        _activePassives.Add(passiveTable);
        foreach (var unit in _unitManager.PlayerUnits)
        {
            if (passiveTable.targetIds.Contains(unit.UnitTable.id) || passiveTable.targetIds.Contains("All"))
            {
                unit.UnitUpgradeController.ApplyPassive(passiveTable);
            }
        }
    }
}