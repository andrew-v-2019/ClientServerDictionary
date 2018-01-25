namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Terms",
                c => new
                    {
                        TermId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TermId);
            
            CreateTable(
                "dbo.Translations",
                c => new
                    {
                        TranslationId = c.Int(nullable: false, identity: true),
                        TermId = c.Int(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.TranslationId)
                .ForeignKey("dbo.Terms", t => t.TermId, cascadeDelete: true)
                .Index(t => t.TermId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Translations", "TermId", "dbo.Terms");
            DropIndex("dbo.Translations", new[] { "TermId" });
            DropTable("dbo.Translations");
            DropTable("dbo.Terms");
        }
    }
}
