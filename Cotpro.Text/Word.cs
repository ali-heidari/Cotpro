using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cotpro.Text
{
    /// <summary>
    /// Word class to store a word.
    /// </summary>
    [Serializable]
    public class Word
    {
        /// <summary>
        /// Word.
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// locale of word.
        /// </summary>
        public System.Globalization.CultureInfo Locale { get; set; }

        /// <summary>
        /// Cunstructor.
        /// </summary>
        public Word()
        {
        }
    }
}
