using AdPlatforms.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdPlatforms.Web.Controllers;

[ApiController]
[Route("api/adplatforms")]
public class AdPlatformsController : ControllerBase
{
    private readonly IAdPlatformService _adPlatformService;

    public AdPlatformsController(IAdPlatformService adPlatformService)
    {
        _adPlatformService = adPlatformService;
    }
    
    /// <summary>
    /// Загружает площадки и локации из файла
    /// </summary>
    /// <response code="200"> Успешный запрос. Данные загружены из файла. </response>
    /// <response code="500"> Ошибка загрузки. </response>
    [HttpPost("upload")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType( StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Upload(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл не передан или пуст.");

        using var reader = new StreamReader(file.OpenReadStream());
        var lines = new List<string>();
        
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (line != null)
                lines.Add(line);
        }

        _adPlatformService.LoadFromFile(lines);
        return Ok("Файл загружен.");
    }

    /// <summary>
    /// Ищет площадки по локации
    /// </summary>
    /// <response code="200"> Успешный запрос. Возвращает список площадок. </response>
    /// <response code="500"> Ошибка поиска. </response>
    [HttpGet("search")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType( StatusCodes.Status500InternalServerError)]
    public IActionResult Search([FromQuery] string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            return BadRequest("Укажите локацию.");

        var result = _adPlatformService.Search(location);
        
        return Ok(result);
    }
}