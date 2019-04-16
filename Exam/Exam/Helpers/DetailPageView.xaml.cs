using Exam.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Exam.Helpers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailPageView : ContentPage
	{
		public DetailPageView()
		{
			InitializeComponent();
        }
        public DetailPageView(MasterNavigationItem item)
        {
            InitializeComponent();
            lblImageName.Text = item.Title;
            imgDetail.Source = item.Icon;
        }
    }
}