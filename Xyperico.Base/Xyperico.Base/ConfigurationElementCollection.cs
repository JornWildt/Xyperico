using System.Configuration;

namespace Xyperico.Base
{
  // (C) http://www.sidesofmarch.com/index.php/archive/2007/07/27/simplify-configuration-with-a-generic-configurationelementcollection-class/

  // The ConfigurationElementCollection<t> provides a simple generic implementation of ConfigurationElementCollection.
  // Note one key part of this implementation: 
  //   You must override the ToString() method of your custom ConfigurationElement class 
  //   to return a unique value. The generic ConfigurationElementCollection uses the 
  //   ToString() method to obtain a unique key for each element in the collection.

  [ConfigurationCollection(typeof(ConfigurationElement))]
  public class ConfigurationElementCollection<T> : ConfigurationElementCollection where T : ConfigurationElement, new()
  {
    protected override ConfigurationElement CreateNewElement()
    {
      return new T();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
      return ((T)(element)).ToString();
    }

    public T this[int idx]
    {
      get { return (T)BaseGet(idx); }
    }

    public new T this[string idx]
    {
      get { return (T)BaseGet(idx); }
    }
  }


  /* EXAMPLE
   * 
  // The Filespec class is an example of a custom configuration element.
  // Note that we inherit from ConfigurationElement, and use ConfigurationProperty attributes.
  public class Filespec : ConfigurationElement
  {
	  public Filespec()
	  {
	  }
  	
    [ConfigurationProperty("path", DefaultValue="", IsKey=true, IsRequired=true)]
	  public string Path
	  {
		  get { return (string)(base["path"]); }
		  set { base["path"] = value; }
	  }
  	
    [ConfigurationProperty("type", IsKey=false, IsRequired = true)]
	  public FilespecType Type
	  {
		  get { return (FilespecType)(base["type"]); }
		  set { base["type"] = value; }
	  }

	  public override string ToString()
	  {
		  return this.Path;
	  }
  }

  // Finally, our ConfigurationSettings class (only part of the class is included).
  // Note how we use the generic ConfigurationElementCollection.
  public class ConfigurationSettings : ConfigurationSection
  {
  // ...
	  [ConfigurationProperty("filespecs", IsRequired=true)]
	  public ConfigurationElementCollection<filespec> FileSpecs
	  {
		  get { return (ConfigurationElementCollection<filespec>)this["filespecs"]; }
	  }
  // ...
  }

  */
}
