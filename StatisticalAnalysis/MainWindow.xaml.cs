using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Charts;

namespace StatisticalAnalysis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _fileName;
        private StaticticsManager _manager;
        private bool _visitorsBaseLoaded;
        public MainWindow()
        {
            InitializeComponent();
            _manager = new StaticticsManager();
            Menu.SelectionChanged += ListBox_SelectionChanged;
            InitPage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };
            var result = dlg.ShowDialog();

            if (result == true)
            {
                _fileName = dlg.FileName;
                _manager.CreateVisitorsDatabase(_fileName);
                RenderVisitorsDatabase();
            }
        }

        private void RenderVisitorsDatabase()
        {
            if (!_visitorsBaseLoaded)
                if (_manager.IsDatabaseLoaded)
                {
                    VisitorsBase.Items.Clear();
                    VisitorsBase.ItemsSource = _manager.VisitorsBase.Visitors;
                    _visitorsBaseLoaded = true;
                }
        }

        private void RenderStatistics()
        {
            foreach (var category in _manager.ProductsBase.Categories)
            {
                var counter = _manager.VisitorsBase.Visitors.Select(
                    visitor => (
                        from product in _manager.ProductsBase.Products 
                        where visitor.ProductId == product.Id 
                        select product.CategoryId)
                        .FirstOrDefault())
                        .Count(cat => cat == category.Id);
                Series.Points.Add(new SeriesPoint { Argument = category.Name, Value = counter });
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _manager.SaveDatabases();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitPage();
        }

        private void InitPage()
        {
            Caption.Content = ((ListBoxItem)Menu.SelectedItem).Content;
            HideControls();
            switch (Menu.SelectedIndex)
            {
                //case 0: GeneralPage.Visibility = Visibility.Visible;
                //    break;
                case 0: VisitorsBase.Visibility = Visibility.Visible;
                    RenderVisitorsDatabase();
                    break;
                case 1: StatisticsPage.Visibility = Visibility.Visible;
                    RenderStatistics();
                    break;  
            }
        }
        private void HideControls()
        {
            GeneralPage.Visibility = Visibility.Collapsed;
            VisitorsBase.Visibility = Visibility.Collapsed;
            StatisticsPage.Visibility = Visibility.Collapsed;
        }
    }
}
