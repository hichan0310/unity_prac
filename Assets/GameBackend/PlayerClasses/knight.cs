using System.Collections.Generic;
using GameBackend;
using UnityEditor;
using UnityEngine;

//제네릭으로 깔끔하게 처리 가능할 것 같은데 오늘은 여기까지만

namespace GameBackend.PlayerClasses
{
    public class Knight : PlayerClass
    {
        public class Gaurd:Buff
        {
            public const int maxGaurd = 3;
            public int gaurd { get; set; }
            
            public Gaurd() : base(
                "가드", 
                "지속시간 : 8초" +
                "방어력을 100% 증가시키고 피격을 받을 때마다 가드 스택을 최대 3까지 쌓는다. " +
                "가드 스택이 최대치에 도달하거나 가드 수치가 0이 아닌 상태에서 지속시간이 종료되면 에너지를 방출하여 주변에 50%+10%*(가드 스택) 계수의 물리 피해를 가한다. ", 
                5)
            {
                gaurd = 0;
            }

            public override void registrarTarget(Entity target)
            {
                this.target = target;
                base.registrarTarget(target);
            }
            
            public override void takeDmg(DmgEvent dmgEvent)
            {
                gaurd += 1;
                if (gaurd == maxGaurd)
                {
                    this.removeBuff();
                }
            }

            public override PlayerStatus statusUp(PlayerStatus before)
            {
                PlayerStatus b = base.statusUp(before);
                b.defPercent += 100;
                return b;
            }

            public override void removeBuff()
            {
                if (gaurd > 0)
                {
                    PlayerController pl=this.target.GetComponent<PlayerController>();
                    List<Tags.atkTags> tag = new List<Tags.atkTags>();
                    tag.Add(Tags.atkTags.fireAttack);
                    DmgEvent dmgEvent = new DmgEvent(pl, pl.enemytest, 100, tag);
                    
                    pl.dmggive(dmgEvent);
                    pl.enemytest.dmgtake(dmgEvent);
                }
                base.removeBuff();
            }
        }
        
        public class SetGaurd:Skill
        {
            public SetGaurd():base("", "", 10, 0) { }
            public override void execute(Player attacker)
            {
                base.execute(attacker);
                attacker.addBuff(new Gaurd());
                //motion?
            }
        }
        
        public Knight()
        {
            this.level = 100;
            this.hpBase = 10000;
            this.defBase = 1000;
            this.atkBase = 400;
            this.mp = 100;
            this.passive = new SetGaurd();
        }
    }
}