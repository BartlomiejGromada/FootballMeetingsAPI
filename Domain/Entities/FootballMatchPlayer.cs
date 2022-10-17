namespace Domain.Entities
{
    public class FootballMatchPlayer
    {
        public FootballMatch FootballMatch { get; set; }
        public int FootballMatchId { get; set; }
        public User Player { get; set; }
        public int PlayerId { get; set; }

        public DateTime JoiningDate { get; set; }
    }
}
