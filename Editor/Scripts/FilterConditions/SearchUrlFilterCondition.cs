using System;

namespace LazyRedpaw.FigmaToUnity
{
    public class SearchUrlFilterCondition: StringFilterCondition
    {
        public SearchUrlFilterCondition(string inspectorName) : base(inspectorName) { }
        protected override bool IsStringConditionTrue(RequestImageData imageData)
        {
            return imageData.URL.Contains(_conditionString, StringComparison.OrdinalIgnoreCase);
        }
    }
}