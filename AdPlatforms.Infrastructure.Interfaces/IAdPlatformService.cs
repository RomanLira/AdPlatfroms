namespace AdPlatforms.Infrastructure.Interfaces;

public interface IAdPlatformService
{
    /// <summary>
    /// Загрузка площадок и локаций из файла
    /// </summary>
    void LoadFromFile(IEnumerable<string> lines);

    /// <summary>
    /// Поиск площадок по локации
    /// </summary>
    List<string> Search(string location);
}