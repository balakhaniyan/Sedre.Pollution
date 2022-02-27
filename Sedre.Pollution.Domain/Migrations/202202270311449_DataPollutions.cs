namespace Sedre.Pollution.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataPollutions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DayIndicators",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.Int(nullable: false),
                        ALatitude = c.Double(nullable: false),
                        ALongitude = c.Double(nullable: false),
                        BLatitude = c.Double(nullable: false),
                        BLongitude = c.Double(nullable: false),
                        CLatitude = c.Double(nullable: false),
                        CLongitude = c.Double(nullable: false),
                        DLatitude = c.Double(nullable: false),
                        DLongitude = c.Double(nullable: false),
                        O3 = c.Double(nullable: false),
                        Co = c.Double(nullable: false),
                        No2 = c.Double(nullable: false),
                        So2 = c.Double(nullable: false),
                        Pm10 = c.Double(nullable: false),
                        Pm25 = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HourIndicators",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.Int(nullable: false),
                        Time = c.Int(nullable: false),
                        ALatitude = c.Double(nullable: false),
                        ALongitude = c.Double(nullable: false),
                        BLatitude = c.Double(nullable: false),
                        BLongitude = c.Double(nullable: false),
                        CLatitude = c.Double(nullable: false),
                        CLongitude = c.Double(nullable: false),
                        DLatitude = c.Double(nullable: false),
                        DLongitude = c.Double(nullable: false),
                        O3 = c.Double(nullable: false),
                        Co = c.Double(nullable: false),
                        No2 = c.Double(nullable: false),
                        So2 = c.Double(nullable: false),
                        Pm10 = c.Double(nullable: false),
                        Pm25 = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MonthIndicators",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.Int(nullable: false),
                        ALatitude = c.Double(nullable: false),
                        ALongitude = c.Double(nullable: false),
                        BLatitude = c.Double(nullable: false),
                        BLongitude = c.Double(nullable: false),
                        CLatitude = c.Double(nullable: false),
                        CLongitude = c.Double(nullable: false),
                        DLatitude = c.Double(nullable: false),
                        DLongitude = c.Double(nullable: false),
                        O3 = c.Double(nullable: false),
                        Co = c.Double(nullable: false),
                        No2 = c.Double(nullable: false),
                        So2 = c.Double(nullable: false),
                        Pm10 = c.Double(nullable: false),
                        Pm25 = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MonthIndicators");
            DropTable("dbo.HourIndicators");
            DropTable("dbo.DayIndicators");
        }
    }
}
