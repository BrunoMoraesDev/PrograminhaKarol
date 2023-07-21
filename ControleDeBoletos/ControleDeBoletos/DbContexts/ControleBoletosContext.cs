using ControleDeBoletos.Data.EntityConfigurations;
using ControleDeBoletos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeBoletos.Data.DbContexts
{
    public class ControleBoletosContext : DbContext
    {
        public ControleBoletosContext(DbContextOptions<ControleBoletosContext> options)
            : base(options) { }

        public DbSet<Boleto> Boleto { get; set; }
        public DbSet<TipoBoleto> TipoBoleto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BoletoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TipoBoletoEntityTypeConfiguration());
        }

        public override int SaveChanges()
        {
            AtribuirDataCadastroEDataAlteracao();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AtribuirDataCadastroEDataAlteracao();
            return await base.SaveChangesAsync();
        }

        private void AtribuirDataCadastroEDataAlteracao()
        {
            var entidades = ChangeTracker.Entries().Where(entidadeDeEntrada => entidadeDeEntrada.Entity is BaseEntity && (entidadeDeEntrada.State == EntityState.Added || entidadeDeEntrada.State == EntityState.Modified));

            foreach (var entidade in entidades)
            {
                if (entidade.State == EntityState.Added)
                {
                    ((BaseEntity)entidade.Entity).DataCadastro = DateTime.Now;
                }
                else
                {
                    ((BaseEntity)entidade.Entity).DataAlteracao = DateTime.Now;
                }
            }
        }
    }
}
