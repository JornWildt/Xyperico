namespace Xyperico.Base.DomainEvents
{
  public interface IDomainEventHandler<T> where T : IDomainEvent
  {
    void Handle(T args);
  } 
}
