using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories;

public interface IAuctionRepository
{
    Task<Auction> CreateAsync(Auction auction);
    Task<Auction> SetIsActiveAsync(int auctionId);
    Task<Auction> GetByIdAsync(int id);
    Task<List<Auction>> GetAllActiveAsync();
    Task<List<Auction>> GetAllAsync();
    Task<List<Auction>> GetBySellerIdAsync(int sellerId);
    Task<bool> ExistsAsync(int id);
    Task<bool> DeleteAsync(int id);
}

