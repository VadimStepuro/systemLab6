using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace systemLab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RegistryTreeItem selectedItem;
        private string valueName;

        public MainWindow()
        {
            InitializeComponent();

            createBaseTree();
            
        }

        private void createBaseTree()
        {
            RegistryKey localMachine = Registry.LocalMachine;
            RegistryTreeItem localMachineItem = new RegistryTreeItem(localMachine, "LocalMachine");
            treeView.Items.Add(localMachineItem);

            RegistryKey currentUser = Registry.CurrentUser;
            RegistryTreeItem currentUserItem = new RegistryTreeItem(currentUser, "CurrentUser");
            treeView.Items.Add(currentUserItem);


            RegistryKey users = Registry.Users;
            RegistryTreeItem usersItem = new RegistryTreeItem(users, "Users");
            treeView.Items.Add(usersItem);

            RegistryKey classesRoot = Registry.ClassesRoot;
            RegistryTreeItem classesRootItem = new RegistryTreeItem(classesRoot, "ClassesRoot");
            treeView.Items.Add(classesRootItem);

            RegistryKey currentConfig = Registry.CurrentConfig;
            RegistryTreeItem currentConfigItem = new RegistryTreeItem(currentConfig, "CurrentConfig");
            treeView.Items.Add(currentConfigItem);

        }

        

        

        private void onSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBlock block = (TextBlock)((StackPanel)((ListBox)sender).SelectedItems[0]).Children[0];
                valueName = block.Text;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public void onSelectedItem(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            listValues.Items.Clear();

            selectedItem = (RegistryTreeItem)sender;

            for (int i = 0; i < selectedItem.Values.Length; i++)
            {
                StackPanel stackPanel = new StackPanel();
                
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Margin = new Thickness(3);
                stackPanel.Height = 50;

                TextBlock textBlockValueName = new TextBlock();
                textBlockValueName.Text = selectedItem.ValuesNames[i].ToString();
                textBlockValueName.FontSize = 10;
                textBlockValueName.Height = 25;
                textBlockValueName.Width = 100;
                textBlockValueName.Margin = new Thickness(0, 0, 10, 0);


                TextBlock textBlockValue = new TextBlock();
                if (selectedItem.RegistryValueKinds[i] == RegistryValueKind.Binary)
                {
                   
                    byte[] bytes = (byte[])selectedItem.Values[i];
                    string txt = "";
                    foreach (byte b in bytes)
                        txt += b + ", ";
                    textBlockValue.Text = txt;
                    textBlockValue.FontSize = 10;
                    textBlockValue.Height = 25;
                    textBlockValue.Width = 100;
                    textBlockValue.Margin = new Thickness(0, 0, 10, 0);
                }
                else
                {
                    textBlockValue.Text = selectedItem.Values[i].ToString();
                    textBlockValue.FontSize = 10;
                    textBlockValue.Height = 25;
                    textBlockValue.Width = 100;
                    textBlockValue.Margin = new Thickness(0, 0, 10, 0);
                }

                TextBlock textBlockValueKind = new TextBlock();
                textBlockValueKind.Text = selectedItem.RegistryValueKinds[i].ToString();
                textBlockValueKind.FontSize = 10;
                textBlockValueKind.Height = 25;
                textBlockValueKind.Width = 100;
                textBlockValueKind.Margin = new Thickness(0, 0, 10, 0);

                stackPanel.Children.Add(textBlockValueName);
                stackPanel.Children.Add(textBlockValueKind);
                stackPanel.Children.Add(textBlockValue);

                listValues.Items.Add(stackPanel);



            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            e.Handled= true;

            if (subKeyName.Text.Length > 0 && selectedItem != null) 
            {
                try
                {
                    RegistryKey registryKey = selectedItem.RegistryKey.CreateSubKey(subKeyName.Text);
                    RegistryTreeItem newItem = new RegistryTreeItem(registryKey, subKeyName.Text);
                    selectedItem.Items.Add(newItem);

                }
                catch {
                    Debug.WriteLine("Cannot write subkey");
                }
            }
        }

        private void subKeyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(subKeyName.Text.Length==0)
                addSubKeyButton.IsEnabled = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(selectedItem != null)
            {
                RegistryTreeItem parent = selectedItem.Parent as RegistryTreeItem;
                parent.RegistryKey.DeleteSubKey(selectedItem.Name);
                parent.Items.Remove(selectedItem);
                selectedItem = null;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            CreateValueWindow createValueWindow = new CreateValueWindow(selectedItem.RegistryKey);
            createValueWindow.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            selectedItem.RegistryKey.DeleteValue(valueName);
        }
    }
}
