using DAL.SqlServer.Context;
using Domain.Entities;
using Repository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.SqlServer.Infrastructure
{
    public class SqlAuctionRepository(AppDbContext context) : IAuctionRepository
    {
        private readonly AppDbContext _context = context;


        public async Task<Auction> CreateAsync(Auction auction)
        {
            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();
            return auction; 
        }

        public async Task<Auction> SetIsActiveAsync(int auctionId)
        {
            var auction = await _context.Auctions
                .Include(c => c.Seller)
                .Include(c => c.Car)
                .ThenInclude(c => c.CarImagePaths)
                .FirstOrDefaultAsync(a => a.Id == auctionId);

            auction.IsActive = true;
            _context.Auctions.Update(auction);
            await _context.SaveChangesAsync();

            return auction;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null) return false;

            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync();
            return true; 
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Auctions.AnyAsync(a => a.Id == id);
        }

        public async Task<List<Auction>> GetAllActiveAsync()
        {
            return await _context.Auctions
                                 .Where(a => a.IsActive)
                                 .Include(a => a.Car)
                                 .ThenInclude(a => a.CarImagePaths)
                                 .ToListAsync();
        }

        public async Task<List<Auction>> GetAllAsync()
        {
            return await _context.Auctions
                                 .Include(a => a.Car)
                                 .ThenInclude(a => a.CarImagePaths)
                                 .ToListAsync();
        }

        public async Task<Auction> GetByIdAsync(int id)
        {
            return await _context.Auctions
                                 .Include(a => a.Car) 
                                 .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Auction>> GetBySellerIdAsync(int sellerId)
        {
            return await _context.Auctions
                                 .Where(a => a.SellerId == sellerId)
                                 .Include(a => a.Car)
                                 .ToListAsync();
        }

    }
}
