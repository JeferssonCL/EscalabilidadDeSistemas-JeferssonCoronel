using Backend.Data.Models;

namespace Backend.Business.Dtos;

public class InfoDto<T>
{
    public string Source{ get; set; }
    public T? Data { get; set; }
    public string TimeToGet { get; set; }
}