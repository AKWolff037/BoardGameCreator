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
    /// Interaction logic for ucConditionEditor.xaml
    /// </summary>
    public partial class ucConditionEditor : UserControl
    {
        public ICondition Condition;
        public ucConditionEditor(ICondition cond)
        {
            InitializeComponent();
            Condition = cond;
        }
        public ucConditionEditor()
        {
            InitializeComponent();
        }
        private void LoadControl()
        {
            if (Condition != null)
            {

            }
        }
    }
}
