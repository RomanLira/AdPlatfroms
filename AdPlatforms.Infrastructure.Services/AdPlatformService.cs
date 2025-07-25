using AdPlatforms.DomainModel.Entities;
using AdPlatforms.Infrastructure.Interfaces;

namespace AdPlatforms.Infrastructure.Services;

public class AdPlatformService : IAdPlatformService
{
    private List<AdPlatform> _platforms = new();
    private ReaderWriterLockSlim _lock = new();
    
    public void LoadFromFile(IEnumerable<string> lines)
    {
        var newList = ParseLines(lines);

        _lock.EnterWriteLock();

        try
        {
            _platforms = newList;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
    
    public List<string> Search(string location)
    {
        _lock.EnterReadLock();

        try
        {
            return _platforms
                .Where(p => p.Locations.Any(loc => location.StartsWith(loc, StringComparison.OrdinalIgnoreCase)))
                .Select(p => p.Name)
                .Distinct()
                .ToList();
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }
    
    private List<AdPlatform> ParseLines(IEnumerable<string> lines)
    {
        var result = new List<AdPlatform>();
        
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || !line.Contains(':'))
                continue;
            
            var parts = line.Split(':', 2);
            var name = parts[0].Trim();
            var locations = parts[1].Split(',').Select(x => x.Trim()).Where(x => x != "").ToList();
            
            if (locations.Any())
            {
                result.Add(new AdPlatform
                {
                    Name = name,
                    Locations = locations
                });
            }
        }
        return result;
    }
}