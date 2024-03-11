using IssueHub.Services.Interfaces;

namespace IssueHub.Services
{
    public class IssueHubFileService : IIssueHubFileService
    {
        private readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
        public string ConvertByteArrayToFile(byte[] fileData, string extension)
        {
            try
            {
                string imageBase64Data = Convert.ToBase64String(fileData);

                return string.Format($"data:{extension};base64,{imageBase64Data}");
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<byte[]> ConvertFiletoByteArray(IFormFile file)
        {
            try
            {
                MemoryStream memoryStream = new ();
                await file.CopyToAsync(memoryStream);
                byte[] byteFile = memoryStream.ToArray();

                memoryStream.Close();  
                memoryStream.Dispose();

                return byteFile;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string FormatFileSize(long bytes)
        {
            int counter = 0;
            decimal fileSize = bytes;

            while(Math.Round(fileSize / 1023) >= 1)
            {
                fileSize /= bytes;
                counter++;
            }
                          // Format the first arg into 1 decimal place, 2nd arg is the element of the suffixes array
            return string.Format("{0:n1}{1}", bytes, suffixes[counter]);
        }

        public string GetFileIcon(string file)
        {
            string fileImage = "default";

            // If the file is not Null or Whitespace
            if (!string.IsNullOrWhiteSpace(file))
            {
                fileImage = Path.GetExtension(file).Replace(".", "");

                return $"/img/png/{fileImage}.png";
            }

            return fileImage;
        }
    }
}
