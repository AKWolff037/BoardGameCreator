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
using System.Data;
using BoardGameDesigner.Projects;
namespace BoardGameDesigner.UserControls
{
    /// <summary>
    /// Interaction logic for ucDataSetEditor.xaml
    /// </summary>
    public partial class ucDataSetEditor : UserControl
    {
        private IProject _project = null;
        public ucDataSetEditor(object context, IProject proj)
        {
            InitializeComponent();
            _project = proj;
            dgMain.DataContext = context;
            LoadTableComboBox(context as DataSet);
        }
        public ucDataSetEditor()
        {
            InitializeComponent();
        }

        private void dgMain_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            dgMain.UnselectAll();
        }

        private void btnInsertColumn_Click(object sender, RoutedEventArgs e)
        {
            if (dgMain.ItemsSource != null)
            {
                var siblings = new List<string>();
                var newCol = new DataColumn();
                var columns = (dgMain.ItemsSource as DataView).Table.Columns;
                foreach (DataColumn column in columns)
                {
                   siblings.Add(column.ColumnName);
                } 
                var renameWindow = new Input.RenameWindow(newCol, siblings);
                if (renameWindow.ShowDialog() == true)
                {
                    (dgMain.ItemsSource as DataView).Table.Columns.Add(newCol);
                    dgMain.ItemsSource = (dgMain.ItemsSource as DataView).Table.AsDataView();
                }                
            }
        }
        private void LoadTableComboBox(DataSet ds)
        {
            cboTables.Items.Clear();
            foreach (DataTable dt in ds.Tables)
            {
                cboTables.Items.Add(dt);
            }
            cboTables.Items.Add(new DataTable("<Add New Table>"));           
        }

        private void cboTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cboTables.SelectedItem as DataTable).TableName == "<Add New Table>")
            {
                var newTable = new DataTable();
                var siblings = new List<string>();
                var tables = (dgMain.DataContext as DataSet).Tables;
                foreach (DataTable table in tables)
                {
                    siblings.Add(table.TableName);                    
                }
                var renameWindow = new Input.RenameWindow(newTable, siblings);
                if (renameWindow.ShowDialog() == true)
                {
                    (dgMain.DataContext as DataSet).Tables.Add(newTable);
                    dgMain.ItemsSource = newTable.DefaultView;
                    cboTables.SelectedItem = newTable;
                }
            }
            else if (cboTables.SelectedItem != null && cboTables.SelectedItem is DataTable)
            {
                dgMain.ItemsSource = (cboTables.SelectedItem as DataTable).DefaultView;                
            }
        }
        private void InsertNewRow()
        {
            if (dgMain.SelectedCells.Count == 1)
            {
                if (dgMain.SelectedCells[0].Column == dgMain.Columns.Last())
                {
                    var dgSource = (dgMain.DataContext as DataTable);
                    if (dgSource != null)
                    {
                        var newRow = dgSource.NewRow();
                        newRow.BeginEdit();
                    }
                }
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (_project != null)
                _project.Save();
            if (this.Parent is ContentControl)
            {
                (this.Parent as ContentControl).Content = null;
            }
        }
    }
}
