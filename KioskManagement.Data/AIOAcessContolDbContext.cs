using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KioskManagement.Data.Migrations;
using KioskManagement.Model.Models;
using KioskManagement.Model.MappingModels;
using KioskManagement.Model.ViewModels;

namespace KioskManagement.Data
{
    public class AIOAcessContolDbContext : IdentityDbContext<IdentityUser>
    {

        public AIOAcessContolDbContext(DbContextOptions<AIOAcessContolDbContext> options) : base(options)
        {
            this.Database.SetCommandTimeout(0);
        }

        public virtual DbSet<AppGroup> AppGroups { get; set; }
        public virtual DbSet<AppMenu> AppMenus { get; set; }
        public virtual DbSet<AppMenuUser> AppMenuUsers { get; set; }
        public virtual DbSet<AppRole> AppRoles { get; set; }
        public virtual DbSet<AppRoleGroup> AppRoleGroups { get; set; }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<AppUserGroup> AppUserGroups { get; set; }
        public virtual DbSet<AppUserRole> AppUserRoles { get; set; }
        public virtual DbSet<ADateAttendance> ADateAttendances { get; set; }
        public virtual DbSet<ADateIoInfo> ADateIoInfos { get; set; }
        public virtual DbSet<AIoInfo> AIoInfos { get; set; }
        public virtual DbSet<AMonthAtt> AMonthAtts { get; set; }
        public virtual DbSet<AMonthAtt2> AMonthAtt2s { get; set; }
        public virtual DbSet<AOfftime> AOfftimes { get; set; }
        public virtual DbSet<AOfftimeDetail> AOfftimeDetails { get; set; }
        public virtual DbSet<AProject> AProjects { get; set; }
        public virtual DbSet<AReader> AReaders { get; set; }
        public virtual DbSet<ARealtimeMonitor> ARealtimeMonitors { get; set; }
        public virtual DbSet<AResultDateAtt> AResultDateAtts { get; set; }
        public virtual DbSet<AResultDateAtt2> AResultDateAtt2s { get; set; }
        public virtual DbSet<AResultMonthAtt> AResultMonthAtts { get; set; }
        public virtual DbSet<AResultMonthAtt2> AResultMonthAtt2s { get; set; }
        public virtual DbSet<AResultTimeAtt> AResultTimeAtts { get; set; }
        public virtual DbSet<AResultTimeAtt2> AResultTimeAtt2s { get; set; }
        public virtual DbSet<ASchedule> ASchedules { get; set; }
        public virtual DbSet<AScheduleDeviceDetail> AScheduleDeviceDetails { get; set; }
        public virtual DbSet<ATimeAdd> ATimeAdds { get; set; }
        public virtual DbSet<ATimeAttendance> ATimeAttendances { get; set; }
        public virtual DbSet<AZone> AZones { get; set; }
        public virtual DbSet<TAccount> TAccounts { get; set; }
        public virtual DbSet<TDepartment> TDepartments { get; set; }
        public virtual DbSet<TDevice> TDevices { get; set; }
        public virtual DbSet<TDeviceLicense> TDeviceLicenses { get; set; }
        public virtual DbSet<TDeviceType> TDeviceTypes { get; set; }
        public virtual DbSet<TEmployee> TEmployees { get; set; }
        public virtual DbSet<TEmployeeFace> TEmployeeFaces { get; set; }
        public virtual DbSet<TEmployeeType> TEmployeeTypes { get; set; }
        public virtual DbSet<TGroupAccess> TGroupAccesses { get; set; }
        public virtual DbSet<TGroupAccessDetail> TGroupAccessDetails { get; set; }
        public virtual DbSet<TLog> TLogs { get; set; }
        public virtual DbSet<TMenulist> TMenulists { get; set; }
        public virtual DbSet<TPrivile> TPriviles { get; set; }
        public virtual DbSet<TRegency> TRegencies { get; set; }
        public virtual DbSet<TTimeAttendancyCheck> TTimeAttendancyChecks { get; set; }
        public virtual DbSet<TVisiblecontrol> TVisiblecontrols { get; set; }


        public virtual DbSet<AppMenuMappingSQL> GetMenuList { get; set; }

        //map thông kê
      
        
        //end map thống kê
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=192.168.10.15;Database=HANET_TEST;Trusted_Connection=false;User ID=sa_rd;Password=@astec2020");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppMenuUser>().ToTable("AppMenuUsers").HasKey(x => new { x.UserId, x.MenuId });
            modelBuilder.Entity<AppUserGroup>().ToTable("AppUserGroups").HasKey(x => new { x.UserId, x.GroupId });
            modelBuilder.Entity<AppRoleGroup>().ToTable("AppRoleGroups").HasKey(x => new { x.RoleId, x.GroupId });
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AppUserRoles").HasKey(i => new { i.UserId, i.RoleId });
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AppUserLogins").HasKey(i => i.UserId);
            modelBuilder.Entity<IdentityRole>().ToTable("AppRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AppUserClaims").HasKey(i => i.UserId);
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AppUserTokens").HasKey(i => i.UserId);
            modelBuilder.Entity<IdentityUser<string>>().ToTable("AppUsers").HasKey(i => i.Id);


           

            modelBuilder.Entity<ADateAttendance>(entity =>
            {
                entity.HasKey(e => e.DatId);

                entity.ToTable("A_DATE_ATTENDANCE");

                entity.Property(e => e.DatId).HasColumnName("DAT_ID");

                entity.Property(e => e.DatValue)
                    .HasColumnType("date")
                    .HasColumnName("DAT_VALUE");

                entity.Property(e => e.DevId).HasColumnName("DEV_ID");
            });

            modelBuilder.Entity<ADateIoInfo>(entity =>
            {
                entity.HasKey(e => e.DiId);

                entity.ToTable("A_DATE_IO_INFO");

                entity.Property(e => e.DiId)
                    .ValueGeneratedNever()
                    .HasColumnName("DI_ID");

                entity.Property(e => e.DevId).HasColumnName("DEV_ID");

                entity.Property(e => e.DiValue)
                    .HasColumnType("date")
                    .HasColumnName("DI_VALUE");
            });

            
           

            modelBuilder.Entity<AIoInfo>(entity =>
            {
                entity.HasKey(e => e.IoIdA);

                entity.ToTable("A_IO_INFO");

                entity.Property(e => e.IoIdA).HasColumnName("IO_ID_A");

                entity.Property(e => e.CaId).HasColumnName("CA_ID");

                entity.Property(e => e.DevId)
                    .HasColumnName("DEV_ID")
                    .HasComment("Mã thiết bị");

                entity.Property(e => e.EmId).HasColumnName("EM_ID");

                entity.Property(e => e.IoDate)
                    .HasColumnType("date")
                    .HasColumnName("IO_DATE");

                entity.Property(e => e.IoTime)
                    .HasColumnType("time(0)")
                    .HasColumnName("IO_TIME");

                entity.HasOne(d => d.Dev)
                    .WithMany(p => p.AIoInfos)
                    .HasForeignKey(d => d.DevId)
                    .HasConstraintName("FK_A_IO_INFO_DEV_ID");
            });


            modelBuilder.Entity<AMonthAtt>(entity =>
            {
                entity.HasKey(e => e.MaId);

                entity.ToTable("A_MONTH_ATT");

                entity.Property(e => e.MaId).HasColumnName("MA_ID");

                entity.Property(e => e.MaMonth).HasColumnName("MA_MONTH");

                entity.Property(e => e.MaYear).HasColumnName("MA_YEAR");
            });

            modelBuilder.Entity<AMonthAtt2>(entity =>
            {
                entity.HasKey(e => e.MaId2);

                entity.ToTable("A_MONTH_ATT2");

                entity.Property(e => e.MaId2).HasColumnName("MA_ID2");

                entity.Property(e => e.MaMonth2).HasColumnName("MA_MONTH2");

                entity.Property(e => e.MaYear2).HasColumnName("MA_YEAR2");
            });

            modelBuilder.Entity<AOfftime>(entity =>
            {
                entity.HasKey(e => e.OffId);

                entity.ToTable("A_OFFTIME");

                entity.Property(e => e.OffId).HasColumnName("OFF_ID");

                entity.Property(e => e.Accept).HasColumnName("ACCEPT");

                entity.Property(e => e.EmId).HasColumnName("EM_ID");

                entity.Property(e => e.FromDate)
                    .HasColumnType("date")
                    .HasColumnName("FROM_DATE");

                entity.Property(e => e.FromTime)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FROM_TIME");

                entity.Property(e => e.ToDate)
                    .HasColumnType("date")
                    .HasColumnName("TO_DATE");

                entity.Property(e => e.ToTime)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TO_TIME");
            });

            modelBuilder.Entity<AOfftimeDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("A_OFFTIME_DETAILS");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("DATE");

                entity.Property(e => e.EmId).HasColumnName("EM_ID");

                entity.Property(e => e.FromTime)
                    .HasMaxLength(50)
                    .HasColumnName("FROM_TIME");

                entity.Property(e => e.OffId).HasColumnName("OFF_ID");

                entity.Property(e => e.OffdId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("OFFD_ID");

                entity.Property(e => e.ToTime)
                    .HasMaxLength(50)
                    .HasColumnName("TO_TIME");
            });

            modelBuilder.Entity<AProject>(entity =>
            {
                entity.HasKey(e => e.ProId);

                entity.ToTable("A_PROJECT");

                entity.Property(e => e.ProId).HasColumnName("PRO_ID");

                entity.Property(e => e.ProName)
                    .HasMaxLength(200)
                    .HasColumnName("PRO_NAME");

                entity.Property(e => e.ProStatus)
                    .HasColumnName("PRO_STATUS")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<AReader>(entity =>
            {
                entity.HasKey(e => e.ReId);

                entity.ToTable("A_READER");

                entity.Property(e => e.ReId).HasColumnName("RE_ID");

                entity.Property(e => e.ReElevator)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("RE_ELEVATOR");

                entity.Property(e => e.ReNumber)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("RE_NUMBER");
            });

            modelBuilder.Entity<ARealtimeMonitor>(entity =>
            {
                entity.HasKey(e => e.RtId)
                    .HasName("PK_T_REALTIME_MONITOR");

                entity.ToTable("A_REALTIME_MONITOR");

                entity.Property(e => e.RtId).HasColumnName("RT_ID");

                entity.Property(e => e.CaId).HasColumnName("CA_ID");

                entity.Property(e => e.DevId).HasColumnName("DEV_ID");

                entity.Property(e => e.EmId).HasColumnName("EM_ID");

                entity.Property(e => e.IoStatus)
                    .HasColumnName("IO_STATUS")
                    .HasComment("trạng thái vào - ra 0: KHÔNG PHÂN BIỆT, 1: VÀO , 2: RA ");

                entity.Property(e => e.TacId).HasColumnName("TAC_ID");

                entity.Property(e => e.TatDate)
                    .HasColumnType("date")
                    .HasColumnName("TAT_DATE");

                entity.Property(e => e.TatTime)
                    .HasColumnType("time(0)")
                    .HasColumnName("TAT_TIME");

                entity.Property(e => e.Zone).HasColumnName("ZONE");
            });

            modelBuilder.Entity<AResultDateAtt>(entity =>
            {
                entity.HasKey(e => e.RdaId);

                entity.ToTable("A_RESULT_DATE_ATT");

                entity.Property(e => e.RdaId).HasColumnName("RDA_ID");

                entity.Property(e => e.RdaValue)
                    .HasColumnType("date")
                    .HasColumnName("RDA_VALUE");
            });

            modelBuilder.Entity<AResultDateAtt2>(entity =>
            {
                entity.HasKey(e => e.RdaId2);

                entity.ToTable("A_RESULT_DATE_ATT2");

                entity.Property(e => e.RdaId2).HasColumnName("RDA_ID2");

                entity.Property(e => e.RdaValue2)
                    .HasColumnType("date")
                    .HasColumnName("RDA_VALUE2");
            });

            modelBuilder.Entity<AResultMonthAtt>(entity =>
            {
                entity.HasKey(e => e.RmaId);

                entity.ToTable("A_RESULT_MONTH_ATT");

                entity.Property(e => e.RmaId).HasColumnName("RMA_ID");

                entity.Property(e => e.EmId).HasColumnName("EM_ID");

                entity.Property(e => e.MaId).HasColumnName("MA_ID");

                entity.Property(e => e.Rma1)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_1");

                entity.Property(e => e.Rma10)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_10");

                entity.Property(e => e.Rma11)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_11");

                entity.Property(e => e.Rma12)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_12");

                entity.Property(e => e.Rma13)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_13");

                entity.Property(e => e.Rma14)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_14");

                entity.Property(e => e.Rma15)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_15");

                entity.Property(e => e.Rma16)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_16");

                entity.Property(e => e.Rma17)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_17");

                entity.Property(e => e.Rma18)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_18");

                entity.Property(e => e.Rma19)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_19");

                entity.Property(e => e.Rma2)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_2");

                entity.Property(e => e.Rma20)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_20");

                entity.Property(e => e.Rma21)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_21");

                entity.Property(e => e.Rma22)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_22");

                entity.Property(e => e.Rma23)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_23");

                entity.Property(e => e.Rma24)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_24");

                entity.Property(e => e.Rma25)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_25");

                entity.Property(e => e.Rma26)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_26");

                entity.Property(e => e.Rma27)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_27");

                entity.Property(e => e.Rma28)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_28");

                entity.Property(e => e.Rma29)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_29");

                entity.Property(e => e.Rma3)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_3");

                entity.Property(e => e.Rma30)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_30");

                entity.Property(e => e.Rma31)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_31");

                entity.Property(e => e.Rma4)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_4");

                entity.Property(e => e.Rma5)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_5");

                entity.Property(e => e.Rma6)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_6");

                entity.Property(e => e.Rma7)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_7");

                entity.Property(e => e.Rma8)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_8");

                entity.Property(e => e.Rma9)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_9");

                entity.Property(e => e.RmaMiss)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("RMA_MISS");

                entity.Property(e => e.RmaSum).HasColumnName("RMA_SUM");

                entity.Property(e => e.TacId).HasColumnName("TAC_ID");

                entity.HasOne(d => d.Ma)
                    .WithMany(p => p.AResultMonthAtts)
                    .HasForeignKey(d => d.MaId)
                    .HasConstraintName("FK_A_RESULT_MONTH_ATT_MONTH_ATT");
            });

            modelBuilder.Entity<AResultMonthAtt2>(entity =>
            {
                entity.HasKey(e => e.RmaId2);

                entity.ToTable("A_RESULT_MONTH_ATT2");

                entity.Property(e => e.RmaId2).HasColumnName("RMA_ID2");

                entity.Property(e => e.EmId).HasColumnName("EM_ID");

                entity.Property(e => e.MaId2).HasColumnName("MA_ID2");

                entity.Property(e => e.Rma1)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_1");

                entity.Property(e => e.Rma10)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_10");

                entity.Property(e => e.Rma11)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_11");

                entity.Property(e => e.Rma12)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_12");

                entity.Property(e => e.Rma13)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_13");

                entity.Property(e => e.Rma14)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_14");

                entity.Property(e => e.Rma15)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_15");

                entity.Property(e => e.Rma16)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_16");

                entity.Property(e => e.Rma17)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_17");

                entity.Property(e => e.Rma18)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_18");

                entity.Property(e => e.Rma19)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_19");

                entity.Property(e => e.Rma2)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_2");

                entity.Property(e => e.Rma20)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_20");

                entity.Property(e => e.Rma21)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_21");

                entity.Property(e => e.Rma22)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_22");

                entity.Property(e => e.Rma23)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_23");

                entity.Property(e => e.Rma24)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_24");

                entity.Property(e => e.Rma25)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_25");

                entity.Property(e => e.Rma26)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_26");

                entity.Property(e => e.Rma27)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_27");

                entity.Property(e => e.Rma28)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_28");

                entity.Property(e => e.Rma29)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_29");

                entity.Property(e => e.Rma3)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_3");

                entity.Property(e => e.Rma30)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_30");

                entity.Property(e => e.Rma31)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_31");

                entity.Property(e => e.Rma4)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_4");

                entity.Property(e => e.Rma5)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_5");

                entity.Property(e => e.Rma6)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_6");

                entity.Property(e => e.Rma7)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_7");

                entity.Property(e => e.Rma8)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_8");

                entity.Property(e => e.Rma9)
                    .HasMaxLength(20)
                    .HasColumnName("RMA_9");

                entity.Property(e => e.RmaMiss)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("RMA_MISS");

                entity.Property(e => e.RmaSum).HasColumnName("RMA_SUM");

                entity.Property(e => e.TacId).HasColumnName("TAC_ID");

                entity.HasOne(d => d.MaId2Navigation)
                    .WithMany(p => p.AResultMonthAtt2s)
                    .HasForeignKey(d => d.MaId2)
                    .HasConstraintName("FK_RESULT_MONTH_ATT2_MONTH_ATT2");
            });

            modelBuilder.Entity<AResultTimeAtt>(entity =>
            {
                entity.HasKey(e => e.RtaId);

                entity.ToTable("A_RESULT_TIME_ATT");

                entity.Property(e => e.RtaId).HasColumnName("RTA_ID");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("DATE");

                entity.Property(e => e.Day)
                    .HasMaxLength(50)
                    .HasColumnName("DAY");

                entity.Property(e => e.EmId).HasColumnName("EM_ID");

                entity.Property(e => e.RdaId).HasColumnName("RDA_ID");

                entity.Property(e => e.Result1)
                    .HasMaxLength(50)
                    .HasColumnName("RESULT1");

                entity.Property(e => e.Result2)
                    .HasMaxLength(50)
                    .HasColumnName("RESULT2");

                entity.Property(e => e.Result3)
                    .HasMaxLength(50)
                    .HasColumnName("RESULT3");

                entity.Property(e => e.Result4)
                    .HasMaxLength(50)
                    .HasColumnName("RESULT4");

                entity.Property(e => e.ResultAll)
                    .HasMaxLength(200)
                    .HasColumnName("RESULT_ALL");

                entity.Property(e => e.ResultCheck)
                    .HasColumnName("RESULT_CHECK")
                    .HasComment("0: nghỉ, 1: vào muộn hoặc về sớm, 2: vào muộn và về sớm");

                entity.Property(e => e.TacId).HasColumnName("TAC_ID");

                entity.Property(e => e.Time1)
                    .HasColumnType("time(0)")
                    .HasColumnName("TIME1");

                entity.Property(e => e.Time2)
                    .HasColumnType("time(0)")
                    .HasColumnName("TIME2");

                entity.Property(e => e.Time3)
                    .HasColumnType("time(0)")
                    .HasColumnName("TIME3");

                entity.Property(e => e.Time4)
                    .HasColumnType("time(0)")
                    .HasColumnName("TIME4");

                entity.HasOne(d => d.Rda)
                    .WithMany(p => p.AResultTimeAtts)
                    .HasForeignKey(d => d.RdaId)
                    .HasConstraintName("FK_A_RESULT_TIME_ATT_A_RESULT_DATE_ATT");
            });

            modelBuilder.Entity<AResultTimeAtt2>(entity =>
            {
                entity.HasKey(e => e.RtaId2);

                entity.ToTable("A_RESULT_TIME_ATT2");

                entity.Property(e => e.RtaId2).HasColumnName("RTA_ID2");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("DATE");

                entity.Property(e => e.Day)
                    .HasMaxLength(50)
                    .HasColumnName("DAY");

                entity.Property(e => e.EmId).HasColumnName("EM_ID");

                entity.Property(e => e.RdaId2).HasColumnName("RDA_ID2");

                entity.Property(e => e.Result1)
                    .HasMaxLength(50)
                    .HasColumnName("RESULT1");

                entity.Property(e => e.Result2)
                    .HasMaxLength(50)
                    .HasColumnName("RESULT2");

                entity.Property(e => e.Result3)
                    .HasMaxLength(50)
                    .HasColumnName("RESULT3");

                entity.Property(e => e.Result4)
                    .HasMaxLength(50)
                    .HasColumnName("RESULT4");

                entity.Property(e => e.ResultAll)
                    .HasMaxLength(200)
                    .HasColumnName("RESULT_ALL");

                entity.Property(e => e.ResultCheck).HasColumnName("RESULT_CHECK");

                entity.Property(e => e.TacId).HasColumnName("TAC_ID");

                entity.Property(e => e.Time1)
                    .HasColumnType("time(0)")
                    .HasColumnName("TIME1");

                entity.Property(e => e.Time2)
                    .HasColumnType("time(0)")
                    .HasColumnName("TIME2");

                entity.Property(e => e.Time3)
                    .HasColumnType("time(0)")
                    .HasColumnName("TIME3");

                entity.Property(e => e.Time4)
                    .HasColumnType("time(0)")
                    .HasColumnName("TIME4");

                entity.HasOne(d => d.RdaId2Navigation)
                    .WithMany(p => p.AResultTimeAtt2s)
                    .HasForeignKey(d => d.RdaId2)
                    .HasConstraintName("FK_RESULT_TIME_ATT2_RESULT_DATE_ATT2");
            });

            modelBuilder.Entity<ASchedule>(entity =>
            {
                entity.HasKey(e => e.ScheId);

                entity.ToTable("A_SCHEDULE");

                entity.Property(e => e.ScheId).HasColumnName("SCHE_ID");

                entity.Property(e => e.ScheCode).HasColumnName("SCHE_CODE");

                entity.Property(e => e.ScheDay)
                    .HasMaxLength(20)
                    .HasColumnName("SCHE_DAY");

                entity.Property(e => e.ScheMiddleTime)
                    .HasColumnType("time(0)")
                    .HasColumnName("SCHE_MIDDLE_TIME");

                entity.Property(e => e.ScheTime1)
                    .HasColumnType("time(0)")
                    .HasColumnName("SCHE_TIME1");

                entity.Property(e => e.ScheTime2)
                    .HasColumnType("time(0)")
                    .HasColumnName("SCHE_TIME2");

                entity.Property(e => e.ScheTime3)
                    .HasColumnType("time(0)")
                    .HasColumnName("SCHE_TIME3");

                entity.Property(e => e.ScheTime4)
                    .HasColumnType("time(0)")
                    .HasColumnName("SCHE_TIME4");

                entity.Property(e => e.ScheTimeDistance)
                    .HasColumnName("SCHE_TIME_DISTANCE")
                    .HasComment("Khoảng cách giao ca");
            });

            modelBuilder.Entity<AScheduleDeviceDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("A_SCHEDULE_DEVICE_DETAIL");

                entity.Property(e => e.Fri)
                    .HasMaxLength(500)
                    .HasColumnName("FRI");

                entity.Property(e => e.Mon)
                    .HasMaxLength(500)
                    .HasColumnName("MON");

                entity.Property(e => e.Sat)
                    .HasMaxLength(500)
                    .HasColumnName("SAT");

                entity.Property(e => e.SchDevId).HasColumnName("SCH_DEV_ID");

                entity.Property(e => e.SchDevName)
                    .HasMaxLength(200)
                    .HasColumnName("SCH_DEV_NAME");

                entity.Property(e => e.Sun)
                    .HasMaxLength(500)
                    .HasColumnName("SUN");

                entity.Property(e => e.Thu)
                    .HasMaxLength(500)
                    .HasColumnName("THU");

                entity.Property(e => e.Tue)
                    .HasMaxLength(500)
                    .HasColumnName("TUE");

                entity.Property(e => e.Wed)
                    .HasMaxLength(500)
                    .HasColumnName("WED");
            });

            modelBuilder.Entity<ATimeAdd>(entity =>
            {
                entity.HasKey(e => e.TiaId);

                entity.ToTable("A_TIME_ADD");

                entity.Property(e => e.TiaId).HasColumnName("TIA_ID");

                entity.Property(e => e.EmId).HasColumnName("EM_ID");

                entity.Property(e => e.ProId).HasColumnName("PRO_ID");

                entity.Property(e => e.TiaFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("TIA_FROM");

                entity.Property(e => e.TiaTo)
                    .HasColumnType("datetime")
                    .HasColumnName("TIA_TO");

                entity.HasOne(d => d.Em)
                    .WithMany(p => p.ATimeAdds)
                    .HasForeignKey(d => d.EmId)
                    .HasConstraintName("FK_A_TIME_ADD_EMPLOYEE");

                entity.HasOne(d => d.Pro)
                    .WithMany(p => p.ATimeAdds)
                    .HasForeignKey(d => d.ProId)
                    .HasConstraintName("FK_A_TIME_ADD_PROJECT");
            });

            modelBuilder.Entity<ATimeAttendance>(entity =>
            {
                entity.HasKey(e => e.TatId);

                entity.ToTable("A_TIME_ATTENDANCE");

                entity.Property(e => e.TatId).HasColumnName("TAT_ID");

                entity.Property(e => e.CaId).HasColumnName("CA_ID");

                entity.Property(e => e.DatId).HasColumnName("DAT_ID");

                entity.Property(e => e.DevId).HasColumnName("DEV_ID");

                entity.Property(e => e.EmId).HasColumnName("EM_ID");

                entity.Property(e => e.TacId).HasColumnName("TAC_ID");

                entity.Property(e => e.TatDate)
                    .HasColumnType("date")
                    .HasColumnName("TAT_DATE");

                entity.Property(e => e.TatTime)
                    .HasColumnType("time(0)")
                    .HasColumnName("TAT_TIME");
            });

            modelBuilder.Entity<AZone>(entity =>
            {
                entity.HasKey(e => e.ZonId);

                entity.ToTable("A_ZONE");

                entity.Property(e => e.ZonId).HasColumnName("ZON_ID");

                entity.Property(e => e.ZonDescription)
                    .HasMaxLength(200)
                    .HasColumnName("ZON_DESCRIPTION");

                entity.Property(e => e.ZonName)
                    .HasMaxLength(200)
                    .HasColumnName("ZON_NAME");

                entity.Property(e => e.ZonStatus)
                    .HasColumnName("ZON_STATUS")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.PlaceId).HasColumnName("PLACE_ID");
            });

           
            modelBuilder.Entity<TAccount>(entity =>
            {
                entity.HasKey(e => e.AccId);

                entity.ToTable("T_ACCOUNTS");

                entity.Property(e => e.AccId).HasColumnName("ACC_ID");

                entity.Property(e => e.AccPassword)
                    .HasMaxLength(200)
                    .HasColumnName("ACC_PASSWORD");

                entity.Property(e => e.AccStatus)
                    .HasColumnName("ACC_STATUS")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.AccUsername)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ACC_USERNAME");

                entity.Property(e => e.EmId).HasColumnName("EM_ID");

                entity.Property(e => e.IpLogin)
                    .HasMaxLength(400)
                    .HasColumnName("IP_LOGIN");

                entity.Property(e => e.PriId).HasColumnName("PRI_ID");

                entity.Property(e => e.ShiftId).HasColumnName("SHIFT_ID");

                entity.HasOne(d => d.Em)
                    .WithMany(p => p.TAccounts)
                    .HasForeignKey(d => d.EmId)
                    .HasConstraintName("FK_T_ACCOUNTS_T_EMPLOYEE1");

                entity.HasOne(d => d.Pri)
                    .WithMany(p => p.TAccounts)
                    .HasForeignKey(d => d.PriId)
                    .HasConstraintName("FK_T_ACCOUNTS_T_PRIVILES");
            }); 

            modelBuilder.Entity<TDepartment>(entity =>
            {
                entity.HasKey(e => e.DepId);

                entity.ToTable("T_DEPARTMENT");

                entity.Property(e => e.DepId).HasColumnName("DEP_ID");

                entity.Property(e => e.DepDescription)
                    .HasMaxLength(200)
                    .HasColumnName("DEP_DESCRIPTION");

                entity.Property(e => e.DepName)
                    .HasMaxLength(100)
                    .HasColumnName("DEP_NAME");

                entity.Property(e => e.DepStatus)
                    .HasColumnName("DEP_STATUS")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TDevice>(entity =>
            {
                entity.HasKey(e => e.DevId)
                    .HasName("PK_A_DEVICE");

                entity.ToTable("T_DEVICE");

                entity.Property(e => e.DevId).HasColumnName("DEV_ID");

                entity.Property(e => e.DevCode)
                    .HasMaxLength(20)
                    .HasColumnName("DEV_CODE");

                entity.Property(e => e.DevIp)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("DEV_IP");

                entity.Property(e => e.DevLaneCheck)
                    .HasColumnName("DEV_LANE_CHECK")
                    .HasComment("trạng thái vào - ra 0: KHÔNG PHÂN BIỆT, 1: VÀO , 2: RA ");

                entity.Property(e => e.DevMacaddress)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEV_MACADDRESS");

                entity.Property(e => e.DevName)
                    .HasMaxLength(100)
                    .HasColumnName("DEV_NAME");

                entity.Property(e => e.DevPartnumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEV_PARTNUMBER");

                entity.Property(e => e.DevPort).HasColumnName("DEV_PORT");

                entity.Property(e => e.DevSerialnumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEV_SERIALNUMBER");

                entity.Property(e => e.DevStatus)
                    .HasColumnName("DEV_STATUS")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DevTimeCheck)
                    .HasColumnName("DEV_TIME_CHECK")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DevTypeId).HasColumnName("DEV_TYPE_ID");

                entity.Property(e => e.ZonId).HasColumnName("ZON_ID");

                entity.HasOne(d => d.DevType)
                    .WithMany(p => p.TDevices)
                    .HasForeignKey(d => d.DevTypeId)
                    .HasConstraintName("FK_T_DEVICE_DEVICE_TYPE");

                entity.HasOne(d => d.Zon)
                    .WithMany(p => p.TDevices)
                    .HasForeignKey(d => d.ZonId)
                    .HasConstraintName("FK_T_DEVICE_ZONE");
            });

            modelBuilder.Entity<TDeviceLicense>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("T_DEVICE_LICENSE");

                entity.Property(e => e.License)
                    .HasMaxLength(100)
                    .HasColumnName("LICENSE");
            });

            modelBuilder.Entity<TDeviceType>(entity =>
            {
                entity.HasKey(e => e.DevTypeId)
                    .HasName("PK_A_DEVICE_TYPE");

                entity.ToTable("T_DEVICE_TYPE");

                entity.Property(e => e.DevTypeId).HasColumnName("DEV_TYPE_ID");

                entity.Property(e => e.DevTypeCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DEV_TYPE_CODE");

                entity.Property(e => e.DevTypeName)
                    .HasMaxLength(50)
                    .HasColumnName("DEV_TYPE_NAME");
            });

            modelBuilder.Entity<TEmployee>(entity =>
            {
                entity.HasKey(e => e.EmId);

                entity.ToTable("T_EMPLOYEE");

                entity.Property(e => e.EmId).HasColumnName("EM_ID");

                entity.Property(e => e.DepId).HasColumnName("DEP_ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(400)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.EmBirthdate)
                    .HasColumnType("date")
                    .HasColumnName("EM_BIRTHDATE");

                entity.Property(e => e.EmCode)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("EM_CODE");

                entity.Property(e => e.EmEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EM_EMAIL");

                entity.Property(e => e.EmGender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EM_GENDER")
                    .HasDefaultValueSql("((1))")
                    .IsFixedLength();


                entity.Property(e => e.EmIdentityNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("EM_IDENTITY_NUMBER");

                entity.Property(e => e.EmImage).HasColumnName("EM_IMAGE");

                entity.Property(e => e.EmName)
                    .HasMaxLength(100)
                    .HasColumnName("EM_NAME");

                entity.Property(e => e.EmPhone)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("EM_PHONE");

                entity.Property(e => e.EmStatus)
                    .HasColumnName("EM_STATUS")
                    .HasDefaultValueSql("((1))");


                entity.Property(e => e.EmTypeId).HasColumnName("EM_TYPE_ID");

                entity.Property(e => e.FaceExist).HasColumnName("FACE_EXIST");


                entity.Property(e => e.RegId).HasColumnName("REG_ID");

                entity.Property(e => e.PlaceId).HasColumnName("PLACE_ID");

                entity.Property(e => e.ZonId).HasColumnName("ZON_ID");

                entity.HasOne(d => d.Dep)
                    .WithMany(p => p.TEmployees)
                    .HasForeignKey(d => d.DepId)
                    .HasConstraintName("FK_T_EMPLOYEE_T_DEPARTMENT");

                entity.HasOne(d => d.EmType)
                    .WithMany(p => p.TEmployees)
                    .HasForeignKey(d => d.EmTypeId)
                    .HasConstraintName("FK_T_EMPLOYEE_T_EMPLOYEE_TYPE1");

                entity.HasOne(d => d.Reg)
                    .WithMany(p => p.TEmployees)
                    .HasForeignKey(d => d.RegId)
                    .HasConstraintName("FK_T_EMPLOYEE_REGENCY");

                entity.HasOne(d => d.Zon)
                    .WithMany(p => p.TEmployees)
                    .HasForeignKey(d => d.ZonId)
                    .HasConstraintName("FK_T_EMPLOYEE_A_ZONE");
            });

            modelBuilder.Entity<TEmployeeFace>(entity =>
            {
                entity.HasKey(e => e.FaceId)
                    .HasName("PK_EM_FACE");

                entity.ToTable("T_EMPLOYEE_FACE");

                entity.Property(e => e.FaceId).HasColumnName("FACE_ID");

                entity.Property(e => e.DevTypeCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DEV_TYPE_CODE");

                entity.Property(e => e.EmId).HasColumnName("EM_ID");

                entity.Property(e => e.FaceData).HasColumnName("FACE_DATA");

                entity.HasOne(d => d.Em)
                    .WithMany(p => p.TEmployeeFaces)
                    .HasForeignKey(d => d.EmId)
                    .HasConstraintName("FK_EM_FACE_T_EMPLOYEE");
            });


            modelBuilder.Entity<TEmployeeType>(entity =>
            {
                entity.HasKey(e => e.EmTypeId);

                entity.ToTable("T_EMPLOYEE_TYPE");

                entity.Property(e => e.EmTypeId).HasColumnName("EM_TYPE_ID");

                entity.Property(e => e.EmCheck)
                    .HasColumnName("EM_CHECK")
                    .HasDefaultValueSql("((1))")
                    .HasComment("1 là nhân viên, 0 là cư dân, 2 khách hàng ...");

                entity.Property(e => e.EmType)
                    .HasMaxLength(100)
                    .HasColumnName("EM_TYPE");
            });

            modelBuilder.Entity<TGroupAccess>(entity =>
            {
                entity.HasKey(e => e.GaId)
                    .HasName("PK_A_GROUP_DEVICE");

                entity.ToTable("T_GROUP_ACCESS");

                entity.Property(e => e.GaId).HasColumnName("GA_ID");

                entity.Property(e => e.GaName)
                    .HasMaxLength(50)
                    .HasColumnName("GA_NAME");

                entity.Property(e => e.GaStatus)
                    .HasColumnName("GA_STATUS")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TGroupAccessDetail>(entity =>
            {
                entity.HasKey(e => e.GadId)
                    .HasName("PK_A_GROUP_DEVICE_DETAIL");

                entity.ToTable("T_GROUP_ACCESS_DETAIL");

                entity.Property(e => e.GadId).HasColumnName("GAD_ID");

                entity.Property(e => e.DevId).HasColumnName("DEV_ID");

                entity.Property(e => e.GaId).HasColumnName("GA_ID");

                entity.Property(e => e.GadStatus)
                    .HasColumnName("GAD_STATUS")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Dev)
                    .WithMany(p => p.TGroupAccessDetails)
                    .HasForeignKey(d => d.DevId)
                    .HasConstraintName("FK_T_GROUP_DEVICE_DETAIL_T_DEVICE");

                entity.HasOne(d => d.Ga)
                    .WithMany(p => p.TGroupAccessDetails)
                    .HasForeignKey(d => d.GaId)
                    .HasConstraintName("FK_T_GROUP_DEVICE_DETAIL_T_GROUP_DEVICE");
            });

            modelBuilder.Entity<TLog>(entity =>
            {
                entity.HasKey(e => e.LogId);

                entity.ToTable("T_LOG");

                entity.Property(e => e.LogId).HasColumnName("LOG_ID");

                entity.Property(e => e.CaId).HasColumnName("CA_ID");

                entity.Property(e => e.DevId).HasColumnName("DEV_ID");

                entity.Property(e => e.EmId).HasColumnName("EM_ID");

                entity.Property(e => e.LogDate)
                    .HasColumnType("date")
                    .HasColumnName("LOG_DATE");

                entity.Property(e => e.LogTime)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LOG_TIME");

                entity.Property(e => e.TacId).HasColumnName("TAC_ID");
            });

            modelBuilder.Entity<TMenulist>(entity =>
            {
                entity.HasKey(e => e.MnuId);

                entity.ToTable("T_MENULIST");

                entity.Property(e => e.MnuId).HasColumnName("MNU_ID");

                entity.Property(e => e.MnuDisplay)
                    .HasMaxLength(150)
                    .HasColumnName("MNU_DISPLAY");

                entity.Property(e => e.MnuName)
                    .HasMaxLength(100)
                    .HasColumnName("MNU_NAME");

                entity.Property(e => e.MnuParent)
                    .HasMaxLength(100)
                    .HasColumnName("MNU_PARENT");
            });

            modelBuilder.Entity<TPrivile>(entity =>
            {
                entity.HasKey(e => e.PriId);

                entity.ToTable("T_PRIVILES");

                entity.Property(e => e.PriId).HasColumnName("PRI_ID");

                entity.Property(e => e.PriDescription)
                    .HasMaxLength(50)
                    .HasColumnName("PRI_DESCRIPTION");

                entity.Property(e => e.PriStatus)
                    .HasColumnName("PRI_STATUS")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TRegency>(entity =>
            {
                entity.HasKey(e => e.RegId);

                entity.ToTable("T_REGENCY");

                entity.Property(e => e.RegId).HasColumnName("REG_ID");

                entity.Property(e => e.RegDescription)
                    .HasMaxLength(200)
                    .HasColumnName("REG_DESCRIPTION");

                entity.Property(e => e.RegName)
                    .HasMaxLength(100)
                    .HasColumnName("REG_NAME");

                entity.Property(e => e.RegStatus)
                    .HasColumnName("REG_STATUS")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TTimeAttendancyCheck>(entity =>
            {
                entity.HasKey(e => e.TacId)
                    .HasName("PK_A_DEVICE_CHECK");

                entity.ToTable("T_TIME_ATTENDANCY_CHECK");

                entity.Property(e => e.TacId).HasColumnName("TAC_ID");

                entity.Property(e => e.TacCheck)
                    .HasColumnName("TAC_CHECK")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TacCode).HasColumnName("TAC_CODE");

                entity.Property(e => e.TacType)
                    .HasMaxLength(50)
                    .HasColumnName("TAC_TYPE");
            });

            modelBuilder.Entity<TVisiblecontrol>(entity =>
            {
                entity.HasKey(e => e.ViId);

                entity.ToTable("T_VISIBLECONTROLS");

                entity.Property(e => e.ViId).HasColumnName("VI_ID");

                entity.Property(e => e.MnuId).HasColumnName("MNU_ID");

                entity.Property(e => e.PriId).HasColumnName("PRI_ID");

                entity.HasOne(d => d.Mnu)
                    .WithMany(p => p.TVisiblecontrols)
                    .HasForeignKey(d => d.MnuId)
                    .HasConstraintName("FK_T_VISIBLECONTROLS_MENULIST");

                entity.HasOne(d => d.Pri)
                    .WithMany(p => p.TVisiblecontrols)
                    .HasForeignKey(d => d.PriId)
                    .HasConstraintName("FK_T_VISIBLECONTROLS_PRIVILES");
            });

        }
    }
}