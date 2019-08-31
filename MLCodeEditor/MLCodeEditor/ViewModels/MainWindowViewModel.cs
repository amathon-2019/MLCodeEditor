using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MLCodeEditor.Messages;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MLCodeEditor.ViewModels
{
    class MainWindowViewModel : BindableBase
    {

        private string _ttheme;
        public string bTheme
        {
            get { return _ttheme; }
            set { SetProperty(ref _ttheme, value); }
        }

        private string _fontColorTheme;
        public string bFontColorTheme
        {
            get { return _fontColorTheme; }
            set { SetProperty(ref _fontColorTheme, value); }
        }

        private string _syntaxHighlighting;
        public string bSyntaxHighlighting
        {
            get { return _syntaxHighlighting; }
            set { SetProperty(ref _syntaxHighlighting, value); }
        }

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        private DelegateCommand _clickToTalk;
        public DelegateCommand cClickToTalk =>
            _clickToTalk ?? (_clickToTalk = new DelegateCommand(onClickToTalk));

        private IAmazonS3 _client;
        private readonly IEventAggregator _ea;

        private readonly string bucketName = "mlcodeeditor";
        private readonly string filePath = "saved.wav";

        public MainWindowViewModel(IEventAggregator ea)
        {
            this._ea = ea;
            _ea.GetEvent<ThemeMessage>().Subscribe(onChangeTheme);
            _ea.GetEvent<fontColorThemeMessage>().Subscribe(onChangeFontColorTheme);
            _ea.GetEvent<syntaxThemeMessage>().Subscribe(onChangeSyntaxTheme);
        }

        void onClickToTalk()
        {
            mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
            mciSendString("record recsound", "", 0, 0);
            Console.WriteLine("recording, press Enter to stop and save ...");
            DispatcherTimer dt = new DispatcherTimer();

            dt.Interval = new TimeSpan(0, 0, 3);
            dt.Tick +=  async (s, e) =>
            {
                mciSendString("save recsound saved.wav", "", 0, 0);
                mciSendString("close recsound ", "", 0, 0);
                Console.WriteLine("recorded");

                string result = await SendWavFileAndGetReuqest();
                OperateCommandAsync(result);
                dt.Stop();
            };
            dt.Start();
        }

        private async Task<string> SendWavFileAndGetReuqest()
        {
            try
            {
                TransferUtility ftu =
                    new TransferUtility(_client);

                PutObjectRequest request = new PutObjectRequest()
                {
                    BucketName = string.Format(@"{0}", bucketName),
                    FilePath = filePath
                };
                PutObjectResponse response1 = await _client.PutObjectAsync(request);
                //s3 put 

                GetPreSignedUrlRequest request2 = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = filePath,
                    Expires = DateTime.Now.AddMinutes(10)
                };

                string urlString = _client.GetPreSignedURL(request2);
                //s3 link 

                JObject jobject = new JObject();
                jobject["MediaLeng"] = "ko-KR";
                jobject["MediaFileUri"] = urlString;

                var rrequest = WebRequest.Create("https://22aizbnnx6.execute-api.ap-northeast-2.amazonaws.com/mlCodeEdit/mediatotext");
                rrequest.ContentType = "application/json";
                rrequest.Method = "POST";
                using (var streamWriter = new StreamWriter(rrequest.GetRequestStream()))
                {
                    string json = jobject.ToString();
                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)rrequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine(result.ToString());
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void OperateCommandAsync(string result)
        {
            result = result.ToLower();

            if( result == "save")
            {

            }
            else if ( result == "save and exit")
            {

            }
            else if ( result == "scroll down")
            {

            }
            else if (result == "scroll up")
            {

            }
            else if (result == "theme dark")
            {

            }
            else if (result == "theme light")
            {

            }

        }

        private void onChangeTheme(string theme) => bTheme = theme;
        private void onChangeFontColorTheme(string fontColor) => bFontColorTheme = fontColor;
        private void onChangeSyntaxTheme(string syntax) => bSyntaxHighlighting = syntax;

    }
}
