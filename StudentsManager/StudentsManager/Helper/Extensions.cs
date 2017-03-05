using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace StudentsManager.Helper
{
    public static class Extensions
    {
        #region Visual Tree Helper

        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject element) where T : Visual
        {
            if (element != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(element, i);
                    if (child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        #endregion
    }
}
