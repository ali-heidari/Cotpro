using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cotpro.Text.Translation
{
    public class MeaningListBinder : System.Runtime.Serialization.SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (typeName.Contains("TranslatedWord"))
                return typeof(TranslatedWord);
            else if (typeName.Contains("List`1[[Cotpro.Text.Word"))
                return typeof(List<Word>);
            else if (typeName.Contains("Cotpro.Text.Word"))
                return typeof(Word);
            else if (typeName.Contains("Cotpro.Text.Translation.MeaningList"))
                return typeof(MeaningList);
            else if (typeName.Contains("CultureInfo"))
                return typeof(System.Globalization.CultureInfo);
            else if (typeName.Contains("CompareInfo"))
                return typeof(System.Globalization.CompareInfo);
            else
                return typeof(Word);

        }
    }
}
