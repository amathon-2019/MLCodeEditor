using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.CognitiveServices.Speech;

namespace MLCodeEditor.Messages
{
    class MessageListener
    {
        private SpeechConfig config;

        /// <summary>
        /// 시스템 환경 변수 AZURE_KEY 라는 이름으로,
        /// API 요청 KEY 저장
        /// </summary>
        /// <param name="lang"></param>
        public MessageListener(string lang)
        {
            this.config = SpeechConfig.FromSubscription(Environment.GetEnvironmentVariable("AZURE_KEY"), "westus");
            config.SpeechRecognitionLanguage = lang;
        }

        public async Task<string> RecognizeSpeechSync()
        {
            string stt = string.Empty;
            using (var recognizer = new SpeechRecognizer(config))
            {
                // Start Listening..
                var res = await recognizer.RecognizeOnceAsync();

                // Chceks result
                switch(res.Reason)
                {
                    case ResultReason.RecognizedSpeech:
                        // Return 
                        stt = res.Text;
                        break;

                    case ResultReason.NoMatch:
                        stt = "NO MATCH";
                        break;

                    case ResultReason.Canceled:
                        var canceliation = CancellationDetails.FromResult(res);
                        if(canceliation.Reason == CancellationReason.Error)
                        {
                            // Error
                            stt = "ERROR";
                        }
                        break;
                }
            }
            Debug.WriteLine(stt);
            return stt;
        }
    }
}
