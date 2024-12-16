using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameBackend
{
    public class Weapon : EntityEventListener
    {
        public String name { get; set; }
        public String explaination { get; set; }
        public int atkBase { get; }
        public PlayerStatus addStatus { get; }
        
        public Skill normalSkill { get; }
        public Skill specialSkill { get; }
        
        public float timer { get; set; }
        private float normalAttackTimer { get; set; }
        private int atkCount { get; set; }
        public List<float> atkDelay { get; set; }
        public List<float> atkCancelDelay { get; set; }
        public List<float> normalAttackCoef { get; set; }
        public int atkCountMax { get; set; }

        public Weapon(String name, String explaination, int atkBase, PlayerStatus addStatus, Skill normalSkill, Skill specialSkill)
        {
            this.name = name;
            this.explaination = explaination;
            this.atkBase = atkBase;
            this.addStatus = addStatus;
            this.normalSkill = normalSkill;
            this.specialSkill = specialSkill;
            this.atkCount = 1;
        }

        public virtual void registrarPlayer(Player player){}

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

        public virtual void normalAttack(PlayerController player, float deltaTime)
        {
            timer += deltaTime;
            if (Input.GetMouseButtonDown(0))
            {
                player.animator.SetBool("IsAttacking", true);

                if (atkCount == 1 || timer > atkDelay[atkCount - 1])
                {
                    player.animator.SetTrigger($"NormalAttack{atkCount}");
                    
                    Entity[] entities = new[] { player.enemytest };
                    
                    List<Tags.atkTags> tags = new List<Tags.atkTags>();
                    tags.Add(Tags.atkTags.normalAttack);
                    tags.Add(Tags.atkTags.physicalAttack);
                    int damage=player.calculateTrueDmg(tags, normalAttackCoef[atkCount-1], 0, 0);
                    
                    player.executeNormalAttackEvent();

                    foreach (Entity entity in entities)
                    {
                        DmgEvent dmgEvent = new DmgEvent(player, entity, damage, tags);
                        player.dmggive(dmgEvent);
                        entity.dmgtake(dmgEvent);
                    }
                    
                    atkCount += 1;
                    if (atkCount > atkCountMax) atkCount = 1;
                    timer = 0;
                } 
            }
            else
            {
                if (atkCancelDelay[atkCount - 1] < timer)
                {
                    atkCount = 1;
                    timer = 0;
                    player.animator.SetBool("IsAttacking", false);
                }
            }
        }

        public virtual void applyBuff(Player player)
        {
            //player.addBuff();
        }

        public virtual void executeNormalSkill(Player attacker)
        {
            attacker.executeNormalSkillEvent();
            this.normalSkill.execute(attacker);
        }
        
        public virtual void executeSpecialSkill(Player attacker)
        {
            attacker.executeSpecialSkillEvent();
            this.specialSkill.execute(attacker);
        }
    }

    public class Hand : Weapon
    {
        public Hand() : base(
            "", 
            "", 
            0, 
            new PlayerStatus(), 
            new NoSkill(), 
            new NoSkill())
        {
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
    }
}