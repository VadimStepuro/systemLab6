using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace systemLab6
{
    public class RegistryTreeItem : TreeViewItem
    {
        public RegistryKey RegistryKey { get; set; }
        public string Name { get; set; }
        public string[] ValuesNames { get; set; }
        public object[] Values { get; set; }
        public RegistryValueKind[] RegistryValueKinds { get; set; }

        public RegistryTreeItem(RegistryKey key, string name):base()
        {
            Name = name;
            RegistryKey = key;

            ValuesNames = key.GetValueNames();
            Values = new object[ValuesNames.Length];
            RegistryValueKinds= new RegistryValueKind[ValuesNames.Length];
            for(int i = 0; i < ValuesNames.Length;i++)
            {
                Values[i] = key.GetValue(ValuesNames[i]);
                RegistryValueKinds[i] = key.GetValueKind(ValuesNames[i]); 
            }

            this.Header = name;
            this.Selected += ((MainWindow)Application.Current.MainWindow).onSelectedItem;
            this.Expanded += onExpandedEvent;
            this.Collapsed += onCollapsedEvent;
        }


        private void onExpandedEvent(object sender, RoutedEventArgs e)
        {
            string[] strings = RegistryKey.GetSubKeyNames();
            if (strings.Length > 0)
            {
                foreach (string s in strings)
                {
                    try
                    {
                        RegistryKey? key = RegistryKey.OpenSubKey(s, true);
                        if (key != null)
                        {
                            RegistryTreeItem registryTreeItem = new RegistryTreeItem(key, s);
                            this.Items.Add(registryTreeItem);
                        }
                    }
                    catch
                    {
                        Debug.WriteLine(s + " is under secure");
                    }
                }
            }
        }

        private void onCollapsedEvent(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.Items.Clear();
        }

        
    }
}
