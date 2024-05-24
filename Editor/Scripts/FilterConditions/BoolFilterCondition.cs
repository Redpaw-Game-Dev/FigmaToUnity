namespace LazyRedpaw.FigmaToUnity
{
    public abstract class BoolFilterCondition : FilterCondition
    {
        protected BoolFilterCondition(string inspectorName) : base(inspectorName) { }
        public override bool IsTrue(RequestImageData imageData) => IsBoolConditionTrue(imageData);
        public override void Clear() => _isEnabled = false;
        protected abstract bool IsBoolConditionTrue(RequestImageData imageData);

    }
}