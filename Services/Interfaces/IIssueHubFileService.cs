namespace IssueHub.Services.Interfaces
{
    public interface IIssueHubFileService
    { 
    
        /* IFormfile is a .Net provided interface to manipulate files. And we can easily get access to it by using
         * Microsoft.AspNetCore.Http namespace. IFormFile represents a file sent with a HTTP request, a file that is 
         * sent, that is presented in the browser, from the front end to our backend code
         * 
         */
        public Task<byte[]> ConvertFiletoByteArray(IFormFile file);

        // Retrieving info byte[] from the db as a string
        public string ConvertByteArrayToFile(byte[] fileData, string extension);

        // Based on the type of file the user chooses, it will present an icon
        public string GetFileIcon(string file);


                      //Estimate the size of the file 
        public string FormatFileSize(long bytes);
    }
}
