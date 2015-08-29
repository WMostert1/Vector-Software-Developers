namespace DataAccess.Interface.Domain
{
    public class ScoutBug
    {
        public int NumberOfBugs { get; set; }
        public string Comments { get; set; }

        public Species Species { get; set; }
    }
}
