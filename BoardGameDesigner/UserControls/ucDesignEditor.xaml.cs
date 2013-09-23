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

namespace BoardGameDesigner.UserControls
{
    /// <summary>
    /// Interaction logic for ucDesignEditor.xaml
    /// </summary>
    public partial class ucDesignEditor : UserControl
    {
        public ucDesignEditor(object context)
        {
            InitializeComponent();
            this.DataContext = context;
        }
        public ucDesignEditor()
        {
            InitializeComponent();
        }
    }
}
