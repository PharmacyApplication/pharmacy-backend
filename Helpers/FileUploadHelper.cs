namespace PharmacyAPI.Helpers
{
    public class FileUploadHelper
    {
        private readonly IWebHostEnvironment _env;

        public FileUploadHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Saves the uploaded file to wwwroot/prescriptions/ using GUID + original extension.
        /// Returns the relative path stored in the DB, e.g. "prescriptions/abc123.pdf"
        /// </summary>
        public async Task<string> SavePrescriptionFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null.");

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";

            var folderPath = Path.Combine(_env.WebRootPath, "prescriptions");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path for DB storage
            return Path.Combine("prescriptions", fileName).Replace("\\", "/");
        }

        /// <summary>
        /// Deletes a file from wwwroot given its relative path.
        /// </summary>
        public void DeleteFile(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return;

            var fullPath = Path.Combine(_env.WebRootPath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
    }
