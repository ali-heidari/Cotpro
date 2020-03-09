using System;
using System.Collections.Generic;
using System.Text;

namespace Cotpro.UI.Web
{
    [System.Web.UI.PersistChildren(true)]
    public class MultiUI : System.Web.UI.ScriptControl
    {
        private System.Globalization.CultureInfo _locale = new System.Globalization.CultureInfo("en-US");
        /// <summary>
        /// Locale of control. Ex. en-US. Default is en-US.
        /// </summary>
        public string Locale
        {
            get
            {
                return this._locale.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("Value can not be null or empty.");
                if (!value.Contains("-"))
                    throw new FormatException("Format of value is incorrect. Try like this: en-US");
                this._locale = new System.Globalization.CultureInfo(value);
            }
        }

        private List<MultiUIMenuItem> _items = new List<MultiUIMenuItem>();
        /// <summary>
        /// Items which will be showed on the menu pane.
        /// </summary>
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        public List<MultiUIMenuItem> Items
        {
            get
            {
                return _items;
            }
        }
        public MultiUI()
        {

        }
         
        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            //First div shows List of menus.
            if (this._locale.TextInfo.IsRightToLeft)
                writer.AddStyleAttribute("float", "right");
            else
                writer.AddStyleAttribute("float", "left");
            System.Web.UI.WebControls.ListBox lbox = new System.Web.UI.WebControls.ListBox();
            foreach (MultiUIMenuItem m in this._items)
                lbox.Items.Add(new System.Web.UI.WebControls.ListItem(m.Text, m.WebMethod));
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            lbox.RenderControl(writer);
            writer.RenderEndTag();

            //Second div shows usercontrols.
            if (this._locale.TextInfo.IsRightToLeft)
                writer.AddStyleAttribute("float", "left");
            else
                writer.AddStyleAttribute("float", "right");
            writer.AddStyleAttribute("background-color", "red");
            writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Width, "200px");
            writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Height, "100px");
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);
            writer.RenderEndTag();

            base.RenderContents(writer);
        }

        protected override System.Web.UI.HtmlTextWriterTag TagKey
        {
            get
            {
                return System.Web.UI.HtmlTextWriterTag.Div;
            }
        }

        protected override IEnumerable<System.Web.UI.ScriptDescriptor> GetScriptDescriptors()
        {
            System.Web.UI.ScriptControlDescriptor scd = new System.Web.UI.ScriptControlDescriptor("Cotpro.UI.Web.MultiUI", this.ClientID);
            scd.AddProperty("Locale", this.Locale);
            yield return scd;
        }

        protected override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            yield return new System.Web.UI.ScriptReference("Cotpro.UI.Web.MultiUI.js", this.GetType().Assembly.FullName);
        }
    }
}
