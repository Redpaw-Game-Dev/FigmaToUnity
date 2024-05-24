using System.Collections.Generic;

namespace LazyRedpaw.FigmaToUnity
{
    [System.Serializable]
    public class FigmaResponseData
    {
        private string err;
        private Dictionary<string, string> images;

        public FigmaResponseData(string err, Dictionary<string, string> images) 
        {
            this.err = err;
            this.images = images;
        }

        public string Err => err;
        public Dictionary<string, string> Images => images;
    }
}