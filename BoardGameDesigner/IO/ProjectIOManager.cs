using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGameDesigner.Projects;
using System.Configuration;
using System.Drawing;
using System.Xml.Linq;
using System.Windows.Media;
namespace BoardGameDesigner.IO
{
    public static class ProjectIOManager
    {
        public static Microsoft.Win32.OpenFileDialog GetImageFileDialog()
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
            return ofd;
        }

        public static Microsoft.Win32.SaveFileDialog GetSaveFileDialog()
        {
            var sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Reset();
            sfd.FileName = "New Project.bgProj";
            sfd.DefaultExt = ".bgProj";
            sfd.Filter = "Board Game Designer Projects (.bgProj)|*.bgProj";
            sfd.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ConfigurationManager.AppSettings["DefaultDirectory"]));
            sfd.AddExtension = true;
            sfd.CheckFileExists = false;
            sfd.CheckPathExists = true;
            sfd.DereferenceLinks = true;
            return sfd;
        }
        public static Microsoft.Win32.OpenFileDialog GetOpenFileDialog()
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "*.bgProj";
            ofd.DefaultExt = ".bgProj";
            ofd.Filter = "Board Game Designer Projects (.bgProj)|*.bgProj";
            ofd.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ConfigurationManager.AppSettings["DefaultDirectory"]));
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.DereferenceLinks = true;
            return ofd;
        }
        public static IProject LoadProject(string filePath)
        {
            var xDoc = System.Xml.Linq.XDocument.Load(filePath);
            var project = LoadProjectFromXmlDocument(xDoc);
            return project;
        }
        public static void SaveProject(IProject project, string filePath = null)
        {
            if (filePath == null)
                filePath = project.ProjectFilePath;
            var fileInfo = new System.IO.FileInfo(filePath);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            try
            {
                var xDoc = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "utf-8", "yes"));
                var projElem = project.ToXmlElement();
                xDoc.Add(projElem);
                xDoc.Save(filePath, System.Xml.Linq.SaveOptions.None);
                project.IsDirty = false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while saving the project.", ex);
            }
        }
        private static IProject LoadProjectFromXmlDocument(System.Xml.Linq.XDocument xDoc)
        {
            var projElement = xDoc.Element("Project");
            var proj = new GameProject().FromXmlElement(projElement) as GameProject;
            return proj;
        }
        public static XElement ConvertFontToXmlElement(FontFamily font)
        {
            return new XElement("Font", font.Source);
        }
        public static FontFamily ConvertFontFromXmlElement(XElement element)
        {
            if (element != null)
            {
                return new FontFamily(element.Value);
            }
            else
            {
                return new FontFamily("Arial");
            }
        }
    //    public static string ConvertImageToBase64String(System.Drawing.Image img)
    //    {
    //        using (var memStrm = new System.IO.MemoryStream())
    //        {
    //            img.Save(memStrm, System.Drawing.Imaging.ImageFormat.Png);
    //            var buffer = memStrm.ToArray();
    //            return Convert.ToBase64String(buffer);
    //        }

    //    }
    //    public static System.Drawing.Image ConvertImageFromBase64String(string base64)
    //    {
    //        byte[] img = Convert.FromBase64String(base64);
    //        using (var memStrm = new System.IO.MemoryStream(img, 0, img.Length))
    //        {
    //            memStrm.Write(img, 0, img.Length);
    //            return System.Drawing.Image.FromStream(memStrm);
    //        }
    //    }
    }
}
