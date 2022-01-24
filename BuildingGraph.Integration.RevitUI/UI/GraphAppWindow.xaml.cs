using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using BuildingGraph.Integration.Revit.UIAddin.ViewModel;

namespace BuildingGraph.Integration.Revit.UIAddin
{
    internal partial class GraphAppWindow : Window
    {
        GraphAppViewModel _gappVM;
        public GraphAppWindow(GraphAppViewModel gappVM)
        {
            InitializeComponent();

            _gappVM = gappVM;
            this.DataContext = gappVM;

            pwBox.PasswordChanged += PwBox_PasswordChanged;
        }

        private void PwBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _gappVM.Password = pwBox.Password;
        }
    }
}
