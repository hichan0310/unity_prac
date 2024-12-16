using System;

namespace GameBackend
{
    public static class Tags
    {
        public enum atkTags
        {
            normalAttack=0,
            normalSkill=1,
            specialSkill=2,
            strongAttack=3,
            physicalAttack=4,
            fireAttack=5,
            waterAttack=6,
            windAttack=7,
            earthAttack=8,
            lightningAttack=9,
            shadowAttack=10
        }

        public static int tagCount = Enum.GetValues(typeof(atkTags)).Length;
    }
}