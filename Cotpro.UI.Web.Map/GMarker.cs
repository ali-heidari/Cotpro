using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
namespace Cotpro.UI.Web.Map
{
    [Serializable]
    public class GMarker
    {
        public int UID
        {
            get;
            set;
        }
        public double Latitude
        {
            get;
            set;
        }
        public double Longitude
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), UrlProperty]
        public string IconUrl
        {
            get;
            set;
        }
        public virtual string InfoWindowHtml
        {
            get;
            set;
        }
    }
}
