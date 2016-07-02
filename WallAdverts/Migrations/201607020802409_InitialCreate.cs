namespace WallAdverts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adverts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        DateCreate = c.DateTime(nullable: false),
                        AuthorId = c.Int(nullable: false),
                        ImageSrc = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Number = c.String(maxLength: 12),
                        Email = c.String(nullable: false),
                        DateBirthday = c.DateTime(nullable: false),
                        DateRegister = c.DateTime(nullable: false),
                        ImageSrc = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Adverts");
        }
    }
}
