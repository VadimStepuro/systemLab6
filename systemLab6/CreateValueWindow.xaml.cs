using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace systemLab6
{
    /// <summary>
    /// Логика взаимодействия для CreateValueWindow.xaml
    /// </summary>
    public partial class CreateValueWindow : Window
    {
        private RegistryKey registry;

        public CreateValueWindow()
        {
            InitializeComponent();
        }

        public CreateValueWindow(RegistryKey registryKey)
        {
            InitializeComponent();
            registry = registryKey;
            type.Items.Add(RegistryValueKind.String);
            type.Items.Add(RegistryValueKind.Binary);
            type.Items.Add(RegistryValueKind.DWord);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(type.SelectedItem!= null && value.Text != null && name.Text != null) {
                switch (type.SelectedItem)
                {
                    case RegistryValueKind.Binary:
                    {
                        byte[] bytes = value.Text.Split(", ").Select(x => Convert.ToByte(x)).ToArray();
                        registry.SetValue(name.Text, bytes, (RegistryValueKind)type.SelectedItem);
                        break;
                    }
                    default:
                    {
                        registry.SetValue(name.Text, value.Text, (RegistryValueKind)type.SelectedItem);
                        break;
                    }
                }
            }
            this.Close();
        }
    }
}
