using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pelmenara_AUI_RUI;

public partial class PelmenaraContext : DbContext
{
    public PelmenaraContext()
    {
    }

    public PelmenaraContext(DbContextOptions<PelmenaraContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql($"Host=localhost;Port=5432;Database=Pelmenara;Username=postgres;Password=1234").UseLazyLoadingProxies();
    //=> optionsBuilder.UseNpgsql($"Host=192.168.12.211;Port=5432;Database=user47;Username=user47;Password=HP2Y8V;SearchPath=pelmenara").UseLazyLoadingProxies();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FavoriteRecipe>(entity =>
        {
            entity.HasKey(e => e.FavoriteRecipeId).HasName("FavoriteRecipe_pkey");

            entity.ToTable("FavoriteRecipe");

            entity.Property(e => e.FavoriteRecipeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("FavoriteRecipeID");
            entity.Property(e => e.RecipeId).HasColumnName("RecipeID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Recipe).WithMany(p => p.FavoriteRecipes)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FavoriteRecipe_RecipeID_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.FavoriteRecipes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FavoriteRecipe_UserID_fkey");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("Recipe_pkey");

            entity.ToTable("Recipe");

            entity.Property(e => e.RecipeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("RecipeID");
            entity.Property(e => e.CookingTime).HasColumnType("character varying");
            entity.Property(e => e.CreationDate).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.Description).HasColumnType("character varying");
            entity.Property(e => e.Ingredients).HasColumnType("character varying");
            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Ingredients).HasMaxLength(200);
            entity.Property(e => e.CookingTime).HasMaxLength(15);

            entity.HasOne(d => d.Owner).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Recipe_OwnerID_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("User_pkey");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "User_Email_key").IsUnique();

            entity.HasIndex(e => e.Login, "User_Login_key").IsUnique();

            entity.Property(e => e.UserId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("UserID");
            entity.Property(e => e.Email).HasColumnType("character varying");
            entity.Property(e => e.Login).HasMaxLength(25);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
