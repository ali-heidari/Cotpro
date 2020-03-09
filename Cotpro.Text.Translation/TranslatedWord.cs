using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cotpro.Text.Translation
{
    [Serializable]
    public class TranslatedWord : Word
    {
        private List<Word> _meanings = new List<Word>();
        /// <summary>
        /// Meanigns of the words will be stored.
        /// </summary>
        public List<Word> Meanings
        {
            get
            {
                return _meanings;
            }
            set
            {
                _meanings = value;
            }
        }
    }
}
