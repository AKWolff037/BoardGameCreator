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
using System.Configuration;
using System.Windows.Controls.Primitives;
namespace BoardGameDesigner.UserControls
{
    /// <summary>
    /// Interaction logic for ucDesignEditor.xaml
    /// </summary>
    public partial class ucDesignEditor : UserControl
    {
        private IDesign Design;
        public ucDesignEditor(IDesign design)
        {
            InitializeComponent();
            this.DataContext = design;
            Design = design;
            LoadDesignElements();
        }
        public ucDesignEditor()
        {
            InitializeComponent();
        }

        private void LoadDesignElements()
        {
            foreach (IDesignElement elem in Design.DesignElements.OrderBy(el => el.Layer))
            {
                var contentCtrl = new BoardGameDesigner.Lib.DesignerContent();
                contentCtrl.BeginInit();
                var style = Application.Current.FindResource("DesignerControlStyle") as Style;
                contentCtrl.DataContext = elem;                         
                contentCtrl.Style = style;
                var dataTemplate = contentCtrl.ContentTemplate;
                contentCtrl.Height = elem.Size.Height;
                contentCtrl.Width = elem.Size.Width;
                contentCtrl.ContentUpdated += contentCtrl_ContentUpdated;
                Canvas.SetLeft(contentCtrl, elem.X_Offset);
                Canvas.SetTop(contentCtrl, elem.Y_Offset);
                cvsMain.Children.Add(contentCtrl);
                CreateAndDrawImage();
            }
        }

        void contentCtrl_ContentUpdated(object sender, RoutedEventArgs e)
        {
            CreateAndDrawImage();
        }

        private void CreateAndDrawImage()
        {
            var img = (this.DataContext as IDesign).DrawImage();
            imgTemplate.Source = img;
            imgTemplate.Height = img.Height;
            imgTemplate.Width = img.Width;
        }

        private void mnuAddNewImageElement_Click(object sender, RoutedEventArgs e)
        {
            var ofd = IO.ProjectIOManager.GetImageFileDialog();
            if(ofd.ShowDialog() == true)
            {
                var img = new BitmapImage(new Uri(ofd.FileName));
                Design.DesignManager.Project.IsDirty = true;
                Design.DesignElements.Add(new ImageDesignElement(Design, img));
                LoadDesignElements();
            }
        }

        private void mnuAddNewTextElement_Click(object sender, RoutedEventArgs e)
        {
            Design.DesignElements.Add(new TextDesignElement(Design));
            LoadDesignElements();
        }

        private void mnuSetTemplate_Click(object sender, RoutedEventArgs e)
        {
            var ofd = IO.ProjectIOManager.GetImageFileDialog();
            if (ofd.ShowDialog() == true)
            {
                Design.Template = new BitmapImage(new Uri(ofd.FileName));
                Design.DesignManager.Project.IsDirty = true;
                imgTemplate.Source = Design.Template;
            }
            LoadDesignElements();
        }

        public void PreviewFirstItem()
        {

        }
        private void mnuPreviewFirstRow(object sender, RoutedEventArgs e)
        {

        }

    }
}
