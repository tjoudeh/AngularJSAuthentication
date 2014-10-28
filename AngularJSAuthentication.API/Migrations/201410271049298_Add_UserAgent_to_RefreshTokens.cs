namespace AngularJSAuthentication.API.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Add_UserAgent_to_RefreshTokens : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RefreshTokens", "UserAgent", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RefreshTokens", "UserAgent");
        }
    }
}
