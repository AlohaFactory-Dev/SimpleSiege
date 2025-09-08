using UnityEngine;

namespace FactorySystem
{
    public class MapFactory : FactorySystem<MapController, string>
    {
        protected override string LabelId => "Unit";

        protected override string TranslateStringKeyToID(string primaryKey)
        {
            return primaryKey;
        }

        public MapController GetMap(string attackObjectId)
        {
            tempObject = GetObject(attackObjectId);
            return tempObject;
        }
    }
}