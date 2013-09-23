using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BoardGameDesigner.Designs;
namespace BoardGameDesigner.UserControls
{
    /// <summary>
    /// Interaction logic for ucBindToDataSet.xaml
    /// </summary>
    public partial class ucBindToDataSet : UserControl
    {
        private IDesignElement _element;
        public ucBindToDataSet(IDesignElement element)
        {
            _element = element;
            InitializeComponent();
            InitializeBindings();
        }
        public ucBindToDataSet()
        {
            InitializeComponent();
            InitializeBindings();
        }
        private void InitializeBindings()
        {
            this.DataContext = _element;
            lbxDataTables.DataContext = _element;
            lbxDataColumns.DataContext = lbxDataTables;

            lbxDataTables.ItemsSource = _element.Design.DesignManager.Dataset.Tables;
            lbxDataTables.SetBinding(ListBox.SelectedItemProperty, "DataSource");
            lbxDataColumns.SetBinding(ListBox.ItemsSourceProperty, "SelectedItem.Columns");
            lbxDataColumns.SetBinding(ListBox.SelectedItemProperty, "DataContext.ValueSource");
        }
    }
}
