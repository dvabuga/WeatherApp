using System.IO;
using System.Threading.Tasks;

namespace WeatherApp.Services.Interfaces
{
    public interface IStorageService
    {
        Task Upload(Stream file);
    }


}