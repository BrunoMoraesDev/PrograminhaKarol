using ControleDeBoletos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleDeBoletos.Data.EntityConfigurations
{
    public class TipoBoletoEntityTypeConfiguration : IEntityTypeConfiguration<TipoBoleto>
    {
        public void Configure(EntityTypeBuilder<TipoBoleto> builder)
        {
            builder.HasKey(c => c.Id)
                  .HasName("tipo_boleto_pkey");

            builder.ToTable("tipo_boleto");

            builder.Property(c => c.Id).HasColumnName("id");

            builder.Property(c => c.Descricao).HasColumnName("descricao");

            builder.Property(c => c.DataCadastro)
                .HasColumnType("datetime")
                .HasColumnName("data_cadastro");

            builder.Property(c => c.DataAlteracao)
                .HasColumnType("datetime")
                .HasColumnName("data_alteracao");
        }
    }
}
