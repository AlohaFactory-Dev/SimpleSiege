namespace FactorySystem
{
    public class UnitFactroy : FactorySystem<UnitController, string>
    {
        protected override string LabelId => "Unit";

        protected override string TranslateStringKeyToID(string primaryKey)
        {
            return primaryKey;
        }

        public UnitController GetUnit(string attackObjectId)
        {
            tempObject = GetObject(attackObjectId);
            return tempObject;
        }
    }
}