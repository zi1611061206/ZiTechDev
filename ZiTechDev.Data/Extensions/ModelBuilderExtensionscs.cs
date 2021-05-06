using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ZiTechDev.Data.Entities;
using ZiTechDev.Data.Enums;

namespace ZiTechDev.Data.Extensions
{
    public static class ModelBuilderExtensionscs
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = "vi-VN", Name = "Tiếng Việt", IsDefault = Default.Yes },
                new Language() { Id = "en-US", Name = "Tiếng Anh" , IsDefault = Default.No }
                );
            modelBuilder.Entity<Setting>().HasData(
                new Setting() { Key = "HomeTitle", Value = "ZiTechDev - HomePage", Type = ConfigType.String },
                new Setting() { Key = "AboutTitle", Value = "ZiTechDev - About", Type = ConfigType.String },
                new Setting() { Key = "ContactTitle", Value = "ZiTechDev - Contact", Type = ConfigType.String },
                new Setting() { Key = "MaxCommentLevel", Value = "2", Type = ConfigType.Int },
                new Setting() { Key = "PostOfPage", Value = "12", Type = ConfigType.Int },
                new Setting() { Key = "CommentOfPage", Value = "10", Type = ConfigType.Int }
                );
            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, ParentId = null },
                new Category() { Id = 2, ParentId = null },
                new Category() { Id = 3, ParentId = null },
                new Category() { Id = 4, ParentId = null }
                );
            modelBuilder.Entity<CategoryTranslation>().HasData(
                new CategoryTranslation()
                {
                    Id = 1,
                    CategoryId = 1,
                    LanguageId = "vi-VN",
                    Name = "Ứng dụng Desktop",
                    SEODescription = "Những bài viết về các ứng dụng trên máy tính để bàn, laptop ...",
                    SEOTitle = "Ứng dụng dành cho máy tính để bàn, laptop",
                    SEOAlias = "ung-dung-desktop"
                },
                new CategoryTranslation()
                {
                    Id = 2,
                    CategoryId = 1,
                    LanguageId = "en-US",
                    Name = "Desktop Application",
                    SEODescription = "Articles about applications on desktop computers, laptops ...",
                    SEOTitle = "Application for computer, laptop",
                    SEOAlias = "desktop-application"
                },
                new CategoryTranslation()
                {
                    Id = 3,
                    CategoryId = 2,
                    LanguageId = "vi-VN",
                    Name = "Ứng dụng Web",
                    SEODescription = "Những bài viết về lập trình web, ứng dụng trực tuyến, đám mây, trình duyệt web ...",
                    SEOTitle = "Ứng dụng dành cho nền tảng website & internet",
                    SEOAlias = "ung-dung-web"
                },
                new CategoryTranslation()
                {
                    Id = 4,
                    CategoryId = 2,
                    LanguageId = "en-US",
                    Name = "Web Application",
                    SEODescription = "Articles about web programming, online applications, cloud, web browser ...",
                    SEOTitle = "Application for website & internet platform",
                    SEOAlias = "web-application"
                },
                new CategoryTranslation()
                {
                    Id = 5,
                    CategoryId = 3,
                    LanguageId = "vi-VN",
                    Name = "Ứng dụng đi động",
                    SEODescription = "Những bài viết về lập trình trên thiết bị di dộng như: điện thoại thông minh, máy tính bảng, đồng hồ thông minh, TV thông minh, Ô tô thông minh ...",
                    SEOTitle = "Ứng dụng dành cho thiết bị di động",
                    SEOAlias = "ung-dung-di-dong"
                },
                new CategoryTranslation()
                {
                    Id = 6,
                    CategoryId = 3,
                    LanguageId = "en-US",
                    Name = "Web Application",
                    SEODescription = "Articles on programming on mobile devices such as smartphones, tablets, smart watches, smart TVs, smart cars ...",
                    SEOTitle = "Application for mobile devices",
                    SEOAlias = "mobile-application"
                },
                new CategoryTranslation()
                {
                    Id = 7,
                    CategoryId = 4,
                    LanguageId = "vi-VN",
                    Name = "Các học thuật",
                    SEODescription = "Những bài viết về: trí tuệ nhân tạo, chuỗi khối, dữ liệu lớn, internet vạn vật ...",
                    SEOTitle = "Nghiên cứu & thực hành các công nghệ 4.0",
                    SEOAlias = "cac-hoc-thuat"
                },
                new CategoryTranslation()
                {
                    Id = 8,
                    CategoryId = 4,
                    LanguageId = "en-US",
                    Name = "academics",
                    SEODescription = "Articles about: artificial intelligence, blockchain, big data, internet of things ...",
                    SEOTitle = "Research & practice 4.0 technologies",
                    SEOAlias = "academics"
                }
                );
            modelBuilder.Entity<Post>().HasData(
                new Post()
                {
                    Id = 1,
                    CreatedDate = DateTime.Now,
                    LastModify = null,
                    ViewCount = 0,
                    LikeCount = 0,
                    SharedCount = 0,
                    Status = PostStatus.Published,
                    Thumbnail = null, 
                    CategoryId = 1,
                    UserId = new Guid()
                },
                new Post()
                {
                    Id = 2,
                    CreatedDate = DateTime.Now,
                    LastModify = null,
                    ViewCount = 0,
                    LikeCount = 0,
                    SharedCount = 0,
                    Status = PostStatus.Pending,
                    Thumbnail = null,
                    CategoryId = 2,
                    UserId = new Guid()
                },
                new Post()
                {
                    Id = 3,
                    CreatedDate = DateTime.Now,
                    LastModify = null,
                    ViewCount = 0,
                    LikeCount = 0,
                    SharedCount = 0,
                    Status = PostStatus.Hidden,
                    Thumbnail = null,
                    CategoryId = 3,
                    UserId = new Guid()
                }
                );
            modelBuilder.Entity<PostTranslation>().HasData(
                new PostTranslation()
                {
                    Id = 1,
                    PostId = 1,
                    LanguageId = "vi-VN",
                    SEODescription = "Mô tả",
                    SEOTitle = "Tiêu đề",
                    SEOAlias = "tieu-de",
                    Content = "Nội dung"
                },
                new PostTranslation()
                {
                    Id = 2,
                    PostId = 1,
                    LanguageId = "en-US",
                    SEODescription = "Description",
                    SEOTitle = "Title",
                    SEOAlias = "description",
                    Content = "Content"
                },
                new PostTranslation()
                {
                    Id = 3,
                    PostId = 2,
                    LanguageId = "vi-VN",
                    SEODescription = "Mô tả",
                    SEOTitle = "Tiêu đề",
                    SEOAlias = "tieu-de",
                    Content = "Nội dung"
                },
                new PostTranslation()
                {
                    Id = 4,
                    PostId = 2,
                    LanguageId = "en-US",
                    SEODescription = "Description",
                    SEOTitle = "Title",
                    SEOAlias = "description",
                    Content = "Content"
                },
                new PostTranslation()
                {
                    Id = 5,
                    PostId = 3,
                    LanguageId = "vi-VN",
                    SEODescription = "Mô tả",
                    SEOTitle = "Tiêu đề",
                    SEOAlias = "tieu-de",
                    Content = "Nội dung"
                },
                new PostTranslation()
                {
                    Id = 6,
                    PostId = 3,
                    LanguageId = "en-US",
                    SEODescription = "Description",
                    SEOTitle = "Title",
                    SEOAlias = "description",
                    Content = "Content"
                }
                );
            modelBuilder.Entity<Comment>().HasData(
                new Comment()
                {
                    Id = 1, 
                    PostId = 1,
                    Time = DateTime.Now,
                    Content = "Bình luận 1",
                    ParentId = null,
                    LikeCount = 0,
                    LastModify = null
                },
                new Comment()
                {
                    Id = 2,
                    PostId = 1,
                    Time = DateTime.Now,
                    Content = "Bình luận 2",
                    ParentId = null,
                    LikeCount = 0,
                    LastModify = null
                },
                new Comment()
                {
                    Id = 3,
                    PostId = 2,
                    Time = DateTime.Now,
                    Content = "Bình luận 3",
                    ParentId = null,
                    LikeCount = 0,
                    LastModify = null
                },
                new Comment()
                {
                    Id = 4,
                    PostId = 1,
                    Time = DateTime.Now,
                    Content = "Bình luận con 1",
                    ParentId = 1,
                    LikeCount = 5,
                    LastModify = null
                },
                new Comment()
                {
                    Id = 5,
                    PostId = 1,
                    Time = DateTime.Now,
                    Content = "Bình luận con 2",
                    ParentId = 1,
                    LikeCount = 0,
                    LastModify = null
                },
                new Comment()
                {
                    Id = 6,
                    PostId = 1,
                    Time = DateTime.Now,
                    Content = "Bình luận con con 5",
                    ParentId = 5,
                    LikeCount = 0,
                    LastModify = null
                }
                );
            modelBuilder.Entity<Function>().HasData(
                new Function() { Id = 1, Name = "Admin", Description="Tất cả các tác vụ có trong ứng dụng", Url="/ziadmin/", ParentId = null },
                new Function() { Id = 2, Name = "Mod", Description="Các tác vụ về viết, duyệt, đăng bài và kiểm duyệt người dùng ...", Url="/zimod/", ParentId = 1 },
                new Function() { Id = 3, Name = "PostMod", Description="Các tác vụ về viết, duyệt, đăng bài ...", Url="/zimod/postmod/", ParentId = 2 },
                new Function() { Id = 4, Name = "UserMod", Description="Các tác vụ về lọc, kiểm duyệt người dùng ...", Url="/zimod/usermod/", ParentId = 2 },
                new Function() { Id = 5, Name = "User", Description="Các tác vụ đọc, bình luận, like, share, đăng ký thành viên, gửi phản hồi ... ", Url="/ziuser/", ParentId = null }
                );
            modelBuilder.Entity<Activity>().HasData(
                new Activity() { Id = 1, Name = "AdminLogin", Description = "Đăng nhập vào trang quản trị", FunctionId = 1 },
                new Activity() { Id = 2, Name = "AdminLogout", Description = "Đăng xuất khỏi tài khoản quản trị", FunctionId = 1 },
                new Activity() { Id = 3, Name = "UserLogin", Description = "Đăng nhập vào trang người dùng", FunctionId = 3 },
                new Activity() { Id = 4, Name = "UserLogout", Description = "Đăng xuất tài khoản người dùng", FunctionId = 3 },
                new Activity() { Id = 5, Name = "UserRegister", Description = "Đăng ký tài khoản người dùng", FunctionId = 3 }
                );
            modelBuilder.Entity<Log>().HasData(
                new Log() { Id = 1, ActivityId = 1, ActionTime = DateTime.Now, UserId = new Guid() }
                );
        }
    }
}
