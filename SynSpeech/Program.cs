using System;
using System.IO;
using Syn.Speech.Api;


namespace SynSpeech
{
    /// <summary>
    /// Ref - https://www.codeproject.com/Articles/890117/Speech-Recognition-in-Mono-and-NET-Csharp 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Syn Speech Demo");

                var modelsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Models");
                var audioDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Audio");
                var audioFile = Path.Combine(audioDirectory, "Long Audio 2.wav");
                //var audioFile = Path.Combine(audioDirectory, "RE6dfad35e2acc830a26d36bef8f866146.wav");

                var transcription = GetTranscription(audioDirectory, audioFile, modelsDirectory);
                Console.WriteLine(transcription);

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static string GetTranscription(string audioDirectory, string audioFile, string modelsDirectory)
        {
            try
            {
                if (!Directory.Exists(modelsDirectory) || !Directory.Exists(audioDirectory))
                {
                    return "No Models or Audio directory found!! Aborting..."; 
                }

                var speechConfiguration = new Configuration
                {
                    AcousticModelPath = modelsDirectory,
                    DictionaryPath = Path.Combine(modelsDirectory, "cmudict-en-us.dict"),
                    LanguageModelPath = Path.Combine(modelsDirectory, "en-us.lm.dmp"),
                    UseGrammar = true,
                    GrammarPath = modelsDirectory,
                    GrammarName = "hello"
                };
                var speechRecognizer = new StreamSpeechRecognizer(speechConfiguration);
                var stream = new FileStream(audioFile, FileMode.Open);
                speechRecognizer.StartRecognition(stream);

                Console.WriteLine("Transcribing...");
                var result = speechRecognizer.GetResult();

                return result != null ? result.GetHypothesis() : "Sorry! Coudn't Transcribe";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
