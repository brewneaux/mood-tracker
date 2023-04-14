using FluentMigrator.Runner.VersionTableInfo;

namespace MoodTrackerAPI.DatabaseMigrations;

[VersionTableMetaData]
public class CustomVersionTableMetaData : IVersionTableMetaData
{
    public virtual string SchemaName => "";

    public virtual string TableName => "version_info";

    public virtual string ColumnName => "version";

    public virtual string UniqueIndexName => "UC_Version";

    public virtual string AppliedOnColumnName => "applied_on";

    public virtual string DescriptionColumnName => "description";

    public object ApplicationContext { get; set; }
    public virtual bool OwnsSchema => true;
}