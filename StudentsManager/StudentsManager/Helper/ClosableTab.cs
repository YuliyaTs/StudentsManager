using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using StudentsManager.View.Base;
using StudentsManager.View.NewFeatures;

namespace StudentsManager.Helper
{
    public class ClosableTab : TabItem
    {
        #region Fields

        private ObservableCollection<ClosableTab> _viewsInTab; 

        #endregion

        #region Constructors

        public ClosableTab(BaseView view, ObservableCollection<ClosableTab> viewsInTab, IList listOfInstances = null)
        {

            var closableTabHeader = new ClosableHeader(view, viewsInTab, this, listOfInstances);

            Header = closableTabHeader;
            _viewsInTab = viewsInTab;

            closableTabHeader.ButtonClose.MouseEnter += ButtonCloseMouseEnter;
            closableTabHeader.ButtonClose.MouseLeave += ButtonCloseMouseLeave;
            closableTabHeader.LabelTabTitle.SizeChanged += LabelTabTitleSizeChanged;
        }

        #endregion

        #region Methods

        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            ((ClosableHeader)Header).ButtonClose.Visibility = Visibility.Visible;
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            ((ClosableHeader)Header).ButtonClose.Visibility = Visibility.Hidden;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            ((ClosableHeader)Header).ButtonClose.Visibility = Visibility.Visible;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!IsSelected)
            {
                ((ClosableHeader)Header).ButtonClose.Visibility = Visibility.Hidden;
            }
        }

        #endregion

        #region Methods

        void ButtonCloseMouseEnter(object sender, MouseEventArgs e)
        {
            ((ClosableHeader)Header).ButtonClose.Foreground = Brushes.SteelBlue;
        }

        void ButtonCloseMouseLeave(object sender, MouseEventArgs e)
        {
            ((ClosableHeader)Header).ButtonClose.Foreground = Brushes.Black;
        }

        void LabelTabTitleSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((ClosableHeader)Header).ButtonClose.Margin = new Thickness(
               ((ClosableHeader)Header).LabelTabTitle.ActualWidth + 5, 3, 4, 0);
        }

        #endregion
    }
}
