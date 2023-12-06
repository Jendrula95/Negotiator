// NegotiationsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Negotiator.Data;
using Negotiator.Models;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class NegotiationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public NegotiationsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Negotiation>> NegotiatePrice(Negotiation negotiation)
    {
        if (negotiation.ProposedPrice <= 0)
        {
            return BadRequest("Proposed price must be greater than 0.");
        }

        var product = await _context.Products.FindAsync(negotiation.ProductId);

        if (product == null)
        {
            return NotFound("Product not found.");
        }

        // Sprawdź, czy cena nie przekracza dwukrotnie ceny bazowej
        if (negotiation.ProposedPrice > 2 * product.Price)
        {
            negotiation.IsAutomaticallyRejected = true;
        }

        if (!negotiation.IsAutomaticallyRejected)
        {
            // Tutaj można dodać dodatkową logikę negocjacji, np. akceptacja ceny
            negotiation.AcceptedPrice = negotiation.ProposedPrice * 0.9M; // Przykładowa logika negocjacji (np. 10% zniżki)
        }

        negotiation.AttemptCount++;

        // Jeśli liczba prób przekroczy 3, odrzuć negocjację
        if (negotiation.AttemptCount > 3)
        {
            negotiation.IsAutomaticallyRejected = true;
        }

        _context.Negotiations.Add(negotiation);
        await _context.SaveChangesAsync();

        if (negotiation.IsAutomaticallyRejected)
        {
            return BadRequest("Negotiation rejected. Maximum attempts reached or proposed price too high.");
        }

        return Ok(new { negotiation.AcceptedPrice });
    }
}
