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
                SubscriptionKey = "fa398908b0e641109be0dfb770e15312"
            };
        }


        public void GetKeyPhrases()
        {
            KeyPhraseBatchResult keyPhrases = _client.KeyPhrases(
               new MultiLanguageBatchInput(
                   new List<MultiLanguageInput>()
                   {
                          new MultiLanguageInput("ja", "1", "猫は幸せ"),
                          new MultiLanguageInput("de", "2", "Fahrt nach Stuttgart und dann zum Hotel zu Fu."),
                          new MultiLanguageInput("en", "3", "My cat is stiff as a rock."),
                          new MultiLanguageInput("es", "4", "A mi me encanta el fútbol!")
                   }));
        }

        public void GetSentiment()
        {
            SentimentBatchResult sentiments = _client.Sentiment(
                new MultiLanguageBatchInput(
                    new List<MultiLanguageInput>()
                    {
                        new MultiLanguageInput("en", "0", "I had the best day of my life."),
                        new MultiLanguageInput("en", "1", "This was a waste of my time. The speaker put me to sleep."),
                        new MultiLanguageInput("es", "2", "No tengo dinero ni nada que dar..."),
                        new MultiLanguageInput("it", "3", "L'hotel veneziano era meraviglioso. È un bellissimo pezzo di architettura."),
                    }));
        }

    }
}
