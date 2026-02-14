using Microsoft.AspNetCore.Mvc;
using NouvoStudio.Services;
using NouvoStudio.Models;

namespace NouvoStudio.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtworksApiController : ControllerBase
    {
        private readonly IArtworkService _artworkService;

        public ArtworksApiController(IArtworkService artworkService)
        {
            _artworkService = artworkService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artwork>>> GetArtworks([FromQuery] string? search, [FromQuery] string? size, [FromQuery] string? medium)
        {
            IEnumerable<Artwork> artworks;

            if (!string.IsNullOrEmpty(search) || !string.IsNullOrEmpty(size) || !string.IsNullOrEmpty(medium))
            {
                artworks = await _artworkService.SearchAsync(search ?? "", size, medium);
            }
            else
            {
                artworks = await _artworkService.GetAllAsync();
            }

            return Ok(artworks);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<IEnumerable<Artwork>>> GetFeaturedArtworks()
        {
            var artworks = await _artworkService.GetFeaturedAsync();
            return Ok(artworks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Artwork>> GetArtwork(int id)
        {
            var artwork = await _artworkService.GetByIdAsync(id);
            if (artwork == null)
            {
                return NotFound();
            }
            return Ok(artwork);
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<Artwork>> GetArtworkByCode(string code)
        {
            var artwork = await _artworkService.GetByCodeAsync(code);
            if (artwork == null)
            {
                return NotFound();
            }
            return Ok(artwork);
        }

        [HttpPost]
        public async Task<ActionResult<Artwork>> CreateArtwork(Artwork artwork)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdArtwork = await _artworkService.CreateAsync(artwork);
            return CreatedAtAction(nameof(GetArtwork), new { id = createdArtwork.Id }, createdArtwork);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArtwork(int id, Artwork artwork)
        {
            if (id != artwork.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exists = await _artworkService.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            await _artworkService.UpdateAsync(artwork);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtwork(int id)
        {
            var exists = await _artworkService.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            await _artworkService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<Artwork>>> SearchArtworks([FromBody] SearchRequest request)
        {
            var artworks = await _artworkService.SearchAsync(request.Query, request.Size, request.Medium);
            return Ok(artworks);
        }
    }

    public class SearchRequest
    {
        public string Query { get; set; } = string.Empty;
        public string? Size { get; set; }
        public string? Medium { get; set; }
    }
}
