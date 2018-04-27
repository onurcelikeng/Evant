using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evant.Cognitive
{
    public class TextAnalytics
    {
        private ITextAnalyticsAPI _client;


        public TextAnalytics()
        {
            _client = new TextAnalyticsAPI()
            {
                AzureRegion = AzureRegions.Westus,
                SubscriptionKey = "094adce6de9f4237875c0103f846c468"
            };
        }


        public LanguageBatchResult DetectLanguage(List<Input> data)
        {
            return _client.DetectLanguage(new BatchInput(data));
        }

        public KeyPhraseBatchResult GetKeyPhrases(List<MultiLanguageInput> data)
        {
            return _client.KeyPhrases(new MultiLanguageBatchInput(data));
        }

        public SentimentBatchResult GetSentiment(List<MultiLanguageInput> data)
        {
            return _client.Sentiment(new MultiLanguageBatchInput(data));
        }

    }
}
