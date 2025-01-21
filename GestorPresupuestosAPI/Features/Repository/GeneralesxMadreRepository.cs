using GestorPresupuestosAPI.Infraestructure.DataBases;
using Microsoft.EntityFrameworkCore;

namespace GestorPresupuestosAPI.Features.Repository
{
    public class GeneralesxMadreRepository
    {
        private readonly GestorPresupuestosAHM _context;

        public GeneralesxMadreRepository(GestorPresupuestosAHM context)
        {
            _context = context;
        }

        public async Task<List<GeneralesxMadre>> GetAllAsync()
        {
            return await _context.GeneralesxMadre.ToListAsync();
        }

        public async Task<GeneralesxMadre> GetByIdAsync(int id)
        {
            return await _context.GeneralesxMadre.FindAsync(id);
        }

        public async Task<GeneralesxMadre> AddAsync(GeneralesxMadre entity)
        {
            _context.GeneralesxMadre.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<GeneralesxMadre> UpdateAsync(GeneralesxMadre entity)
        {
            _context.GeneralesxMadre.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.GeneralesxMadre.FindAsync(id);
            if (entity != null)
            {
                _context.GeneralesxMadre.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

    }
}
