namespace AdPlatforms.DomainModel.Entities;

public class AdPlatform
{
    /// <summary>
    /// Наименование рекламной площадки
    /// </summary>
    public string Name { get; set; }
    
    
    /// <summary>
    /// Локации
    /// </summary>
    public List<string> Locations { get; set; } = new();
}