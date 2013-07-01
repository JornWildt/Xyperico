namespace Xyperico.Agres
{
  public class EventStoreItem
  {
    public long Id { get; private set; }

    public string Key { get; private set; }

    public IEvent Event { get; private set; }


    public EventStoreItem(long id, string key, IEvent e)
    {
      Id = id;
      Key = key;
      Event = e;
    }
  }
}
