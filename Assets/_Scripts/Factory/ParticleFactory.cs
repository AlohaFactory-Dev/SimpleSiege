using Aloha.Particle;

namespace FactorySystem
{
    public class ParticleFactory : FactorySystem<RecycleParticle, string>
    {
        protected override string LabelId => "Particle";

        protected override string TranslateStringKeyToID(string primaryKey)
        {
            return primaryKey;
        }

        public RecycleParticle GetParticle(string particleId)
        {
            tempObject = GetObject(particleId);
            return tempObject;
        }
    }
}