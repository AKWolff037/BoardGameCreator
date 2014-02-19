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
using System.Configuration;
using BoardGameDesigner.Designs;
namespace BoardGameDesigner.UserControls
{
    /// <summary>
    /// Interaction logic for ucTemplateEditor.xaml
    /// </summary>
    public partial class ucTemplateEditor : UserControl
    {
        public IDesign _design;
        public ucTemplateEditor(IDesign context)
        {
            InitializeComponent();
            if (context.Template == null)
            {
                SetTemplate(context);
            }
            this.DataContext = context;
            _design = context;
            var fullImage = context.DrawImage();
            imgMain.Source = fullImage;
        }
        public ucTemplateEditor()
        {
            InitializeComponent();
        }
        public void SetTemplate(IDesign context)
        {
            var ofd = IO.ProjectIOManager.GetImageFileDialog();
            if (ofd.ShowDialog() == true)
            {
                context.Template = new BitmapImage(new Uri(ofd.FileName));
                context.DesignManager.Project.IsDirty = true;
                imgMain.Source = context.Template;
                _design.DesignManager.Project.Save();
            }
            else
            {
                (this.Parent as ContentControl).Content = null;
            }            
        }
    }
}
