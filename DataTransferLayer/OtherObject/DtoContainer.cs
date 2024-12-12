namespace DataTransferLayer.OtherObject
{
    public class DtoContainer<T,TOt>
    {
        public T dto { get; set; }
        public List<T> listDto { get; set; }
        public TOt? other { get; set; }
    }
}
