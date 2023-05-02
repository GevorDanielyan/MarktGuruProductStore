using FluentMigrator;
using FluentMigrator.Postgres;

namespace Infrastructure.Migrations
{
    [Migration(202301302120)]
    public class Migration202301302120 : Migration
    {
        public override void Down()
        {
            Delete.Index("idx_product_name").OnTable("product");
            
            Delete.Table("product");

            Delete.Schema("public");
        }

        public override void Up()
        {
            // Check if the database exists
            if (!Schema.Schema("public").Exists())
            {
                // Create the database
                Create.Schema("public");
            }

            // Check if the table already exists
            if (!Schema.Table("product").Exists())
            {
                Create.Table("product")
                .WithColumn("id").AsGuid().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("price").AsDecimal(10, 2).NotNullable()
                .WithColumn("available").AsBoolean().NotNullable()
                .WithColumn("description").AsString()
                .WithColumn("dateCreated").AsDate().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime);

                Create.Index("idx_product_name").OnTable("product").OnColumn("name").Ascending().WithOptions().UsingBTree();
            }
        }
    }
}
