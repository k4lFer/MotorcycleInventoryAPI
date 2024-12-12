using DataTransferLayer.OtherObject;

namespace Service.Generic
{
    public abstract class SoGeneric<T, TOt>
    {
        public DtoMessage message { get; set; }
        public DtoContainer<T, TOt> body { get; set; }

        public SoGeneric()
        {
            message = new DtoMessage();
            body = new DtoContainer<T, TOt>();
        }
    }
}
