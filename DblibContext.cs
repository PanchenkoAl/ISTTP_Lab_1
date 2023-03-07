using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LIBWebApplication1;

public partial class DblibContext : DbContext
{
    public DblibContext()
    {
    }

    public DblibContext(DbContextOptions<DblibContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GameRecord> GameRecords { get; set; }

    public virtual DbSet<Lobby> Lobbies { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamsPlayer> TeamsPlayers { get; set; }

    public virtual DbSet<Tournament> Tournaments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= DESKTOP-6TT02DE; Database=DBLib; Trusted_Connection=True; TrustServerCertificate=True ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameRecord>(entity =>
        {
            entity.Property(e => e.GameRecordId).ValueGeneratedNever();
            entity.Property(e => e.GameDate).HasColumnType("date");
            entity.Property(e => e.Info).HasColumnType("text");

            entity.HasOne(d => d.Team1).WithMany(p => p.GameRecordTeam1s)
                .HasForeignKey(d => d.Team1Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GameRecords_Teams");

            entity.HasOne(d => d.Team2).WithMany(p => p.GameRecordTeam2s)
                .HasForeignKey(d => d.Team2Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GameRecords_Teams1");

            entity.HasOne(d => d.Tournament).WithMany(p => p.GameRecords)
                .HasForeignKey(d => d.TournamentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GameRecords_Tournaments");
        });

        modelBuilder.Entity<Lobby>(entity =>
        {
            entity.Property(e => e.LobbyId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.Property(e => e.Info).HasColumnType("ntext");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nickname)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.Property(e => e.StaffId).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nickname)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Staff)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Staff_Roles");

            entity.HasOne(d => d.Team).WithMany(p => p.Staff)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Staff_Teams");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.Property(e => e.TeamId).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TeamsPlayer>(entity =>
        {
            entity.HasKey(e => e.TeamsPlayersId);

            entity.Property(e => e.TeamsPlayersId).ValueGeneratedNever();
            entity.Property(e => e.JoinDate).HasColumnType("date");
            entity.Property(e => e.LeftDate).HasColumnType("date");

            entity.HasOne(d => d.Player).WithMany(p => p.TeamsPlayers)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeamsPlayers_Players");

            entity.HasOne(d => d.Role).WithMany(p => p.TeamsPlayers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeamsPlayers_Roles");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamsPlayers)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeamsPlayers_Teams");
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.Property(e => e.TournamentId).ValueGeneratedNever();
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("date");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
