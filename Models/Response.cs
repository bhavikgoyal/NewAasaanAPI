namespace Aasaan_API.Models
{
  public class Response<T>
  {
    public T Data { get; set; }
    public string message { get; set; }
    public int code { get; set; }
  }
}
