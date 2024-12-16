using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameBackend
{
    public class Skill
    {
        public String name { get; set; }
        public String explaination { get; set; }
        public float coolTime { get; set; }
        public float lastActive { get; set; }
        public int mana { get; set; }

        public Skill(String name, String explaination, int coolTime, int mana)
        {
            this.name = name;
            this.explaination = explaination;
            this.coolTime = coolTime;
            this.lastActive = -coolTime;
        }

        public virtual bool active(Player attacker)
        {
            return Time.time-lastActive>coolTime && attacker.mp>=mana;
        }

        public virtual void execute(Player attacker)
        {
            attacker.mp -= mana;
            lastActive = Time.time;
        }

        public virtual List<Entity> GetHitboxEntities(Player attacker)
        {
            return new List<Entity>();
        }
    }

    public class NoSkill : Skill
    {
        public NoSkill() : base("", "", 0, 0)
        {
        }
    }
}