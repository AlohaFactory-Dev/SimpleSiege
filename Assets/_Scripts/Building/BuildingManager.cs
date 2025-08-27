using System.Collections.Generic;

public class BuildingManager
{
    List<Building> _buildings = new List<Building>();

    public void AddBuilding(Building building)
    {
        if (!_buildings.Contains(building))
            _buildings.Add(building);
    }

    public void RemoveBuilding(Building building)
    {
        if (_buildings.Contains(building))
            _buildings.Remove(building);
    }
}