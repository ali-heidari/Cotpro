using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cotpro.Text.Translation
{
    public static class Extension
    {

        /// <summary>
        /// This translate string word to specified culture in offline mode.
        /// If it returned exact word as translation, it means it does not exist any meaning for the word in the sent resource stream.
        /// </summary>
        /// <param name="word">Word to be translated.</param>
        /// <param name="culture">Target culture.</param>
        /// <param name="resourceStream">A stream of words.</param>
        /// <param name="binder">A binder for BinaryFormatter.</param>
        /// <returns>Word Equal to the main word in traget culture language.</returns>
        public static string GetTranslation(this string word,string locale, System.IO.Stream resourceStream, System.Runtime.Serialization.SerializationBinder binder)
        {
            Cotpro.Text.Translation.MeaningList m;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            bf.Binder = binder;
            m = (Cotpro.Text.Translation.MeaningList)bf.Deserialize(resourceStream);

            foreach (TranslatedWord tw in m)
            {
                if (tw.Value == word)
                    foreach (Cotpro.Text.Word w in tw.Meanings)
                        if (w.Locale.Name == locale)
                            return w.Value;
            }
            return word;
        }
    }
}
