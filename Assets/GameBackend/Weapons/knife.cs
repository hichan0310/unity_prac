using System.Collections.Generic;
using UnityEngine;

namespace GameBackend.Weapons
{
    public class BloodKnife:Weapon
    {
        public class SurpriseAttack : Skill
        {
            public SurpriseAttack() : base(
                "기습", 
                "", 
                5, 
                5)
            {
            }

            public override void execute(Player attacker)
            {
                base.execute(attacker);
                //범위 정하고 타겟 정해서 피해 가하는 것까지
            }
        }
        
        public class Blood : Buff
        {
            private int blood;
            private int maxblood = 100;
            
            public Blood() : base(
                "피", "피 스택으로 일공 피증", 0)
            {
            }

            public override bool active()
            {
                return true;
            }

            public override void giveDmg(DmgEvent dmgEvent)
            {
                this.blood += 3;
                if (blood > 100) blood = 100;
                base.giveDmg(dmgEvent);
            }

            public override void hpDecrease(Entity player)
            {
                this.blood += 2;
                if (blood > 100) blood = 100;
                base.hpDecrease(player);
            }

            public override void normalAttackExecute(Entity player)
            {
                Debug.Log($"{blood}");
                base.normalAttackExecute(player);
            }

            public override PlayerStatus statusUp(PlayerStatus before)
            {
                PlayerStatus b = base.statusUp(before);
                b.dmgUp[(int)(Tags.atkTags.normalAttack)] += blood*1;
                return b;
            }
        }
        
        
        public int stack { get; set; }
        public int stackMax = 5;
        public int blood { get; set; }
        public int bloodMax = 100;
        
        public BloodKnife() : base(
            "피의 나이프", 
            "날카로운 나이프다. 공격력을 20% 증가시킨다. " +
            "전투 스킬로 피 스택을 쌓아서 특수 공격을 강화한다. ",
            100, 
            new PlayerStatus().Apply(status=>status.atkPercent=20), 
            new SurpriseAttack(), 
            new NoSkill())
        {
            this.stack = 0;
            this.blood = 0;
            
            atkCountMax = 3;
            
            atkDelay = new List<float>();
            atkDelay.Add(0.3f);
            atkDelay.Add(0.3f);
            atkDelay.Add(0.3f);

            atkCancelDelay = new List<float>();
            atkCancelDelay.Add(0.6f);
            atkCancelDelay.Add(0.6f);
            atkCancelDelay.Add(0.6f);

            normalAttackCoef = new List<float>();
            normalAttackCoef.Add(50.0f);
            normalAttackCoef.Add(30.0f);
            normalAttackCoef.Add(100.0f);
        }

        public override void registrarPlayer(Player player)
        {
            base.registrarPlayer(player);
            player.addBuff(new Blood());
        }

        public override void executeNormalSkill(Player attacker)
        {
            this.blood += 5;
            base.executeNormalSkill(attacker);
        }

        public override void executeSpecialSkill(Player attacker)
        {
            this.blood = 0;
            base.executeSpecialSkill(attacker);
        }
    }
}