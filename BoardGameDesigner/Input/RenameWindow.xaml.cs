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
using System.Windows.Shapes;
using BoardGameDesigner.Designs;
using System.Data;
namespace BoardGameDesigner.Input
{
    /// <summary>
    /// Interaction logic for RenameWindow.xaml
    /// </summary>
    public partial class RenameWindow : Window
    {
        private INameable _input;
        private DataColumn _inputColumn;
        private DataTable _inputTable;
        private List<string> _siblings;
        public RenameWindow(DataTable input, List<string> siblings)
        {
            InitializeComponent();
            _inputTable = input;
            _siblings = siblings;
            txtName.Focus();
        }
        public RenameWindow(DataColumn input, List<string> siblings)
        {
            InitializeComponent();
            _inputColumn = input;
            _siblings = siblings;
            txtName.Focus();
        }
        public RenameWindow(INameable input, List<string> siblings)
        {
            InitializeComponent();
            _input = input;
            _siblings = siblings;
            txtName.Focus();
        }
        private RenameWindow()
        {
            InitializeComponent();
            txtName.Focus();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool exists = false;
            foreach (string sibling in _siblings)
            {
                if (sibling.ToUpper() == txtName.Text.ToUpper())
                {
                    exists = true;
                    break;
                }                
            }
            if (exists)
            {
                MessageBox.Show("Name " + txtName.Text + " is already in use. Names must be unique in the same context.");
                return;
            }
            else
            {
                if (_input != null)
                {
                    _input.Name = txtName.Text;
                }
                if (_inputColumn != null)
                {
                    _inputColumn.ColumnName = txtName.Text;
                }
                if (_inputTable != null)
                {
                    _inputTable.TableName = txtName.Text;
                }
                this.DialogResult = true;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
