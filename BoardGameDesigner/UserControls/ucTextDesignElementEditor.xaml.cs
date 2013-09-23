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
    /// Interaction logic for ucTextDesignElement.xaml
    /// </summary>
    public partial class ucTextDesignElementEditor : UserControl
    {
        private ITextDesignElement _element;
        public ucTextDesignElementEditor(ITextDesignElement context)
        {
            _element = context;
            InitializeComponent();
            InitializeData();            
        }
        public ucTextDesignElementEditor(IDesign design)
        {
            _element = new TextDesignElement(design);
            InitializeComponent();
            InitializeData();
        }
        private ucTextDesignElementEditor()
        {
            InitializeComponent();
            InitializeData();
        }
        private void InitializeData()
        {
            LoadFontsIntoComboBox();
            if (_element != null)
            {
                txtDisplayText.DataContext = _element;
                cboFontChoice.DataContext = _element;
                sldFontSize.DataContext = _element;
                if (_element.DataSource != null && _element.ValueSource != null)
                {
                    chkUseDataBinding.IsChecked = true;
                    txtDisplayText.IsEnabled = false;
                    ccDataSetBinding.IsEnabled = true;
                }
                else
                {
                    chkUseDataBinding.IsChecked = false;
                    txtDisplayText.IsEnabled = true;
                    ccDataSetBinding.IsEnabled = false;
                }
                ccDataSetBinding.Content = new UserControls.ucBindToDataSet(_element);
            }
            
        }
        private void LoadFontsIntoComboBox()
        {
            cboFontChoice.ItemsSource = Fonts.SystemFontFamilies;
        }
        private void UpdateTextboxWithFontOptions()
        {
            txtDisplayText.FontSize = _element.FontSize;
            txtDisplayText.FontFamily = _element.Font;
            txtDisplayText.FontStyle = _element.Style;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            _element.Design.DesignManager.Project.IsDirty = true;
            if (this.Parent is ContentControl)
            {
                (this.Parent as ContentControl).Content = null;
            }
        }

        private void chkUseDataBinding_Checked(object sender, RoutedEventArgs e)
        {
            txtDisplayText.IsEnabled = false;
            ccDataSetBinding.IsEnabled = true;
        }

        private void chkUserDataBinding_Unchecked(object sender, RoutedEventArgs e)
        {
            txtDisplayText.IsEnabled = true;
            ccDataSetBinding.IsEnabled = false;
            _element.DataSource = null;
            _element.ValueSource = null;
        }

    }
}
