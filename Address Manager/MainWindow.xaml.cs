using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Address_Manager
{

  public class DataObject
  {
    public string Name { get; set; }
    public string Family { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
  }

  /// <summary>
  /// Interaktionslogik für MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var list = new ObservableCollection<DataObject>();
            list.Add(new DataObject() { Name = "Name", Family = "Family", City = "City", Street = "Street", StreetNumber = "StreetNumber" });
            this.DatGridNameList.ItemsSource = list;
    }
    }
}
