namespace ToyParser.Writers
{
    public interface IWriter<T>
    {
        public void Write(T model);
    }
}
