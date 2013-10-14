using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace BoardGameDesigner.Lib
{
    public class DesignerContent : ContentControl
    {
        public event RoutedEventHandler ContentUpdated;
        public DesignerContent()
            : base()
        {

        }
        public void UpdateContent(object sender)
        {
            if (ContentUpdated != null)
            {
                var eventArgs = new RoutedEventArgs();
                ContentUpdated.Invoke(this, eventArgs);
            }
        }
    }
    
}
