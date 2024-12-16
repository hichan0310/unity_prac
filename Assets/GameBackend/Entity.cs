using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameBackend
{
    public class Entity : MonoBehaviour
    {
        public GameObject damageDisplay;
        public List<EntityEventListener> eventListener;
        private int maxHp { get; set; }
        public int hp { get; set; }
        public List<Buff> buffs { get; set; }

        public virtual void Awake()
        {
            eventListener = new List<EntityEventListener>();
            maxHp = 1;
            hp = 1;
        }

        public virtual void update()
        {
            foreach (Buff buff in buffs)
            {
                if(!buff.active()) this.removeBuff(buff);
            }
        }

        public bool isAlive()
        {
            return hp > 0;
        }

        public void registrarListener(EntityEventListener listener)
        {
            eventListener.Add(listener);
        }

        public void removeListener(EntityEventListener listener)
        {
            eventListener.Remove(listener);
        }   
        
        public virtual void addBuff(Buff buff)
        {
            this.buffs.Add(buff);
            buff.registrarTarget(this);
        }

        public virtual void removeBuff(Buff buff)
        {
            this.buffs.Remove(buff);
            this.removeListener(buff);
        }

        public virtual void dmggive(DmgEvent dmgEvent)
        {
            foreach (EntityEventListener listener in this.eventListener)
            {
                listener.giveDmg(dmgEvent);
            }
        }
        
        public virtual void dmgtake(DmgEvent dmgEvent)
        {
            foreach (EntityEventListener listener in this.eventListener)
            {
                listener.takeDmg(dmgEvent);
            }

            this.hp -= dmgEvent.damage;
            GameObject text = Instantiate(damageDisplay);
            text.GetComponent<DamageDisplay>().ShowDamage(dmgEvent);

            if (dmgEvent.damage > 0)
            {
                this.hpDecreaseEvent();
            }
            else if (dmgEvent.damage < 0)
            {
                this.hpIncreaseEvent();
            }
        }
        
        public virtual void activeNormalSkillEvent()
        {
            foreach (EntityEventListener listener in this.eventListener)
            {
                listener.normalSkillActive(this);
            }
        }
        
        public virtual void activeSpecialSkillEvent()
        {
            foreach (EntityEventListener listener in this.eventListener)
            {
                listener.specialSkillActive(this);
            }
        }
        
        public virtual void activeSnergySkillEvent()
        {
            foreach (EntityEventListener listener in this.eventListener)
            {
                listener.energySkillActive(this);
            }
        }

        public virtual void executeNormalAttackEvent()
        {
            foreach (EntityEventListener listener in this.eventListener)
            {
                listener.normalAttackExecute(this);
            }
        }
        
        public virtual void executeNormalSkillEvent()
        {
            foreach (EntityEventListener listener in this.eventListener)
            {
                listener.normalSkillExecute(this);
            }
        }
        
        public virtual void executeSpecialSkillEvent()
        {
            foreach (EntityEventListener listener in this.eventListener)
            {
                listener.specialSkillExecute(this);
            }
        }
        
        public virtual void executeEnergySkillEvent()
        {
            foreach (EntityEventListener listener in this.eventListener)
            {
                listener.energySkillExecute(this);
            }
        }

        public virtual void hpDecreaseEvent()
        {
            foreach (EntityEventListener listener in this.eventListener)
            {
                listener.hpDecrease(this);
            }
        }
        
        public virtual void hpIncreaseEvent()
        {
            foreach (EntityEventListener listener in this.eventListener)
            {
                listener.hpIncrease(this);
            }
        }
    }
}