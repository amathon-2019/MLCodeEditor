using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.CognitiveServices.Speech;

namespace MLCodeEditor.Messages
{
  
    public class MessageListener
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

        public async Task<(string origin, string result)> RecognizeSpeechSync()
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

            //convert
            (string orgin, string result) ret;
            
            if(stt.Contains( "저장") || stt.Contains("세이브" ))
            {
                if(stt.Contains("나가기") || stt.Contains ("닫기"))
                {
                    ret = (stt, "saveAsExit");    
                }
                else
                {
                    ret = (stt, "save");
                }
            }else if(stt.Contains("확대"))
            {
                ret = (stt, "zoomIn");
            }else if(stt.Contains("축소"))
            {
                ret = (stt, "zoomOut");
            }else if(stt.Contains("위") || stt.Contains("상단"))
            {
                ret = (stt, "moveTop");
            }else if(stt.Contains("아래") || stt.Contains("밑") || stt.Contains("하단"))
            {
                ret = (stt, "moveBottom");
            }else
            {
                ret = (stt, "none");
            }
            return ret;
        }
    }
}
