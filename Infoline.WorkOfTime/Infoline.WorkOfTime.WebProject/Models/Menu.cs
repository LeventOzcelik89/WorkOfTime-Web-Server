using System.Collections.Generic;

namespace System.Web.Mvc
{
    public class Menu
    {
        public string name { get; set; }
        public string url { get; set; }
        public string icon { get; set; }
        public bool newBlank { get; set; }
        public bool visible { get; set; } = true;
        public List<Menu> childs { get; set; } = new List<Menu>();

        public Menu()
        {
        }

        public Menu(string Name, string Url = "#", string Icon = "", bool newBlank = false)
        {
            this.name = Name;
            this.url = Url;
            this.icon = Icon;
            this.newBlank = newBlank;
        }

        public void AddChild(Menu child)
        {
            childs.Add(child);
        }

        public Menu Visible(bool visible)
        {
            this.visible = visible;
            return this;
        }
    }
}