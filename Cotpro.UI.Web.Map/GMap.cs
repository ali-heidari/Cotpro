using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Web.UI;
namespace Cotpro.UI.Web.Map
{
    [ToolboxBitmap(typeof(GMap)), ParseChildren(true)]
    public class GMap : ScriptControl
    {
        private int _Zoom = 10;
        private List<GMarker> markers = new List<GMarker>();
        private string _webServiceUrl = "";
        private int _interval = 5000;
        private ScriptManager sm;
        public int Zoom
        {
            get
            {
                return this._Zoom;
            }
            set
            {
                this._Zoom = value;
            }
        }
        public double CenterLatitude
        {
            get;
            set;
        }
        public double CenterLongitude
        {
            get;
            set;
        }
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<GMarker> Markers
        {
            get
            {
                return this.markers;
            }
        }
        [UrlProperty]
        public string WebServiceUrl
        {
            get
            {
                return this._webServiceUrl;
            }
            set
            {
                this._webServiceUrl = value;
            }
        }
        private string _popupWebService = string.Empty;
        [System.Web.UI.UrlProperty]
        public string PopupWebService
        {
            get
            {
                return _popupWebService;
            }
            set
            {
                _popupWebService = value;
            }
        }
        private List<Line> _lines = new List<Line>();
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<Line> Lines
        {
            get { return this._lines; }
        }
        public int Interval
        {
            get
            {
                return this._interval;
            }
            set
            {
                this._interval = value;
            }
        }
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }
        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write("<script type='text/javascript' src='http://maps.googleapis.com/maps/api/js?sensor=false'></script>");
            base.RenderContents(writer);
        }
        protected override void OnPreRender(EventArgs e)
        {
            if (!base.DesignMode)
            {
                this.sm = ScriptManager.GetCurrent(this.Page);
                if (this.sm == null)
                {
                    throw new HttpException("A ScriptManager control must exist on the current page.");
                }
                this.sm.RegisterScriptControl<GMap>(this);
            }
            base.OnPreRender(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (!base.DesignMode)
            {
                this.sm.RegisterScriptDescriptors(this);
            }
            base.Render(writer);
        }
        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor scriptControlDescriptor = new ScriptControlDescriptor("Cotpro.UI.Web.Map.GMap", this.ClientID);
            scriptControlDescriptor.AddProperty("zoom", this.Zoom);
            scriptControlDescriptor.AddProperty("centerLatitude", this.CenterLatitude);
            scriptControlDescriptor.AddProperty("centerLongitude", this.CenterLongitude);
            scriptControlDescriptor.AddProperty("markers", this.Markers);
            scriptControlDescriptor.AddProperty("webServiceUrl", this.WebServiceUrl);
            scriptControlDescriptor.AddProperty("interval", this.Interval);
            scriptControlDescriptor.AddProperty("popupWebService", this.PopupWebService);
            scriptControlDescriptor.AddProperty("lines", this.Lines);
            yield return scriptControlDescriptor;
            yield break;
        }
        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            yield return new ScriptReference("Cotpro.UI.Web.Map.GMap.js", base.GetType().Assembly.FullName);
            yield break;
        }
    }
}
