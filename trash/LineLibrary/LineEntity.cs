using IContract;

namespace LineLibrary
{
    public class LineEntity : IShapeEntity, ICloneable
    {
        public string Name => "Line";

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}