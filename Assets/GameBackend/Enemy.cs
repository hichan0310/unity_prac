using System.Collections.Generic;
using UnityEngine;

namespace GameBackend
{
    public class Enemy:Entity
    {
        public int def;

        public virtual void ai()
        {
            
        }

        public override void dmgtake(DmgEvent dmgEvent)
        {
            int realDmg = dmgEvent.damage;
            int C = 200;
            realDmg -= (int)(realDmg * ((float)(def) / (def + C)));

            DmgEvent dmgEventReal = new DmgEvent(dmgEvent.attacker, dmgEvent.target, realDmg, dmgEvent.damageTags);
            base.dmgtake(dmgEventReal);
        }
    }
}