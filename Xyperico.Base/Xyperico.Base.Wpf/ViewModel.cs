﻿using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite;


namespace Xyperico.Base.Wpf
{
  public class ViewModel : INotifyPropertyChanged
  {
    public ViewModel(ViewModel parent)
    {
      Parent = parent;
      if (Parent != null)
        Parent.RegisterChildViewModel(this);
    }


    #region Parent/child managing

    private ViewModel _parent;
    public ViewModel Parent
    {
      get { return _parent; }
      set
      {
        if (value != _parent)
        {
          _parent = value;
          OnPropertyChanged("Parent");
        }
      }
    }


    private IList<ViewModel> ChildViewModels = new List<ViewModel>();


    protected void RegisterChildViewModel(ViewModel viewModel)
    {
      ChildViewModels.Add(viewModel);
    }


    protected void ClearChildViewModels()
    {
      ChildViewModels.Clear();
    }

    #endregion


    #region Command handling

    protected List<IActiveAware> Commands = new List<IActiveAware>();

    public void RegisterCommand<T>(DelegateCommand<T> command)
    {
      Commands.Add(command);
    }

    #endregion


    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;


    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null)
        handler(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
  }
}
