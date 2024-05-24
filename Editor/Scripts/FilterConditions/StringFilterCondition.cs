using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    public abstract class StringFilterCondition : FilterCondition
    {
        [SerializeField] protected string _conditionString;
        
        protected StringFilterCondition(string inspectorName) : base(inspectorName){ }
        public override bool IsTrue(RequestImageData imageData) => IsStringConditionTrue(imageData);
        public override void Clear() => _conditionString = string.Empty;
        protected abstract bool IsStringConditionTrue(RequestImageData imageData);

    }
}