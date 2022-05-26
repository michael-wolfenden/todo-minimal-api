namespace TodoApi.Persistence;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        CreatedOn = DateTime.UtcNow;
        CreatedBy = "anonymous";

        LastModifiedOn = DateTime.UtcNow;
        LastModifiedBy = "anonymous";
    }

    public long Id { get; }
    public DateTime CreatedOn { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime? LastModifiedOn { get; private set; }
    public string LastModifiedBy { get; private set; }

    public void UpdateCreationProperties(DateTime createdOn, string createdBy)
    {
        CreatedOn = createdOn;
        CreatedBy = createdBy;
    }

    public void UpdateModifiedProperties(DateTime? lastModifiedOn, string lastModifiedBy)
    {
        LastModifiedOn = lastModifiedOn;
        LastModifiedBy = lastModifiedBy;
    }
}
