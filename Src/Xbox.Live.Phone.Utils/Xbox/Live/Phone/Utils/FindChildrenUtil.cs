// *********************************************************
// Type: Xbox.Live.Phone.Utils.FindChildrenUtil
// Assembly: Xbox.Live.Phone.Utils, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50120E1B-39E8-4952-8A70-ED03AE032ACB
// *********************************************************Xbox.Live.Phone.Utils.dll

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;


namespace Xbox.Live.Phone.Utils
{
  public static class FindChildrenUtil
  {
    public static T FindFirstChildOfType<T>(DependencyObject root) where T : class
    {
      Queue<DependencyObject> dependencyObjectQueue = new Queue<DependencyObject>();
      dependencyObjectQueue.Enqueue(root);
      while (0 < dependencyObjectQueue.Count)
      {
        DependencyObject dependencyObject = dependencyObjectQueue.Dequeue();
        if (dependencyObject != null)
        {
          for (int index = VisualTreeHelper.GetChildrenCount(dependencyObject) - 1; 0 <= index; --index)
          {
            DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, index);
            if (child is T firstChildOfType)
              return firstChildOfType;
            dependencyObjectQueue.Enqueue(child);
          }
        }
      }
      return default (T);
    }

    public static List<T> FindChildrenOfType<T>(DependencyObject root) where T : class
    {
      List<T> childrenOfType = (List<T>) null;
      Queue<DependencyObject> dependencyObjectQueue = new Queue<DependencyObject>();
      dependencyObjectQueue.Enqueue(root);
      while (0 < dependencyObjectQueue.Count)
      {
        DependencyObject dependencyObject = dependencyObjectQueue.Dequeue();
        if (dependencyObject != null)
        {
          for (int index = VisualTreeHelper.GetChildrenCount(dependencyObject) - 1; 0 <= index; --index)
          {
            DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, index);
            if (child is T obj)
            {
              if (childrenOfType == null)
                childrenOfType = new List<T>();
              childrenOfType.Add(obj);
            }
            else
              dependencyObjectQueue.Enqueue(child);
          }
        }
      }
      return childrenOfType;
    }
  }
}
