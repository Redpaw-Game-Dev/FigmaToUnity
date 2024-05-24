namespace LazyRedpaw.FigmaToUnity
{
    public class IsEmptyNameFilterCondition : BoolFilterCondition
    {
        public IsEmptyNameFilterCondition(string inspectorName) : base(inspectorName) { }
        protected override bool IsBoolConditionTrue(RequestImageData imageData)
        {
            return string.IsNullOrEmpty(imageData.Name);
        }

    }
}