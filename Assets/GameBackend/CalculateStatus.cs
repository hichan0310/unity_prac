using System.Collections.Generic;
using System;
using GameBackend;
using Unity.VisualScripting;
using UnityEngine;


namespace GameBackend
{
    //버프, 장비가 추가될 때는 PlayerStatus로 계산하지만
    //결국 최종 형태는 Status 타입으로 계산됨
    //합연산, 곱연산 적당히 있으면서 교환법칙 성립함
    //원신의 스텟 계산 방식에서 아이디어 얻음
    
    public static class ObjectExtensions
    {
        public static T Apply<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }
    }
    
    
    public class Status
    {
        public int hpMax { get; set; }
        public int hp { get; set; }
        public int def { get; set; }
        public int atk { get; set; }
        public int mp { get; set; }
        public float crit { get; set; }
        public float critdmg { get; set; }
        public float[] dmgUp = new float[Tags.tagCount];

        public Status(int hpMax, int hp, int def, int atk, int mp, float crit, float critdmg, float[] dmgUp)
        {
            this.hpMax = hpMax;
            this.hp = hp;
            this.def = def;
            this.atk = atk;
            this.mp = mp;
            this.crit = crit;
            this.critdmg = critdmg;
            this.dmgUp = (float[])dmgUp.Clone();
        }

        public void applyStatus(PlayerStatus statusBuff)
        {
            this.hpMax *= statusBuff.hpPercent / 100 + 1;
            this.hpMax += statusBuff.hpAdd;
            this.hp *= statusBuff.hpPercent / 100 + 1;
            this.hp += statusBuff.hpAdd;
            this.def *= statusBuff.defPercent / 100 + 1;
            this.def += statusBuff.defAdd;
            this.atk *= statusBuff.atkPercent / 100 + 1;
            this.atk += statusBuff.atkAdd;
            this.mp *= statusBuff.mpPercent / 100 + 1;
            this.mp += statusBuff.mpAdd;
            this.crit += statusBuff.crit;
            this.critdmg += statusBuff.critDmg;
            for (int i = 0; i < Tags.tagCount; i++)
            {
                this.dmgUp[i] += statusBuff.dmgUp[i];
            }
        }
    }

    public class PlayerStatus
    {
        public int hpAdd { get; set; }
        public int defAdd { get; set; }
        public int atkAdd { get; set; }
        public int hpPercent{ get; set; }
        public int defPercent{ get; set; }
        public int atkPercent{ get; set; }
        public int mpAdd{ get; set; }
        public int mpPercent { get; set; }
        public float crit{ get; set; }
        public float critDmg{ get; set; }
        public float[] dmgUp = new float[Tags.tagCount];

        public PlayerStatus(PlayerStatus status)
        {
            this.hpAdd = status.hpAdd;
            this.defAdd = status.defAdd;
            this.atkAdd = status.atkAdd;
            this.hpPercent = status.hpPercent;
            this.defPercent = status.defPercent;
            this.atkPercent = status.atkPercent;
            this.mpAdd = status.mpAdd;
            this.mpPercent = status.mpPercent;
            this.crit = status.crit;
            this.critDmg = status.critDmg;
            this.dmgUp = (float[])status.dmgUp.Clone();
        }

        public PlayerStatus(
            int hpAdd,
            int defAdd,
            int atkAdd,
            int hpPercent,
            int defPercent,
            int atkPercent,
            int mpAdd,
            int mpPercent,
            float crit,
            float critDmg,
            float[] dmgUp)
        {
            this.hpAdd = hpAdd;
            this.defAdd = defAdd;
            this.atkAdd = atkAdd;
            this.hpPercent = hpPercent;
            this.defPercent = defPercent;
            this.atkPercent = atkPercent;
            this.mpAdd = mpAdd;
            this.mpPercent = mpPercent;
            this.crit = crit;
            this.critDmg = critDmg;
            this.dmgUp = (float[])dmgUp.Clone();
        }

        public PlayerStatus()
        {
            this.hpAdd = 0;
            this.defAdd = 0;
            this.atkAdd = 0;
            this.hpPercent = 0;
            this.defPercent = 0;
            this.atkPercent = 0;
            this.mpAdd = 0;
            this.mpPercent = 0;
            this.crit = 0;
            this.critDmg = 0;
            this.dmgUp = new float[Tags.tagCount];
        }
    }
    
    
}