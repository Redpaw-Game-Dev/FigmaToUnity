using System;

namespace LazyRedpaw.FigmaToUnity
{
    public class SearchNameFilterCondition : StringFilterCondition
    {
        public SearchNameFilterCondition(string inspectorName) : base(inspectorName) { }
        protected override bool IsStringConditionTrue(RequestImageData imageData)
        {
            return imageData.Name.Contains(_conditionString, StringComparison.OrdinalIgnoreCase);
        }
    }
}