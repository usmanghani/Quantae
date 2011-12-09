using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Quantae.Engine;
using System.Configuration;
using Quantae.Repositories;
using Quantae.DataModel;
using SentenceMediaHandler.BingSearchService;
using System.Net;
using System.IO;

namespace SentenceMediaHandler
{
    public partial class Form1 : Form
    {
        private delegate void SearchCompletedDelegate(FlickrNet.PhotoCollection photos);
        private SearchCompletedDelegate SearchCompletedInstance = null;

        public Form1()
        {
            InitializeComponent();

            QuantaeEngine.Init(
                ConfigurationManager.AppSettings["MONGOHQ_DB"],
                ConfigurationManager.AppSettings["MONGOHQ_URL"]);

            SearchCompletedInstance = new SearchCompletedDelegate(SearchCompleted);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var topics = Repositories.Topics.GetAllItems();

            foreach (var topic in topics.OrderBy(t => t.Index))
            {
                var sentences = Repositories.Sentences.FindAs(SentenceQueries.GetSentencesByTopic(new TopicHandle(topic)));

                string nodeName = string.Format("{0} - {1} ({2})", topic.Index, topic.TopicName, sentences.Count());

                TreeNode node = new TreeNode(nodeName);
                node.Tag = "topic";
                foreach (var sentence in sentences)
                {
                    node.Nodes.Add(sentence.ObjectId.ToString(), sentence.SentenceText + " -- " + sentence.SentenceTranslation);
                }

                treeView1.Nodes.Add(node);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string tag = (string)e.Node.Tag;

            if (tag == "topic")
                return;

            var sentence = Repositories.Sentences.FindById(e.Node.Name);

            //List<string> tags = new List<string>();
            //foreach (var vocab in sentence.VocabEntries)
            //{
            //    VocabEntry entry = Repositories.Vocabulary.GetItemByHandle(vocab);
            //    tags.Add(entry.Translation);
            //}
            
            //GetImageUrlsForSentence(string.Join(",", tags.ToArray()));
            GetImageUrlsForSentence(sentence.SentenceTranslation);
        }

        private void GetImageUrlsForSentence(string query)
        {
            if (rdoFlickr.Checked)
            {
                FlickrNet.Flickr flickr = new FlickrNet.Flickr("580162ba802ed95a92fc92494dcdecbf", "a6b6d63a6ce977c5");
                FlickrNet.PhotoSearchOptions options = new FlickrNet.PhotoSearchOptions();
                options.Tags = query;
                options.PerPage = 10;
                //options.IsCommons = true;
                options.Licenses.Add(FlickrNet.LicenseType.NoKnownCopyrightRestrictions);
                options.Licenses.Add(FlickrNet.LicenseType.AttributionCC);
                options.Licenses.Add(FlickrNet.LicenseType.AttributionNoDerivativesCC);
                options.Licenses.Add(FlickrNet.LicenseType.AttributionShareAlikeCC);

                var result = flickr.PhotosSearch(options);
                SearchCompleted(result);

                flickr.PhotosSearchAsync(options, asyncResult =>
                    {
                        if (asyncResult.HasError)
                        {
                            TextBox txtError = new TextBox();
                            txtError.Multiline = true;
                            txtError.Dock = DockStyle.Fill;
                            txtError.Text = asyncResult.Error.ToString();
                            flowLayoutPanel1.Controls.Add(txtError);

                            return;
                        }

                        var photos = asyncResult.Result;

                        SearchCompleted(photos);
                    });
            }

            else if (rdoBing.Checked)
            {
                using (BingPortTypeClient client = new BingPortTypeClient())
                {
                    client.SearchCompleted += new EventHandler<SearchCompletedEventArgs>(client_SearchCompleted);
                    var searchRequest = new SearchRequest();
                    searchRequest.Adult = BingSearchService.AdultOption.Strict;
                    searchRequest.AdultSpecified = true;
                    searchRequest.Market = "en-US";
                    searchRequest.Version = "2.2";
                    searchRequest.AppId = "C208A7E582F635C7768950E74C8FDC274A0EA7B4";
                    searchRequest.Sources = new BingSearchService.SourceType[] { SourceType.Image };
                    searchRequest.Query = query;

                    searchRequest.Image = new ImageRequest();
                    searchRequest.Image.Count = 10;
                    progressBar1.Step = (int)100 / (int)searchRequest.Image.Count;
                    searchRequest.Image.CountSpecified = true;

                    searchRequest.Image.Offset = 0;
                    searchRequest.Image.OffsetSpecified = true;

                    client.SearchAsync(searchRequest);
                }
            }
        }

        void client_SearchCompleted(object sender, SearchCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }

            flowLayoutPanel1.Controls.Clear();
            if (e.Error != null)
            {
                TextBox txtError = new TextBox();
                txtError.Multiline = true;
                txtError.Dock = DockStyle.Fill;
                txtError.Text = e.Error.ToString();
                
                flowLayoutPanel1.Controls.Add(txtError);
            }

            var response = e.Result;

            if (response.Image != null && response.Image.Results.Count() > 0)
            {
                foreach (var imageUrl in response.Image.Results.Select(ir => ir.MediaUrl))
                {
                    WebClient client = new WebClient();
                    client.DownloadDataCompleted += new DownloadDataCompletedEventHandler(client_DownloadDataCompleted);
                    client.DownloadDataAsync(new Uri(imageUrl));
                }
            }
        }

        void SearchCompleted(FlickrNet.PhotoCollection photos)
        {
            flowLayoutPanel1.Controls.Clear();

            foreach (FlickrNet.Photo photo in photos)
            {
                WebClient client = new WebClient();
                client.DownloadDataCompleted += new DownloadDataCompletedEventHandler(client_DownloadDataCompleted);
                client.DownloadDataAsync(new Uri(photo.SmallUrl), photo);
            }
        }

        void client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null)
            {
                return;
            }

            int width = 200;
            int height = 200;


            if (e.UserState != null)
            {
                FlickrNet.Photo photo = (FlickrNet.Photo)e.UserState;

                width = photo.SmallWidth.HasValue ? photo.SmallWidth.Value : width;
                height = photo.SmallHeight.HasValue ? photo.SmallHeight.Value : height;
            }
            
            byte[] imageData = e.Result;
            PictureBox pictureBox = new PictureBox();
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Size = new Size(width, height);

            using (MemoryStream stream = new MemoryStream(imageData))
            {
                pictureBox.Image = Image.FromStream(stream);
            }

            progressBar1.PerformStep();

            flowLayoutPanel1.Controls.Add(pictureBox);
        }
    }
}

