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
using CoreLib;

namespace Address_Manager
{

  /// <summary>
  /// Interaktionslogik für MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    SqlHandler _sqlHandler;
    public MainWindow()
    {
        InitializeComponent();
      _sqlHandler = new SqlHandler(/*defaultDbLocation: location in config file*/);
      _sqlHandler.DbInitialize();
      //_sqlHandler.
    }

    private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
    {
      //throw new Exception(sender.ToString());
      var listPeople = _sqlHandler.GetListPerson();
      this.DatGridNameList.ItemsSource = listPeople;
    }

    private void DatGridNameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void DatGridNameList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (DatGridNameList.SelectedIndex == -1)
        return;
      var dataPerson = (PersonHandler)this.DatGridNameList.SelectedItem;
      this.TBPId.Text = dataPerson.PId.ToString();
      this.TBName.Text = dataPerson.Name;
      this.TBFamily.Text = dataPerson.Family;
      this.TBPostalCode.Text = dataPerson.PostalCode;
      this.TBCity.Text = dataPerson.City;
      this.TBStreet.Text = dataPerson.Street;
      this.TBSNumber.Text = dataPerson.StreetNumber;
      this.TabControlMain.SelectedIndex = 1;
    }

    private void BCProfileAdd_Click(object sender, RoutedEventArgs e)
    {
      if (DatGridNameList.SelectedIndex == -1)
        _sqlHandler.AddPerson(
          nameFirst: this.TBName.Text,
          nameLast: this.TBName.Text,
          postalCode: this.TBPostalCode.Text,
          cityName: this.TBName.Text,
          streetName: this.TBName.Text,
          houseNumber: this.TBName.Text
          );
      else
        _sqlHandler.AddPerson(
          nameFirst: "",
          nameLast: "",
          postalCode: "",
          cityName: "",
          streetName: "",
          houseNumber: ""
          );

    }

    private void BCProfileRemove_Click(object sender, RoutedEventArgs e)
    {

    }

    private void BCPhoneAdd_Click(object sender, RoutedEventArgs e)
    {

    }

    private void BCPhoneRemove_Click(object sender, RoutedEventArgs e)
    {

    }

    private void BCPictureAdd_Click(object sender, RoutedEventArgs e)
    {

    }

    private void BCPictureRemove_Click(object sender, RoutedEventArgs e)
    {

    }

    private void BCPictureSave_Click(object sender, RoutedEventArgs e)
    {

    }
  }
}
