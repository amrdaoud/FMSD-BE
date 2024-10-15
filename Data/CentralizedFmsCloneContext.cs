using System;
using System.Collections.Generic;
using FMSD_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace FMSD_BE.Data;

public partial class CentralizedFmsCloneContext : DbContext
{
    public CentralizedFmsCloneContext()
    {
    }

    public CentralizedFmsCloneContext(DbContextOptions<CentralizedFmsCloneContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<Alarm> Alarms { get; set; }

    public virtual DbSet<AlarmStatus> AlarmStatuses { get; set; }

    public virtual DbSet<AlarmsVw> AlarmsVws { get; set; }

    public virtual DbSet<Calibration> Calibrations { get; set; }

    public virtual DbSet<CalibrationDetail> CalibrationDetails { get; set; }

    public virtual DbSet<CalibrationView> CalibrationViews { get; set; }

    public virtual DbSet<ConfigsNew> ConfigsNews { get; set; }

    public virtual DbSet<EngineerConfig> EngineerConfigs { get; set; }

    public virtual DbSet<FailedJob> FailedJobs { get; set; }

    public virtual DbSet<FuelTransaction> FuelTransactions { get; set; }

    public virtual DbSet<FuelTransactionsVw> FuelTransactionsVws { get; set; }

    public virtual DbSet<FuelTransactionsVwSummary> FuelTransactionsVwSummaries { get; set; }

    public virtual DbSet<FuelTransactionsVwSummaryUpdated> FuelTransactionsVwSummaryUpdateds { get; set; }

    public virtual DbSet<FuelTransactionsVwUpdated> FuelTransactionsVwUpdateds { get; set; }

    public virtual DbSet<LastReadingVw> LastReadingVws { get; set; }

    public virtual DbSet<LastReadingVw1> LastReadingVw1s { get; set; }

    public virtual DbSet<LastReadingVw1Updated> LastReadingVw1Updateds { get; set; }

    public virtual DbSet<Leakage> Leakages { get; set; }

    public virtual DbSet<LeakagesVw> LeakagesVws { get; set; }

    public virtual DbSet<LocalServersLocalDb> LocalServersLocalDbs { get; set; }

    public virtual DbSet<Migration> Migrations { get; set; }

    public virtual DbSet<ModelHasPermission> ModelHasPermissions { get; set; }

    public virtual DbSet<ModelHasRole> ModelHasRoles { get; set; }

    public virtual DbSet<MyLog> MyLogs { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationType> NotificationTypes { get; set; }

    public virtual DbSet<NotificationTypeRole> NotificationTypeRoles { get; set; }

    public virtual DbSet<NotificationVw> NotificationVws { get; set; }

    public virtual DbSet<NotificationsNew> NotificationsNews { get; set; }

    public virtual DbSet<NozzleStatus> NozzleStatuses { get; set; }

    public virtual DbSet<OauthAccessToken> OauthAccessTokens { get; set; }

    public virtual DbSet<OauthAuthCode> OauthAuthCodes { get; set; }

    public virtual DbSet<OauthClient> OauthClients { get; set; }

    public virtual DbSet<OauthPersonalAccessClient> OauthPersonalAccessClients { get; set; }

    public virtual DbSet<OauthRefreshToken> OauthRefreshTokens { get; set; }

    public virtual DbSet<OperationType> OperationTypes { get; set; }

    public virtual DbSet<PasswordReset> PasswordResets { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PersonalAccessToken> PersonalAccessTokens { get; set; }

    public virtual DbSet<Pump> Pumps { get; set; }

    public virtual DbSet<PumpStatus> PumpStatuses { get; set; }

    public virtual DbSet<PumpTank> PumpTanks { get; set; }

    public virtual DbSet<RfidCard> RfidCards { get; set; }

    public virtual DbSet<RfidDevice> RfidDevices { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleHasPermission> RoleHasPermissions { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<StationTanksPumVw> StationTanksPumVws { get; set; }

    public virtual DbSet<SystemConfig> SystemConfigs { get; set; }

    public virtual DbSet<TablesLastTranDate> TablesLastTranDates { get; set; }

    public virtual DbSet<Tank> Tanks { get; set; }

    public virtual DbSet<TankMeasurement> TankMeasurements { get; set; }

    public virtual DbSet<TankMeasurementsEvery15Vw> TankMeasurementsEvery15Vws { get; set; }

    public virtual DbSet<TankMeasurementsNew> TankMeasurementsNews { get; set; }

    public virtual DbSet<TankMeasurementsTwiceADay> TankMeasurementsTwiceADays { get; set; }

    public virtual DbSet<TankMeasurementsVw> TankMeasurementsVws { get; set; }

    public virtual DbSet<TankStatus> TankStatuses { get; set; }

    public virtual DbSet<TransactionDetail> TransactionDetails { get; set; }

    public virtual DbSet<TransactionStatus> TransactionStatuses { get; set; }

    public virtual DbSet<UpdateLogsView> UpdateLogsViews { get; set; }

    public virtual DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__activity__3213E83FFA4CD859");

            entity.ToTable("activity_log");

            entity.HasIndex(e => new { e.LocalId, e.StationGuid }, "UQ_ACTIVITYLOG").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BatchUuid).HasColumnName("batch_uuid");
            entity.Property(e => e.CauserId).HasColumnName("causer_id");
            entity.Property(e => e.CauserType)
                .HasMaxLength(255)
                .HasColumnName("causer_type");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Event)
                .HasMaxLength(255)
                .HasColumnName("event");
            entity.Property(e => e.LocalId).HasColumnName("local_id");
            entity.Property(e => e.LogName)
                .HasMaxLength(255)
                .HasColumnName("log_name");
            entity.Property(e => e.Properties).HasColumnName("properties");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.SubjectId)
                .HasMaxLength(255)
                .HasColumnName("subject_id");
            entity.Property(e => e.SubjectType)
                .HasMaxLength(255)
                .HasColumnName("subject_type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Station).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.StationGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACTIVITYLOG_STATIONS");
        });

        modelBuilder.Entity<Alarm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__alarms__3213E83FFED6966C");

            entity.ToTable("alarms");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcknowledgedTime)
                .HasColumnType("datetime")
                .HasColumnName("acknowledged_time");
            entity.Property(e => e.AcknowledgedUserId).HasColumnName("acknowledged_user_id");
            entity.Property(e => e.AlarmCode)
                .HasMaxLength(255)
                .HasColumnName("alarm_code");
            entity.Property(e => e.AlarmStatusId).HasColumnName("alarm_status_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FixTime)
                .HasColumnType("datetime")
                .HasColumnName("fix_time");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.Show)
                .HasDefaultValueSql("('0')")
                .HasColumnName("show");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.TrrigerId).HasColumnName("trriger_id");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Station).WithMany(p => p.Alarms)
                .HasForeignKey(d => d.StationGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("alarms_station_guid_foreign");

            entity.HasOne(d => d.User).WithMany(p => p.Alarms)
                .HasForeignKey(d => new { d.AcknowledgedUserId, d.StationGuid })
                .HasConstraintName("FK_ALARMS_USERS");

            entity.HasOne(d => d.AlarmStatus).WithMany(p => p.Alarms)
                .HasPrincipalKey(p => new { p.LocalId, p.StationGuid })
                .HasForeignKey(d => new { d.AlarmStatusId, d.StationGuid })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ALARMS_ALARM_STATUSES");
        });

        modelBuilder.Entity<AlarmStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__alarm_st__3213E83F13808649");

            entity.ToTable("alarm_statuses");

            entity.HasIndex(e => new { e.LocalId, e.StationGuid }, "UQ_ALARM_STATUSES").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.LocalId)
                .IsRequired()
                .HasColumnName("local_id");
            entity.Property(e => e.StationGuid)
                .HasDefaultValue(new Guid("96700415-8913-406a-a131-464d1f5f05fb"))
                .HasColumnName("station_guid");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<AlarmsVw>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("alarms_vw");

            entity.Property(e => e.AcknowledgedTime)
                .HasColumnType("datetime")
                .HasColumnName("acknowledged_time");
            entity.Property(e => e.AcknowledgedUserId).HasColumnName("acknowledged_user_id");
            entity.Property(e => e.AlarmCode)
                .HasMaxLength(255)
                .HasColumnName("alarm_code");
            entity.Property(e => e.AlarmStatusId).HasColumnName("alarm_status_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FixTime)
                .HasColumnType("datetime")
                .HasColumnName("fix_time");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.RowNumber).HasColumnName("row_number");
            entity.Property(e => e.Show).HasColumnName("show");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.StatusEdited)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("status_edited");
            entity.Property(e => e.TimeToAcknowledge).HasColumnName("time_to_acknowledge");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Calibration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__calibrat__3213E83FBAFF59BB");

            entity.ToTable("calibrations");

            entity.HasIndex(e => new { e.OldId, e.StationGuid }, "UQ_CALIBRATIONS").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DispAmountPump).HasColumnName("disp_amountPump");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.MeasuredAmount).HasColumnName("measured_amount");
            entity.Property(e => e.Note)
                .HasMaxLength(2000)
                .HasColumnName("note");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.OrderedAmount).HasColumnName("ordered_amount");
            entity.Property(e => e.PumpGuid).HasColumnName("pump_guid");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.StationGuid)
                .HasDefaultValue(new Guid("96700415-8913-406a-a131-464d1f5f05fb"))
                .HasColumnName("station_guid");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Pump).WithMany(p => p.Calibrations)
                .HasForeignKey(d => d.PumpGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("calibrations_pump_guid_foreign");

            entity.HasOne(d => d.User).WithMany(p => p.Calibrations)
                .HasForeignKey(d => new { d.UserId, d.StationGuid })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CALIBRATIONS_USERS");
        });

        modelBuilder.Entity<CalibrationDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__calibrat__3213E83F9477219B");

            entity.ToTable("calibration_details");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CalibrationId).HasColumnName("calibration_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.FuelAfter).HasColumnName("fuel_after");
            entity.Property(e => e.FuelBefore).HasColumnName("fuel_before");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.StationGuid)
                .HasDefaultValue(new Guid("96700415-8913-406a-a131-464d1f5f05fb"))
                .HasColumnName("station_guid");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.TcvAfter).HasColumnName("tcv_after");
            entity.Property(e => e.TcvBefore).HasColumnName("tcv_before");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Tank).WithMany(p => p.CalibrationDetails)
                .HasForeignKey(d => d.TankGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("calibration_details_tank_guid_foreign");
        });

        modelBuilder.Entity<CalibrationView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("calibration_View");

            entity.Property(e => e.CalibrationLimit).HasColumnName("calibration_limit");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DispAmountPump).HasColumnName("disp_amountPump");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LogicalAddress)
                .HasMaxLength(255)
                .HasColumnName("logical_address");
            entity.Property(e => e.MeasuredAmount).HasColumnName("measured_amount");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.OrderedAmount).HasColumnName("ordered_amount");
            entity.Property(e => e.PhysicalAddress)
                .HasMaxLength(255)
                .HasColumnName("physical_address");
            entity.Property(e => e.PumpGuid).HasColumnName("pump_guid");
            entity.Property(e => e.RowNumber).HasColumnName("row_number");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.TotalDispAmount).HasColumnName("total_disp_amount");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        modelBuilder.Entity<ConfigsNew>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__configs___3213E83FF6211F43");

            entity.ToTable("configs_new");

            entity.HasIndex(e => new { e.Type, e.StationGuid, e.LocalId }, "UQ_CONFIGS_NEW").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Info).HasColumnName("info");
            entity.Property(e => e.Key)
                .HasMaxLength(255)
                .HasColumnName("key");
            entity.Property(e => e.LocalId).HasColumnName("local_id");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Station).WithMany(p => p.ConfigsNews)
                .HasForeignKey(d => d.StationGuid)
                .HasConstraintName("FK_CONFIGSNEW_STATIONS");
        });

        modelBuilder.Entity<EngineerConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__engineer__3213E83FFE31CFE9");

            entity.ToTable("engineer_configs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AtgProtocol)
                .HasMaxLength(255)
                .HasColumnName("atg_protocol");
            entity.Property(e => e.ComAtg)
                .HasMaxLength(255)
                .HasColumnName("com_atg");
            entity.Property(e => e.ComDesp)
                .HasMaxLength(255)
                .HasColumnName("com_desp");
            entity.Property(e => e.ComRfid)
                .HasMaxLength(255)
                .HasColumnName("com_rfid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PumpDurRate).HasColumnName("pump_dur_rate");
            entity.Property(e => e.PumpOutRate).HasColumnName("pump_out_rate");
            entity.Property(e => e.SenDurRate).HasColumnName("sen_dur_rate");
            entity.Property(e => e.SenOutRate).HasColumnName("sen_out_rate");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Station).WithMany(p => p.EngineerConfigs)
                .HasForeignKey(d => d.StationGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("engineer_configs_station_guid_foreign");
        });

        modelBuilder.Entity<FailedJob>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__failed_j__3213E83F17571ACB");

            entity.ToTable("failed_jobs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Connection).HasColumnName("connection");
            entity.Property(e => e.Exception).HasColumnName("exception");
            entity.Property(e => e.FailedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("failed_at");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.Queue).HasColumnName("queue");
            entity.Property(e => e.Uuid)
                .HasMaxLength(255)
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<FuelTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__fuel_tra__3213E83F9B95593F");

            entity.ToTable("fuel_transactions");

            entity.HasIndex(e => new { e.OldId, e.StartTime, e.StationGuid }, "IX_fuel_transactions").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccompanyingName)
                .HasMaxLength(255)
                .HasColumnName("accompanying_name");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DispensedAmount).HasColumnName("dispensed_amount");
            entity.Property(e => e.DispensedTankGuid).HasColumnName("dispensed_tank_guid");
            entity.Property(e => e.DriverLicense)
                .HasMaxLength(255)
                .HasColumnName("driver_license");
            entity.Property(e => e.DriverName)
                .HasMaxLength(255)
                .HasColumnName("driver_name");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.LiterPrice).HasColumnName("liter_price");
            entity.Property(e => e.MeasuredAmount).HasColumnName("measured_amount");
            entity.Property(e => e.Mode)
                .HasMaxLength(255)
                .HasColumnName("mode");
            entity.Property(e => e.Note)
                .HasMaxLength(2000)
                .HasColumnName("note");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.OperationTypeId).HasColumnName("operation_type_id");
            entity.Property(e => e.OrderedAmount).HasColumnName("ordered_amount");
            entity.Property(e => e.PumpGuid).HasColumnName("pump_guid");
            entity.Property(e => e.RequestedTankGuid).HasColumnName("requested_tank_guid");
            entity.Property(e => e.RequisitionNumber)
                .HasMaxLength(255)
                .HasColumnName("requisition_number");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.StationGuid)
                .HasDefaultValue(new Guid("96700415-8913-406a-a131-464d1f5f05fb"))
                .HasColumnName("station_guid");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.TankerPlate)
                .HasMaxLength(255)
                .HasColumnName("tanker_plate");
            entity.Property(e => e.TransactionStatusId).HasColumnName("transaction_status_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.OperationType).WithMany(p => p.FuelTransactions)
                .HasPrincipalKey(p => new { p.LocalId, p.StationGuid })
                .HasForeignKey(d => new { d.OperationTypeId, d.StationGuid })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FUELTRANSACTIONS_OPERATIONTYPES");

            entity.HasOne(d => d.TransactionStatus).WithMany(p => p.FuelTransactions)
                .HasPrincipalKey(p => new { p.LocalId, p.StationGuid })
                .HasForeignKey(d => new { d.TransactionStatusId, d.StationGuid })
                .HasConstraintName("FK_FUELTRANSACTIONS_TRANSACTIONSTATUSES");
        });

        modelBuilder.Entity<FuelTransactionsVw>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("fuel_transactions_vw");

            entity.Property(e => e.AccompanyingName)
                .HasMaxLength(255)
                .HasColumnName("accompanying_name");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DetailTankName)
                .HasMaxLength(255)
                .HasColumnName("detail_tank_name");
            entity.Property(e => e.DetailsOldId).HasColumnName("details_old_id");
            entity.Property(e => e.DispensedAmount).HasColumnName("dispensed_amount");
            entity.Property(e => e.DispensedTankCapacity).HasColumnName("dispensed_tank_capacity");
            entity.Property(e => e.DispensedTankGuid).HasColumnName("dispensed_tank_guid");
            entity.Property(e => e.DispensedTankName)
                .HasMaxLength(255)
                .HasColumnName("dispensed_tank_name");
            entity.Property(e => e.DriverLicense)
                .HasMaxLength(255)
                .HasColumnName("driver_license");
            entity.Property(e => e.DriverName)
                .HasMaxLength(255)
                .HasColumnName("driver_name");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.FuelTransactionId).HasColumnName("fuel_transaction_id");
            entity.Property(e => e.FuelVolumeAfter).HasColumnName("fuel_volume_after");
            entity.Property(e => e.FuelVolumeBefore).HasColumnName("fuel_volume_before");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LiterPrice).HasColumnName("liter_price");
            entity.Property(e => e.LogicalAddress)
                .HasMaxLength(255)
                .HasColumnName("logical_address");
            entity.Property(e => e.MeasuredAmount).HasColumnName("measured_amount");
            entity.Property(e => e.Mode)
                .HasMaxLength(255)
                .HasColumnName("mode");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.OperationTypeId).HasColumnName("operation_type_id");
            entity.Property(e => e.OrderedAmount).HasColumnName("ordered_amount");
            entity.Property(e => e.PhysicalAddress)
                .HasMaxLength(255)
                .HasColumnName("physical_address");
            entity.Property(e => e.PumpGuid).HasColumnName("pump_guid");
            entity.Property(e => e.RequestedTankGuid).HasColumnName("requested_tank_guid");
            entity.Property(e => e.RequisitionNumber)
                .HasMaxLength(255)
                .HasColumnName("requisition_number");
            entity.Property(e => e.RowNumber).HasColumnName("row_number");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.StationCity)
                .HasMaxLength(255)
                .HasColumnName("station_city");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.TankerPlate)
                .HasMaxLength(255)
                .HasColumnName("tanker_plate");
            entity.Property(e => e.TcvAfter).HasColumnName("tcv_after");
            entity.Property(e => e.TcvBefore).HasColumnName("tcv_before");
            entity.Property(e => e.TransStatus)
                .HasMaxLength(255)
                .HasColumnName("trans_status");
            entity.Property(e => e.TransactionStatusId).HasColumnName("transaction_status_id");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        modelBuilder.Entity<FuelTransactionsVwSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("fuel_transactions_vw_summary");

            entity.Property(e => e.CentralizedDbId).HasColumnName("centralized_db_id");
            entity.Property(e => e.DriverName)
                .HasMaxLength(255)
                .HasColumnName("driver_name");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.FuelAmount).HasColumnName("fuel_amount");
            entity.Property(e => e.RowNumber).HasColumnName("row_number");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.StationCity)
                .HasMaxLength(255)
                .HasColumnName("station_city");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.StationsId).HasColumnName("stations_id");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TankerPlateNumber)
                .HasMaxLength(255)
                .HasColumnName("tanker_plate_number");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(255)
                .HasColumnName("transaction_type");
            entity.Property(e => e.TransactionTypeId).HasColumnName("transaction_type_id");
        });

        modelBuilder.Entity<FuelTransactionsVwSummaryUpdated>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("fuel_transactions_vw_summary_updated");

            entity.Property(e => e.CentralizedDbId).HasColumnName("centralized_db_id");
            entity.Property(e => e.DriverName)
                .HasMaxLength(255)
                .HasColumnName("driver_name");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.FuelAmount).HasColumnName("fuel_amount");
            entity.Property(e => e.RowNumber).HasColumnName("row_number");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.StationCity)
                .HasMaxLength(255)
                .HasColumnName("station_city");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.StationsId).HasColumnName("stations_id");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TankerPlateNumber)
                .HasMaxLength(255)
                .HasColumnName("tanker_plate_number");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(255)
                .HasColumnName("transaction_type");
            entity.Property(e => e.TransactionTypeId).HasColumnName("transaction_type_id");
        });

        modelBuilder.Entity<FuelTransactionsVwUpdated>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("fuel_transactions_vw_updated");

            entity.Property(e => e.AccompanyingName)
                .HasMaxLength(255)
                .HasColumnName("accompanying_name");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DetailTankName)
                .HasMaxLength(255)
                .HasColumnName("detail_tank_name");
            entity.Property(e => e.DetailsOldId).HasColumnName("details_old_id");
            entity.Property(e => e.DispensedAmount).HasColumnName("dispensed_amount");
            entity.Property(e => e.DispensedTankCapacity).HasColumnName("dispensed_tank_capacity");
            entity.Property(e => e.DispensedTankGuid).HasColumnName("dispensed_tank_guid");
            entity.Property(e => e.DispensedTankName)
                .HasMaxLength(255)
                .HasColumnName("dispensed_tank_name");
            entity.Property(e => e.DriverLicense)
                .HasMaxLength(255)
                .HasColumnName("driver_license");
            entity.Property(e => e.DriverName)
                .HasMaxLength(255)
                .HasColumnName("driver_name");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.FuelTransactionId).HasColumnName("fuel_transaction_id");
            entity.Property(e => e.FuelVolumeAfter).HasColumnName("fuel_volume_after");
            entity.Property(e => e.FuelVolumeBefore).HasColumnName("fuel_volume_before");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LiterPrice).HasColumnName("liter_price");
            entity.Property(e => e.LogicalAddress)
                .HasMaxLength(255)
                .HasColumnName("logical_address");
            entity.Property(e => e.MeasuredAmount).HasColumnName("measured_amount");
            entity.Property(e => e.Mode)
                .HasMaxLength(255)
                .HasColumnName("mode");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.OperationTypeId).HasColumnName("operation_type_id");
            entity.Property(e => e.OrderedAmount).HasColumnName("ordered_amount");
            entity.Property(e => e.PhysicalAddress)
                .HasMaxLength(255)
                .HasColumnName("physical_address");
            entity.Property(e => e.PumpGuid).HasColumnName("pump_guid");
            entity.Property(e => e.RequestedTankGuid).HasColumnName("requested_tank_guid");
            entity.Property(e => e.RequisitionNumber)
                .HasMaxLength(255)
                .HasColumnName("requisition_number");
            entity.Property(e => e.RowNumber).HasColumnName("row_number");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.StationCity)
                .HasMaxLength(255)
                .HasColumnName("station_city");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.TankerPlate)
                .HasMaxLength(255)
                .HasColumnName("tanker_plate");
            entity.Property(e => e.TcvAfter).HasColumnName("tcv_after");
            entity.Property(e => e.TcvBefore).HasColumnName("tcv_before");
            entity.Property(e => e.TransStatus)
                .HasMaxLength(255)
                .HasColumnName("trans_status");
            entity.Property(e => e.TransactionStatusId).HasColumnName("transaction_status_id");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        modelBuilder.Entity<LastReadingVw>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("last_reading_vw");

            entity.Property(e => e.AvgConsumption).HasColumnName("avg_consumption");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(4000)
                .HasColumnName("created_at");
            entity.Property(e => e.DLLimit).HasColumnName("d_l_limit");
            entity.Property(e => e.DaysCoverd).HasColumnName("days_coverd");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FuelLevel).HasColumnName("fuel_level");
            entity.Property(e => e.FuelVolume).HasColumnName("fuel_volume");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.HighHighLimit).HasColumnName("high_high_limit");
            entity.Property(e => e.HighLimit).HasColumnName("high_limit");
            entity.Property(e => e.Hysteresis).HasColumnName("hysteresis");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LogicalAddress).HasColumnName("logical_address");
            entity.Property(e => e.LowLimit).HasColumnName("low_limit");
            entity.Property(e => e.LowLowLimit).HasColumnName("low_low_limit");
            entity.Property(e => e.MLLimit).HasColumnName("m_l_limit");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.PhysicalAddress).HasColumnName("physical_address");
            entity.Property(e => e.RowNumber).HasColumnName("row_number");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.TankName)
                .HasMaxLength(255)
                .HasColumnName("tank_name");
            entity.Property(e => e.TanksDate)
                .HasMaxLength(4000)
                .HasColumnName("tanks_date");
            entity.Property(e => e.Tcv).HasColumnName("tcv");
            entity.Property(e => e.Temperature).HasColumnName("temperature");
            entity.Property(e => e.Tg).HasColumnName("tg");
            entity.Property(e => e.Ullage).HasColumnName("ullage");
            entity.Property(e => e.UpdatedAt)
                .HasMaxLength(4000)
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedAtminus30)
                .HasColumnType("datetime")
                .HasColumnName("updated_atminus30");
            entity.Property(e => e.WLLimit).HasColumnName("w_l_limit");
            entity.Property(e => e.WaterHighLimit).HasColumnName("water_high_limit");
            entity.Property(e => e.WaterLevel).HasColumnName("water_level");
            entity.Property(e => e.WaterVolume).HasColumnName("water_volume");
        });

        modelBuilder.Entity<LastReadingVw1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("last_reading_vw1");

            entity.Property(e => e.AvgConsumption).HasColumnName("avg_consumption");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(4000)
                .HasColumnName("created_at");
            entity.Property(e => e.DLLimit).HasColumnName("d_l_limit");
            entity.Property(e => e.DaysCoverd).HasColumnName("days_coverd");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FuelLevel).HasColumnName("fuel_level");
            entity.Property(e => e.FuelVolume).HasColumnName("fuel_volume");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.HighHighLimit).HasColumnName("high_high_limit");
            entity.Property(e => e.HighLimit).HasColumnName("high_limit");
            entity.Property(e => e.Hysteresis).HasColumnName("hysteresis");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LogicalAddress).HasColumnName("logical_address");
            entity.Property(e => e.LowLimit).HasColumnName("low_limit");
            entity.Property(e => e.LowLowLimit).HasColumnName("low_low_limit");
            entity.Property(e => e.MLLimit).HasColumnName("m_l_limit");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.PhysicalAddress).HasColumnName("physical_address");
            entity.Property(e => e.RowNumber).HasColumnName("row_number");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.TankName)
                .HasMaxLength(255)
                .HasColumnName("tank_name");
            entity.Property(e => e.TankNameNoUse)
                .HasMaxLength(255)
                .HasColumnName("tank_name_no_use");
            entity.Property(e => e.TanksDate)
                .HasMaxLength(4000)
                .HasColumnName("tanks_date");
            entity.Property(e => e.Tcv).HasColumnName("tcv");
            entity.Property(e => e.Temperature).HasColumnName("temperature");
            entity.Property(e => e.Ullage).HasColumnName("ullage");
            entity.Property(e => e.UpdatedAt)
                .HasMaxLength(4000)
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedAtminus30)
                .HasColumnType("datetime")
                .HasColumnName("updated_atminus30");
            entity.Property(e => e.WLLimit).HasColumnName("w_l_limit");
            entity.Property(e => e.WaterHighLimit).HasColumnName("water_high_limit");
            entity.Property(e => e.WaterLevel).HasColumnName("water_level");
            entity.Property(e => e.WaterVolume).HasColumnName("water_volume");
        });

        modelBuilder.Entity<LastReadingVw1Updated>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("last_reading_vw1_updated");

            entity.Property(e => e.AvgConsumption).HasColumnName("avg_consumption");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(4000)
                .HasColumnName("created_at");
            entity.Property(e => e.DLLimit).HasColumnName("d_l_limit");
            entity.Property(e => e.DaysCoverd).HasColumnName("days_coverd");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FuelLevel).HasColumnName("fuel_level");
            entity.Property(e => e.FuelVolume).HasColumnName("fuel_volume");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.HighHighLimit).HasColumnName("high_high_limit");
            entity.Property(e => e.HighLimit).HasColumnName("high_limit");
            entity.Property(e => e.Hysteresis).HasColumnName("hysteresis");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LogicalAddress).HasColumnName("logical_address");
            entity.Property(e => e.LowLimit).HasColumnName("low_limit");
            entity.Property(e => e.LowLowLimit).HasColumnName("low_low_limit");
            entity.Property(e => e.MLLimit).HasColumnName("m_l_limit");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.PhysicalAddress).HasColumnName("physical_address");
            entity.Property(e => e.RowNumber).HasColumnName("row_number");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.TankName)
                .HasMaxLength(255)
                .HasColumnName("tank_name");
            entity.Property(e => e.TankNameNoUse)
                .HasMaxLength(255)
                .HasColumnName("tank_name_no_use");
            entity.Property(e => e.TanksDate)
                .HasMaxLength(4000)
                .HasColumnName("tanks_date");
            entity.Property(e => e.Tcv).HasColumnName("tcv");
            entity.Property(e => e.Temperature).HasColumnName("temperature");
            entity.Property(e => e.Ullage).HasColumnName("ullage");
            entity.Property(e => e.UpdatedAt)
                .HasMaxLength(4000)
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedAtminus30)
                .HasColumnType("datetime")
                .HasColumnName("updated_atminus30");
            entity.Property(e => e.WLLimit).HasColumnName("w_l_limit");
            entity.Property(e => e.WaterHighLimit).HasColumnName("water_high_limit");
            entity.Property(e => e.WaterLevel).HasColumnName("water_level");
            entity.Property(e => e.WaterVolume).HasColumnName("water_volume");
        });

        modelBuilder.Entity<Leakage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__leakages__3213E83FCE822830");

            entity.ToTable("leakages");

            entity.HasIndex(e => new { e.TankGuid, e.OldId }, "UQ_LEAKAGES").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Deviation).HasColumnName("deviation");
            entity.Property(e => e.Leakage1)
                .HasMaxLength(255)
                .HasColumnName("leakage");
            entity.Property(e => e.LeakageType)
                .HasMaxLength(255)
                .HasColumnName("leakage_type");
            entity.Property(e => e.Limit).HasColumnName("limit");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Tank).WithMany(p => p.Leakages)
                .HasForeignKey(d => d.TankGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("leakages_tank_guid_foreign");
        });

        modelBuilder.Entity<LeakagesVw>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("leakages_Vw");

            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Deviation).HasColumnName("deviation");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Leakage)
                .HasMaxLength(255)
                .HasColumnName("leakage");
            entity.Property(e => e.Limit).HasColumnName("limit");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.RowNumber).HasColumnName("row_number");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.TankName)
                .HasMaxLength(255)
                .HasColumnName("tank_name");
            entity.Property(e => e.Type)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Week).HasColumnName("week");
            entity.Property(e => e.WeekOfMonth).HasColumnName("WEEK_OF_MONTH");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<LocalServersLocalDb>(entity =>
        {
            entity.HasKey(e => new { e.LocalServer, e.LocalDb }).HasName("PK__local_se__6DB3D9EF6B9F6C18");

            entity.ToTable("local_servers_local_dbs");

            entity.Property(e => e.LocalServer)
                .HasMaxLength(100)
                .HasColumnName("local_server");
            entity.Property(e => e.LocalDb)
                .HasMaxLength(100)
                .HasColumnName("local_db");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<Migration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__migratio__3213E83F3CDA61F1");

            entity.ToTable("migrations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Batch).HasColumnName("batch");
            entity.Property(e => e.Migration1)
                .HasMaxLength(255)
                .HasColumnName("migration");
        });

        modelBuilder.Entity<ModelHasPermission>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("model_has_permissions");

            entity.HasIndex(e => new { e.PermissionId, e.ModelType, e.ModelId, e.StationGuid }, "UQ_MODEL_HAS_PERMISSIONS").IsUnique();

            entity.Property(e => e.ModelId).HasColumnName("model_id");
            entity.Property(e => e.ModelType)
                .HasMaxLength(255)
                .HasColumnName("model_type");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");

            entity.HasOne(d => d.Permission).WithMany()
                .HasForeignKey(d => new { d.PermissionId, d.StationGuid })
                .HasConstraintName("FK_MODELHASPERMISSIONS_PERMISSIONS");
        });

        modelBuilder.Entity<ModelHasRole>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("model_has_roles");

            entity.HasIndex(e => new { e.RoleId, e.ModelType, e.ModelId, e.StationGuid }, "UQ_MODEL_HAS_ROLES").IsUnique();

            entity.Property(e => e.ModelId).HasColumnName("model_id");
            entity.Property(e => e.ModelType)
                .HasMaxLength(255)
                .HasColumnName("model_type");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");

            entity.HasOne(d => d.Role).WithMany()
                .HasPrincipalKey(p => new { p.LocalId, p.StationGuid })
                .HasForeignKey(d => new { d.RoleId, d.StationGuid })
                .HasConstraintName("FK_MODELHASROLES_ROLES");
        });

        modelBuilder.Entity<MyLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__my_logs__3213E83FAD32AEA7");

            entity.ToTable("my_logs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.After)
                .HasMaxLength(255)
                .HasColumnName("after");
            entity.Property(e => e.Before)
                .HasMaxLength(255)
                .HasColumnName("before");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.Setting)
                .HasMaxLength(255)
                .HasColumnName("setting");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("user_name");

            entity.HasOne(d => d.Station).WithMany(p => p.MyLogs)
                .HasForeignKey(d => d.StationGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("my_logs_station_guid_foreign");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__notifica__3213E83FAC5D12D5");

            entity.ToTable("notifications");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcknowledgedUserId).HasColumnName("acknowledged_user_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.NotificationTypeId).HasColumnName("notification_type_id");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.Show)
                .HasMaxLength(255)
                .HasColumnName("show");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => new { d.AcknowledgedUserId, d.StationGuid })
                .HasConstraintName("FK_NOTIFICATIONS_USERS");

            entity.HasOne(d => d.NotificationType).WithMany(p => p.Notifications)
                .HasPrincipalKey(p => new { p.LocalId, p.StationGuid })
                .HasForeignKey(d => new { d.NotificationTypeId, d.StationGuid })
                .HasConstraintName("FK_NOTIFICATIONS_NOTIFICATIONTYPES");
        });

        modelBuilder.Entity<NotificationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__notifica__3213E83F8D7E40C1");

            entity.ToTable("notification_types");

            entity.HasIndex(e => new { e.LocalId, e.StationGuid }, "UQ_NOTIF_TYPES").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasMaxLength(2000)
                .HasColumnName("description");
            entity.Property(e => e.LocalId)
                .IsRequired()
                .HasColumnName("local_id");
            entity.Property(e => e.StationGuid)
                .HasDefaultValue(new Guid("96700415-8913-406a-a131-464d1f5f05fb"))
                .HasColumnName("station_guid");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<NotificationTypeRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__notifica__3213E83F146D143D");

            entity.ToTable("notification_type_role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.LocalId).HasColumnName("local_id");
            entity.Property(e => e.NotificationTypeId).HasColumnName("notification_type_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.NotificationType).WithMany(p => p.NotificationTypeRoles)
                .HasPrincipalKey(p => new { p.LocalId, p.StationGuid })
                .HasForeignKey(d => new { d.NotificationTypeId, d.StationGuid })
                .HasConstraintName("FK_NOTIFICATIONTYPEROLE_NOTIFICATIONTYPES");

            entity.HasOne(d => d.Role).WithMany(p => p.NotificationTypeRoles)
                .HasPrincipalKey(p => new { p.LocalId, p.StationGuid })
                .HasForeignKey(d => new { d.RoleId, d.StationGuid })
                .HasConstraintName("FK_NOTIFICATIONTYPEROLE_ROLES");
        });

        modelBuilder.Entity<NotificationVw>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("notification_vw");

            entity.Property(e => e.AcknowledgedUserId).HasColumnName("acknowledged_user_id");
            entity.Property(e => e.AcknowledgedUsername)
                .HasMaxLength(255)
                .HasColumnName("acknowledged_username");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.NotificationType)
                .HasMaxLength(2000)
                .HasColumnName("notification_type");
            entity.Property(e => e.NotificationTypeId).HasColumnName("notification_type_id");
            entity.Property(e => e.RowNumber).HasColumnName("row_number");
            entity.Property(e => e.Show)
                .HasMaxLength(255)
                .HasColumnName("show");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("userName");
        });

        modelBuilder.Entity<NotificationsNew>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__notifica__3213E83F51705C16");

            entity.ToTable("notifications_new");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.LocalId).HasColumnName("local_id");
            entity.Property(e => e.NotifiableId).HasColumnName("notifiable_id");
            entity.Property(e => e.NotifiableType)
                .HasMaxLength(255)
                .HasColumnName("notifiable_type");
            entity.Property(e => e.ReadAt)
                .HasColumnType("datetime")
                .HasColumnName("read_at");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Station).WithMany(p => p.NotificationsNews)
                .HasForeignKey(d => d.StationGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NOTIFICATIONS_STATIONS");
        });

        modelBuilder.Entity<NozzleStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__nozzle_s__3213E83F58390377");

            entity.ToTable("nozzle_statuses");

            entity.HasIndex(e => new { e.LocalId, e.StationGuid }, "UQ_NOZZLESTATUSES").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.LocalId)
                .IsRequired()
                .HasColumnName("local_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.StationGuid)
                .IsRequired()
                .HasColumnName("station_guid");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<OauthAccessToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("oauth_access_tokens_id_primary");

            entity.ToTable("oauth_access_tokens");

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime")
                .HasColumnName("expires_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Revoked).HasColumnName("revoked");
            entity.Property(e => e.Scopes).HasColumnName("scopes");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<OauthAuthCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("oauth_auth_codes_id_primary");

            entity.ToTable("oauth_auth_codes");

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime")
                .HasColumnName("expires_at");
            entity.Property(e => e.Revoked).HasColumnName("revoked");
            entity.Property(e => e.Scopes).HasColumnName("scopes");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<OauthClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__oauth_cl__3213E83F00C612AC");

            entity.ToTable("oauth_clients");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PasswordClient).HasColumnName("password_client");
            entity.Property(e => e.PersonalAccessClient).HasColumnName("personal_access_client");
            entity.Property(e => e.Provider)
                .HasMaxLength(255)
                .HasColumnName("provider");
            entity.Property(e => e.Redirect).HasColumnName("redirect");
            entity.Property(e => e.Revoked).HasColumnName("revoked");
            entity.Property(e => e.Secret)
                .HasMaxLength(100)
                .HasColumnName("secret");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<OauthPersonalAccessClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__oauth_pe__3213E83F5574F5C7");

            entity.ToTable("oauth_personal_access_clients");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<OauthRefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("oauth_refresh_tokens_id_primary");

            entity.ToTable("oauth_refresh_tokens");

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .HasColumnName("id");
            entity.Property(e => e.AccessTokenId)
                .HasMaxLength(100)
                .HasColumnName("access_token_id");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime")
                .HasColumnName("expires_at");
            entity.Property(e => e.Revoked).HasColumnName("revoked");
        });

        modelBuilder.Entity<OperationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__operatio__3213E83FB466E52B");

            entity.ToTable("operation_types");

            entity.HasIndex(e => new { e.LocalId, e.StationGuid }, "UQ_OPERATION_TYPES").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.LocalId)
                .IsRequired()
                .HasColumnName("local_id");
            entity.Property(e => e.StationGuid)
                .HasDefaultValue(new Guid("96700415-8913-406a-a131-464d1f5f05fb"))
                .HasColumnName("station_guid");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<PasswordReset>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("password_resets");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .HasColumnName("token");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => new { e.LocalId, e.StationGuid }).HasName("PK__permissi__2279C755886FBEBA");

            entity.ToTable("permissions");

            entity.HasIndex(e => new { e.LocalId, e.StationGuid }, "UQ_PERMISSIONS").IsUnique();

            entity.Property(e => e.LocalId).HasColumnName("local_id");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Group)
                .HasMaxLength(255)
                .HasColumnName("group");
            entity.Property(e => e.GuardName)
                .HasMaxLength(255)
                .HasColumnName("guard_name");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<PersonalAccessToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__personal__3213E83FA19C7AF1");

            entity.ToTable("personal_access_tokens");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Abilities).HasColumnName("abilities");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.LastUsedAt)
                .HasColumnType("datetime")
                .HasColumnName("last_used_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Token)
                .HasMaxLength(64)
                .HasColumnName("token");
            entity.Property(e => e.TokenableId).HasColumnName("tokenable_id");
            entity.Property(e => e.TokenableType)
                .HasMaxLength(255)
                .HasColumnName("tokenable_type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Pump>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("pumps_guid_primary");

            entity.ToTable("pumps");

            entity.Property(e => e.Guid)
                .ValueGeneratedNever()
                .HasColumnName("guid");
            entity.Property(e => e.CalibrationLimit).HasColumnName("calibration_limit");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LogicalAddress)
                .HasMaxLength(255)
                .HasColumnName("logical_address");
            entity.Property(e => e.NozzleStatusId).HasColumnName("nozzle_status_id");
            entity.Property(e => e.PhysicalAddress)
                .HasMaxLength(255)
                .HasColumnName("physical_address");
            entity.Property(e => e.PumpFunction)
                .HasMaxLength(255)
                .HasColumnName("pump_function");
            entity.Property(e => e.StationGuid)
                .HasDefaultValue(new Guid("96700415-8913-406a-a131-464d1f5f05fb"))
                .HasColumnName("station_guid");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TotalDispAmount).HasColumnName("total_disp_amount");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.NozzleStatus).WithMany(p => p.Pumps)
                .HasPrincipalKey(p => new { p.LocalId, p.StationGuid })
                .HasForeignKey(d => new { d.NozzleStatusId, d.StationGuid })
                .HasConstraintName("FK_PUMPS_NOZZLESTATUSES");

            entity.HasOne(d => d.PumpStatus).WithMany(p => p.Pumps)
                .HasPrincipalKey(p => new { p.LocalId, p.StationGuid })
                .HasForeignKey(d => new { d.StatusId, d.StationGuid })
                .HasConstraintName("FK_PUMPS_PUMPSTATUSES");
        });

        modelBuilder.Entity<PumpStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pump_sta__3213E83F0AF836EA");

            entity.ToTable("pump_statuses");

            entity.HasIndex(e => new { e.LocalId, e.StationGuid }, "UQ_PUMP_STATUSES").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.LocalId)
                .IsRequired()
                .HasColumnName("local_id");
            entity.Property(e => e.PumpStatus1)
                .HasMaxLength(255)
                .HasColumnName("pump_status");
            entity.Property(e => e.StationGuid)
                .HasDefaultValue(new Guid("96700415-8913-406a-a131-464d1f5f05fb"))
                .HasColumnName("station_guid");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<PumpTank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pump_tan__3213E83FB873F399");

            entity.ToTable("pump_tank");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PumpGuid).HasColumnName("pump_guid");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Pump).WithMany(p => p.PumpTanks)
                .HasForeignKey(d => d.PumpGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pump_tank_pump_guid_foreign");

            entity.HasOne(d => d.Tank).WithMany(p => p.PumpTanks)
                .HasForeignKey(d => d.TankGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pump_tank_tank_guid_foreign");
        });

        modelBuilder.Entity<RfidCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__rfid_car__3213E83F4B9D3AAF");

            entity.ToTable("rfid_cards");

            entity.HasIndex(e => new { e.LocalId, e.StationGuid }, "UQ_RFID_CARDS").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.LocalId).HasColumnName("local_id");
            entity.Property(e => e.RfidCode)
                .HasMaxLength(255)
                .HasColumnName("rfid_code");
            entity.Property(e => e.StationGuid)
                .HasDefaultValue(new Guid("96700415-8913-406a-a131-464d1f5f05fb"))
                .HasColumnName("station_guid");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.RfidCards)
                .HasForeignKey(d => new { d.UserId, d.StationGuid })
                .HasConstraintName("FK_RFIDCARDS_USERS");
        });

        modelBuilder.Entity<RfidDevice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__rfid_dev__3213E83F053FCE55");

            entity.ToTable("rfid_devices");

            entity.HasIndex(e => new { e.LocalId, e.StationGuid }, "UQ_RFID_DEVICES").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.LocalId).HasColumnName("local_id");
            entity.Property(e => e.PumpGuid).HasColumnName("pump_guid");
            entity.Property(e => e.RfidAddress)
                .HasMaxLength(255)
                .HasColumnName("rfid_address");
            entity.Property(e => e.RfidModel)
                .HasMaxLength(255)
                .HasColumnName("rfid_model");
            entity.Property(e => e.RfidProtocol)
                .HasMaxLength(255)
                .HasColumnName("rfid_protocol");
            entity.Property(e => e.StationGuid)
                .HasDefaultValue(new Guid("96700415-8913-406a-a131-464d1f5f05fb"))
                .HasColumnName("station_guid");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Pump).WithMany(p => p.RfidDevices)
                .HasForeignKey(d => d.PumpGuid)
                .HasConstraintName("rfid_devices_pump_guid_foreign");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83F16664CDC");

            entity.ToTable("roles");

            entity.HasIndex(e => new { e.LocalId, e.StationGuid }, "UQ_ROLES").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.GuardName)
                .HasMaxLength(255)
                .HasColumnName("guard_name");
            entity.Property(e => e.LocalId)
                .IsRequired()
                .HasColumnName("local_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.StationGuid)
                .HasDefaultValue(new Guid("96700415-8913-406a-a131-464d1f5f05fb"))
                .HasColumnName("station_guid");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<RoleHasPermission>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("role_has_permissions");

            entity.HasIndex(e => new { e.PermissionId, e.RoleId, e.StationGuid }, "UQ_ROLE_HAS_PERMISSIONS").IsUnique();

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");

            entity.HasOne(d => d.Permission).WithMany()
                .HasForeignKey(d => new { d.PermissionId, d.StationGuid })
                .HasConstraintName("FK_ROLEHASPERMISSIONS_PERMISSIONS");

            entity.HasOne(d => d.Role).WithMany()
                .HasPrincipalKey(p => new { p.LocalId, p.StationGuid })
                .HasForeignKey(d => new { d.RoleId, d.StationGuid })
                .HasConstraintName("FK_ROLEHASPERMISSIONS_ROLES");
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("stations_guid_primary");

            entity.ToTable("stations");

            entity.Property(e => e.Guid)
                .ValueGeneratedNever()
                .HasColumnName("guid");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PumpNumber).HasColumnName("pump_number");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.StationType)
                .HasMaxLength(255)
                .HasColumnName("station_type");
            entity.Property(e => e.TankNumber).HasColumnName("tank_number");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<StationTanksPumVw>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("station_tanks_pum_vw");

            entity.Property(e => e.CalibrationLimit).HasColumnName("calibration_limit");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.DLLimit).HasColumnName("d_l_limit");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Guid).HasColumnName("guid");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.HighHighLimit).HasColumnName("high_high_limit");
            entity.Property(e => e.HighLimit).HasColumnName("high_limit");
            entity.Property(e => e.Hysteresis).HasColumnName("hysteresis");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LogicalAddress).HasColumnName("logical_address");
            entity.Property(e => e.LowLimit).HasColumnName("low_limit");
            entity.Property(e => e.LowLowLimit).HasColumnName("low_low_limit");
            entity.Property(e => e.MLLimit).HasColumnName("m_l_limit");
            entity.Property(e => e.PLogicalAddress)
                .HasMaxLength(255)
                .HasColumnName("p_logical_address");
            entity.Property(e => e.PPhysicalAddress)
                .HasMaxLength(255)
                .HasColumnName("p_physical_address");
            entity.Property(e => e.PhysicalAddress).HasColumnName("physical_address");
            entity.Property(e => e.PumpFunction)
                .HasMaxLength(255)
                .HasColumnName("pump_function");
            entity.Property(e => e.PumpNumber).HasColumnName("pump_number");
            entity.Property(e => e.PumpStatus)
                .HasMaxLength(255)
                .HasColumnName("pump_status");
            entity.Property(e => e.PumpsGuid).HasColumnName("pumps_guid");
            entity.Property(e => e.PumpsId).HasColumnName("pumps_id");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TankName)
                .HasMaxLength(255)
                .HasColumnName("tank_name");
            entity.Property(e => e.TankNumber).HasColumnName("tank_number");
            entity.Property(e => e.TanksGuid).HasColumnName("tanks_guid");
            entity.Property(e => e.TanksId).HasColumnName("tanks_id");
            entity.Property(e => e.TotalDispAmount).HasColumnName("total_disp_amount");
            entity.Property(e => e.WLLimit).HasColumnName("w_l_limit");
            entity.Property(e => e.WaterHighLimit).HasColumnName("water_high_limit");
        });

        modelBuilder.Entity<SystemConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__system_c__3213E83F7E3DF8EF");

            entity.ToTable("system_configs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.LiterPrice).HasColumnName("liter_price");
            entity.Property(e => e.RefreshTime).HasColumnName("refresh_time");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Station).WithMany(p => p.SystemConfigs)
                .HasForeignKey(d => d.StationGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("system_configs_station_guid_foreign");
        });

        modelBuilder.Entity<TablesLastTranDate>(entity =>
        {
            entity.HasKey(e => new { e.ServerName, e.TableName }).HasName("PK__tables_l__CCDA730A1B0D57C8");

            entity.ToTable("tables_last_tran_dates");

            entity.Property(e => e.ServerName)
                .HasMaxLength(100)
                .HasColumnName("server_name");
            entity.Property(e => e.TableName)
                .HasMaxLength(100)
                .HasColumnName("table_name");
            entity.Property(e => e.LastTranDate)
                .HasColumnType("datetime")
                .HasColumnName("last_tran_date");
        });

        modelBuilder.Entity<Tank>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("tanks_guid_primary");

            entity.ToTable("tanks");

            entity.Property(e => e.Guid)
                .ValueGeneratedNever()
                .HasColumnName("guid");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DLLimit).HasColumnName("d_l_limit");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.HighHighLimit).HasColumnName("high_high_limit");
            entity.Property(e => e.HighLimit).HasColumnName("high_limit");
            entity.Property(e => e.Hysteresis).HasColumnName("hysteresis");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LogicalAddress).HasColumnName("logical_address");
            entity.Property(e => e.LowLimit).HasColumnName("low_limit");
            entity.Property(e => e.LowLowLimit).HasColumnName("low_low_limit");
            entity.Property(e => e.MLLimit).HasColumnName("m_l_limit");
            entity.Property(e => e.PhysicalAddress).HasColumnName("physical_address");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.TankName)
                .HasMaxLength(255)
                .HasColumnName("tank_name");
            entity.Property(e => e.TankStatusId).HasColumnName("tank_status_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.WLLimit).HasColumnName("w_l_limit");
            entity.Property(e => e.WaterHighLimit).HasColumnName("water_high_limit");

            entity.HasOne(d => d.Station).WithMany(p => p.Tanks)
                .HasForeignKey(d => d.StationGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tanks_station_guid_foreign");

            entity.HasOne(d => d.TankStatus).WithMany(p => p.Tanks)
                .HasPrincipalKey(p => new { p.LocalId, p.StationGuid })
                .HasForeignKey(d => new { d.TankStatusId, d.StationGuid })
                .HasConstraintName("FK_TANKS_TANKSTATUSES");
        });

        modelBuilder.Entity<TankMeasurement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tank_mea__3213E83F88220965");

            entity.ToTable("tank_measurements");

            entity.HasIndex(e => new { e.TankGuid, e.OldId }, "UQ_TANK_MEASUREMENTS").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.FuelLevel).HasColumnName("fuel_level");
            entity.Property(e => e.FuelVolume).HasColumnName("fuel_volume");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.Tcv).HasColumnName("tcv");
            entity.Property(e => e.Temperature).HasColumnName("temperature");
            entity.Property(e => e.Ullage).HasColumnName("ullage");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.WaterLevel).HasColumnName("water_level");
            entity.Property(e => e.WaterVolume).HasColumnName("water_volume");

            entity.HasOne(d => d.Tank).WithMany(p => p.TankMeasurements)
                .HasForeignKey(d => d.TankGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tank_measurements_tank_guid_foreign");
        });

        modelBuilder.Entity<TankMeasurementsEvery15Vw>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("tank_measurements_every_15_vw");

            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(4000)
                .HasColumnName("created_at");
            entity.Property(e => e.DLLimit).HasColumnName("d_l_limit");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FuelLevel).HasColumnName("fuel_level");
            entity.Property(e => e.FuelVolume).HasColumnName("fuel_volume");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.HighHighLimit).HasColumnName("high_high_limit");
            entity.Property(e => e.HighLimit).HasColumnName("high_limit");
            entity.Property(e => e.Hysteresis).HasColumnName("hysteresis");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LogicalAddress).HasColumnName("logical_address");
            entity.Property(e => e.LowLimit).HasColumnName("low_limit");
            entity.Property(e => e.LowLowLimit).HasColumnName("low_low_limit");
            entity.Property(e => e.MLLimit).HasColumnName("m_l_limit");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.PhysicalAddress).HasColumnName("physical_address");
            entity.Property(e => e.RowNumber).HasColumnName("row_number");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.TankName)
                .HasMaxLength(255)
                .HasColumnName("tank_name");
            entity.Property(e => e.Tcv).HasColumnName("tcv");
            entity.Property(e => e.Temperature).HasColumnName("temperature");
            entity.Property(e => e.Ullage).HasColumnName("ullage");
            entity.Property(e => e.UpdatedAt)
                .HasMaxLength(4000)
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedAtminus30)
                .HasColumnType("datetime")
                .HasColumnName("updated_atminus30");
            entity.Property(e => e.WLLimit).HasColumnName("w_l_limit");
            entity.Property(e => e.WaterHighLimit).HasColumnName("water_high_limit");
            entity.Property(e => e.WaterLevel).HasColumnName("water_level");
            entity.Property(e => e.WaterVolume).HasColumnName("water_volume");
        });

        modelBuilder.Entity<TankMeasurementsNew>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("tank_measurements_new");

            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(4000)
                .HasColumnName("created_at");
            entity.Property(e => e.DLLimit).HasColumnName("d_l_limit");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FuelLevel).HasColumnName("fuel_level");
            entity.Property(e => e.FuelVolume).HasColumnName("fuel_volume");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.HighHighLimit).HasColumnName("high_high_limit");
            entity.Property(e => e.HighLimit).HasColumnName("high_limit");
            entity.Property(e => e.Hysteresis).HasColumnName("hysteresis");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LogicalAddress).HasColumnName("logical_address");
            entity.Property(e => e.LowLimit).HasColumnName("low_limit");
            entity.Property(e => e.LowLowLimit).HasColumnName("low_low_limit");
            entity.Property(e => e.MLLimit).HasColumnName("m_l_limit");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.PhysicalAddress).HasColumnName("physical_address");
            entity.Property(e => e.RowNumber).HasColumnName("row_number");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.TankName)
                .HasMaxLength(255)
                .HasColumnName("tank_name");
            entity.Property(e => e.Tcv).HasColumnName("tcv");
            entity.Property(e => e.Temperature).HasColumnName("temperature");
            entity.Property(e => e.Ullage).HasColumnName("ullage");
            entity.Property(e => e.UpdatedAt)
                .HasMaxLength(4000)
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedAtminus30)
                .HasColumnType("datetime")
                .HasColumnName("updated_atminus30");
            entity.Property(e => e.WLLimit).HasColumnName("w_l_limit");
            entity.Property(e => e.WaterHighLimit).HasColumnName("water_high_limit");
            entity.Property(e => e.WaterLevel).HasColumnName("water_level");
            entity.Property(e => e.WaterVolume).HasColumnName("water_volume");
        });

        modelBuilder.Entity<TankMeasurementsTwiceADay>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("tank_measurements_twice_a_day");

            entity.Property(e => e.AvgConsumption).HasColumnName("avg_consumption");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(4000)
                .HasColumnName("created_at");
            entity.Property(e => e.DLLimit).HasColumnName("d_l_limit");
            entity.Property(e => e.DaysCoverd).HasColumnName("days_coverd");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FuelLevel).HasColumnName("fuel_level");
            entity.Property(e => e.FuelVolume).HasColumnName("fuel_volume");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.HighHighLimit).HasColumnName("high_high_limit");
            entity.Property(e => e.HighLimit).HasColumnName("high_limit");
            entity.Property(e => e.Hysteresis).HasColumnName("hysteresis");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LogicalAddress).HasColumnName("logical_address");
            entity.Property(e => e.LowLimit).HasColumnName("low_limit");
            entity.Property(e => e.LowLowLimit).HasColumnName("low_low_limit");
            entity.Property(e => e.MLLimit).HasColumnName("m_l_limit");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.PhysicalAddress).HasColumnName("physical_address");
            entity.Property(e => e.RowNumber).HasColumnName("row_number");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.TankName)
                .HasMaxLength(255)
                .HasColumnName("tank_name");
            entity.Property(e => e.TanksDate)
                .HasMaxLength(4000)
                .HasColumnName("tanks_date");
            entity.Property(e => e.Tcv).HasColumnName("tcv");
            entity.Property(e => e.Temperature).HasColumnName("temperature");
            entity.Property(e => e.Ullage).HasColumnName("ullage");
            entity.Property(e => e.UpdatedAt)
                .HasMaxLength(4000)
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedAtminus30)
                .HasColumnType("datetime")
                .HasColumnName("updated_atminus30");
            entity.Property(e => e.WLLimit).HasColumnName("w_l_limit");
            entity.Property(e => e.WaterHighLimit).HasColumnName("water_high_limit");
            entity.Property(e => e.WaterLevel).HasColumnName("water_level");
            entity.Property(e => e.WaterVolume).HasColumnName("water_volume");
        });

        modelBuilder.Entity<TankMeasurementsVw>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("tank_measurements_vw");

            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DLLimit).HasColumnName("d_l_limit");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FuelLevel).HasColumnName("fuel_level");
            entity.Property(e => e.FuelVolume).HasColumnName("fuel_volume");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.HighHighLimit).HasColumnName("high_high_limit");
            entity.Property(e => e.HighLimit).HasColumnName("high_limit");
            entity.Property(e => e.Hysteresis).HasColumnName("hysteresis");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LogicalAddress).HasColumnName("logical_address");
            entity.Property(e => e.LowLimit).HasColumnName("low_limit");
            entity.Property(e => e.LowLowLimit).HasColumnName("low_low_limit");
            entity.Property(e => e.MLLimit).HasColumnName("m_l_limit");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.PhysicalAddress).HasColumnName("physical_address");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.TankName)
                .HasMaxLength(255)
                .HasColumnName("tank_name");
            entity.Property(e => e.Tcv).HasColumnName("tcv");
            entity.Property(e => e.Temperature).HasColumnName("temperature");
            entity.Property(e => e.Ullage).HasColumnName("ullage");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.WLLimit).HasColumnName("w_l_limit");
            entity.Property(e => e.WaterHighLimit).HasColumnName("water_high_limit");
            entity.Property(e => e.WaterLevel).HasColumnName("water_level");
            entity.Property(e => e.WaterVolume).HasColumnName("water_volume");
        });

        modelBuilder.Entity<TankStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tank_sta__3213E83F30FD54C0");

            entity.ToTable("tank_statuses");

            entity.HasIndex(e => new { e.LocalId, e.StationGuid }, "UQ_TANKSTATUSES").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.LocalId).HasColumnName("local_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<TransactionDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__transact__3213E83FB533B955");

            entity.ToTable("transaction_details");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.FuelTransactionId).HasColumnName("fuel_transaction_id");
            entity.Property(e => e.FuelVolumeAfter).HasColumnName("fuel_volume_after");
            entity.Property(e => e.FuelVolumeBefore).HasColumnName("fuel_volume_before");
            entity.Property(e => e.OldId).HasColumnName("old_id");
            entity.Property(e => e.StationGuid)
                .HasDefaultValue(new Guid("96700415-8913-406a-a131-464d1f5f05fb"))
                .HasColumnName("station_guid");
            entity.Property(e => e.TankGuid).HasColumnName("tank_guid");
            entity.Property(e => e.TcvAfter).HasColumnName("tcv_after");
            entity.Property(e => e.TcvBefore).HasColumnName("tcv_before");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Tank).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.TankGuid)
                .HasConstraintName("transaction_details_tank_guid_foreign");
        });

        modelBuilder.Entity<TransactionStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__transact__3213E83F590D3128");

            entity.ToTable("transaction_statuses");

            entity.HasIndex(e => new { e.LocalId, e.StationGuid }, "UQ_TRANSACTION_STATUSES").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.LocalId)
                .IsRequired()
                .HasColumnName("local_id");
            entity.Property(e => e.StationGuid)
                .HasDefaultValue(new Guid("96700415-8913-406a-a131-464d1f5f05fb"))
                .HasColumnName("station_guid");
            entity.Property(e => e.TransStatus)
                .HasMaxLength(255)
                .HasColumnName("trans_status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<UpdateLogsView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("update_logs_View");

            entity.Property(e => e.After)
                .HasMaxLength(255)
                .HasColumnName("after");
            entity.Property(e => e.Before)
                .HasMaxLength(255)
                .HasColumnName("before");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.RowNumber).HasColumnName("row_number");
            entity.Property(e => e.Setting)
                .HasMaxLength(255)
                .HasColumnName("setting");
            entity.Property(e => e.StationGuid).HasColumnName("station_guid");
            entity.Property(e => e.StationName)
                .HasMaxLength(255)
                .HasColumnName("station_name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("user_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.StationGuid }).HasName("PK_USERS");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StationGuid)
                .HasDefaultValue(new Guid("96700415-8913-406a-a131-464d1f5f05fb"))
                .HasColumnName("station_guid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.EmailVerifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("email_verified_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RememberToken)
                .HasMaxLength(100)
                .HasColumnName("remember_token");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasPrincipalKey(p => new { p.LocalId, p.StationGuid })
                .HasForeignKey(d => new { d.RoleId, d.StationGuid })
                .HasConstraintName("FK_USERS_ROLES");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
