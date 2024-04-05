using CsvHelper;
using CsvHelper.Configuration;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace ToDoList.Backend.Domain.Utils
{
    public class CsvBaseService<T>
    {
        private readonly CsvConfiguration _csvConfiguration;

        public CsvBaseService() 
        {
            _csvConfiguration = GetConfiration();
        }

        public byte[] DownloadFile<T>(IEnumerable<T> data)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (var csvWriter = new CsvWriter(writer, _csvConfiguration))
            {
                csvWriter.WriteRecords(data);
                writer.Flush();
                return stream.ToArray();
            }
        }

        public CsvConfiguration GetConfiration()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                Encoding = Encoding.UTF8,
                NewLine = "\r\n"
            };
        }
    }
}
