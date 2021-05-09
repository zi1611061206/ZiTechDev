# ASP.NET CORE 3.1 project from ZiTechDev
## TECHNOLOGIES
 - ASP.NET CORE 3.1 (MVC)
## SOLUTION STRUCTURE
 - DATA
 - BUSINESS
 - WEBAPP
## PAKAGES
 - DATA:
	 + Microsoft.EntityFrameworkCore.SqlServer 5.0.5
	 + Microsoft.EntityFrameworkCore.Design 5.0.5
	 + Microsoft.EntityFrameworkCore.Tools 5.0.5
	 + Microsoft.AspNetCore.Identity.EntityFrameworkCore 5.0.5
	 + Microsoft.Extensions.Configuration 5.0.0
	 + Microsoft.Extensions.Configuration.FileExtensions 5.0.0
	 + Microsoft.Extensions.Configuration.Json 5.0.0
 - BUSINESS
	 + FluentValidation.AspNetCore 10.1.0
 - BACKEND API
	 + Swashbuckle.AspNetCore 6.1.4
	 + Microsoft.AspNetCore.Authentication.JwtBearer 5.0.5
## DEVELOPMENT PROCESS
 - DATA - Class Library: (Sử dụng Fluent API thay cho Data Annotation)
	 + Cài đặt các gói cần thiết cho EntityFrameworkCore
	 + Cài đặt các gói cần thiết cho IdentityServer
	 + Tạo các Enums cho các thuộc tính options của thực thể (Ngoại trừ boolean options)
	 + Cấu hình ZiTechDevDbContext kế thừa IdentityDbContext, tạo constructure, ghi đề phương thức OnModelCreating
	 + Tạo các lớp thực thể (Entities Class) mô tả các thuộc tính (Attributes) đối tượng trừ các khoắ ngoại (ForeignKeys)
	 + Tạo các lớp thực thể liên quan IdentityServer: Users, Roles => UserRoles, UserLogins, UserTokens, UserClaims, RoleClaims
	 + Khai báo các thực thể trong ZiTechDevDbContext với DbSet<>
	 + Tạo các lớp cấu hình (Configurations Class) mô tả các tính chất (Properties) của thực thể tương ứng trừ các khóa ngoại: ToTable(), IsRequired(), IsUnicode(), HasKey(), HasMaxLength(), HasDefaultValue(), HasDefaultSqlValue(), UseIdentityColumn()...
	 + Thêm các thuộc tính khóa ngoại: OneToOne, OneToMany, ManyToMany
	 + Cấu hình quan hệ cho các thuộc tính khóa ngoại: HasOne(), WithMany(), WithOne(), HasForeignKey()...
	 + Áp dụng các cấu hình trong phương thức OnModelCreating (ZiTechDevDbContext)
	 + Tạo dữ liệu mẫu (Seeding Data) theo tuần tự trong ModelBuilderExtensions
	 + Áp dụng ModelBuilderExtensions vào phương thức OnModelCreating (ZiTechDevDbContext)
	 + Cài đặt các gói cần thiết cho ExtensionsConfiguration
	 + Tạo appsettings.json thêm dữ liệu về ConnectionString, đặt KeyName trong ProjectConstants (Common) nhằm thông nhất và tái sử dụng
	 + Cấu hình ZiTechDevDBContextFactory đọc file appsettings.json lấy ConnectionString và nạp vào ZiTechDevDbContext
	 + Tools -> Nuget Pakage Manager -> Pakage Manager Console
	 + Nhập Add-Migration <name> tạo Migration Class -> Kiểm tra lại phương thức Up()
	 + Nhập update-database để tạo cơ sở dữ liệu
	 + Fix bug nếu có