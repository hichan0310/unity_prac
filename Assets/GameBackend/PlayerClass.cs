namespace GameBackend
{
    public class PlayerClass
    {
        public int level { get; set; }
        public int hpBase { get; set; }
        public int defBase { get; set; }
        public int atkBase { get; set; }
        public int mp { get; set; }
        public Skill passive { get; set; }
    }
}