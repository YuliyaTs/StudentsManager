using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using Microsoft.Windows.Controls.Ribbon;
using StudentsManager.Helper;
using StudentsManager.ViewModel;

namespace StudentsManager
{
    public partial class AppView : Window
    {
        #region Variables

        private List<RibbonTab> _ribbonTabs;

        // ADORNER______________________ 

        AdornerLayer AdornerLayer;

        //_______________________________

        #endregion

        #region Initialization

        public AppView()
        {
            InitializeComponent();
            DataContext = new AppViewModel();

            Loaded += (sender, args) =>
            {
                InitializeRibbon();

                var toggleButton = RibbonApplicationMenu.FindVisualChildren<RibbonToggleButton>().FirstOrDefault(el => el.Name == "PART_ToggleButton");
                if (toggleButton != null)
                {
                    //toggleButton.Template = (ControlTemplate)FindResource("RibbonMenuToggleButtonTemplate");
                    toggleButton.Style = (Style)FindResource("ToggleButtonTemplate");
                }

                var ribbonMenuPopup = RibbonApplicationMenu.FindVisualChildren<Popup>().FirstOrDefault(el => el.Name == "PART_Popup");
                if (ribbonMenuPopup != null)
                {
                    RibbonToggleButton popupToggleButton = ribbonMenuPopup.Child.FindVisualChildren<RibbonToggleButton>().FirstOrDefault();
                    if (popupToggleButton != null)
                    {
                        popupToggleButton.Style = (Style)FindResource("ToggleButtonTemplate");
                    }
                }

                ribbonMenuPopup = RibbonApplicationMenu.FindVisualChildren<Popup>().FirstOrDefault(el => el.Name == "PART_Popup");
                if (ribbonMenuPopup != null)
                {
                    var popupBorder = ribbonMenuPopup.Child.FindVisualChildren<Border>().FirstOrDefault();
                    if (popupBorder != null)
                    {
                        popupBorder.Style = (Style)FindResource("PopupBorderStyle");
                    }
                }

                // ADORNER

                AdornerLayer = AdornerLayer.GetAdornerLayer(ListView);
                AdornerLayer.Add(new ResizingAdorner(ListView, ListViewSplitter));


            };
        }

        protected void InitializeRibbon()
        {
            AddRibbonGroup("RibbonTabHome", "RibbonGroupRefresh");
            AddRibbonGroup("RibbonTabHome", "RibbonGroupSave");
            AddRibbonGroup("RibbonTabHome", "RibbonGroupEdit");
            AddRibbonGroup("RibbonTabHome", "RibbonGroupImportExport");
            AddRibbonGroup("RibbonTabHome", "RibbonGroupSwitchView");

            foreach (var tab in RibbonTabs)
            {
                PART_RibbonMenu.Items.Add(tab);
            }
        }

        #endregion

        #region Properties

        protected List<RibbonTab> RibbonTabs
        {
            get
            {
                if (_ribbonTabs == null)
                    _ribbonTabs = new List<RibbonTab>();

                return _ribbonTabs;
            }
        }

        #endregion

        #region Methods

        private void AppWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (((AppViewModel)DataContext).CloseAppCommand.CanExecute(null))
                ((AppViewModel)DataContext).CloseAppCommand.Execute(null);
        }

        protected void AddRibbonGroup(string tabName, string groupName)
        {
            RibbonTab tab = RibbonTabs.Find(rt => rt.Name == tabName);
            if (tab == null)
            {
                tab = FindResource(tabName) as RibbonTab;
                if (tab == null)
                {
                    return;
                }

                tab.Name = tabName;
                RibbonTabs.Add(tab);
            }

            var group = FindResource(groupName) as RibbonGroup;
            if (group != null)
            {
                group.Name = groupName;
                tab.Items.Add(group);
            }
        }

        #endregion

    }
}
