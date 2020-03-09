using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cotpro.Text.Translation.Bing
{
    public static class Extensions
    {
        /// <summary>
        /// Serialize a WordList instance into a binary file.
        /// </summary>
        /// <param name="mlist"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool BinarySerialize(this MeaningList mlist, string path)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            try
            {
                using (System.IO.FileStream file = new System.IO.FileStream(path, System.IO.FileMode.Create))
                {
                    bf.Binder = new MeaningListBinder();
                    bf.Serialize(file, mlist);
                }
                return true;
            }
            catch (System.Runtime.Serialization.SerializationException se)
            {
                return false;
            }
        }
        /// <summary>
        /// Return a WordList instance that deserialized from a binary file.
        /// </summary>
        /// <param name="mlist"></pkearam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static MeaningList BinaryDeserialize(this MeaningList mlist, string path)
        {
            MeaningList words = null;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            try
            {
                using (System.IO.FileStream file = new System.IO.FileStream(path, System.IO.FileMode.Open))
                {
                    bf.Binder = new MeaningListBinder();
                    words = (MeaningList)bf.Deserialize(file);
                }
                return words;
            }
            catch (System.Runtime.Serialization.SerializationException se)
            {
                return words;
            }
        }
    }
}
