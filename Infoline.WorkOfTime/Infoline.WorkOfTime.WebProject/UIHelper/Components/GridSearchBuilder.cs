using System.Collections.Generic;
using System.ComponentModel;

namespace System.Web.Mvc
{
    public class GridSearchBuilder : FactoryBuilderBase<GridSearch, GridSearchBuilder>
    {

        public GridSearchBuilder(GridSearch component) : base(component)
        {
            this.Component = component;
        }

        public GridSearchBuilder Value(object Value)
        {
            this.Component._Value = Value == null ? "" : Value.ToString();
            return this;
        }

        public GridSearchBuilder Text(string text)
        {
            this.Component._Text = text;
            return this;
        }

        public GridSearchBuilder IconClass(string iconClass)
        {
            this.Component._IconClass = iconClass;
            return this;
        }

        public GridSearchBuilder Color(string color)
        {
            this.Component._Color = color;
            return this;
        }

        public GridSearchBuilder Grid(string grid)
        {
            this.Component._Grid = grid;
            return this;
        }

        public GridSearchBuilder Name(string name)
        {
            this.Component._Name = name;
            return this;
        }

        public GridSearchBuilder PlaceHolder(string placeHolder)
        {
            this.Component._PlaceHolder = placeHolder;
            return this;
        }

        public GridSearchBuilder NameButton(string nameButton)
        {
            this.Component._NameButton = nameButton;
            return this;
        }

        public GridSearchBuilder GridSearchField(string gridSearchField)
        {
            this.Component._GridSearchField = gridSearchField;
            return this;
        }


        
        public GridSearchBuilder Color(Card.Colors color)
        {
            this.Component._Color = color.ToString();
            return this;
        }

        public GridSearchBuilder Link(string link)
        {
            this.Component._Link = link;
            return this;
        }

        public GridSearchBuilder GridFilter(string Grid, string Filter)
        {
            this.Component._Grid = Grid;
            this.Component._GridFilter = Filter;
            return this;
        }

        public GridSearchBuilder HtmlAttributes(Dictionary<string, object> HtmlAttributes)
        {
            this.Component.HtmlAttributes = HtmlAttributes;
            return this;
        }

        public override void Render()
        {

        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToHtmlString()
        {
            return this.Component.Render();
        }

    }
}