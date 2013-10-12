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
            foreach (IDesignElement elem in Design.DesignElements)
            {
                var contentCtrl = new ContentControl();
                contentCtrl.BeginInit();
                var style = Application.Current.FindResource("DesignerControlStyle") as Style;                                         
                if (elem is ITextDesignElement)
                {
                    var textElem = (elem as ITextDesignElement);
                    contentCtrl.Content = textElem.Text;
                    contentCtrl.FontSize = textElem.FontSize;
                    contentCtrl.FontFamily = textElem.Font;
                    contentCtrl.FontStyle = textElem.Style;
                    contentCtrl.Foreground = textElem.Color;
                }
                else if (elem is IImageDesignElement)
                {
                    contentCtrl.Content = (elem as IImageDesignElement).Image;
                    this.Height = (elem as IImageDesignElement).Image.Height;
                    this.Width = (elem as IImageDesignElement).Image.Width;
                }
                contentCtrl.Style = style;
                var dataTemplate = contentCtrl.ContentTemplate;
                Canvas.SetLeft(contentCtrl, elem.Origin.X);
                Canvas.SetTop(contentCtrl, elem.Origin.Y);
                contentCtrl.Height = 30;
                contentCtrl.Width = 70;
                //contentCtrl.Height = Math.Max(elem.Origin.Height, contentCtrl.ActualHeight);
                //contentCtrl.Width = Math.Max(elem.Origin.Width, contentCtrl.ActualWidth);
                cvsMain.Children.Add(contentCtrl);
            }
        }

        private void mnuAddNewImageElement_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "";
            ofd.DefaultExt = ".png";
            ofd.Filter = "Image Files (PNG, JPG, JPEG, GIF, BMP)|*.png;*.jpg;*.jpeg;*.gif;*.bmp";
            ofd.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ConfigurationManager.AppSettings["DefaultDirectory"]));
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.DereferenceLinks = true;
            if (ofd.ShowDialog() == true)
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
            var ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "";
            ofd.DefaultExt = ".png";
            ofd.Filter = "Image Files (PNG, JPG, JPEG, GIF, BMP)|*.png;*.jpg;*.jpeg;*.gif;*.bmp";
            ofd.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ConfigurationManager.AppSettings["DefaultDirectory"]));
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.DereferenceLinks = true;
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
