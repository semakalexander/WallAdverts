namespace WallAdverts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Adverts", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Adverts", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Adverts", "Description", c => c.String());
            AlterColumn("dbo.Adverts", "Name", c => c.String());
        }
    }
}
