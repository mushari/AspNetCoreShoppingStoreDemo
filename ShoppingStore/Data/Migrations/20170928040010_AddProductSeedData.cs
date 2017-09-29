using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using ShoppingStore.Models;
using System;
using System.Collections.Generic;

namespace ShoppingStore.Data.Migrations
{
    public partial class AddProductSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var context = new ApplicationDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    var products = new List<Product>
                    {
                        new Product
                        {
                            ProductID=1,
                            Name="男初階款7號高爾夫球鋼鐵桿 INESIS",
                            Description="這款球桿使打球更加輕鬆，使您能夠從最終結果中獲得鼓勵",
                            Category="高爾夫",
                            Price=399M
                        },
                        new Product
                        {
                            ProductID=2,
                            Name="男百慕達高爾夫短褲 INESIS S",
                            Description="享受100％純棉百慕達短褲帶來的舒適感！適合於氣候炎熱時從事高爾夫運動。",
                            Category="高爾夫",
                            Price=400M
                        },
                        new Product
                        {
                            ProductID=3,
                            Name="可摺疊迷你桌球桌 ARTENGO FT Mini Colo 1",
                            Description="本款迷你桌球桌僅占一點空間，可摺疊且品質絕佳，也是一個幫助您提升精確度的好方法",
                            Category="桌球",
                            Price=1731M
                        },
                        new Product
                        {
                            ProductID=4,
                            Name="初階桌球拍套組 (2入) ARTENGO FR130",
                            Description="Artengo FR 130 球拍陪伴您從初學者到進階玩家！橡膠層良好且具黏性，幫助您改善控球與推進。",
                            Category="桌球",
                            Price=299M
                        },
                        new Product
                        {
                            ProductID=5,
                            Name="基本款羽毛球拍袋" ,
                            Description="這羽毛球包可以讓你輕鬆地將羽毛球攜帶至於你的訓練或比賽場地。多隔層設計讓你的運動裝備安全免受衝擊。",
                            Category="羽球",
                            Price=149M
                        },
                        new Product
                        {
                            ProductID=6,
                            Name="入門款塑膠羽毛球 ARTENGO BSC 700",
                            Description="本款單入裝的羽毛球採用塑膠球翼，可提供您穩定的軌跡顏色可使本款羽毛球具良好的可視性 ",
                            Category="羽球",
                            Price=29M
                        },
                        new Product
                        {
                            ProductID=7,
                            Name="選手練習款網球 ARTENGO",
                            Description="無壓、舒適、耐用網球是網球培訓期的首選。.",
                            Category="網球",
                            Price=19M
                        },
                        new Product
                        {
                            ProductID=8,
                            Name="成人基本款網球拍 ARTENGO TR 700",
                            Description="適中的拍面加強回球力度，橢圓形的握柄時抓握時更加舒適，這個球拍非常適合學習打網球的人。",
                            Category="網球",
                            Price=249M
                        },
                        new Product
                        {
                            ProductID=9,
                            Name="彈力4號足球 KIPSTA First Kick",
                            Description="本款足球不易變形，同時可有效維持彈力。",
                            Category="足球",
                            Price=130M
                        },
                        new Product
                        {
                            ProductID=10,
                            Name="英式橄欖球(4號球) KIPSTA Full H 300 4號",
                            Description="好抓好握又堅固耐用是這款球適合做為訓練球的特性，尤其適合初學者與英式橄欖球社團(俱樂部)。",
                            Category="足球",
                            Price=120M
                        },
                        new Product
                        {
                            ProductID=11,
                            Name="耐磨籃球(5號/7號) KIPSTA Tarmak 100 5",
                            Description="本款高度耐用的籃球可提供絕佳的控球力，可輕易地學會打籃球",
                            Category="籃球",
                            Price=149M
                        },
                        new Product
                        {
                            ProductID=12,
                            Name="台灣製mini籃板組合 KIPSTA",
                            Description="本迷你樹脂玻璃籃球架安裝容易且迅速！",
                            Category="籃球",
                            Price=599M
                        },
                        new Product
                        {
                            ProductID=13,
                            Name="標準戶外排球 KIPSTA 5號",
                            Description="學習排球的良伴。適合家庭使用。",
                            Category="排球",
                            Price=249M
                        },
                        new Product
                        {
                            ProductID=14,
                            Name="排球護膝 KIPSTA V 300 0",
                            Description="本款護膝最適合室內排球初學者使用。尺寸從兒童用0碼開始。",
                            Category="排球",
                            Price=249M
                        },
                        new Product
                        {
                            ProductID=15,
                            Name="鋁製棒球棒 KIPSTA",
                            Description="鋁製球棒耐用度高、打擊力強，適合棒球初學者。",
                            Category="棒球",
                            Price=899M
                        },
                        new Product
                        {
                            ProductID=16,
                            Name="安全泡棉棒球 KIPSTA",
                            Description="橡膠泡棉球棒讓棒球初學者使用起來更容易、安全。",
                            Category="棒球",
                            Price=69M
                        },

                    };
                    context.Products.AddRange(products);
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Products] ON");
                    context.SaveChanges();
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Products] OFF");

                    transaction.Commit();

                }
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
