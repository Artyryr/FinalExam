using Exam.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Exam.Helpers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPageView : ContentPage
    {
        private const string KEY = "a2b003481632a966c5f43b9671f70a9b";
        private static HttpClient flickrClient = new HttpClient();
        Task<string> flickrTask = null;
        List<FlickrResult> flickrPhotos = new List<FlickrResult>() { };
        List<MasterNavigationItem> masterNavigationItems = new List<MasterNavigationItem>() { };

        public MasterPageView()
        {
            InitializeComponent();
        }

        private async void BtnSearch_Clicked(object sender, EventArgs e)
        {
            if (txtSearch.Text != " ")
            {
                // if flickrTask already running, prompt user 
                if (flickrTask?.Status != TaskStatus.RanToCompletion)
                {
                    bool answer = await DisplayAlert("Alert", "Cancel the current Flickr search?", "Yes", "No");
                    if (answer)
                    {
                        return;
                    }
                    else
                    {
                        flickrClient.CancelPendingRequests(); // cancel search
                    }
                }

                // Flickr's web service URL for searches                         
                var flickrURL = "https://api.flickr.com/services/rest/?method=" +
                   $"flickr.photos.search&api_key={KEY}&" +
                   $"tags={txtSearch.Text.Replace(" ", ",")}" +
                   "&tag_mode=all&per_page=500&privacy_filter=1";

                ListView.ItemsSource = null;
                flickrPhotos = new List<FlickrResult>() { };
                masterNavigationItems = new List<MasterNavigationItem>() { };

                flickrTask = flickrClient.GetStringAsync(flickrURL);

                XDocument flickrXML = XDocument.Parse(await flickrTask);

                flickrPhotos =
                (from photo in flickrXML.Descendants("photo")
                 let id = photo.Attribute("id").Value
                 let title = photo.Attribute("title").Value
                 let secret = photo.Attribute("secret").Value
                 let server = photo.Attribute("server").Value
                 let farm = photo.Attribute("farm").Value
                 select new FlickrResult
                 {
                     Title = title,
                     URL = $"https://farm{farm}.staticflickr.com/" +
                       $"{server}/{id}_{secret}.jpg"
                 }).ToList();

                if (flickrPhotos.Any())
                {
                    await Task.Factory.StartNew(() =>
                    {
                        ParallelLoopResult loopResult = Parallel.ForEach<FlickrResult>(flickrPhotos, photo =>
                        {
                            MasterNavigationItem item = new MasterNavigationItem
                            {
                                Icon = photo.URL,
                                Title = photo.Title,
                                Target = typeof(DetailPageView)
                            };

                            masterNavigationItems.Add(item);
                        });
                    });
                    ListView.ItemsSource = masterNavigationItems;
                }
                else 
                {
                    await DisplayAlert("Alert", "No matches", "OK");
                }
            }
        }
    }
}