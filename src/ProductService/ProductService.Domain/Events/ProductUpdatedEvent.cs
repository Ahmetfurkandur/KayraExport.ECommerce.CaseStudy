namespace ProductService.Domain.Events
{
    /// <summary>
    /// EventBus'ta kullanmak üzere oluşturulan event. Normalde bunlar building-blocks kütüphanelerinde ortak olarak tutulur fakat case study için bu kadarı yeterli. 
    /// </summary>
    public record ProductUpdatedEvent(Guid ProductId, string Name, decimal Price, DateTime OccurredOn);

}
