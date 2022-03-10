namespace JustHelpDesk.Models;

public class IssueResource
{
    public DateTime CreatedDate { get; set; }

    public long Id { get; set; }

    public long CreatedUserId {get;set;}

    public string? Title { get; set; }

    public int TypeCode {get;set;}
}
