using System.IO;
using System.Threading.Tasks;

namespace AzureApp.Interfaces
{
    public interface IProcessProvider
    {
        Stream GetDummyStream();

        Task<string> ProcessParticipantAsync(Stream stream);
    }
}