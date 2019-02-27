using System;

namespace ContactsApp.API.Dtos
{
    public class ImageUploadOutput
    {
        public string Url  { get; set; }
        public int Description { get; set; }
        public DateTime  DateAdded { get; set; }
        public string PublicId  { get; set; }

        public ImageUploadOutput()
        {
            DateAdded = DateTime.Now;
            
        }
    }
}