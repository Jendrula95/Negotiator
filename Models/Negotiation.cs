
public class Negotiation
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal ProposedPrice { get; set; }
    public decimal AcceptedPrice { get; set; }
    public int AttemptCount { get; set; }  // Liczba prób negocjacji
    public bool IsAutomaticallyRejected { get; set; }  // Flaga odrzucenia automatycznego

}
