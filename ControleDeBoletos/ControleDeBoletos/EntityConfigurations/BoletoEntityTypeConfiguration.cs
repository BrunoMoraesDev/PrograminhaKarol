using ControleDeBoletos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleDeBoletos.Data.EntityConfigurations
{
    public class BoletoEntityTypeConfiguration : IEntityTypeConfiguration<Boleto>
    {
        public void Configure(EntityTypeBuilder<Boleto> builder)
        {
            builder.HasKey(c => c.Id)
                  .HasName("boleto_pkey");

            builder.ToTable("boleto");

            builder.Property(c => c.Id).HasColumnName("id");

            builder.Property(c => c.TipoId).HasColumnName("tipo_id");

            builder.Property(c => c.Descricao)
                .HasColumnName("descricao");

            builder.Property(c => c.Valor).HasColumnName("valor");

            builder.Property(c => c.Emissao)
                .HasColumnType("datetime")
                .HasColumnName("data_emissao");

            builder.Property(c => c.Vencimento)
                .HasColumnType("datetime")
                .HasColumnName("data_vencimento");

            builder.Property(c => c.Situacao).HasColumnName("situacao");

            builder.Property(c => c.DataCadastro)
                .HasColumnType("datetime")
                .HasColumnName("data_cadastro");

            builder.Property(c => c.DataAlteracao)
                .HasColumnType("datetime")
                .HasColumnName("data_alteracao");

            builder.HasOne(c => c.Tipo)
                .WithMany()
                .HasForeignKey(c => c.TipoId)
                .HasConstraintName("FK_idTipoBoleto_Boleto");
        }
    }
}
