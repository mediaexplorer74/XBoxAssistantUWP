// *********************************************************
// Type: LRC.ViewModel.DelegateCommand
// Assembly: LRC.ViewModel, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17E0F5E4-B7C1-404D-8514-ABB31C1FE054
// *********************************************************LRC.ViewModel.dll

using System;
using System.Windows.Input;


namespace LRC.ViewModel
{
  public class DelegateCommand : ICommand
  {
    private Func<object, bool> canExecuteFunc;
    private Action<object> executeAction;
    private bool previousCanExecute;

    public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecute)
    {
      this.executeAction = executeAction;
      this.canExecuteFunc = canExecute;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      if (this.canExecuteFunc == null)
        return true;
      bool flag = this.canExecuteFunc(parameter);
      if (this.previousCanExecute != flag)
      {
        this.previousCanExecute = flag;
        EventHandler canExecuteChanged = this.CanExecuteChanged;
        if (canExecuteChanged != null)
          canExecuteChanged((object) this, EventArgs.Empty);
      }
      return this.previousCanExecute;
    }

    public void Execute(object parameter) => this.executeAction(parameter);
  }
}
