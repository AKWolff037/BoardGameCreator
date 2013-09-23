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
using System.Data;
using BoardGameDesigner.IO;
using BoardGameDesigner.Designs;
using BoardGameDesigner.Projects;
using System.Drawing;
namespace BoardGameDesigner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnNewProject_Click(object sender, RoutedEventArgs e)
        {
            var sfd = IO.ProjectIOManager.GetSaveFileDialog(); 
            if (sfd.ShowDialog() == true)
            {
                var newProject = new Projects.GameProject(sfd.SafeFileName.Replace(".bgProj", ""));
                IO.ProjectIOManager.SaveProject(newProject, sfd.FileName);
            }
        }

        private void btnLoadProject_Click(object sender, RoutedEventArgs e)
        {
            var ofd = IO.ProjectIOManager.GetOpenFileDialog(); 

            if (ofd.ShowDialog() == true)
            {
                var project = IO.ProjectIOManager.LoadProject(ofd.FileName);
                var projMan = new ProjectManager(project, ofd.FileName);
                projMan.ShowDialog();
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnTestLoad_Click(object sender, RoutedEventArgs e)
        {
            var derp = IO.ProjectIOManager.LoadProject(@"C:\Users\Alex\Desktop\TestProjectOmg.bgProj");
        }
    }
}
