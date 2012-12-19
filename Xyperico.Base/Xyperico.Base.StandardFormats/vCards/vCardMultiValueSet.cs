using System.Collections.Generic;
using System.Linq;


namespace Xyperico.Base.StandardFormats.vCards
{
  public class vCardMultiValueSet<T>
    where T : vCardMultiValue<T>
  {
    public List<T> Items { get; private set; }


    public T Default
    {
      get
      {
        if (Items.Count > 0)
        {
          T defaultItem = Items[0];
          foreach (T item in Items)
          {
            if (item.Pref < defaultItem.Pref)
              defaultItem = item;
          }
          return defaultItem;
        }
        else
        {
          return null;
        }
      }
    }


    public T this[string type]
    {
      get
      {
        return Items.Where(i => i.Type == type).FirstOrDefault();
      }
    }


    public vCardMultiValueSet()
    {
      Items = new List<T>();
    }
  }
}
