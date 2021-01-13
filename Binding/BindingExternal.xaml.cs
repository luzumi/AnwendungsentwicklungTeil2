using System.Windows;
using System.Windows.Controls;

namespace Binding
{
    /// <summary>
    /// Interaktionslogik für BindingExternal.xaml
    /// </summary>
    public partial class BindingExternal : Page
    {
        public BindingExternal(Window bindingTarget = null)
        {
            InitializeComponent();
            DataContext = bindingTarget;
            WindowContext = bindingTarget;
        }


        public Window WindowContext 
        {
            get;
            set;
        }
    }
}
