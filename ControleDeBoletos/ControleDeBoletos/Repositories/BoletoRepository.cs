﻿using ControleDeBoletos.Data.DbContexts;
using ControleDeBoletos.Enums;
using ControleDeBoletos.Models;
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

        public IEnumerable<Boleto> GetAll()
        {
            return _context.Boleto.AsNoTracking();
        }

        public Boleto? GetById(int id)
        {
            return _context.Boleto
                .AsNoTracking()
                .Include(b => b.Tipo)
                .FirstOrDefault(boleto => boleto.Id == id);
        }

        public Boleto Add(Boleto entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            _context.SaveChanges();
            return entity;
        }

        public IEnumerable<Boleto> Add(IEnumerable<Boleto> entities)
        {
            foreach (var boleto in entities)
            {
                _context.Entry(boleto).State = EntityState.Added;
            }
            _context.SaveChanges();
            return entities;
        }

        public Boleto Update(Boleto entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return entity;
        }

        public void Delete(Boleto entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public IEnumerable<Boleto> Delete(IEnumerable<Boleto> entities)
        {
            foreach (var boleto in entities)
            {
                _context.Entry(boleto).State = EntityState.Deleted;
            }
            _context.SaveChanges();
            return entities;
        }

        public IQueryable<Boleto> GetFilteredBoletos(string? descricao, TiposSituacao situacao, int? dia, int? mes, int? ano)
        {
            var query = _context.Boleto
                .Include(boleto => boleto.Tipo)
                .AsQueryable();

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
                    query = query.Where(boleto => boleto.Situacao == false && boleto.Vencimento.Date < DateTime.Now.Date);
                    break;

                default:
                    break;
            }

            if (dia.HasValue)
            {
                query = query.Where(boleto => boleto.Vencimento.Day == dia);
            }

            if (mes.HasValue)
            {
                query = query.Where(boleto => boleto.Vencimento.Month == mes);
            }

            if (ano.HasValue)
            {
                query = query.Where(boleto => boleto.Vencimento.Year == ano);
            }

            if (!string.IsNullOrEmpty(descricao))
            {
                query = query.Where(boleto => boleto.Descricao.ToLower().Contains(descricao.ToLower()));
            }

            return query;
        }
    }
}
