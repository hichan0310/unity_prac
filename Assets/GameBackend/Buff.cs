using System;
using UnityEngine;

namespace GameBackend
{
    public class Buff : EntityEventListener
    {
        public float startTime { get; set; }
        public float coolDown { get; set; }

        public Buff(String name, String explaination, float coolDown)
        {
            this.name = name;
            this.explaination = explaination;
            startTime = Time.time;
            this.coolDown = coolDown;
        }
        
        public String name { get; set; }
        public String explaination { get; set; }
        public Entity target { get; set; }
        public virtual PlayerStatus statusUp(PlayerStatus before)
        {
            PlayerStatus after = new PlayerStatus(before);
            return after;
        }

        public virtual bool active()
        {
            return Time.time-startTime<coolDown;
        }

        public virtual void removeBuff()
        {
            this.target.removeBuff(this);
        }

        public virtual void registrarTarget(Entity target)
        {
            target.registrarListener(this);
        }

        public virtual void giveDmg(DmgEvent dmgEvent){}

        public virtual void takeDmg(DmgEvent dmgEvent){}

        public virtual void normalSkillActive(Entity player){}

        public virtual void specialSkillActive(Entity player){}

        public virtual void energySkillActive(Entity player){}
        
        public virtual void normalAttackExecute(Entity player){}
        
        public virtual void normalSkillExecute(Entity player){}

        public virtual void specialSkillExecute(Entity player){}

        public virtual void energySkillExecute(Entity player){}

        public virtual void hpDecrease(Entity player){}

        public virtual void hpIncrease(Entity player){}
    }
}