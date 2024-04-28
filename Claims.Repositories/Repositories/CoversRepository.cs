﻿using Claims.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Claims.Repositories.Repositories
{
    public class CoversRepository : DbContext, ICoversRepository
	{
        public DbSet<CoverEntity> Covers { get; init; }

        public CoversRepository(DbContextOptions options)
            : base(options)
		{
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CoverEntity>().ToCollection("covers");
        }

        public async Task CreateCoverAsync(CoverEntity cover)
        {
            Covers.Add(cover);
            await SaveChangesAsync();
        }

        public async Task DeleteCoverAsync(string id)
        {
            var cover = await GetCoverAsync(id);
            if (cover is not null)
            {
                Covers.Remove(cover);
                await SaveChangesAsync();
            }
        }

        public async Task<CoverEntity> GetCoverAsync(string id)
        {
            return await Covers
                .Where(cover => cover.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CoverEntity>> GetCoversAsync()
        {
            return await Covers.ToListAsync();
        }
    }
}

