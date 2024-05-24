namespace LazyRedpaw.FigmaToUnity
{
    public class IsEmptyUrlFilterCondition : BoolFilterCondition
    {
        public IsEmptyUrlFilterCondition(string inspectorName) : base(inspectorName) { }
        protected override bool IsBoolConditionTrue(RequestImageData imageData)
        {
            return string.IsNullOrEmpty(imageData.URL);
        }
    }
}