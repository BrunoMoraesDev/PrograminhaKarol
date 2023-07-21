using ControleDeBoletos.Data.DbContexts;
using ControleDeBoletos.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ControleDeBoletos.Data.Repositories
{
    public class TipoBoletoRepository
    {
        private readonly ControleBoletosContext _context;

        public TipoBoletoRepository(ControleBoletosContext context)
        {
            _context = context;
        }

        public IEnumerable<TipoBoleto> GetAll()
        {
            return _context.TipoBoleto.AsNoTracking();
        }

        public TipoBoleto? GetById(int id)
        {
            return _context.TipoBoleto
                .AsNoTracking()
                .FirstOrDefault(tipo => tipo.Id == id);
        }

        public TipoBoleto Add(TipoBoleto entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            _context.SaveChanges();
            return entity;
        }

        public TipoBoleto Update(TipoBoleto entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return entity;
        }

        public void Delete(TipoBoleto entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            _context.SaveChanges();
        }
    }
}
