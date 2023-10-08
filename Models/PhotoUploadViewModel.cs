using System.ComponentModel.DataAnnotations;
using System.Web;

public class PhotoUploadViewModel
{
    [Required(ErrorMessage = "Please select a file.")]
    [Display(Name = "Upload Photo")]
    public HttpPostedFileBase PhotoFile { get; set; }
}