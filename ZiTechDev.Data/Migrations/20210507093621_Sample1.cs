using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZiTechDev.Data.Migrations
{
    public partial class Sample1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "AppUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTokens", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Url = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastAccess = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfJoin = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Gender = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FunctionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SEODescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SEOTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SEOAlias = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    LanguageId = table.Column<string>(type: "nvarchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryTranslations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    LastModify = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LikeCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SharedCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Thumbnail = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => new { x.ActivityId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Logs_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Logs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => new { x.RoleId, x.ActivityId });
                    table.ForeignKey(
                        name: "FK_Permissions_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    LikeCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LastModify = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostGalleries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Path = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    IsThumbnail = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FileSize = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    SortOrder = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    EncodeString = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostGalleries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostGalleries_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    SEODescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SEOTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SEOAlias = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostTranslations_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("105e57d3-188c-40ba-9409-358f55415061"), new Guid("b2d8f0ba-64d4-448d-92d7-d300465d0337") },
                    { new Guid("0a6a76db-e350-43bb-b023-74be2ae4be2c"), new Guid("b2d8f0ba-64d4-448d-92d7-d300465d0337") },
                    { new Guid("6185636e-7edc-4826-99b9-c862727de029"), new Guid("b2d8f0ba-64d4-448d-92d7-d300465d0337") },
                    { new Guid("0a6a76db-e350-43bb-b023-74be2ae4be2c"), new Guid("a2e0ca3e-1a80-4554-9389-0ec66ab7a259") },
                    { new Guid("6185636e-7edc-4826-99b9-c862727de029"), new Guid("a2e0ca3e-1a80-4554-9389-0ec66ab7a259") },
                    { new Guid("6185636e-7edc-4826-99b9-c862727de029"), new Guid("2d2a1cdd-d4a5-4ae4-9f26-f2fa1bedd6ae") }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ParentId", "SortOrder" },
                values: new object[,]
                {
                    { 4, null, 4 },
                    { 2, null, 2 },
                    { 1, null, 1 },
                    { 3, null, 3 }
                });

            migrationBuilder.InsertData(
                table: "Functions",
                columns: new[] { "Id", "Description", "Name", "ParentId", "Url" },
                values: new object[,]
                {
                    { 4, "Các tác vụ về lọc, kiểm duyệt người dùng ...", "UserMod", 2, "/zimod/usermod/" },
                    { 1, "Tất cả các tác vụ có trong ứng dụng", "Admin", null, "/ziadmin/" },
                    { 2, "Các tác vụ về viết, duyệt, đăng bài và kiểm duyệt người dùng ...", "Mod", 1, "/zimod/" },
                    { 3, "Các tác vụ về viết, duyệt, đăng bài ...", "PostMod", 2, "/zimod/postmod/" },
                    { 5, "Các tác vụ đọc, bình luận, like, share, đăng ký thành viên, gửi phản hồi ... ", "User", null, "/ziuser/" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "IsDefault", "Name" },
                values: new object[] { "vi-VN", true, "Tiếng Việt" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Name" },
                values: new object[] { "en-US", "Tiếng Anh" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("6185636e-7edc-4826-99b9-c862727de029"), "", "User role", "USER", "user" },
                    { new Guid("0a6a76db-e350-43bb-b023-74be2ae4be2c"), "", "Modifier role", "MOD", "mod" },
                    { new Guid("105e57d3-188c-40ba-9409-358f55415061"), "", "Administrator role", "ADMIN", "admin" }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Key", "Type", "Value" },
                values: new object[,]
                {
                    { "MaxCommentLevel", 1, "2" },
                    { "ContactTitle", 0, "ZiTechDev - Contact" },
                    { "AboutTitle", 0, "ZiTechDev - About" },
                    { "CommentOfPage", 1, "10" },
                    { "PostOfPage", 1, "12" },
                    { "HomeTitle", 0, "ZiTechDev - HomePage" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "DateOfJoin", "DisplayName", "Email", "EmailConfirmed", "FirstName", "LastAccess", "LastName", "LockoutEnabled", "LockoutEnd", "MiddleName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("b2d8f0ba-64d4-448d-92d7-d300465d0337"), 0, "", new DateTime(1998, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 7, 16, 36, 20, 75, DateTimeKind.Local).AddTicks(9140), "Zi_Admin", "ZITECH.DEV@GMAIL.COM", true, "Nguyễn", new DateTime(2021, 5, 7, 16, 36, 20, 73, DateTimeKind.Local).AddTicks(8565), "Hiếu", false, null, "Ngọc", "zitech.dev@gmail.com", "admin", "AQAAAAEAACcQAAAAEGqOSPxvI79aFtIGQQcbSVY5uQ9T4c2zVnIVhZ0BOOn4bGDCDiarj7V03HNXdDR1tQ==", "(+84) 943 144 178", true, "", true, "ADMIN" },
                    { new Guid("2d2a1cdd-d4a5-4ae4-9f26-f2fa1bedd6ae"), 0, "", new DateTime(1998, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 7, 16, 36, 20, 125, DateTimeKind.Local).AddTicks(1579), "Zi_User", "ZITECH.DEV@GMAIL.COM", true, "Nguyễn", new DateTime(2021, 5, 7, 16, 36, 20, 125, DateTimeKind.Local).AddTicks(1555), "Hiếu", false, null, "Ngọc", "zitech.dev@gmail.com", "user", "AQAAAAEAACcQAAAAEPhfIruwKqVlheUTv73/PAbJB+3ZwZuQ0fDemCr+XWkBCEEnRMg3j20BPFnmDbyA0g==", "(+84) 943 144 178", true, "", true, "User" },
                    { new Guid("a2e0ca3e-1a80-4554-9389-0ec66ab7a259"), 0, "", new DateTime(1998, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 5, 7, 16, 36, 20, 110, DateTimeKind.Local).AddTicks(8754), "Zi_Mod", "ZITECH.DEV@GMAIL.COM", true, "Nguyễn", new DateTime(2021, 5, 7, 16, 36, 20, 110, DateTimeKind.Local).AddTicks(8724), "Hiếu", false, null, "Ngọc", "zitech.dev@gmail.com", "mod", "AQAAAAEAACcQAAAAEAQOdwWSSZR1IFj+MxgFpIJS/FyFUAL5Y40MmLaYiw0dKMy0aboP11JxckxlpImTYA==", "(+84) 943 144 178", true, "", true, "MOD" }
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Description", "FunctionId", "Name" },
                values: new object[,]
                {
                    { 1, "Đăng nhập vào trang quản trị", 1, "AdminLogin" },
                    { 2, "Đăng xuất khỏi tài khoản quản trị", 1, "AdminLogout" },
                    { 3, "Đăng nhập vào trang người dùng", 3, "UserLogin" },
                    { 4, "Đăng xuất tài khoản người dùng", 3, "UserLogout" },
                    { 5, "Đăng ký tài khoản người dùng", 3, "UserRegister" }
                });

            migrationBuilder.InsertData(
                table: "CategoryTranslations",
                columns: new[] { "Id", "CategoryId", "LanguageId", "Name", "SEOAlias", "SEODescription", "SEOTitle" },
                values: new object[,]
                {
                    { 1, 1, "vi-VN", "Ứng dụng Desktop", "ung-dung-desktop", "Những bài viết về các ứng dụng trên máy tính để bàn, laptop ...", "Ứng dụng dành cho máy tính để bàn, laptop" },
                    { 3, 2, "vi-VN", "Ứng dụng Web", "ung-dung-web", "Những bài viết về lập trình web, ứng dụng trực tuyến, đám mây, trình duyệt web ...", "Ứng dụng dành cho nền tảng website & internet" },
                    { 5, 3, "vi-VN", "Ứng dụng đi động", "ung-dung-di-dong", "Những bài viết về lập trình trên thiết bị di dộng như: điện thoại thông minh, máy tính bảng, đồng hồ thông minh, TV thông minh, Ô tô thông minh ...", "Ứng dụng dành cho thiết bị di động" },
                    { 7, 4, "vi-VN", "Các học thuật", "cac-hoc-thuat", "Những bài viết về: trí tuệ nhân tạo, chuỗi khối, dữ liệu lớn, internet vạn vật ...", "Nghiên cứu & thực hành các công nghệ 4.0" },
                    { 2, 1, "en-US", "Desktop Application", "desktop-application", "Articles about applications on desktop computers, laptops ...", "Application for computer, laptop" },
                    { 4, 2, "en-US", "Web Application", "web-application", "Articles about web programming, online applications, cloud, web browser ...", "Application for website & internet platform" },
                    { 6, 3, "en-US", "Mobile Application", "mobile-application", "Articles on programming on mobile devices such as smartphones, tablets, smart watches, smart TVs, smart cars ...", "Application for mobile devices" },
                    { 8, 4, "en-US", "academics", "Academics", "Articles about: artificial intelligence, blockchain, big data, internet of things ...", "Research & practice 4.0 technologies" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "LastModify", "Status", "Thumbnail", "UserId" },
                values: new object[] { 1, 1, new DateTime(2021, 5, 7, 16, 36, 20, 140, DateTimeKind.Local).AddTicks(2730), null, 1, null, new Guid("b2d8f0ba-64d4-448d-92d7-d300465d0337") });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "LastModify", "Thumbnail", "UserId" },
                values: new object[] { 2, 2, new DateTime(2021, 5, 7, 16, 36, 20, 141, DateTimeKind.Local).AddTicks(947), null, null, new Guid("b2d8f0ba-64d4-448d-92d7-d300465d0337") });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "LastModify", "Status", "Thumbnail", "UserId" },
                values: new object[] { 3, 3, new DateTime(2021, 5, 7, 16, 36, 20, 141, DateTimeKind.Local).AddTicks(1164), null, 2, null, new Guid("b2d8f0ba-64d4-448d-92d7-d300465d0337") });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "LastModify", "ParentId", "PostId", "Time" },
                values: new object[,]
                {
                    { 1, "Bình luận 1", null, null, 1, new DateTime(2021, 5, 7, 16, 36, 20, 142, DateTimeKind.Local).AddTicks(2472) },
                    { 2, "Bình luận 2", null, null, 1, new DateTime(2021, 5, 7, 16, 36, 20, 142, DateTimeKind.Local).AddTicks(5909) }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "LastModify", "LikeCount", "ParentId", "PostId", "Time" },
                values: new object[] { 4, "Bình luận con 1", null, 5, 1, 1, new DateTime(2021, 5, 7, 16, 36, 20, 142, DateTimeKind.Local).AddTicks(6016) });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "LastModify", "ParentId", "PostId", "Time" },
                values: new object[,]
                {
                    { 5, "Bình luận con 2", null, 1, 1, new DateTime(2021, 5, 7, 16, 36, 20, 142, DateTimeKind.Local).AddTicks(6019) },
                    { 6, "Bình luận con con 5", null, 5, 1, new DateTime(2021, 5, 7, 16, 36, 20, 142, DateTimeKind.Local).AddTicks(6022) },
                    { 3, "Bình luận 3", null, null, 2, new DateTime(2021, 5, 7, 16, 36, 20, 142, DateTimeKind.Local).AddTicks(6012) }
                });

            migrationBuilder.InsertData(
                table: "Logs",
                columns: new[] { "ActivityId", "UserId", "ActionTime" },
                values: new object[] { 1, new Guid("b2d8f0ba-64d4-448d-92d7-d300465d0337"), new DateTime(2021, 5, 7, 16, 36, 20, 144, DateTimeKind.Local).AddTicks(3551) });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "ActivityId", "RoleId" },
                values: new object[,]
                {
                    { 4, new Guid("6185636e-7edc-4826-99b9-c862727de029") },
                    { 3, new Guid("0a6a76db-e350-43bb-b023-74be2ae4be2c") },
                    { 3, new Guid("6185636e-7edc-4826-99b9-c862727de029") },
                    { 2, new Guid("105e57d3-188c-40ba-9409-358f55415061") },
                    { 1, new Guid("105e57d3-188c-40ba-9409-358f55415061") },
                    { 5, new Guid("6185636e-7edc-4826-99b9-c862727de029") },
                    { 4, new Guid("0a6a76db-e350-43bb-b023-74be2ae4be2c") }
                });

            migrationBuilder.InsertData(
                table: "PostTranslations",
                columns: new[] { "Id", "Content", "LanguageId", "PostId", "SEOAlias", "SEODescription", "SEOTitle" },
                values: new object[,]
                {
                    { 5, "Nội dung", "vi-VN", 3, "tieu-de", "Mô tả", "Tiêu đề" },
                    { 3, "Nội dung", "vi-VN", 2, "tieu-de", "Mô tả", "Tiêu đề" },
                    { 2, "Content", "en-US", 1, "description", "Description", "Title" },
                    { 1, "Nội dung", "vi-VN", 1, "tieu-de", "Mô tả", "Tiêu đề" },
                    { 6, "Content", "en-US", 3, "description", "Description", "Title" },
                    { 4, "Content", "en-US", 2, "description", "Description", "Title" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_FunctionId",
                table: "Activities",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTranslations_CategoryId",
                table: "CategoryTranslations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTranslations_LanguageId",
                table: "CategoryTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UserId",
                table: "Logs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ActivityId",
                table: "Permissions",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_PostGalleries_PostId",
                table: "PostGalleries",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTranslations_LanguageId",
                table: "PostTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTranslations_PostId",
                table: "PostTranslations",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRoleClaims");

            migrationBuilder.DropTable(
                name: "AppUserClaims");

            migrationBuilder.DropTable(
                name: "AppUserLogins");

            migrationBuilder.DropTable(
                name: "AppUserRoles");

            migrationBuilder.DropTable(
                name: "AppUserTokens");

            migrationBuilder.DropTable(
                name: "CategoryTranslations");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "PostGalleries");

            migrationBuilder.DropTable(
                name: "PostTranslations");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
