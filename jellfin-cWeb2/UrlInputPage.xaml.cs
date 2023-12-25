using System;
using System.Net;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace jellyfin_cWeb2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UrlInputPage : Page
    {
        private string _uriString;

        public UrlInputPage()
        {
            this.InitializeComponent();
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            BtnConnect.IsEnabled = false;
            TxtError.Visibility = Visibility.Collapsed;
            BtnConfirm.Visibility = Visibility.Collapsed;

            _uriString = UrlInputBox.Text;

            // Append http:// if no scheme is provided
            if (!_uriString.StartsWith("http://") && !_uriString.StartsWith("https://"))
            {
                _uriString = "http://" + _uriString;
            }

            try
            {
                var ub = new UriBuilder(_uriString);
                _uriString = ub.Uri.AbsoluteUri; // Update uriString with the corrected URI
            }
            catch (Exception ex)
            {
                UpdateErrorUI($" {ex.Message}");
                BtnConnect.IsEnabled = true;
                return;
            }

            // Check if the URL is valid
            if (!await CheckURLValidAsync(_uriString))
            {
                // Show confirm button and prompt user for confirmation
                BtnConfirm.Visibility = Visibility.Visible;
                UpdateErrorUI( _uriString + " is accessible, but jellyfin instance verification failed. Please confirm if this is the correct URL. Incorrect url will require clearing app storage");
            }
            else
            {
                // URL is valid, proceed further
                ProceedWithValidURL(_uriString);
            }

            BtnConnect.IsEnabled = true;
        }

        private void UrlInputBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                SubmitButton_Click(this, new RoutedEventArgs());
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Assuming the URL is correct based on user confirmation;
            ProceedWithValidURL(_uriString);
        }

        private async Task<bool> CheckURLValidAsync(string uriString)
        {
            // Ensure the URL is well-formed
            if (!Uri.TryCreate(uriString, UriKind.Absolute, out Uri testUri))
            {
                UpdateErrorUI("Invalid URL format.");
                return false;
            }

            // First attempt to connect with the original URI
            HttpWebResponse response = await AttemptConnection(testUri);
            if (response == null)
            {
                // If connection failed, try with the alternate scheme
                string alternateScheme = testUri.Scheme == Uri.UriSchemeHttp ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
                var alternateUri = new UriBuilder(testUri) { Scheme = alternateScheme }.Uri;
                response = await AttemptConnection(alternateUri);
            }

            if (response == null)
            {
                UpdateErrorUI("Unable to connect to the URL.");
                return false;
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                UpdateErrorUI($"Server responded with status code: {response.StatusCode}");
                return false;
            }

            return await ValidateResponseContent(response);
        }

        private async Task<HttpWebResponse> AttemptConnection(Uri uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            try
            {
                return (HttpWebResponse)(await request.GetResponseAsync());
            }
            catch (Exception)
            {
                return null; // Connection failed
            }
        }

        private async Task<bool> ValidateResponseContent(HttpWebResponse response)
        {
            var encoding = System.Text.Encoding.GetEncoding(response.CharacterSet);
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                string responseText = await reader.ReadToEndAsync();
                if (!responseText.Contains("Jellyfin"))
                {
                    // Update UI with a specific error message
                    UpdateErrorUI("Response does not contain expected content.");
                    return false;
                }
            }

            return true;
        }

        private void UpdateErrorUI(string message)
        {
            TxtError.Text = message;
            TxtError.Visibility = Visibility.Visible;
        }

        private void ProceedWithValidURL(string url)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["savedUrl"] = url; // Save the URL
            Frame.Navigate(typeof(MainPage), url); // Navigate to MainPage with the new URL
        }

    }
}
