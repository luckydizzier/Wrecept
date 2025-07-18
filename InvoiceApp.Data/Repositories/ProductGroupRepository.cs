using Microsoft.EntityFrameworkCore;
using InvoiceApp.Core.Models;
using InvoiceApp.Core.Repositories;
using InvoiceApp.Data.Data;
using System.Text.Json;

namespace InvoiceApp.Data.Repositories;

public class ProductGroupRepository : IProductGroupRepository
{
    private readonly AppDbContext _db;

    public ProductGroupRepository(AppDbContext db)
    {
        _db = db;
    }

    private void Log(string id, string op, object data)
    {
        _db.ChangeLogs.Add(new ChangeLog
        {
            Entity = nameof(ProductGroup),
            EntityId = id,
            Operation = op,
            Data = JsonSerializer.Serialize(data),
            CreatedAt = DateTime.UtcNow
        });
    }

    public Task<List<ProductGroup>> GetAllAsync(CancellationToken ct = default)
        => _db.Set<ProductGroup>().AsNoTracking().ToListAsync(ct);

    public Task<List<ProductGroup>> GetActiveAsync(CancellationToken ct = default)
        => _db.Set<ProductGroup>().AsNoTracking().Where(g => !g.IsArchived).ToListAsync(ct);

    public async Task<Guid> AddAsync(ProductGroup group, CancellationToken ct = default)
    {
        _db.Add(group);
        await _db.SaveChangesAsync(ct);
        Log(group.Id.ToString(), "Insert", group);
        await _db.SaveChangesAsync(ct);
        return group.Id;
    }

    public async Task UpdateAsync(ProductGroup group, CancellationToken ct = default)
    {
        _db.Update(group);
        await _db.SaveChangesAsync(ct);
        Log(group.Id.ToString(), "Update", group);
        await _db.SaveChangesAsync(ct);
    }
}
