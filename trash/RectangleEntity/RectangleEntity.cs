using IContract;

namespace RectangleEntity
{
    public class RectangleEntity : IShapeEntity, ICloneable
    {
        public string Name => "Rectangle";

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}