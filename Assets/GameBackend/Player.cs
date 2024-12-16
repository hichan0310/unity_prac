using System;
using System.Collections.Generic;
using GameBackend.PlayerClasses;
using GameBackend.Weapons;
using Unity.VisualScripting;
using UnityEngine;

namespace GameBackend
{
    public class Player : Entity
    {
        public PlayerClass playerClassInfo { get; set; }

        public int level { get; set; }
        public int hpBase { get; set; }
        public int defBase { get; set; }
        public int atkBase { get; set; }
        public int mp { get; set; }
        public float crit { get; set; }
        public float critDmg { get; set; }
        public float[] dmgUp { get; set; }
        public Weapon weapon { get; set; }

        public Skill passive { get; set; }

        public override void update()
        {
            base.update();
            if(!isAlive()) Debug.Log("die");
            if(passive.active(this)) passive.execute(this);
        }

        public override void Awake()
        {
            base.Awake();
            PlayerClass pclass = new Knight();
            this.playerClassInfo = pclass;
            this.level = pclass.level;
            this.hpBase = pclass.hpBase;
            this.hp = this.hpBase;
            this.defBase = pclass.defBase;
            this.atkBase = pclass.atkBase;
            this.mp = pclass.mp;
            this.crit = 5.0f;
            this.critDmg = 50.0f;
            this.dmgUp = new float[Tags.tagCount];
            this.buffs = new List<Buff>();
            
            this.weapon = new BloodKnife();
            
            this.weapon.registrarPlayer(this);
            this.registrarListener(this.weapon);
            this.passive = pclass.passive;
        }

        public override void dmgtake(DmgEvent dmgEvent)
        {
            //damage 계산(방어력)
            Status status = this.getStatus();
            int realDmg = dmgEvent.damage;
            int C = 200;
            realDmg -= (int)(realDmg * ((float)(status.def) / (status.def + C)));

            DmgEvent dmgEventReal = new DmgEvent(dmgEvent.attacker, dmgEvent.target, realDmg, dmgEvent.damageTags);
            base.dmgtake(dmgEventReal);
        }

        public PlayerStatus getPlayerStatus()
        {
            PlayerStatus status = new PlayerStatus();
            foreach (Buff buff in buffs)
            {
                status = buff.statusUp(status);
            }

            return status;
        }

        public void applyWeapon(Weapon weapon)
        {
            this.weapon = weapon;
            this.atkBase = this.playerClassInfo.atkBase + this.weapon.atkBase;
            this.weapon.applyBuff(this);
        }

        public Status getStatus()
        {
            Status status = new Status(hpBase, hp, defBase, atkBase, mp, crit, critDmg, dmgUp);
            status.applyStatus(getPlayerStatus());
            return status;
        }

        public int calculateTrueDmg(List<Tags.atkTags> atkTagsList, float atkCoefficient, float hpCoefficient = 0,
            float defCoefficient = 0)
        {
            Status status = this.getStatus();
            float dmg = status.atk * atkCoefficient / 100 +
                        status.hpMax * hpCoefficient / 100 +
                        status.def * defCoefficient / 100;
            float dmgUpSum = 100;
            foreach (Tags.atkTags tag in atkTagsList)
            {
                dmgUpSum += status.dmgUp[(int)tag];
            }
            return (int)(dmg * dmgUpSum / 100);
        }
    }
}