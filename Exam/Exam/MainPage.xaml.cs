using Exam.Helpers;
using Exam.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Exam
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            masterView.ListView.ItemSelected += ListView_ItemSelected;
        }
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterNavigationItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(DetailPageView),item));
                masterView.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
