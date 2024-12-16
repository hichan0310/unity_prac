using System.Collections.Generic;

namespace GameBackend
{
    public class DmgEvent{
        public Entity attacker { get; }
        public Entity target { get; }
        public int damage { get; }
        public List<Tags.atkTags> damageTags { get; }

        public DmgEvent(Entity attacker, Entity target, int damage, List<Tags.atkTags> damageTags)
        {
            this.attacker = attacker;
            this.target = target;
            this.damage = damage;
            this.damageTags = damageTags;
        }
    };
    
    public interface EntityEventListener
    {
        public void giveDmg(DmgEvent dmgEvent);
        public void takeDmg(DmgEvent dmgEvent);
        public void normalSkillActive(Entity player);
        public void specialSkillActive(Entity player);
        public void energySkillActive(Entity player);
        public void normalAttackExecute(Entity player);
        public void normalSkillExecute(Entity player);
        public void specialSkillExecute(Entity player);
        public void energySkillExecute(Entity player);
        public void hpDecrease(Entity player);
        public void hpIncrease(Entity player);
    }
}