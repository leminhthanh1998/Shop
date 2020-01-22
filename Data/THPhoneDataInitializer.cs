using Microsoft.AspNetCore.Identity;
using Shop.Models.Domain;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Shop.Models;
using Shop.Models.Domain.Enum;

namespace Shop.Data
{
    public class THPhoneDataInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public THPhoneDataInitializer(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                Category nokia = new Category("Nokia", "fa-star");
                Category samsung = new Category("Samsung", "fa-star");
                Category oppo = new Category("Oppo", "fa-star");
                Category huawei = new Category("Huawei", "fa-star");
                Category xiaomi = new Category("Xiomi", "fa-star");

                _dbContext.Category.AddRange(nokia, xiaomi, samsung, oppo, huawei);



                _dbContext.SaveChanges();

               //NGuoi dung nay se dung khi mua hang khong tao tai khoan
                _dbContext.User.Add(new User
                {
                    EmailAddress = "nguoidung@gmail.com",
                    FirstName = "Dung",
                    FamilyName = "Nguoi",
                    PhoneNumber = "0317258529",
                    Address = "long son, p ky hoa, q tan quy, tp sam son",
                    Sex = Sex.Male
                });
                //admin example
                await CreateUser("leminhthanh1998@outlook.com", "leminhthanh1998@outlook.com", "123456", "admin");
                await CreateUser("nguoidung@gmail.com", "nguoidung@gmail.com", "nguoidung", "customers");
                _dbContext.User.Add(new User
                {
                    EmailAddress = "leminhthanh1998@outlook.com",
                    FirstName = "Thanh",
                    FamilyName = "Le",
                    PhoneNumber = "0317258529",
                    Address = "Long An",
                    Sex = Sex.Male
                });
                //_dbContext.User.Add(new User
                //{
                //    EmailAddress = "longvox98@gmail.com",
                //    FirstName = "Long",
                //    FamilyName = "Hoang",
                //    PhoneNumber = "0317258529",
                //    Address = "long son, p ky hoa, q tan quy, tp hue",
                //    Sex = Sex.Male
                //});
                //_dbContext.User.Add(new User
                //{
                //    EmailAddress = "nguoidung@gmail.com",
                //    FirstName = "Dung",
                //    FamilyName = "Nguoi",
                //    PhoneNumber = "0317258529",
                //    Address = "long son, p ky hoa, q tan quy, tp sam son",
                //    Sex = Sex.Male
                //});

                _dbContext.SaveChanges();

                //seller example
                //var user = new ApplicationUser { UserName = "3bros.suport@gmail.com", Email = "3bros.suport@gmail.com", EmailConfirmed = true };
                //var password = "banhang";
                //var result = await _userManager.CreateAsync(user, password);

                //_dbContext.User.Add(new User
                //{
                //    EmailAddress = "3bros.suport@gmail.com",
                //    FirstName = "Ban",
                //    FamilyName = "Hang",
                //    PhoneNumber = "0879654123",
                //    Address = "tan hanh, p phu quy, q son lam, tp huong tra",
                //    Sex = Sex.Male
                //});
                //await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "seller"));

                Seller localMarket = new Seller("TH-Phone", "thphone.suport@gmail.com", "0123456789", "abc", "Denderstraat", "22", "9300", "Aalst", true);
                _dbContext.Seller.Add(localMarket);

                //Items localMarketItem = new Items("Nokia 8.1", 6590000, "Điện thoại Nokia 8.1", 27, @"images\items\71\", nokia, "Denderstraat", "22", "9300", "Aalst", localMarket, Offer.No, true);
                //_dbContext.Items.Add(localMarketItem);
                //Ten, gia, mo ta, so lan da ban, thu muc chua hinh, hang, , co khuyen mai khong, isenable
                //Nokia 
                //Items Items01 = new Items("Nokia 6.1 plus(x6)", 3450000, "Điện thoại Nokia 6.1 Plus được lột xác khi sử dụng kết hợp giữa khung kim loại và kính cao cấp đã tạo nên một dáng vẻ sang trọng và đem đến tính thẩm mỹ cũng như sự trẻ trung, năng động. Các viền góc của Nokia 6.1 Plus được bo tròn vô cùng tỉ mỉ, tạo cảm giác vừa chắc chắn vừa nhẹ nhàng khi cầm nắm.", 17, @"images\items\1\", nokia, localMarket, Offer.No, true);
                //Items Items02 = new Items("Nokia 7 plus", 4000000, "Nokia 7 Plus là chiếc smartphone đầu tiên đánh dấu bước đi đầu tiên của Nokia vào thế giới màn hình tỉ lệ 18:9.", 242, @"images\items\2\", nokia,localMarket, Offer.Slider, true);
                //Items Items03 = new Items("Nokia 7.2", 6190000, "Nokia 7.2 có thiết kế nguyên khối được làm một cách tỉ mỉ và chi tiết. Các góc cạnh của Nokia 7.2 được bo tròn mềm mại tạo cảm giác cầm nắm dễ chịu. Chất liệu là khung kim loại tạo vẻ sang trọng và chắc chắn cho Nokia 7.2.", 42, @"images\items\3\", nokia, localMarket, Offer.No, true);
                //Items Items04 = new Items("Nokia 8.1(x7)", 6080000, "Màn hình tai thỏ, tràn viền kích thước 6.18 inches, độ phân giải full HD+ cho trải nghiệm thị giác sống động, tuyệt vời", 24, @"images\items\4\", nokia, localMarket, Offer.No, true);
                //Items Items05 = new Items("Nokia 3.1 plus", 2290000, "Điện Thoại Nokia 3.1 Plus sở hữu thiết kế nguyên khối sang trọng, nổi bật với lớp vỏ nhôm được cắt chính xác, tinh tế, được kết hợp với cấu trúc kim loại đúc bên trong để tăng độ bền. Ngoài ra, mặt sau điện thoại là polycarbonate vì vậy khi cầm trên tay cho cảm giá mềm mại, êm ái đồng thời hạn chế vết bẩn, bám vân tay.", 124, @"images\items\5\", nokia, localMarket, Offer.Slider, true);
                //Items Items06 = new Items("Nokia 3.2", 2990000, "Về thiết kế thì Nokia 3.2 không có quá nhiều sự thay đổi so với các smartphone khác đang có mặt trên thị trường. Máy vẫn đi theo xu hướng màn hình giọt nước trên màn hình có kích thước 6.26 inch, tỉ lệ màn hình 19:9, độ phân giải HD+.", 45, @"images\items\6\", nokia, localMarket, Offer.No, true);
                //Items Items07 = new Items("Nokia 8110 (4G) ", 550000, "Sẽ là những kí ức khó quên với những ai đã từng sở hữu những điện thoại”vang danh một thời” đến từ Nokia. Vì thế HMD đã mang đến người dùng một sự bất ngờ lớn khi hồi sinh điện thoại Nokia 8810 với tên gọi Nokia 8110 4Gcùng nhiều tính năng mới.", 98, @"images\items\7\", nokia,  localMarket, Offer.No, true);
                //Items Items08 = new Items("Nokia 3110 (2017)", 990000, "Điện thoại Nokia 3310 (2017) có thiết kế mỏng và nhẹ hơn đáng kể so với phiên bản cũ. Chất liệu nhựa của máy được làm bóng bảy nhìn bắt mắt hơn, đặc biệt là ở phiên bản màu đỏ và vàng.", 21, @"images\items\8\", nokia,  localMarket, Offer.No, true);
                //Items Items09 = new Items("Nokia 9 pureview", 1400000, "Nokia 9 PureView vừa được Nokia giới thiệu tại sự kiện MWC 2019 và là chiếc smartphone đầu tiên trên thế giới sở hữu tới 5 camera sau.", 47, @"images\items\9\", nokia, localMarket, Offer.No, true);

                ////Samsung
                //Items Items10 = new Items("Samsung Galaxy S10+", 2299000, "Samsung Galaxy S10+ (512GB) đi theo kiểu thiết kế màn hình Infinity-O với phần camera được đặt phía trong màn hình rất độc đáo.", 22, @"images\items\10\", samsung, localMarket, Offer.No, true);
                //Items Items11 = new Items("Samsung Galaxy Note 10+", 2699000, "DxOMark là chuyên trang đánh giá camera uy tín thế giới mới đây đã khẳng định, Galaxy Note 10+ là chiếc smartphone có camera tốt nhất hiện nay.", 83, @"images\items\11\", samsung, localMarket, Offer.No, true);
                //Items Items12 = new Items("Samsung Galaxy A80", 1499000, "Samsung Galaxy A80 là chiếc smartphone mang trong mình rất nhiều đột phá của Samsung và hứa hẹn sẽ là ngọn cờ đầu cho những chiếc smartphone sở hữu một màn hình tràn viền thật sự.", 75, @"images\items\12\", samsung, localMarket, Offer.No, true);
                //Items Items13 = new Items("Samsung Galaxy A9 (2018)", 1249000, "Samsung Galaxy A9 (2018) được thừa hưởng nhiều nét đẹp từ những người đàn anh của mình với thân hình mảnh mai, uyển chuyển.", 8, @"images\items\13\", samsung, localMarket, Offer.No, true);
                //Items Items14 = new Items("Samsung Galaxy A70 ", 9200000, "Màn hình của chiếc Galaxy A70 có kích thước khá lớn lên đến 6.7 inch độ phân giải Full HD+ trên tấm nền Super AMOLED.", 53, @"images\items\14\", samsung, localMarket, Offer.No, true);
                //Items Items15 = new Items("Samsung Galaxy A50 ", 6290000, "Nếu như trên chiếc Galaxy M20 Samsung đã giới thiệu màn hình công nghệ Infinity-V thì với Galaxy A50 đây lại là lần đầu tiên Samsung sử dụng màn hình Infinity-U.", 53, @"images\items\15\", samsung, localMarket, Offer.No, true);
                //Items Items16 = new Items("Samsung Galaxy A30s", 6290000, "Thay vì sử dụng camera kép như trên người anh em Samsung Galaxy A30 thì Samsung đã nâng cấp cho chiếc Galaxy A30s bộ 3 camera chất lượng ở mặt lưng.", 72, @"images\items\16\", samsung, localMarket, Offer.No, true);
                //Items Items17 = new Items("Samsung Galaxy M20", 4990000, "Nổi bật hơn cả trong lần ra mắt bộ đôi dòng M của Samsung, chiếc điện thoại Galaxy M20 có màn hình mới Infinity-V, dung lượng pin cực khủng lên tới 5000 mAh, camera siêu rộng và nhiều tính năng hấp dẫn khác.", 65, @"images\items\17\", samsung, localMarket, Offer.No, true);

                ////Oppo
                //Items Items18 = new Items("Oppo Reno 10x Zoom", 1799000, "Máy sở hữu cụm 3 camera với độ phân giải camera chính là 48 MP, một camera góc rộng 8 MP F/2.2 và một ống kính tiềm vọng 13 MP khẩu độ F/3.0 với khả năng zoom hybrid 10X.", 22, @"images\items\18\", oppo, localMarket, Offer.No, true);
                //Items Items19 = new Items("Oppo F11 pro ", 7990000, "Không “tai thỏ”, không “nốt ruồi” giúp cho OPPO F11 Pro 128GB khác biệt hoàn toàn với nhiều smartphone trên thị trường hiện nay.", 75, @"images\items\19\", oppo, localMarket, Offer.No, true);
                //Items Items20 = new Items("Oppo F11", 6290000, "Oppo F11 gây ấn tượng với người dùng bởi thiết kế màn hình tràn viền hình giọt nước và camera sau khủng đến 48 MP. ", 63, @"images\items\20\", oppo, localMarket, Offer.No, true);
                //Items Items21 = new Items("Oppo A9(2020)", 6990000, "Kế thừa phiên bản OPPO A7 đã từng gây hot trước đó, OPPO A9 (2020) có nhiều sự cải tiến hơn về màn hình, camera và hiệu năng trải nghiệm.", 34, @"images\items\21\", oppo, localMarket, Offer.No, true);
                //Items Items22 = new Items("Oppo K3", 2500000, "OPPO K3 là một chiếc smartphone tầm trung với khá nhiều tính năng cao cấp như màn hình không tai thỏ hay cảm biến vân tay bên trong màn hình", 45, @"images\items\22\", oppo, localMarket, Offer.No, true);
                //Items Items23 = new Items("Oppo F9", 5490000, "Trong những giây phút gay cấn như chơi game điện thoại báo hết pin hay sáng dậy đi làm nhưng phát hiện quên sạc pin thì bộ sạc của OPPO F9 sẽ đem lại sự cứu trợ ngay lập tức.", 35, @"images\items\23\", oppo, localMarket, Offer.No, true);
                //Items Items24 = new Items("Oppo A7", 4990000, "OPPO A7 được tạo nên nhờ với một ngôn ngữ thiết kế mới lạ, bắt mắt khi có phần mặt lưng phản chiếu 3D vân lưới ánh sáng cực đẹp và thu hút.", 86, @"images\items\24\", oppo,  localMarket, Offer.No, true);
                //Items Items25 = new Items("Oppo A5s", 3690000, "OPPO A5s là một chiếc máy giá rẻ và vẫn giữ được cho mình những ưu điểm mà người dùng yêu thích của một chiếc smartphone tới từ OPPO.", 35, @"images\items\25\", oppo, localMarket, Offer.No, true);

                //////Huawei
                //Items Items26 = new Items("Huawei P30 pro", 22990000, "Huawei P30 Pro là một bước đột phá của Huawei cũng như camera trên smartphone khi đem lại khả năng zoom như một kính viễn vọng.", 43, @"images\items\26\", huawei, localMarket, Offer.No, true);
                //Items Items27 = new Items("Huawei P30", 16990000, "Huawei P30 là chiếc smartphone cao cấp vừa được Huawei giới thiệu với thiết kế tuyệt đẹp, hiệu năng mạnh mẽ và thiết lập camera ấn tượng.", 68, @"images\items\27\", huawei, localMarket, Offer.No, true);
                //Items Items28 = new Items("Huawei P30 lite", 6020000, "Mới đây Huawei đã chính thức giới thiệu chiếc Huawei P30 Lite bên cạnh các sản phẩm khác trong dòng P30 series và chiếc smartphone được định hướng tới phân khúc tầm trung.", 75, @"images\items\28\", huawei, localMarket, Offer.No, true);
                //Items Items29 = new Items("Huawei Y9 Prime(2019)", 5840000, "Huawei Y9 Prime (2019) là phiên bản kế nhiệm của chiếc Y9 Prime (2018) đã ra mắt năm ngoái và cũng là chiếc điện thoại đầu tiên của Huawei được trang bị công nghệ camera trượt và màn hình tràn viền ra bốn cạnh.", 25, @"images\items\30\", huawei, localMarket, Offer.No, true);
                //Items Items30 = new Items("Huawei Nova 3i", 5390000, "Nova 3i sở hữu mặt lưng kính chuyển màu gradient kiểu như điện thoại Huawei P20 Pro, nhờ thiết kế mặt lưng kính ấn tượng đã đem lại cho máy một thiết kế hiện đại và tinh tế.", 14, @"images\items\30\", huawei, localMarket, Offer.No, true);
                //Items Items31 = new Items("Huawei Y9 (2019)", 4990000, "Huawei Y9 (2019) là chiếc smartphone được thiết kế dành riêng cho giới trẻ, với các màu sắc thời trang đi kèm nhiều xu hướng công nghệ mới nhất.", 35, @"images\items\31\", huawei, localMarket, Offer.No, true);
                //Items Items32 = new Items("Huawei Y7 pro (2019)", 3140000, "Y7 Pro (2019) được trang bị màn hình rộng lên đến 6.26 inch, với độ phân giải HD+.", 76, @"images\items\32\", huawei, localMarket, Offer.No, true);

                //////Xiaomi
                //Items Items33 = new Items("Xiaomi Mi 9T", 8490000, "Xiaomi Mi 9T (tên khác là Redmi K20) là chiếc smartphone vừa được giới thiệu gây rất nhiều chú ý với người dùng bởi nó hội tụ đủ 3 yếu tố ngon bổ rẻ.", 75, @"images\items\33\", xiaomi,  localMarket, Offer.No, true);
                //Items Items34 = new Items("Xiaomi Mi 9SE", 7490000, "Về ngoại hình thì chiếc smartphone Xiaomi Mi 9 SE khá tương đồng với người anh em cao cấp và bạn sẽ vẫn có một thiết kế mặt lưng với hiệu ứng đổi màu gradient đẹp mắt", 14, @"images\items\34", xiaomi, localMarket, Offer.No, true);
                //Items Items35 = new Items("Xiaomi Mi A3 ", 4990000, "Những chiếc máy ra mắt trong năm 2019 hầu hết đều có nhiều camera hơn so với thế hệ tiền nhiệm và Xiaomi Mi A3 cũng không phải là một ngoại lệ.", 57, @"images\items\35", xiaomi,  localMarket, Offer.No, true);
                //Items Items36 = new Items("Xiaomi Redmi Note 7", 4990000, "Xiaomi Redmi Note 7 là chiếc smartphone giá rẻ mới được giới thiệu vào đầu năm 2019 với nhiều trang bị đáng giá như thiết kế notch giọt nước hay camera lên tới 48 MP.", 75, @"images\items\36\", xiaomi, localMarket, Offer.No, true);
                //Items Items37 = new Items("Xiaomi Redmi 7 ", 3690000, "Cấu hình là điểm đáng tiền nhất trên Xiaomi Redmi 7 khi máy sở hữu con chip Snapdragon 632 cho sức mạnh vượt trội trong tầm giá.", 25, @"images\items\37\", xiaomi,  localMarket, Offer.No, true);
                //Items Items38 = new Items("Xiaomi Redmi 7A", 2390000, "Xiaomi Redmi 7A là một chiếc smartphone theo phong cách truyền thống của Xiaomi, mang tới người dùng trải nghiệm Android ổn trên một chiếc máy có mức giá rất hấp dẫn.", 14, @"images\items\38\", xiaomi,  localMarket, Offer.No, true);
                //Items Items39 = new Items("Xiaomi Mi 9 ", 1099000, "Xiaomi Mi 9 là chiếc flagship giữ vững truyền thống điện thoại cấu hình cao, giá “mềm”. Mi 9 sử dụng vi xử lý Snapdragon 855 mới nhất của Qualcomm, được sản xuất trên tiến trình 7nm, tốc độ xung nhip 2.84 GHz. GPU Adreno 640 đảm bảo mang đến cho người dùng trải nghiệm chơi game mượt mà, kể cả các tựa game nặng về đồ hoạ cao như PUBG Mobile.", 38, @"images\items\39\", xiaomi,  localMarket, Offer.No, true);
                //Items Items40 = new Items("Xiaomi Redmi Go", 1900000, "Android Go là phiên bản rút gọn của Android One - dự án phần mềm mới của Google dành cho các smartphone cấu hình giới hạn hoặc RAM 1 GB trở xuống.", 18, @"images\items\40\", xiaomi,  localMarket, Offer.No, true);



                //_dbContext.Items.AddRange(Items01, Items02, Items03, Items04, Items05, Items06, Items07, Items08, Items09, Items10, Items11, Items12, Items13, Items14, Items15, Items16, Items17, Items18, Items19, Items20, Items21, Items22, Items23, Items24, Items25, Items26, Items27, Items28, Items29, Items30, Items31, Items32, Items33, Items34, Items35, Items36, Items37, Items38, Items39, Items40);

                _dbContext.SaveChanges();
            }
        }

        private async Task CreateUser(string userName, string email, string password, string role)
        {
            var user = new ApplicationUser { UserName = userName, Email = email, EmailConfirmed = true };
            await _userManager.CreateAsync(user, password);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
        }
    }
}
