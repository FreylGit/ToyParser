namespace ToyParser
{
    public interface IWriter<T>
    {
        public void Write(T model);
    }
}
