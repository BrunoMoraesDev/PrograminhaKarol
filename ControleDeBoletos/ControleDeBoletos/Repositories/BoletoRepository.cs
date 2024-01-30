using ControleDeBoletos.Data.DbContexts;
using ControleDeBoletos.Enums;
using ControleDeBoletos.Models;
using ControleDeBoletos.ViewModels;
using ControleDeBoletos.ViewModels.TotalPorPeriodoViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

namespace ControleDeBoletos.Data.Repositories
{
    public class BoletoRepository
    {
        private readonly ControleBoletosContext _context;

        public BoletoRepository(ControleBoletosContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Boleto> BuscarTodos()
        {
            return _context.Boleto.AsNoTracking();
        }

        public Boleto? BuscarPorId(int id)
        {
            return _context.Boleto
                .AsNoTracking()
                .Include(b => b.Tipo)
                .FirstOrDefault(boleto => boleto.Id == id);
        }

        public Boleto Incluir(Boleto entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            _context.SaveChanges();
            return entity;
        }

        public IEnumerable<Boleto> Incluir(IEnumerable<Boleto> entities)
        {
            foreach (var boleto in entities)
            {
                _context.Entry(boleto).State = EntityState.Added;
            }
            _context.SaveChanges();
            return entities;
        }

        public Boleto Alterar(Boleto entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return entity;
        }

        public void Excluir(Boleto entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public IEnumerable<Boleto> Excluir(IEnumerable<Boleto> entities)
        {
            foreach (var boleto in entities)
            {
                _context.Entry(boleto).State = EntityState.Deleted;
            }
            _context.SaveChanges();
            return entities;
        }

        public IQueryable<Boleto> BuscarBoletosFiltrados(string? descricao, TipoBoleto? tipo, TiposSituacao situacao, int? diaVencimento, int? mesVencimento, int? anoVencimento, int? diaEmissao, int? mesEmissao, int? anoEmissao)
        {
            var query = _context.Boleto
                .Include(boleto => boleto.Tipo)
                .AsQueryable();

            if (tipo != null)
            {
                query = query.Where(boleto => boleto.TipoId == tipo.Id);
            }

            switch (situacao)
            {
                case TiposSituacao.TODOS:
                    break;

                case TiposSituacao.PAGOS:
                    query = query.Where(boleto => boleto.Situacao == true);
                    break;

                case TiposSituacao.PENDENTES:
                    query = query.Where(boleto => boleto.Situacao == false);
                    break;

                case TiposSituacao.VENCIDOS:
                    query = query.Where(boleto => boleto.Situacao == false && boleto.Vencimento.Date <= DateTime.Now.Date);
                    break;

                default:
                    break;
            }

            if (diaVencimento.HasValue)
            {
                query = query.Where(boleto => boleto.Vencimento.Day == diaVencimento);
            }

            if (mesVencimento.HasValue)
            {
                query = query.Where(boleto => boleto.Vencimento.Month == mesVencimento);
            }

            if (anoVencimento.HasValue)
            {
                query = query.Where(boleto => boleto.Vencimento.Year == anoVencimento);
            }

            if (diaEmissao.HasValue)
            {
                query = query.Where(boleto => boleto.Emissao.Day == diaEmissao);
            }

            if (mesEmissao.HasValue)
            {
                query = query.Where(boleto => boleto.Emissao.Month == mesEmissao);
            }

            if (anoEmissao.HasValue)
            {
                query = query.Where(boleto => boleto.Emissao.Year == anoEmissao);
            }

            if (!string.IsNullOrEmpty(descricao))
            {
                query = query.Where(boleto => boleto.Descricao.ToLower().Contains(descricao.ToLower()));
            }

            return query;
        }

        public IEnumerable<BoletosTotaisPorDiaTotalPorPeriodoViewModel> BuscarBoletosTotaisPorPeriodo(IEnumerable<TipoBoletoTotalPorPeriodoViewModel> tiposSelecionados, DateTime? dataInicial, DateTime? dataFinal)
        {
            var query = _context.Boleto
                .Include(boleto => boleto.Tipo)
                .Where(boleto => boleto.Situacao == false)
                .AsQueryable();

            if (tiposSelecionados.Any())
            {
                query = query.Where(boleto => tiposSelecionados.Select(tipo => tipo.Id).Contains(boleto.TipoId));
            }

            if (dataInicial.HasValue && dataFinal.HasValue)
            {
                query = query.Where(boleto => boleto.Vencimento >= dataInicial && boleto.Vencimento <= dataFinal);
            }
            else if (dataInicial.HasValue)
            {
                query = query.Where(boleto => boleto.Vencimento >= dataInicial);
            }
            else if (dataFinal.HasValue)
            {
                query = query.Where(boleto => boleto.Vencimento <= dataFinal);
            }

            IEnumerable<Boleto> boletosFiltrados = query.ToList();

            IEnumerable<BoletosTotaisPorDiaTotalPorPeriodoViewModel> boletosAgrupados = boletosFiltrados
            .GroupBy(boleto => boleto.Vencimento)
            .Select(grupo => new BoletosTotaisPorDiaTotalPorPeriodoViewModel
            {
                DataVencimento = grupo.Key,
                ValorTotal = grupo.Sum(boleto => boleto.Valor)
            });

            return boletosAgrupados.OrderBy(boleto => boleto.DataVencimento);
        }
    }
}
