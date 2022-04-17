namespace GusMelfordBot.DAL.Applications.ContentCollector;

public class Content : DatabaseEntity
{
    public Chat Chat { get; set; }
    public User User { get; set; }
    public string ContentProvider { get; set; }
    public string SentLink { get; set; }
    public string AccompanyingCommentary { get; set; }
    public bool IsViewed { get; set; }
    public bool IsValid { get; set; } = true;
    public string RefererLink { get; set; }
    public string Description { get; set; }
}