namespace System.Web.Mvc
{
    public class GridSearchBuilderBase<TGridSearch, TGridSearchBuilder> : InputBuilderBase<TGridSearch, TGridSearchBuilder>, IHideMembers
        where TGridSearch : InputBase
        where TGridSearchBuilder : FactoryBuilderBase<TGridSearch, TGridSearchBuilder>
    {
        public GridSearchBuilderBase(TGridSearch component) : base(component)
        {
            base.Component = component;
        }

        public TGridSearchBuilder Template(string template)
        {
            return null;
        }

        public TGridSearchBuilder TemplateId(string templateId)
        {
            return null;
        }

        public TGridSearchBuilder Value(string value)
        {
            return null;
        }

    }
}